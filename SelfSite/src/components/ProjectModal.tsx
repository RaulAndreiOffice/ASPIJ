import { useState, useRef, KeyboardEvent } from 'react';
import { motion } from 'framer-motion';
import { FiX, FiPlus } from 'react-icons/fi';
import { Project } from '../types';
import '../styles/ProjectModal.css';

interface Props {
    onClose: () => void;
    onSave: (project: Project) => void;
    initialData?: Project | null;
}

const emptyProject: Omit<Project, 'id'> = {
    title: '',
    description: '',
    bullets: [],
    techStack: [],
    githubUrl: '',
    liveUrl: '',
    imageUrl: '',
};

export default function ProjectModal({ onClose, onSave, initialData }: Props) {
    const [form, setForm] = useState<Omit<Project, 'id'>>(() => ({
        ...emptyProject,
        ...(initialData ? {
            title: initialData.title,
            description: initialData.description,
            bullets: [...initialData.bullets],
            techStack: [...initialData.techStack],
            githubUrl: initialData.githubUrl || '',
            liveUrl: initialData.liveUrl || '',
            imageUrl: initialData.imageUrl || '',
        } : {}),
    }));

    const [bulletInput, setBulletInput] = useState('');
    const [techInput, setTechInput] = useState('');
    const [errors, setErrors] = useState<Record<string, string>>({});
    const bulletRef = useRef<HTMLInputElement>(null);
    const techRef = useRef<HTMLInputElement>(null);

    const validate = () => {
        const e: Record<string, string> = {};
        if (!form.title.trim()) e.title = 'Title is required.';
        if (!form.description.trim()) e.description = 'Description is required.';
        setErrors(e);
        return Object.keys(e).length === 0;
    };

    const addBullet = () => {
        const val = bulletInput.trim();
        if (val) {
            setForm((f) => ({ ...f, bullets: [...f.bullets, val] }));
            setBulletInput('');
            bulletRef.current?.focus();
        }
    };

    const removeBullet = (i: number) =>
        setForm((f) => ({ ...f, bullets: f.bullets.filter((_, idx) => idx !== i) }));

    const addTech = () => {
        const val = techInput.trim();
        if (val && !form.techStack.includes(val)) {
            setForm((f) => ({ ...f, techStack: [...f.techStack, val] }));
            setTechInput('');
            techRef.current?.focus();
        }
    };

    const removeTech = (t: string) =>
        setForm((f) => ({ ...f, techStack: f.techStack.filter((x) => x !== t) }));

    const handleKeyDown = (e: KeyboardEvent, fn: () => void) => {
        if (e.key === 'Enter') { e.preventDefault(); fn(); }
    };

    const handleSubmit = () => {
        if (!validate()) return;
        onSave({ ...form, id: '' });
    };

    return (
        <motion.div
            className="modal-overlay"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            onClick={onClose}
        >
            <motion.div
                className="modal"
                initial={{ scale: 0.9, opacity: 0, y: 30 }}
                animate={{ scale: 1, opacity: 1, y: 0 }}
                exit={{ scale: 0.9, opacity: 0, y: 30 }}
                transition={{ type: 'spring', damping: 25, stiffness: 300 }}
                onClick={(e) => e.stopPropagation()}
            >
                <div className="modal__header">
                    <h3 className="modal__title">{initialData ? 'Edit Project' : 'Add New Project'}</h3>
                    <button className="modal__close" onClick={onClose}><FiX size={20} /></button>
                </div>

                <div className="modal__body">
                    <div className="modal__field">
                        <label>Project Title <span className="modal__required">*</span></label>
                        <input
                            className={`modal__input ${errors.title ? 'modal__input--error' : ''}`}
                            value={form.title}
                            onChange={(e) => setForm((f) => ({ ...f, title: e.target.value }))}
                            placeholder="e.g. E-Commerce App"
                        />
                        {errors.title && <span className="modal__error-msg">{errors.title}</span>}
                    </div>

                    <div className="modal__field">
                        <label>Description <span className="modal__required">*</span></label>
                        <textarea
                            className={`modal__input modal__textarea ${errors.description ? 'modal__input--error' : ''}`}
                            value={form.description}
                            onChange={(e) => setForm((f) => ({ ...f, description: e.target.value }))}
                            placeholder="Short description of the project..."
                            rows={3}
                        />
                        {errors.description && <span className="modal__error-msg">{errors.description}</span>}
                    </div>

                    <div className="modal__field">
                        <label>Key Points (bullet list)</label>
                        <div className="modal__list-input">
                            <input
                                ref={bulletRef}
                                className="modal__input"
                                value={bulletInput}
                                onChange={(e) => setBulletInput(e.target.value)}
                                onKeyDown={(e) => handleKeyDown(e, addBullet)}
                                placeholder="Type a key point and press Enter..."
                            />
                            <button className="modal__add-btn" onClick={addBullet}><FiPlus size={18} /></button>
                        </div>
                        {form.bullets.length > 0 && (
                            <ul className="modal__chip-list modal__chip-list--bullets">
                                {form.bullets.map((b, i) => (
                                    <li key={i} className="modal__chip">
                                        {b}
                                        <button onClick={() => removeBullet(i)}><FiX size={12} /></button>
                                    </li>
                                ))}
                            </ul>
                        )}
                    </div>

                    <div className="modal__field">
                        <label>Tech Stack</label>
                        <div className="modal__list-input">
                            <input
                                ref={techRef}
                                className="modal__input"
                                value={techInput}
                                onChange={(e) => setTechInput(e.target.value)}
                                onKeyDown={(e) => handleKeyDown(e, addTech)}
                                placeholder="Type a technology and press Enter..."
                            />
                            <button className="modal__add-btn" onClick={addTech}><FiPlus size={18} /></button>
                        </div>
                        {form.techStack.length > 0 && (
                            <div className="modal__chip-list">
                                {form.techStack.map((t) => (
                                    <span key={t} className="modal__chip modal__chip--tag">
                                        {t}
                                        <button onClick={() => removeTech(t)}><FiX size={12} /></button>
                                    </span>
                                ))}
                            </div>
                        )}
                    </div>

                    <div className="modal__row">
                        <div className="modal__field">
                            <label>GitHub URL</label>
                            <input
                                className="modal__input"
                                value={form.githubUrl}
                                onChange={(e) => setForm((f) => ({ ...f, githubUrl: e.target.value }))}
                                placeholder="https://github.com/..."
                            />
                        </div>
                        <div className="modal__field">
                            <label>Live URL</label>
                            <input
                                className="modal__input"
                                value={form.liveUrl}
                                onChange={(e) => setForm((f) => ({ ...f, liveUrl: e.target.value }))}
                                placeholder="https://..."
                            />
                        </div>
                    </div>
                </div>

                <div className="modal__footer">
                    <button className="btn btn-outline" onClick={onClose}>Cancel</button>
                    <button className="btn btn-primary" onClick={handleSubmit}>
                        {initialData ? 'Save Changes' : 'Add Project'}
                    </button>
                </div>
            </motion.div>
        </motion.div>
    );
}
