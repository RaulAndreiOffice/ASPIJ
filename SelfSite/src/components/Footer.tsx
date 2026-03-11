import { FiGithub, FiMail, FiHeart } from 'react-icons/fi';
import { personalInfo } from '../data/portfolioData';
import '../styles/Footer.css';

export default function Footer() {
    const year = new Date().getFullYear();
    return (
        <footer className="footer">
            <div className="footer__container">
                <div className="footer__logo">BA<span className="footer__logo-dot">.</span></div>
                <p className="footer__text">
                    Made with <FiHeart className="footer__heart" /> by {personalInfo.firstName} {personalInfo.lastName} · {year}
                </p>
                <div className="footer__socials">
                    <a href={personalInfo.githubUrl} target="_blank" rel="noreferrer" className="footer__social">
                        <FiGithub size={20} />
                    </a>
                    <a href={`mailto:${personalInfo.email}`} className="footer__social">
                        <FiMail size={20} />
                    </a>
                </div>
            </div>
        </footer>
    );
}
