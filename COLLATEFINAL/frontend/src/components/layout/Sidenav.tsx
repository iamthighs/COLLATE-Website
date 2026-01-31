"use client"

import Link from 'next/link'
import { usePathname } from 'next/navigation'
import { SidebarNavClass, Pages } from '../../utils/manageSidebar'
import { useAuth } from "../../context/AuthContext";
import SidenavSkeleton from './SidenavSkeleton';


interface NavLink {
  href: string;
  label: string;
  icon: string;
  page: string; 
}

interface NavSection {
  heading: string;
  headingClass?: string; 
  links: NavLink[];
}

const navSections: NavSection[] = [
  {
    heading: "Administration",
    headingClass: "text-danger",
    links: [
      { href: "/admin/roles", label: "Roles", icon: "settings", page: "ListRoles" },
      { href: "/admin/users", label: "Users", icon: "users", page: "ListUsers" },
    ],
  },
  {
    heading: "Content Management",
    headingClass: "text-danger",
    links: [
      { href: "/admin/subjects", label: "Instructional Materials", icon: "folder", page: "CMSPrototype" },
      { href: "/admin/software", label: "Software Projects", icon: "code", page: "CMSGameAndWebDev" },
      { href: "/admin/research", label: "Research Papers", icon: "file-text", page: "CMSResearchPapers" },
      { href: "/admin/events", label: "Events", icon: "calendar", page: "CMSEvents" },
    ],
  },
  {
    heading: "Overview",
    links: [
      { href: "/", label: "Home", icon: "home", page: "Home" },
      { href: "/dashboard", label: "Dashboard", icon: "activity", page: "Dashboard" },
      { href: "/about", label: "About", icon: "info", page: "About" },
    ],
  },
  {
    heading: "Explore",
    links: [
      { href: "/category/subjects", label: "Instructional Materials", icon: "folder", page: "Subjects" },
      { href: "/category/software", label: "Software Projects", icon: "code", page: "GameAndWebDev" },
      { href: "/category/research", label: "Research Papers", icon: "file-text", page: "ResearchPapers" },
      { href: "/category/events", label: "Events", icon: "calendar", page: "Events" },
    ],
  },
];

export default function Sidenav(){
  
  const pathname = usePathname()
  const { user, profile, loading } = useAuth()

    if (loading) return <SidenavSkeleton />;
    const filteredSections = navSections.filter((section) => {
    if (section.heading === "Administration" || section.heading === "Content Management") {
      return !!user; 
    }
    return true; 
  });
  return (
    <div id="layoutSidenav_nav">
      <nav className="sidenav shadow-right sidenav-light d-flex flex-column" style={{height: '100%'}}>
        <div className="sidenav-menu flex-grow-1">
          <div className="nav accordion" id="accordionSidenav">
            {filteredSections.map((section) => (
              <div key={section.heading}>
                <div className={`sidenav-menu-heading ${section.headingClass || ""}`}>
                  {section.heading}
                </div>
                {section.links.map((link) => (
                  <Link
                    key={link.href}
                    href={link.href}
                    className={`nav-link ${SidebarNavClass(pathname, link.page)}`}
                  >
                    <div className="nav-link-icon">
                      <i data-feather={link.icon}></i>
                    </div>
                    {link.label}
                  </Link>
                ))}
              </div>
            ))}
          </div>
        </div>

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
