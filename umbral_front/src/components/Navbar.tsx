"use client";

import Link from 'next/link';
import { usePathname } from 'next/navigation';
import styles from './Navbar.module.css';

export default function Navbar() {
  const pathname = usePathname();

  return (
    <nav className={styles.navbar}>
      <Link href="/trivias" className={styles.logo}>
        🌌 UMBRAL ACADÉMICO
      </Link>
      
      <div className={styles.navLinks}>
        <Link 
          href="/trivias" 
          className={`${styles.link} ${pathname === '/trivias' ? styles.activeLink : ''}`}
        >
          📚 Mis Trivias
        </Link>
        <Link 
          href="/sessions/create" 
          className={`${styles.link} ${pathname.startsWith('/sessions/create') ? styles.activeLink : ''}`}
        >
          🚀 Lanzar Trivia
        </Link>

        {/* ⚡ BOTÓN DE ACCESO DIRECTO PARA DESARROLLO */}
        <Link 
          href="/sessions/waiting" 
          className={`${styles.link} ${pathname.startsWith('/sessions/waiting') ? styles.activeLink : ''}`}
          style={{ color: '#fbbf24' }} /* Lo pinto de amarillo para que sepas que es de prueba */
        >
          ⏳ Sala (Test)
        </Link>
      </div>
    </nav>
  );
}