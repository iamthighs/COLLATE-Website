export interface ResearchPaper {
  id: string;
  header: string;
  title: string;
  postedDate: string;
}

export interface SoftwareProject {
  id: string;
  header: string;
  title: string;
  postedDate: string;
}

export interface Subject {
  id: string;
  header: string;
  title: string;
  postedDate: string;
}

export interface Event {
  id: string;
  header: string;
  title: string;
  postedDate: string;
}

export interface Role {
  id: string;
  header: string;
  title: string;
  postedDate: string;
}

export interface User {
  id: string;
  header: string;
  title: string;
  postedDate: string;
}

// Mock data

export const researchPapers: ResearchPaper[] = [
  { id: "1", header: "Computer Science", title: "AI in Modern Web Applications", postedDate: "2024-10-01" },
  { id: "2", header: "Information Systems", title: "Scalable CMS Architecture", postedDate: "2024-09-15" },
  { id: "3", header: "Cybersecurity", title: "Blockchain Security Analysis", postedDate: "2024-08-10" },
];

export const softwareProjects: SoftwareProject[] = [
  { id: "1", header: "Web Development", title: "Next.js E-commerce Platform", postedDate: "2024-10-05" },
  { id: "2", header: "Mobile Apps", title: "React Native Fitness Tracker", postedDate: "2024-09-20" },
  { id: "3", header: "AI", title: "Chatbot Using GPT API", postedDate: "2024-07-30" },
];

export const subjects: Subject[] = [
  { id: "1", header: "Mathematics", title: "Calculus 101", postedDate: "2024-01-15" },
  { id: "2", header: "Physics", title: "Introduction to Quantum Mechanics", postedDate: "2024-02-10" },
  { id: "3", header: "Computer Science", title: "Data Structures & Algorithms", postedDate: "2024-03-05" },
];

export const events: Event[] = [
  { id: "1", header: "Conference", title: "Tech Innovators 2024", postedDate: "2024-05-12" },
  { id: "2", header: "Workshop", title: "React Masterclass", postedDate: "2024-06-08" },
  { id: "3", header: "Seminar", title: "AI & Ethics Panel", postedDate: "2024-07-25" },
];

export const roles: Role[] = [
  { id: "1", header: "Admin", title: "Site Administrator", postedDate: "2023-11-01" },
  { id: "2", header: "Editor", title: "Content Editor", postedDate: "2023-12-10" },
  { id: "3", header: "Viewer", title: "Read-Only User", postedDate: "2024-01-05" },
];

export const users: User[] = [
  { id: "1", header: "Admin", title: "Alice Johnson", postedDate: "2024-01-15" },
  { id: "2", header: "Editor", title: "Bob Smith", postedDate: "2024-02-20" },
  { id: "3", header: "Viewer", title: "Charlie Lee", postedDate: "2024-03-30" },
];
