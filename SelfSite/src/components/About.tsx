import { motion } from 'framer-motion';
import { FiCode, FiCpu, FiBriefcase } from 'react-icons/fi';
import { personalInfo } from '../data/portfolioData';
import '../styles/About.css';

const highlights = [
    { icon: <FiCode size={22} />, label: 'Full-Stack Dev', desc: 'React, Spring Boot, .NET 8' },
    { icon: <FiCpu size={22} />, label: 'IoT & Embedded', desc: 'C++, MQTT, Raspberry Pi' },
    { icon: <FiBriefcase size={22} />, label: 'AI & ML', desc: 'TensorFlow, LLMs, LSTM' },
];

export default function About() {
    return (
        <section id="about" className="section about">
            <div className="container">
                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    whileInView={{ opacity: 1, y: 0 }}
                    viewport={{ once: true }}
                    transition={{ duration: 0.6 }}
                >
                    <h2 className="section-title">About Me</h2>
                    <p className="section-subtitle">A quick look at who I am</p>
                </motion.div>

                <div className="about__grid">
                    <motion.div
                        className="about__avatar-wrapper"
                        initial={{ opacity: 0, x: -40 }}
                        whileInView={{ opacity: 1, x: 0 }}
                        viewport={{ once: true }}
                        transition={{ duration: 0.7 }}
                    >
                        <div className="about__avatar">
                            <span className="about__avatar-initials">BA</span>
                            <div className="about__avatar-ring" />
                        </div>
                        <div className="about__info-pills">
                            <span className="about__pill">📍 {personalInfo.city}</span>
                            <span className="about__pill">🎓 Systems Engineering</span>
                            <span className="about__pill">💼 Open to Work</span>
                        </div>
                    </motion.div>

                    <motion.div
                        className="about__text"
                        initial={{ opacity: 0, x: 40 }}
                        whileInView={{ opacity: 1, x: 0 }}
                        viewport={{ once: true }}
                        transition={{ duration: 0.7, delay: 0.1 }}
                    >
                        <h3 className="about__heading">Hi, I'm <strong>{personalInfo.firstName} {personalInfo.lastName}</strong></h3>
                        <p className="about__bio">{personalInfo.bio}</p>

                        <div className="about__highlights">
                            {highlights.map((h, i) => (
                                <motion.div
                                    key={h.label}
                                    className="glass-card about__highlight-card"
                                    initial={{ opacity: 0, y: 20 }}
                                    whileInView={{ opacity: 1, y: 0 }}
                                    viewport={{ once: true }}
                                    transition={{ delay: 0.2 + i * 0.1 }}
                                >
                                    <div className="about__highlight-icon">{h.icon}</div>
                                    <div>
                                        <div className="about__highlight-label">{h.label}</div>
                                        <div className="about__highlight-desc">{h.desc}</div>
                                    </div>
                                </motion.div>
                            ))}
                        </div>

                        <div className="about__contacts">
                            <a href={`mailto:${personalInfo.email}`} className="about__contact-item">
                                ✉️ {personalInfo.email}
                            </a>
                            <a href={`tel:${personalInfo.phone}`} className="about__contact-item">
                                📞 {personalInfo.phone}
                            </a>
                        </div>
                    </motion.div>
                </div>
            </div>
        </section>
    );
}
