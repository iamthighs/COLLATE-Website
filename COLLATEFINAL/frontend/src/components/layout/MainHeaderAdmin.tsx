"use client";

import Link from "next/link";

interface MainHeaderAdminProps {
  title: string;
  icon?: string; 
  importAction?: () => void;
  createHref?: string;
  importLabel?: string;
  createLabel?: string;
}

export default function MainHeader({
  title,
  icon = "file-text",
  importAction,
  createHref = "#",
  importLabel = "Import",
  createLabel = "Create New",
}: MainHeaderAdminProps) {
  return (
    <header className="page-header page-header-compact page-header-light border-bottom bg-white mb-4">
      <div className="container-fluid px-4">
        <div className="page-header-content">
          <div className="row align-items-center justify-content-between pt-3">
            <div className="col-auto mb-3">
              <h1 className="page-header-title">
                <div className="page-header-icon">
                  <i data-feather={icon} />
                </div>
                {title}
              </h1>
            </div>

            <div className="col-12 col-xl-auto mb-3">
              {importAction && (
                <button
                  className="btn btn-sm btn-light text-primary me-2"
                  type="button"
                  onClick={importAction}
                >
                  <i className="me-1" data-feather="upload" />
                  {importLabel}
                </button>
              )}

              {createHref && (
                <Link
                  href={createHref}
                  className="btn btn-sm btn-light text-primary"
                >
                  <i className="me-1" data-feather="plus" />
                  {createLabel}
                </Link>
              )}
            </div>
          </div>
        </div>
      </div>
    </header>
  );
}
