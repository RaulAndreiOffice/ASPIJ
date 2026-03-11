import { motion } from 'framer-motion';
import { skills } from '../data/portfolioData';
import '../styles/Skills.css';

const categories = ['Languages', 'Frameworks', 'AI/ML', 'IoT', 'Tools'] as const;

const categoryColors: Record<string, string> = {
    Languages: '#6366f1',
    Frameworks: '#8b5cf6',
    'AI/ML': '#06b6d4',
    IoT: '#10b981',
    Tools: '#f59e0b',
};

export default function Skills() {
    return (
        <section id="skills" className="section skills">
            <div className="container">
                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    whileInView={{ opacity: 1, y: 0 }}
                    viewport={{ once: true }}
                >
                    <h2 className="section-title">Skills</h2>
                    <p className="section-subtitle">Technologies I work with</p>
                </motion.div>

                <div className="skills__grid">
                    {categories.map((cat, ci) => {
                        const catSkills = skills.filter((s) => s.category === cat);
                        if (!catSkills.length) return null;
                        return (
                            <motion.div
                                key={cat}
                                className="glass-card skills__category"
                                initial={{ opacity: 0, y: 30 }}
                                whileInView={{ opacity: 1, y: 0 }}
                                viewport={{ once: true }}
                                transition={{ delay: ci * 0.1 }}
                            >
                                <div className="skills__category-header">
                                    <div
                                        className="skills__category-dot"
                                        style={{ background: categoryColors[cat] }}
                                    />
                                    <h3 className="skills__category-title">{cat}</h3>
                                </div>
                                <div className="skills__tags">
                                    {catSkills.map((skill, si) => (
                                        <motion.span
                                            key={skill.name}
                                            className="skills__tag"
                                            style={{ '--color': categoryColors[cat] } as React.CSSProperties}
                                            initial={{ opacity: 0, scale: 0.8 }}
                                            whileInView={{ opacity: 1, scale: 1 }}
                                            viewport={{ once: true }}
                                            transition={{ delay: ci * 0.1 + si * 0.05 }}
                                            whileHover={{ scale: 1.08 }}
                                        >
                                            {skill.name}
                                        </motion.span>
                                    ))}
                                </div>
                            </motion.div>
                        );
                    })}
                </div>
            </div>
        </section>
    );
}
