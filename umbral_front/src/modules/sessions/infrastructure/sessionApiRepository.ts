import { Session, SessionRepository, Participant } from '../domain/Session';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000/api';

export const sessionApiRepository: SessionRepository = {
  create: async (triviaId, timePerQuestion, isPublic) => {
    const payload = {
      triviaId: triviaId,
      timePerQuestion: timePerQuestion,
      isPublic: isPublic
    };

    const response = await fetch(`${API_URL}/sessions`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload),
    });
    
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`Error en el servidor: ${errorText}`);
    }
    
    // Leemos el texto plano y le quitamos las comillas
    const rawText = await response.text(); 
    const cleanSessionId = rawText.replace(/['"]+/g, ''); 

    // 👇 AQUÍ ENGAÑAMOS A TYPESCRIPT 👇
    // Armamos el objeto con lo que tenemos y le aplicamos un "cast"
    return { 
      id: cleanSessionId,
      triviaId: triviaId,
      timePerQuestion: timePerQuestion,
      isPublic: isPublic
    } as unknown as Session; // Esto hace que TS deje de pedir las variables faltantes
  },

  getParticipants: async (sessionId) => {
    const response = await fetch(`${API_URL}/sessions/${sessionId}/participants`);
    if (!response.ok) throw new Error('Error al obtener participantes');
    return response.json();
  },

  startSession: async (sessionId) => {
    const response = await fetch(`${API_URL}/sessions/${sessionId}/start`, { method: 'POST' });
    if (!response.ok) throw new Error('Error al iniciar la sesión');
  },

  deleteParticipant: async (sessionId, participantId) => {
    const response = await fetch(`${API_URL}/sessions/${sessionId}/participants/${participantId}`, {
      method: 'DELETE',
    });
    if (!response.ok) throw new Error('Error al eliminar participante');
  },

  // 👇 AGREGA ESTO PARA LA HU-05 👇
  endSession: async (sessionId) => {
    const response = await fetch(`${API_URL}/sessions/${sessionId}/finish`, { 
      method: 'POST' 
    });
    if (!response.ok) throw new Error('Error al finalizar la sesión');
  },
};