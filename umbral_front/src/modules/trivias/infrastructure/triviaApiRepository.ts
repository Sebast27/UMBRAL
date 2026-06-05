import { Trivia, TriviaRepository, Question } from '../domain/Trivia.js';

// Leemos la URL del backend
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000/api';

export const triviaApiRepository: TriviaRepository = {
  getTrivias: async (page: number = 1, pageSize: number = 10, search: string = '') => {
    // Apunta al [HttpGet] de TriviasController.cs
    const res = await fetch(`${API_URL}/Trivias?page=${page}&pageSize=${pageSize}&search=${search}`);
    if (!res.ok) throw new Error('Error al obtener las trivias del backend');
    
    const data = await res.json();
    // Tu backend devuelve un objeto paginado (PaginatedResult), usualmente los datos vienen en una propiedad como 'items' o 'data'. 
    // Si la lista de trivias no te carga en pantalla, cambia data por data.items (depende de cómo lo armaron en C#).
    return data.items || data; 
  },

  createTrivia: async (trivia: { name: string; description?: string }) => {
    // Apunta al [HttpPost] de TriviasController.cs
    const payload = {
      name: trivia.name,
      description: trivia.description,
      // Le mandamos un ID de creador genérico temporalmente
      createdBy: "00000000-0000-0000-0000-000000000000" 
    };

    const res = await fetch(`${API_URL}/Trivias`, {
      method: 'POST',
      body: JSON.stringify(payload),
      headers: { 'Content-Type': 'application/json' },
    });
    
    if (!res.ok) throw new Error('Error al crear la trivia en el backend');
    
    // El backend devuelve el Guid (ID) de la nueva trivia
    const id = await res.json(); 
    return { id, ...trivia, questions: [] } as unknown as Trivia;
  },

  addQuestion: async (triviaId: string, question: Question) => {
    // ⚠️ NOTA: Este endpoint NO existe en tu TriviasController.cs actual. 
    // Tu compañero de backend debe crearlo o deben usar el UpdateTrivia.
    const res = await fetch(`${API_URL}/Trivias/${triviaId}/questions`, {
      method: 'POST',
      body: JSON.stringify(question),
      headers: { 'Content-Type': 'application/json' },
    });
    
    if (!res.ok) throw new Error('Error al guardar la pregunta');
    return res.json();
  },

  deleteQuestion: async (triviaId: string, questionId: string) => {
    // ⚠️ NOTA: Este endpoint tampoco existe en tu TriviasController.cs actual.
    const res = await fetch(`${API_URL}/Trivias/${triviaId}/questions/${questionId}`, {
      method: 'DELETE',
    });
    
    if (!res.ok) throw new Error('Error al eliminar la pregunta');
  }
};