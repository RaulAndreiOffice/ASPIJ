import { useState, useEffect } from 'react';
import { FiSun, FiMoon, FiMenu, FiX } from 'react-icons/fi';
import '../styles/Navbar.css';

interface NavbarProps {
    theme: string;
    toggleTheme: () => void;
}

const navLinks = [
    { href: '#hero', label: 'Home' },
    { href: '#about', label: 'About' },
    { href: '#projects', label: 'Projects' },
    { href: '#skills', label: 'Skills' },
    { href: '#education', label: 'Education' },
    { href: '#contact', label: 'Contact' },
];

export default function Navbar({ theme, toggleTheme }: NavbarProps) {
    const [scrolled, setScrolled] = useState(false);
    const [menuOpen, setMenuOpen] = useState(false);
    const [activeSection, setActiveSection] = useState('hero');

    useEffect(() => {
        const onScroll = () => setScrolled(window.scrollY > 40);
        window.addEventListener('scroll', onScroll);
        return () => window.removeEventListener('scroll', onScroll);
    }, []);

    useEffect(() => {
        const observer = new IntersectionObserver(
            (entries) => {
                entries.forEach((entry) => {
                    if (entry.isIntersecting) {
                        setActiveSection(entry.target.id);
                    }
                });
            },
            { threshold: 0.4 }
        );
        navLinks.forEach(({ href }) => {
            const el = document.querySelector(href);
            if (el) observer.observe(el);
        });
        return () => observer.disconnect();
    }, []);

    const handleNavClick = (href: string) => {
        setMenuOpen(false);
        document.querySelector(href)?.scrollIntoView({ behavior: 'smooth' });
    };

    return (
        <nav className={`navbar ${scrolled ? 'navbar--scrolled' : ''}`}>
            <div className="navbar__container">
                <a className="navbar__logo" href="#hero" onClick={() => handleNavClick('#hero')}>
                    <span className="navbar__logo-text">BA</span>
                    <span className="navbar__logo-dot" />
                </a>

                <ul className={`navbar__links ${menuOpen ? 'navbar__links--open' : ''}`}>
                    {navLinks.map(({ href, label }) => (
                        <li key={href}>
                            <button
                                className={`navbar__link ${activeSection === href.slice(1) ? 'navbar__link--active' : ''}`}
                                onClick={() => handleNavClick(href)}
                            >
                                {label}
                            </button>
                        </li>
                    ))}
                </ul>

                <div className="navbar__actions">
                    <button className="navbar__theme-btn" onClick={toggleTheme} aria-label="Toggle theme">
                        {theme === 'dark' ? <FiSun size={19} /> : <FiMoon size={19} />}
                    </button>
                    <button
                        className="navbar__burger"
                        onClick={() => setMenuOpen((o) => !o)}
                        aria-label="Toggle menu"
                    >
                        {menuOpen ? <FiX size={22} /> : <FiMenu size={22} />}
                    </button>
                </div>
            </div>
        </nav>
    );
}
