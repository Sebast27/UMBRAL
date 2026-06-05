export interface Session {
  id: string;
  triviaId: string;
  code: string; // Ejemplo: TRIVIA-123
  timePerQuestion: number; // 5-120 segundos 
  isPublic: boolean;
  status: 'Espera' | 'Activa' | 'Finalizada';
  joinUrl: string;
}

export interface Participant {
  id: string;
  name: string;
  type: 'individual' | 'equipo';
  score?: number;
}

export interface SessionRepository {
  create(triviaId: string, timePerQuestion: number, isPublic: boolean): Promise<Session>;
  getParticipants(sessionId: string): Promise<Participant[]>;
  startSession(sessionId: string): Promise<void>;
  deleteParticipant(sessionId: string, participantId: string): Promise<void>;
  endSession(sessionId: string): Promise<void>;
}