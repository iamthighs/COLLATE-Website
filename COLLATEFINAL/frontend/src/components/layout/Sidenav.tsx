"use client"

import Link from 'next/link'
import { usePathname } from 'next/navigation'
import { SidebarNavClass, Pages } from '../../utils/manageSidebar'
import { useAuth } from "../../context/AuthContext";

export default function Sidenav(){
  
  const pathname = usePathname()
  const { user, profile, loading } = useAuth()

  if (loading) return null

  return (
    <div id="layoutSidenav_nav">
      <nav className="sidenav shadow-right sidenav-light d-flex flex-column" style={{height: '100%'}}>
        <div className="sidenav-menu flex-grow-1">
          <div className="nav accordion" id="accordionSidenav">
            <div className="sidenav-menu-heading text-danger">Administration</div>
            <Link href="/admin/roles" className={`nav-link ${SidebarNavClass(pathname, Pages.ListRoles)}`}><div className="nav-link-icon"><i data-feather="settings"></i></div>Roles</Link>
            <Link href="/admin/users" className={`nav-link ${SidebarNavClass(pathname, Pages.ListUsers)}`}><div className="nav-link-icon"><i data-feather="users"></i></div>Users</Link>

            <div className="sidenav-menu-heading text-danger">Content Management</div>
            <Link href="/admin/subjects" className={`nav-link ${SidebarNavClass(pathname, Pages.CMSPrototype)}`}><div className="nav-link-icon"><i data-feather="folder"></i></div>Instructional Materials</Link>
            <Link href="/admin/software" className={`nav-link ${SidebarNavClass(pathname, Pages.CMSGameAndWebDev)}`}><div className="nav-link-icon"><i data-feather="code"></i></div>Software Projects</Link>
            <Link href="/admin/research" className={`nav-link ${SidebarNavClass(pathname, Pages.CMSResearchPapers)}`}><div className="nav-link-icon"><i data-feather="file-text"></i></div>Research Papers</Link>
            <Link href="/admin/events" className={`nav-link ${SidebarNavClass(pathname, Pages.CMSEvents)}`}><div className="nav-link-icon"><i data-feather="calendar"></i></div>Events</Link>

            <div className="sidenav-menu-heading">Overview</div>
            <Link href="/" className={`nav-link ${SidebarNavClass(pathname, Pages.Home)}`}><div className="nav-link-icon"><i data-feather="home"></i></div>Home</Link>
            <Link href="/dashboard" className={`nav-link ${SidebarNavClass(pathname, Pages.Dashboard)}`}><div className="nav-link-icon"><i data-feather="activity"></i></div>Dashboard</Link>
            <Link href="/about" className={`nav-link ${SidebarNavClass(pathname, Pages.About)}`}><div className="nav-link-icon"><i data-feather="info"></i></div>About</Link>

            <div className="sidenav-menu-heading">Explore</div>
            <Link href="/category/subjects" className={`nav-link ${SidebarNavClass(pathname, Pages.Subjects)}`}><div className="nav-link-icon"><i data-feather="folder"></i></div>Instructional Materials</Link>
            <Link href="/category/software" className={`nav-link ${SidebarNavClass(pathname, Pages.GameAndWebDev)}`}><div className="nav-link-icon"><i data-feather="code"></i></div>Software Projects</Link>
            <Link href="/category/research" className={`nav-link ${SidebarNavClass(pathname, Pages.ResearchPapers)}`}><div className="nav-link-icon"><i data-feather="file-text"></i></div>Research Papers</Link>
            <Link href="/category/events" className={`nav-link ${SidebarNavClass(pathname, Pages.Events)}`}><div className="nav-link-icon"><i data-feather="calendar"></i></div>Events</Link>
          </div>
        </div>

        {/* ---------------- Sidenav Footer ---------------- */}
        {user && profile && (
          <div className="sidenav-footer mt-auto p-3 border-top">
            <div className="sidenav-footer-content">
              <div className="sidenav-footer-subtitle">Logged in as:</div>
              <div className="sidenav-footer-title">
                {profile.first_name} {profile.last_name}
              </div>
            </div>
          </div>
        )}
      </nav>
    </div>
  )
}
