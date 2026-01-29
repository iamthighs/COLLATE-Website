// components/layout/NavbarSkeleton.tsx
export default function NavbarSkeleton() {
  return (
    <nav className="topnav navbar navbar-expand shadow bg-white">
      <div className="d-flex align-items-center px-3">
        {/* Sidebar toggle button skeleton */}
        <div
          className="btn btn-icon btn-transparent-dark me-2"
          style={{
            width: "2.5rem",
            height: "2.5rem",
            borderRadius: "0.25rem",
            backgroundColor: "#e0e0e0",
            animation: "pulse 1.5s infinite"
          }}
        ></div>

        {/* Logo skeleton */}
        <div
          className="me-4"
          style={{
            width: "100px",
            height: "40px",
            backgroundColor: "#e0e0e0",
            borderRadius: "0.25rem",
            animation: "pulse 1.5s infinite"
          }}
        ></div>
      </div>

      <ul className="navbar-nav ms-auto d-flex align-items-center gap-3 me-4">
        {/* User avatar skeleton */}
        <li className="nav-item">
          <div
            className="rounded-circle"
            style={{
              width: "2.5rem",
              height: "2.5rem",
              backgroundColor: "#e0e0e0",
              animation: "pulse 1.5s infinite"
            }}
          ></div>
        </li>
      </ul>

      {/* Pulse animation keyframes */}
      <style jsx>{`
        @keyframes pulse {
          0% { opacity: 1; }
          50% { opacity: 0.4; }
          100% { opacity: 1; }
        }
      `}</style>
    </nav>
  );
}
