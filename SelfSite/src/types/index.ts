export interface Project {
  id: string;
  title: string;
  description: string;
  bullets: string[];
  techStack: string[];
  githubUrl?: string;
  liveUrl?: string;
  imageUrl?: string;
  featured?: boolean;
  isUserAdded?: boolean;
}

export interface Skill {
  name: string;
  category: 'Languages' | 'Frameworks' | 'Tools' | 'AI/ML' | 'IoT';
}

export interface Education {
  institution: string;
  degree: string;
  major: string;
  period: string;
  location: string;
  coursework: string[];
}

export interface Language {
  name: string;
  level: string;
}

export interface PersonalInfo {
  firstName: string;
  lastName: string;
  tagline: string;
  bio: string;
  phone: string;
  email: string;
  city: string;
  country: string;
  githubUrl: string;
  linkedinUrl?: string;
  cvUrl?: string;
}
