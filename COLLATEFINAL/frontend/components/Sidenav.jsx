"use client"
import Link from 'next/link'
import { usePathname } from 'next/navigation'
import { SidebarNavClass, Pages } from '../utils/manageSidebar'

export default function Sidenav(){
  const pathname = usePathname()
  return (
    <div id="layoutSidenav_nav">
      <nav className="sidenav shadow-right sidenav-light">
        <div className="sidenav-menu">
          <div className="nav accordion" id="accordionSidenav">
            <div className="sidenav-menu-heading">Overview</div>
            <Link href="/" className={`nav-link ${SidebarNavClass(pathname, Pages.Home)}`}><div className="nav-link-icon"><i data-feather="home"></i></div>Home</Link>
            <Link href="/dashboard" className={`nav-link ${SidebarNavClass(pathname, Pages.Dashboard)}`}><div className="nav-link-icon"><i data-feather="activity"></i></div>Dashboard</Link>
            <Link href="/about" className={`nav-link ${SidebarNavClass(pathname, Pages.About)}`}><div className="nav-link-icon"><i data-feather="info"></i></div>About</Link>
            <div className="sidenav-menu-heading">Explore</div>
            <Link href="/subjects" className={`nav-link ${SidebarNavClass(pathname, Pages.Subjects)}`}><div className="nav-link-icon"><i data-feather="folder"></i></div>Instructional Materials</Link>
            <Link href="/software" className={`nav-link ${SidebarNavClass(pathname, Pages.GameAndWebDev)}`}><div className="nav-link-icon"><i data-feather="code"></i></div>Software Projects</Link>
            <Link href="/research" className={`nav-link ${SidebarNavClass(pathname, Pages.ResearchPapers)}`}><div className="nav-link-icon"><i data-feather="file-text"></i></div>Research Papers</Link>
            <Link href="/events" className={`nav-link ${SidebarNavClass(pathname, Pages.Events)}`}><div className="nav-link-icon"><i data-feather="calendar"></i></div>Events</Link>
          </div>
        </div>
      </nav>
    </div>
  )
}
