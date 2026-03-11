import { useState, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { FiPlus, FiGithub, FiExternalLink, FiEdit2, FiTrash2 } from 'react-icons/fi';
import toast from 'react-hot-toast';
import { Project } from '../types';
import { seedProjects } from '../data/portfolioData';
import ProjectModal from './ProjectModal';
import '../styles/Projects.css';

const STORAGE_KEY = 'portfolio_user_projects';

function loadUserProjects(): Project[] {
    try {
        return JSON.parse(localStorage.getItem(STORAGE_KEY) || '[]');
    } catch {
        return [];
    }
}

export default function Projects() {
    const [userProjects, setUserProjects] = useState<Project[]>(loadUserProjects);
    const [modalOpen, setModalOpen] = useState(false);
    const [editingProject, setEditingProject] = useState<Project | null>(null);

    const allProjects = [...seedProjects, ...userProjects];

    useEffect(() => {
        localStorage.setItem(STORAGE_KEY, JSON.stringify(userProjects));
    }, [userProjects]);

    const handleSave = (project: Project) => {
        if (editingProject) {
            setUserProjects((prev) =>
                prev.map((p) => (p.id === editingProject.id ? { ...project, id: editingProject.id, isUserAdded: true } : p))
            );
            toast.success('Project updated!');
        } else {
            const newProject: Project = { ...project, id: `user_${Date.now()}`, isUserAdded: true };
            setUserProjects((prev) => [...prev, newProject]);
            toast.success('Project added!');
        }
        setModalOpen(false);
        setEditingProject(null);
    };

    const handleEdit = (project: Project) => {
        setEditingProject(project);
        setModalOpen(true);
    };

    const handleDelete = (id: string) => {
        setUserProjects((prev) => prev.filter((p) => p.id !== id));
        toast.success('Project removed.');
    };

    const openAddModal = () => {
        setEditingProject(null);
        setModalOpen(true);
    };

    return (
        <section id="projects" className="section projects">
            <div className="container">
                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    whileInView={{ opacity: 1, y: 0 }}
                    viewport={{ once: true }}
                >
                    <h2 className="section-title">Projects</h2>
                    <p className="section-subtitle">Things I've built</p>
                </motion.div>

                <div className="projects__grid">
                    {allProjects.map((project, i) => (
                        <motion.div
                            className="glass-card projects__card"
                            key={project.id}
                            initial={{ opacity: 0, y: 30 }}
                            whileInView={{ opacity: 1, y: 0 }}
                            viewport={{ once: true }}
                            transition={{ delay: i * 0.1 }}
                        >
                            {project.featured && <div className="projects__badge">Featured</div>}

                            <div className="projects__card-header">
                                <h3 className="projects__card-title">{project.title}</h3>
                                <div className="projects__card-actions">
                                    {project.githubUrl && (
                                        <a href={project.githubUrl} target="_blank" rel="noreferrer" className="projects__icon-btn" title="GitHub">
                                            <FiGithub size={18} />
                                        </a>
                                    )}
                                    {project.liveUrl && (
                                        <a href={project.liveUrl} target="_blank" rel="noreferrer" className="projects__icon-btn" title="Live Demo">
                                            <FiExternalLink size={18} />
                                        </a>
                                    )}
                                    {project.isUserAdded && (
                                        <>
                                            <button className="projects__icon-btn projects__icon-btn--edit" onClick={() => handleEdit(project)} title="Edit">
                                                <FiEdit2 size={16} />
                                            </button>
                                            <button className="projects__icon-btn projects__icon-btn--delete" onClick={() => handleDelete(project.id)} title="Delete">
                                                <FiTrash2 size={16} />
                                            </button>
                                        </>
                                    )}
                                </div>
                            </div>

                            <p className="projects__card-desc">{project.description}</p>

                            {project.bullets.length > 0 && (
                                <ul className="projects__bullets">
                                    {project.bullets.map((b, j) => (
                                        <li key={j}>{b}</li>
                                    ))}
                                </ul>
                            )}

                            <div className="projects__tags">
                                {project.techStack.map((tech) => (
                                    <span key={tech} className="tag">{tech}</span>
                                ))}
                            </div>
                        </motion.div>
                    ))}

                    {/* Add Project Card */}
                    <motion.button
                        className="projects__add-card"
                        onClick={openAddModal}
                        initial={{ opacity: 0, y: 30 }}
                        whileInView={{ opacity: 1, y: 0 }}
                        viewport={{ once: true }}
                        transition={{ delay: allProjects.length * 0.1 }}
                        whileHover={{ scale: 1.02 }}
                    >
                        <FiPlus size={40} className="projects__add-icon" />
                        <span>Add Your Project</span>
                    </motion.button>
                </div>

                <AnimatePresence>
                    {modalOpen && (
                        <ProjectModal
                            onClose={() => { setModalOpen(false); setEditingProject(null); }}
                            onSave={handleSave}
                            initialData={editingProject}
                        />
                    )}
                </AnimatePresence>
            </div>
        </section>
    );
}
