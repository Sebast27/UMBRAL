"use client";

import { useState, useEffect, Suspense } from 'react';
import { useSessionManager } from '../../../modules/sessions/application/useSessionsManager';
import { useRouter, useSearchParams } from 'next/navigation';

// ⚡ Importamos tu repositorio real usando la ruta relativa (para evitar el error del @)
import { triviaApiRepository } from '../../../modules/trivias/infrastructure/triviaApiRepository';

import styles from './CreateSession.module.css';

function CreateSessionForm() {
  const searchParams = useSearchParams();
  const urlTriviaId = searchParams.get('triviaId') || ''; 

  const [triviaId, setTriviaId] = useState(urlTriviaId);
  const [time, setTime] = useState(30);
  const [isPublic, setIsPublic] = useState(true);
  const [localError, setLocalError] = useState<string | null>(null);
  
  // ⚡ Nuevos estados para manejar las trivias que vienen del backend
  const [triviasList, setTriviasList] = useState<any[]>([]);
  const [loadingTrivias, setLoadingTrivias] = useState(true);

  const { createSession, loading } = useSessionManager();
  const router = useRouter();

  // 1. Efecto para tomar el ID de la URL si existe
  useEffect(() => {
    if (urlTriviaId) {
      setTriviaId(urlTriviaId);
    }
  }, [urlTriviaId]);

  // 2. ⚡ Efecto para ir a buscar las trivias reales a tu API
  useEffect(() => {
    const fetchTriviasFromBackend = async () => {
      try {
        setLoadingTrivias(true);
        // Le pasamos los parámetros por defecto de paginación
        const data = await triviaApiRepository.getTrivias(1, 100, '');
        setTriviasList(data as any[]);
      } catch (error) {
        console.error("Error cargando trivias:", error);
        setLocalError("No se pudieron cargar las trivias del servidor. Verifica que el backend esté corriendo.");
      } finally {
        setLoadingTrivias(false);
      }
    };

    fetchTriviasFromBackend();
  }, []);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    setLocalError(null);

    if (!triviaId) {
      alert("Por favor, selecciona una trivia");
      return;
    }

    try {
      const session = await createSession(triviaId, time, isPublic);
      
      if (session && session.id) {
        // ⚡ Le pasamos el ID a la sala de espera por la URL
        router.push(`/sessions/waiting?sessionId=${session.id}`);
      } else {
        setLocalError("El servidor no devolvió una sesión válida. Verifica si el ID de la trivia existe en la base de datos.");
      }
    } catch (err: any) {
      setLocalError(err.message || "Ocurrió un error inesperado al intentar crear la sesión.");
    }
  };

  return (
    <div className={styles.card}>
      <h1 className={styles.title}>Lanzar Trivia</h1>
      <p className={styles.subtitle}>Configura una nueva sesión en vivo para los participantes.</p>
      
      <form onSubmit={handleCreate}>
        <div className={styles.formGroup}>
          <label className={styles.label}>Seleccionar Trivia</label>
          <select 
            value={triviaId} 
            onChange={(e) => setTriviaId(e.target.value)}
            required
            className={styles.select}
            disabled={loadingTrivias}
          >
            {/* ⚡ Renderizado condicional mientras carga la API */}
            {loadingTrivias ? (
              <option value="">Cargando trivias desde el servidor...</option>
            ) : triviasList.length === 0 ? (
              <option value="">No hay trivias disponibles. ¡Crea una primero!</option>
            ) : (
              <>
                <option value="">-- Elige una trivia disponible --</option>
                {triviasList.map((trivia) => (
                  <option key={trivia.id} value={trivia.id}>
                    {/* Dependiendo de cómo lo llames en tu C#, puede ser name o title */}
                    {trivia.name || trivia.title} 
                  </option>
                ))}
              </>
            )}
          </select>
        </div>
        
        <div className={styles.formGroup}>
          <label className={styles.label}>Tiempo por pregunta (segundos)</label>
          <input 
            type="number" 
            min="5" 
            max="120" 
            value={time} 
            onChange={(e) => setTime(Number(e.target.value))}
            required
            className={styles.input}
          />
        </div>
        
        <div className={styles.checkboxGroup} onClick={() => setIsPublic(!isPublic)}>
          <input 
            type="checkbox" 
            checked={isPublic} 
            onChange={(e) => setIsPublic(e.target.checked)}
            className={styles.checkbox}
            onClick={(e) => e.stopPropagation()}
          />
          <span className={styles.checkboxLabel}>Hacer esta sesión visible al público</span>
        </div>

        {localError && (
          <p style={{ color: '#f43f5e', fontSize: '0.875rem', backgroundColor: 'rgba(244, 63, 94, 0.1)', padding: '0.75rem', borderRadius: '0.5rem', border: '1px solid rgba(244, 63, 94, 0.2)', margin: '0 0 1.5rem 0', textAlign: 'center' }}>
            ⚠️ {localError}
          </p>
        )}

        <button type="submit" disabled={loading || loadingTrivias} className={styles.btnSubmit}>
          {loading ? 'Generando sesión...' : 'Generar Código de Acceso'}
        </button>
      </form>
    </div>
  );
}

export default function CreateSessionPage() {
  return (
    <div className={styles.container}>
      <Suspense fallback={<p style={{ color: '#64748b' }}>Cargando configurador...</p>}>
        <CreateSessionForm />
      </Suspense>
    </div>
  );
}