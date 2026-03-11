import { Education, Language, PersonalInfo, Project, Skill } from '../types';

export const personalInfo: PersonalInfo = {
    firstName: 'Bota',
    lastName: 'Andrei',
    tagline: 'Full-Stack Developer & Embedded Systems Engineer',
    bio: "I'm a Systems Engineering student at the Technical University of Cluj-Napoca, passionate about building full-stack web applications, IoT systems, and AI-powered solutions. I love turning complex problems into clean, reliable software.",
    phone: '+40756776340',
    email: 'raulbota360@gmail.com',
    city: 'Cluj-Napoca',
    country: 'Romania',
    githubUrl: 'https://github.com/RaulAndreiOffice',
};

export const seedProjects: Project[] = [
    {
        id: 'meditatio',
        title: 'Meditatio',
        description: 'A substitute for private tutors helping children do their homework at home, with AI assistance for teachers to verify schoolers\' homework remotely.',
        bullets: [
            'Created to help children do their homeworks at home as a substitute for private tutors.',
            'An innovation for teachers who want to be more flexible when verifying students\' homework from home.',
            'Backend built with Spring Boot (Java); frontend built with React + TypeScript.',
            'AI integration using Llama 3 (LLM) to answer questions about the topic loaded by the child.',
        ],
        techStack: ['Java', 'Spring Boot', 'React', 'TypeScript', 'LLM', 'Llama 3'],
        githubUrl: 'https://github.com/RaulAndreiOffice/Meditatio.git',
        featured: true,
    },
    {
        id: 'aspij',
        title: 'Sports Performance & Injury Prevention System',
        description: "Bachelor's Thesis — Full-stack IoT system to monitor athlete biometrics in real time and predict overtraining risks using an LSTM neural network.",
        bullets: [
            'Full-Stack: Developed an IoT system to monitor biometrics and predict overtraining risks.',
            'Backend: Built a .NET 8 REST API with PostgreSQL for high-frequency data persistence.',
            'Real-time UI: Created a React dashboard using SignalR for live Pulse/SpO2 visualization.',
            'AI Module: Implemented an LSTM neural network in Python (TensorFlow) for biometric anomaly detection.',
            'IoT: Developed Raspberry Pi Pico firmware (C++) for sensor interfacing and data transmission.',
        ],
        techStack: ['.NET 8', 'React', 'PostgreSQL', 'Python', 'TensorFlow', 'SignalR', 'C++', 'MQTT', 'Raspberry Pi Pico'],
        githubUrl: 'https://github.com/RaulAndreiOffice/ASPIJ.git',
        featured: true,
    },
];

export const skills: Skill[] = [
    { name: 'Java', category: 'Languages' },
    { name: 'Python', category: 'Languages' },
    { name: 'TypeScript', category: 'Languages' },
    { name: 'C/C++', category: 'Languages' },
    { name: 'SQL', category: 'Languages' },
    { name: 'React', category: 'Frameworks' },
    { name: 'Angular', category: 'Frameworks' },
    { name: 'Spring Boot', category: 'Frameworks' },
    { name: '.NET 8', category: 'Frameworks' },
    { name: 'TensorFlow', category: 'AI/ML' },
    { name: 'Large Language Models', category: 'AI/ML' },
    { name: 'LSTM Networks', category: 'AI/ML' },
    { name: 'SignalR', category: 'IoT' },
    { name: 'MQTT', category: 'IoT' },
    { name: 'Raspberry Pi Pico', category: 'IoT' },
    { name: 'Git', category: 'Tools' },
    { name: 'Jira', category: 'Tools' },
    { name: 'Linux', category: 'Tools' },
    { name: 'Matlab', category: 'Tools' },
    { name: 'PostgreSQL', category: 'Tools' },
];

export const education: Education[] = [
    {
        institution: 'Technical University of Cluj-Napoca',
        degree: 'Bachelor of Science in Systems Engineering',
        major: 'Automation and Applied Informatics',
        period: '2022 – 2026',
        location: 'Cluj-Napoca, Romania',
        coursework: [
            'Data Structures & Algorithms',
            'Object-Oriented Programming (Java / C++)',
            'Database Design (SQL)',
            'Software Engineering',
            'Web Development',
            'Operating Systems',
        ],
    },
];

export const languages: Language[] = [
    { name: 'Romanian', level: 'Native' },
    { name: 'English', level: 'Intermediate' },
    { name: 'German', level: 'Beginner' },
];
