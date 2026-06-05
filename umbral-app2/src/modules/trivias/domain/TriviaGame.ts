export interface AnswerOption {
  id: string;
  text: string;
  color: string;
}

export interface ActiveQuestion {
  id: string;
  text: string;
  options: AnswerOption[];
  timeLimit: number;
}

export interface TriviaGameRepository {
  getCurrentQuestion(sessionId: string): Promise<ActiveQuestion>;
  submitAnswer(sessionId: string, participantId: string, answerId: string): Promise<boolean>;
  // NUEVO: Método para suscribirse a los eventos de SignalR en el futuro
  subscribeToNextQuestion(callback: (question: ActiveQuestion | null) => void): () => void;
}