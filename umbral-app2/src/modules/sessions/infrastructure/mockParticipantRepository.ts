import { SessionParticipantRepository } from '../domain/Participant';

// ⚡ Usamos las variables de entorno de Expo
const API_URL = process.env.EXPO_PUBLIC_API_URL || 'http://192.168.250.5:5000/api';

export const participantApiRepository: SessionParticipantRepository = {
  joinSession: async (sessionCode: string, playerName: string) => {
    // 💡 NOTA: Ajusta esta URL ("/Sessions/join") dependiendo de cómo la llamó el backend en C#
    const res = await fetch(`${API_URL}/Sessions/${sessionCode}/join`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ playerName }) // Lo que pida tu endpoint
    });

    if (!res.ok) {
      const errorData = await res.text();
      throw new Error(errorData || 'Error al intentar unirse a la sala.');
    }

    // El backend debe devolver los datos del participante
    const data = await res.json();
    return {
      id: data.id,
      name: data.name || playerName,
      sessionId: sessionCode.toUpperCase(),
      joinedAt: new Date(),
    };
  },
  
  leaveSession: async (participantId: string) => {
    try {
      await fetch(`${API_URL}/Sessions/leave/${participantId}`, {
        method: 'POST', // o DELETE, según tu backend
      });
    } catch (error) {
      console.error(`Error al salir de la sesión:`, error);
    }
  }
};