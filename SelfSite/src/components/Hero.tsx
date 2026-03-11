import { motion } from 'framer-motion';
import { FiGithub, FiMail, FiDownload, FiChevronDown } from 'react-icons/fi';
import { personalInfo } from '../data/portfolioData';
import '../styles/Hero.css';

export default function Hero() {
    return (
        <section id="hero" className="hero">
            <div className="hero__bg">
                {[...Array(20)].map((_, i) => (
                    <div key={i} className="hero__particle" style={{ '--i': i } as React.CSSProperties} />
                ))}
            </div>

            <div className="hero__content">
                <motion.div
                    initial={{ opacity: 0, y: 30 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 0.7, ease: 'easeOut' }}
                >
                    <div className="hero__greeting">👋 Hello, I'm</div>
                    <h1 className="hero__name">
                        <span className="hero__name-first">{personalInfo.firstName} </span>
                        <span className="hero__name-last">{personalInfo.lastName}</span>
                    </h1>
                    <p className="hero__tagline">{personalInfo.tagline}</p>
                    <p className="hero__location">📍 {personalInfo.city}, {personalInfo.country}</p>

                    <div className="hero__cta">
                        <a href="#projects" className="btn btn-primary">
                            View Projects
                        </a>
                        <a href={`mailto:${personalInfo.email}`} className="btn btn-outline">
                            <FiMail size={16} /> Contact Me
                        </a>
                    </div>

                    <div className="hero__socials">
                        <a href={personalInfo.githubUrl} target="_blank" rel="noreferrer" className="hero__social-link">
                            <FiGithub size={22} />
                        </a>
                        <a href={`mailto:${personalInfo.email}`} className="hero__social-link">
                            <FiMail size={22} />
                        </a>
                        {personalInfo.cvUrl && (
                            <a href={personalInfo.cvUrl} download className="hero__social-link">
                                <FiDownload size={22} />
                            </a>
                        )}
                    </div>
                </motion.div>
            </div>

            <motion.a
                href="#about"
                className="hero__scroll"
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                transition={{ delay: 1.2 }}
            >
                <FiChevronDown size={28} />
            </motion.a>
        </section>
    );
}
