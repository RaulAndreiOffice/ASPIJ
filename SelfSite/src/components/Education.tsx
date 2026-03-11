import { motion } from 'framer-motion';
import { education, languages } from '../data/portfolioData';
import '../styles/Education.css';

export default function Education() {
    return (
        <section id="education" className="section education">
            <div className="container">
                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    whileInView={{ opacity: 1, y: 0 }}
                    viewport={{ once: true }}
                >
                    <h2 className="section-title">Education</h2>
                    <p className="section-subtitle">My academic background</p>
                </motion.div>

                <div className="education__layout">
                    <div className="education__timeline">
                        {education.map((edu, i) => (
                            <motion.div
                                key={i}
                                className="glass-card education__card"
                                initial={{ opacity: 0, x: -40 }}
                                whileInView={{ opacity: 1, x: 0 }}
                                viewport={{ once: true }}
                                transition={{ delay: i * 0.15 }}
                            >
                                <div className="education__card-header">
                                    <div className="education__dot" />
                                    <div>
                                        <h3 className="education__institution">{edu.institution}</h3>
                                        <p className="education__degree">{edu.degree}</p>
                                        <p className="education__major">Major: {edu.major}</p>
                                    </div>
                                    <div className="education__period">{edu.period}</div>
                                </div>
                                <div className="education__location">📍 {edu.location}</div>
                                <div className="education__coursework">
                                    <span className="education__coursework-label">Relevant Coursework:</span>
                                    <div className="education__coursework-tags">
                                        {edu.coursework.map((c) => (
                                            <span key={c} className="tag">{c}</span>
                                        ))}
                                    </div>
                                </div>
                            </motion.div>
                        ))}
                    </div>

                    <motion.div
                        className="glass-card education__languages-card"
                        initial={{ opacity: 0, x: 40 }}
                        whileInView={{ opacity: 1, x: 0 }}
                        viewport={{ once: true }}
                        transition={{ delay: 0.2 }}
                    >
                        <h3 className="education__lang-title">🌐 Languages</h3>
                        <div className="education__lang-list">
                            {languages.map((lang) => (
                                <div key={lang.name} className="education__lang-item">
                                    <span className="education__lang-name">{lang.name}</span>
                                    <span className="education__lang-level">{lang.level}</span>
                                </div>
                            ))}
                        </div>
                    </motion.div>
                </div>
            </div>
        </section>
    );
}
