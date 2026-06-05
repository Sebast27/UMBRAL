export interface Participant {
  id: string;
  name: string;
  sessionId: string; // El código de la sala a la que se unió
  joinedAt: Date;
}

export interface SessionParticipantRepository {
  joinSession(sessionCode: string, playerName: string): Promise<Participant>;
  leaveSession(participantId: string): Promise<void>;
}