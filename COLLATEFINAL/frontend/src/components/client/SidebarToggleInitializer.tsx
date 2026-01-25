"use client"
import { useEffect } from 'react'
import { usePathname } from 'next/navigation'

export default function SidebarToggleInitializer() {
  const pathname = usePathname()

  useEffect(() => {
  const applyStored = () => {
    try {
      const stored = localStorage.getItem('sb|sidebar-toggle')
      document.body.classList.toggle('sidenav-toggled', stored === 'true')
    } catch {}
  }

  applyStored()

  // 🔑 re-apply AFTER all scripts & layout effects
  requestAnimationFrame(() => {
    setTimeout(applyStored, 0)
  })

  const docHandler = (e) => {
    const toggle = e.target?.closest?.('#sidebarToggle')
    if (!toggle) return

    e.preventDefault()

    document.body.classList.toggle('sidenav-toggled')

    try {
      localStorage.setItem(
        'sb|sidebar-toggle',
        document.body.classList.contains('sidenav-toggled') ? 'true' : 'false'
      )
    } catch {}
  }

  document.addEventListener('click', docHandler)

  return () => {
    document.removeEventListener('click', docHandler)
  }
}, []) // 🔑 run once


  return null
}
