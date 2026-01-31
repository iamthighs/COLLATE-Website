"use client";

import Link from "next/link";
import { useMemo, useState } from "react";
import DataTableRDC, { TableColumn } from "react-data-table-component";
import { deleteMultipleResearchPapers } from "../../lib/supabase/researchPaper";

interface DataTableProps<T extends { id: string }> {
  data: T[];
  loading?: boolean;
  categoryKey: keyof T;
  titleKey: keyof T;
  dateKey: keyof T;
  editUrlBase: string;
}

export default function DataTable<T extends { id: string }>({
  data,
  loading,
  categoryKey,
  titleKey,
  dateKey,
  editUrlBase,
}: DataTableProps<T>) {
  const placeholderRow = {
    id: "",
    [categoryKey]: "Loading...",
    [titleKey]: "Loading...",
    [dateKey]: new Date().toISOString(),
  };
  const [selectedRows, setSelectedRows] = useState<T[]>([]);
  const [deleting, setDeleting] = useState(false);
  const columns = useMemo<TableColumn<T>[]>(
    () => [
      {
        name: "Category",
        selector: (row) => String(row[categoryKey]),
        cell: (row) => <span className="badge bg-blue-soft text-blue">{String(row[categoryKey])}</span>,
        sortable: true,
      },
      {
        name: "Title",
        selector: (row) => String(row[titleKey]),
        sortable: true,
      },
      {
        name: "Date Created",
        selector: (row) => String(row[dateKey]),
        cell: (row) =>
          new Date(String(row[dateKey])).toLocaleDateString("en-US", {
            weekday: "long",
            day: "2-digit",
            month: "long",
            year: "numeric",
          }),
        sortable: true,
      },
      {
        name: "Actions",
        cell: (row) => (
          <div className="d-flex align-items-center">
            <Link
              href={row.id ? `${editUrlBase}/${row.id}` : "#"}  // placeholder row will not break link
              className={`btn btn-datatable btn-icon btn-transparent-dark me-2 ${!row.id ? "disabled" : ""}`}
            >
              <i data-feather="edit" />
            </Link>

            <button
              className="btn btn-datatable btn-icon btn-transparent-dark me-2"
              type="button"
              disabled={!row.id}  // disable for placeholder row
            >
              <i data-feather="trash-2" />
            </button>

          </div>
        ),
      },
    ],
    [categoryKey, titleKey, dateKey, editUrlBase]
  );
  const handleRowSelected = (state: { selectedRows: T[] }) => {
    setSelectedRows(state.selectedRows);
  };

  const handleDeleteSelected = async () => {
  if (selectedRows.length === 0) {
    console.warn("No rows selected for deletion");
    return;
  }

  console.log("Selected rows to delete:", selectedRows);

  if (!confirm(`Are you sure you want to delete ${selectedRows.length} selected item(s)?`)) return;

  setDeleting(true);
  const ids = selectedRows.map((row) => row.id);
  console.log("IDs to delete:", ids);

  try {
    const { data, error } = await deleteMultipleResearchPapers(ids);

    if (error) {
      alert("Failed to delete selected rows");
    } else if (!data || data.length === 0) {
      alert("No rows were deleted. Please check the database.");
    } else {
      alert(`Deleted ${data.length} rows successfully`);
    }
  } catch (err) {
    console.error("Unexpected error during deletion:", err);
    alert("An unexpected error occurred while deleting rows");
  } finally {
    setDeleting(false);
  }
};

  return (
    <div className="card card-scrollable">
      <div className="card-header">
        <button
          className="btn btn-datatable btn-icon btn-transparent-dark"
          onClick={handleDeleteSelected}
          disabled={selectedRows.length === 0 || deleting}
        >
          <i data-feather="trash-2" />
        </button>
      </div>

      <div className="card-body" style={{ maxHeight: "50rem" }}>
        <DataTableRDC
          columns={columns}
          data={loading ? [placeholderRow as T] : data} 
          pagination={!loading}                          
          responsive
          highlightOnHover
          fixedHeader
          fixedHeaderScrollHeight="50rem"
          noHeader
          selectableRows               // <-- enables row checkboxes
          selectableRowsHighlight      // optional: highlights selected rows
          onSelectedRowsChange={handleRowSelected}
          progressPending={loading || deleting}
          progressComponent={<p>{deleting ? "Deleting..." : "Loading..."}</p>}
        />
      </div>
    </div>
  );
}
