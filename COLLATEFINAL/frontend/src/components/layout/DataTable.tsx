"use client";

import Link from "next/link";
import Script from "next/script";
import { useEffect } from "react";

interface DataTableProps<T extends { id: string }> {
  data: T[];
  categoryKey: keyof T;    
  titleKey: keyof T;       
  dateKey: keyof T;        
  editUrlBase: string;     
}

export default function DataTable<T extends { id: string }>({
  data,
  categoryKey,
  titleKey,
  dateKey,
  editUrlBase,
}: DataTableProps<T>) {
  const hasData = data.length > 0;

  useEffect(() => {
    const tableEl = document.getElementById("datatablesSimple");
    if (!tableEl || (tableEl as any).dataset.initialized) return;

    (tableEl as any).dataset.initialized = "true";

    if (typeof window !== "undefined" && window.simpleDatatables) {
      new window.simpleDatatables.DataTable("#datatablesSimple", {
        searchable: true,
        fixedHeight: true,
        perPage: 10,
      });
    }
  }, [data]);

  return (
    <>
      <div className="card card-scrollable">
          <div className="card-header">
          <input className="form-check-input me-1" type="checkbox" id="selectAll" />
          <label htmlFor="selectAll">Select All</label>

          <button
            id="btnDeleteSelected"
            className="btn btn-datatable btn-icon btn-transparent-dark float-end"
            data-delete-url="/Subjects/DeleteMultiple"
            disabled
          >
            <i data-feather="trash-2" />
          </button>
        </div>
        <div className="card-body" style={{ maxHeight: "50rem" }}>
          <table className="table" id="datatablesSimple">
            <thead>
              <tr>
                <th>Category</th>
                <th>Title</th>
                <th>Date Created</th>
                <th>Actions</th>
              </tr>
            </thead>

            {hasData && (
              <tbody>
                {data.map((item) => (
                  <tr key={item.id}>
                    <td>
                      <span className="badge bg-blue-soft text-blue">
                         {String(item[categoryKey])}
                      </span>
                    </td>
                    <td> {String(item[titleKey])}</td>
                    <td>
                      {new Date(String(item[dateKey])).toLocaleDateString("en-US", {
                        weekday: "long",
                        day: "2-digit",
                        month: "long",
                        year: "numeric",
                      })}
                    </td>
                    <td>
                      <Link
                        href={`${editUrlBase}/${item.id}`}
                        className="btn btn-datatable btn-icon btn-transparent-dark me-2"
                      >
                        <i data-feather="edit" />
                      </Link>

                      <button
                        className="btn btn-datatable btn-icon btn-transparent-dark me-2"
                        type="button"
                      >
                        <i data-feather="trash-2" />
                      </button>

                      <input type="checkbox" className="row-checkbox ms-2" value={item.id} />
                    </td>
                  </tr>
                ))}
              </tbody>
            )}
          </table>
        </div>
      </div>
    </>
  );
}
