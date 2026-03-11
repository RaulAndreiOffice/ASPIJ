import { useState } from 'react';
import { motion } from 'framer-motion';
import { FiMail, FiPhone, FiGithub, FiLinkedin, FiSend } from 'react-icons/fi';
import toast from 'react-hot-toast';
import { personalInfo } from '../data/portfolioData';
import '../styles/Contact.css';

const contactCards = [
    { icon: <FiMail size={22} />, label: 'Email', value: personalInfo.email, href: `mailto:${personalInfo.email}` },
    { icon: <FiPhone size={22} />, label: 'Phone', value: personalInfo.phone, href: `tel:${personalInfo.phone}` },
    { icon: <FiGithub size={22} />, label: 'GitHub', value: 'RaulAndreiOffice', href: personalInfo.githubUrl },
];

export default function Contact() {
    const [form, setForm] = useState({ name: '', email: '', message: '' });

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        const { name, email, message } = form;
        if (!name.trim() || !email.trim() || !message.trim()) {
            toast.error('Please fill in all fields.');
            return;
        }
        window.open(
            `mailto:${personalInfo.email}?subject=Portfolio Contact from ${name}&body=${encodeURIComponent(message)}\n\nFrom: ${email}`,
            '_blank'
        );
        toast.success('Opening email client...');
        setForm({ name: '', email: '', message: '' });
    };

    return (
        <section id="contact" className="section contact">
            <div className="container">
                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    whileInView={{ opacity: 1, y: 0 }}
                    viewport={{ once: true }}
                >
                    <h2 className="section-title">Contact</h2>
                    <p className="section-subtitle">Let's get in touch</p>
                </motion.div>

                <div className="contact__layout">
                    <motion.div
                        className="contact__info"
                        initial={{ opacity: 0, x: -40 }}
                        whileInView={{ opacity: 1, x: 0 }}
                        viewport={{ once: true }}
                    >
                        <h3 className="contact__heading">Get In Touch</h3>
                        <p className="contact__text">
                            I'm currently open to new opportunities. Whether you have a question, a project idea,
                            or just want to say hi — my inbox is always open!
                        </p>
                        <div className="contact__cards">
                            {contactCards.map((card) => (
                                <a key={card.label} href={card.href} className="glass-card contact__card" target="_blank" rel="noreferrer">
                                    <div className="contact__card-icon">{card.icon}</div>
                                    <div>
                                        <div className="contact__card-label">{card.label}</div>
                                        <div className="contact__card-value">{card.value}</div>
                                    </div>
                                </a>
                            ))}
                        </div>
                    </motion.div>

                    <motion.form
                        className="glass-card contact__form"
                        onSubmit={handleSubmit}
                        initial={{ opacity: 0, x: 40 }}
                        whileInView={{ opacity: 1, x: 0 }}
                        viewport={{ once: true }}
                        transition={{ delay: 0.1 }}
                    >
                        <div className="contact__field">
                            <label>Your Name</label>
                            <input
                                className="contact__input"
                                value={form.name}
                                onChange={(e) => setForm((f) => ({ ...f, name: e.target.value }))}
                                placeholder="John Doe"
                            />
                        </div>
                        <div className="contact__field">
                            <label>Email Address</label>
                            <input
                                type="email"
                                className="contact__input"
                                value={form.email}
                                onChange={(e) => setForm((f) => ({ ...f, email: e.target.value }))}
                                placeholder="john@example.com"
                            />
                        </div>
                        <div className="contact__field">
                            <label>Message</label>
                            <textarea
                                className="contact__input contact__textarea"
                                rows={5}
                                value={form.message}
                                onChange={(e) => setForm((f) => ({ ...f, message: e.target.value }))}
                                placeholder="Your message here..."
                            />
                        </div>
                        <button type="submit" className="btn btn-primary contact__submit">
                            <FiSend size={16} /> Send Message
                        </button>
                    </motion.form>
                </div>
            </div>
        </section>
    );
}
