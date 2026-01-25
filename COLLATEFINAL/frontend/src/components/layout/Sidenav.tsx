"use client"

import Link from 'next/link'
import { usePathname } from 'next/navigation'
import { SidebarNavClass, Pages } from '../../utils/manageSidebar'
import { useEffect, useState } from 'react'
import { supabase } from '../../lib/supabase/supabaseClient'

export default function Sidenav(){
  const pathname = usePathname()
  const [user, setUser] = useState(null)
  const [profile, setProfile] = useState(null)

  useEffect(() => {
    const fetchUser = async () => {
      const { data: { session } } = await supabase.auth.getSession()
      if (!session?.user) return

      setUser(session.user)

      const { data: profileData } = await supabase
        .from('profiles')
        .select('*')
        .eq('id', session.user.id)
        .single()

      setProfile(profileData)
    }

    fetchUser()

    // Listen for login/logout
    const { data: listener } = supabase.auth.onAuthStateChange((_event, session) => {
      if (!session?.user) {
        setUser(null)
        setProfile(null)
      } else {
        supabase
          .from('profiles')
          .select('*')
          .eq('id', session.user.id)
          .single()
          .then(({ data }) => setProfile(data))
      }
    })

    return () => listener.subscription.unsubscribe()
  }, [])

  return (
    <div id="layoutSidenav_nav">
      <nav className="sidenav shadow-right sidenav-light d-flex flex-column" style={{height: '100%'}}>
        <div className="sidenav-menu flex-grow-1">
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
