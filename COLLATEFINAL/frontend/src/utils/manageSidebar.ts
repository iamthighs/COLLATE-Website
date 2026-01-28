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

export function getActivePageFromPath(pathname: string) {
  if (!pathname || pathname === '/') return Pages.Home

  const segments = pathname.split('/').filter(Boolean)
  const [root, sub] = segments

  if (root === 'category') {
    switch (sub) {
      case 'subjects':
        return Pages.Subjects
      case 'software':
        return Pages.GameAndWebDev
      case 'research':
        return Pages.ResearchPapers
      case 'events':
        return Pages.Events
      default:
        return Pages.Home
    }
  }

  if (root === 'admin') {
    switch (sub) {
      case 'subjects':
        return Pages.CMSPrototype
      case 'software':
        return Pages.CMSGameAndWebDev
      case 'research':
        return Pages.CMSResearchPapers
      case 'events':
        return Pages.CMSEvents
      case 'users':
        return Pages.ListUsers
      case 'roles':
        return Pages.ListRoles
      default:
        return Pages.Home
    }
  }

  const map: Record<string, string> = {
    about: Pages.About,
    dashboard: Pages.Dashboard,
    privacy: Pages.Home,
    terms: Pages.Home,
  }

  return map[root] ?? Pages.Home
}


export function SidebarNavClass(pathname, page){
  const activePage = getActivePageFromPath(pathname)
  return String(activePage).toLowerCase() === String(page).toLowerCase() ? 'active' : ''
}
