import { useState } from 'react';
import { Participant } from '../domain/Participant';
import { participantApiRepository as repository } from '../infrastructure/mockParticipantRepository';

export const useJoinSession = () => {
  const [participant, setParticipant] = useState<Participant | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const join = async (sessionCode: string, playerName: string) => {
    setLoading(true);
    setError(null);
    
    try {
      const newParticipant = await repository.joinSession(sessionCode, playerName);
      setParticipant(newParticipant);
      return newParticipant; // Retornamos el participante si fue exitoso
    } catch (err: any) {
      setError(err.message);
      return null; // Retornamos null si falló
    } finally {
      setLoading(false);
    }
  };

  return { join, participant, loading, error };
};