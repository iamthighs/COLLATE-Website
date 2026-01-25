export const Pages = {
  Home: 'Home',
  About: 'About',
  ListRoles: 'ListRoles',
  ListUsers: 'ListUsers',
  Administration: 'Administration',
  Dashboard: 'Dashboard',
  Prototype: 'Prototype',
  GameAndWebDev: 'GameAndWebDev',
  ResearchPapers: 'ResearchPapers',
  Events: 'Events',
  CMSPrototype: 'CMSPrototype',
  CMSGameAndWebDev: 'CMSGameAndWebDev',
  CMSResearchPapers: 'CMSResearchPapers',
  CMSEvents: 'CMSEvents',
  Subjects: 'Subjects',
  Projects: 'Projects'
}

// Derive a logical active page name from the current pathname.
export function getActivePageFromPath(pathname){
  if(!pathname) return Pages.Home
  if(pathname === '/' || pathname === '') return Pages.Home
  const seg = pathname.split('/').filter(Boolean)[0] || ''
  const map = {
    '': Pages.Home,
    'about': Pages.About,
    'subjects': Pages.Subjects,
    'projects': Pages.Projects,
    'dashboard': Pages.Dashboard,
    'prototype': Pages.Prototype,
    'software': Pages.GameAndWebDev,
    'research': Pages.ResearchPapers,
    'events': Pages.Events,
    'admin': Pages.Administration,
    'administration': Pages.Administration
  }
  return map[seg] || (seg.charAt(0).toUpperCase() + seg.slice(1))
}

// Mirror the ASP.NET helper: returns 'active' when page matches the active page.
export function SidebarNavClass(pathname, page){
  const activePage = getActivePageFromPath(pathname)
  return String(activePage).toLowerCase() === String(page).toLowerCase() ? 'active' : ''
}
