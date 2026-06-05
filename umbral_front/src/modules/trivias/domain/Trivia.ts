// Entidad Pregunta según RN-01 y RN-02
export interface Question {
  id?: string;
  triviaId?: string;
  text: string;
  Options: [string, string, string, string]; // Exactamente 4 opciones
  correctOption: 0 | 1 | 2 | 3;
  points: number; // Entre 1 y 1000
}

// Entidad Trivia
export interface Trivia {
  id: string;
  name: string;
  description?: string;
  createdAt: string;
  questions?: Question[];
}

// Puerto de salida (Repository Interface)
export interface TriviaRepository {
  getTrivias(page: number, pageSize: number, search?: string): Promise<Trivia[]>;
  createTrivia(trivia: { name: string; description?: string }): Promise<Trivia>;
  addQuestion(triviaId: string, question: Question): Promise<Question>;
  deleteQuestion(triviaId: string, questionId: string): Promise<void>;
}