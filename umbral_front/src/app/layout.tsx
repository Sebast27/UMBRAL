import React from 'react';
import Navbar from '../components/Navbar'; // 👈 1. Importamos tu barra de navegación premium
import './globals.css'; // Tus estilos globales si tienes

export const metadata = {
  title: 'Umbral Académico - Trivia',
  description: 'Módulo de Administración de Sesiones',
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="es">
      <body style={{ margin: 0, backgroundColor: '#020617' }}>
        
        {/* 👈 2. Colocamos la barra aquí para que se vea en TODAS las páginas */}
        <Navbar /> 
        
        {/* El contenido dinámico de tus páginas (page.tsx) se renderiza aquí */}
        <main>
          {children}
        </main>

      </body>
    </html>
  );
}