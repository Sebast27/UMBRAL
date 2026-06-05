import { useState } from 'react';
import { sessionApiRepository } from '../infrastructure/sessionApiRepository';
import { Session, Participant } from '../domain/Session';

export const useSessionManager = () => {
  // 1. Iniciamos los estados vacíos (Cero mocks)
  const [session, setSession] = useState<Session | undefined>(undefined);
  const [participants, setParticipants] = useState<Participant[]>([]);
  const [loading, setLoading] = useState(false);

  const createSession = async (triviaId: string, time: number, isPublic: boolean) => {
    setLoading(true);
    try {
      // ⚡ Esta es la función que está explotando por culpa del repositorio
      const newSession = await sessionApiRepository.create(triviaId, time, isPublic);
      setSession(newSession);
      return newSession;
    } finally {
      setLoading(false);
    }
  };

  const start = async () => {
    // Cuando conectemos el SignalR, aquí llamaremos a sessionApiRepository.startSession
    setSession(prev => prev ? { ...prev, status: 'Activa' } : undefined);
  };

  const handleEndSession = async (sessionId: string, callback?: () => void) => {
    const seguro = window.confirm('¿Seguro que deseas finalizar la trivia? Esto cerrará la sala.');
    if (!seguro) return;
    
    setLoading(true);
    try {
      await sessionApiRepository.endSession(sessionId);
      setSession(prev => prev ? { ...prev, status: 'Finalizada' } : undefined);
      if (callback) callback();
    } catch (error) {
      console.error('Error al finalizar la sesión:', error);
      alert('No se pudo cerrar la sesión.');
    } finally {
      setLoading(false);
    }
  };

  return {
    session,
    participants,
    createSession,
    start,
    loading,
    handleEndSession,
  };
};