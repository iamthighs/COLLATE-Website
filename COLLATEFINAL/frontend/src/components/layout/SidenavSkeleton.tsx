// components/layout/SidenavSkeleton.tsx
export default function SidenavSkeleton() {
  return (
    <div id="layoutSidenav_nav">
      <nav className="sidenav shadow-right sidenav-light d-flex flex-column" style={{ height: '100%' }}>
        <div className="sidenav-menu flex-grow-1 p-3">
          <div className="sidenav-menu-heading mb-2">
            <div className="bg-gray-200" style={{ height: '1rem', width: '50%', borderRadius: '4px', animation: 'pulse 1.5s infinite' }}></div>
          </div>
          <div className="sidenav-menu-heading mb-4">
            <div className="bg-gray-200" style={{ height: '1rem', width: '35%', borderRadius: '4px', animation: 'pulse 1.5s infinite' }}></div>
          </div>

          {Array.from({ length: 8 }).map((_, idx) => (
            <div key={idx} className="nav-link d-flex align-items-center mb-2">
              <div className="bg-gray-200 rounded-circle me-2" style={{ width: '1.5rem', height: '1.5rem', animation: 'pulse 1.5s infinite' }}></div>
              <div className="bg-gray-200" style={{ height: '1rem', width: '80%', borderRadius: '4px', animation: 'pulse 1.5s infinite' }}></div>
            </div>
          ))}
        </div>

        <div className="sidenav-footer mt-auto p-3 border-top">
          <div className="bg-gray-200 mb-1" style={{ height: '0.8rem', width: '50%', borderRadius: '4px', animation: 'pulse 1.5s infinite' }}></div>
          <div className="bg-gray-200" style={{ height: '1rem', width: '75%', borderRadius: '4px', animation: 'pulse 1.5s infinite' }}></div>
        </div>
      </nav>

      <style jsx>{`
        @keyframes pulse {
          0% { opacity: 1; }
          50% { opacity: 0.4; }
          100% { opacity: 1; }
        }
      `}</style>
    </div>
  );
}
