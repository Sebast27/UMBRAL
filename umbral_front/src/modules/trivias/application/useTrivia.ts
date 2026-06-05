import { useState, useCallback } from 'react';
import { triviaApiRepository } from '../infrastructure/triviaApiRepository';
import { Trivia, Question } from '../domain/Trivia';

export const useTrivias = () => {
  const [trivias, setTrivias] = useState<Trivia[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchTrivias = useCallback(async (search = '') => {
  setLoading(true);
  setError(null);
  try {
    const data = await triviaApiRepository.getTrivias(1, 10, search);
    
    // Normalizar: asegurar que cada pregunta tenga 'options' (minúscula)
    const normalizedData = data.map((trivia: any) => ({
      ...trivia,
      questions: (trivia.questions || []).map((q: any) => ({
        id: q.id,
        text: q.text,
        // Si tiene 'options' (minúscula) usarlo, si no usar 'Options' (mayúscula)
        Options: q.options || q.Options || [],
        correctOption: q.correctOption,
        points: q.points
      }))
    }));
    
    setTrivias(normalizedData);
  } catch (err: any) {
    setError(err.message);
  } finally {
    setLoading(false);
  }
}, []);
const fetchTriviaDetails = useCallback(async (triviaId: string) => {
  setLoading(true);
  setError(null);
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000'}/api/Trivias/${triviaId}`);
    if (!response.ok) throw new Error('Error al cargar los detalles');
    const trivia = await response.json();
    
    // Normalizar las preguntas
    const normalizedTrivia = {
      ...trivia,
      questions: (trivia.questions || []).map((q: any) => ({
        id: q.id,
        text: q.text,
        Options: q.options || q.Options || [],
        correctOption: q.correctOption,
        points: q.points
      }))
    };
    
    setTrivias(prev => prev.map(t => t.id === triviaId ? normalizedTrivia : t));
    return normalizedTrivia;
  } catch (err: any) {
    setError(err.message);
    return null;
  } finally {
    setLoading(false);
  }
}, []);
  const createTrivia = async (name: string, description?: string) => {
    setError(null);
    try {
      const newTrivia = await triviaApiRepository.createTrivia({ name, description });
      // Inicializamos el array de preguntas vacío para la nueva trivia
      setTrivias((prev) => [{ ...newTrivia, questions: [] }, ...prev]);
      return true;
    } catch (err: any) {
      setError(err.message);
      return false;
    }
  };

  // 🔥 NUEVO: Agregar pregunta a una trivia específica
  const addQuestionToTrivia = async (triviaId: string, question: Question) => {
    setError(null);
    try {
      const addedQuestion = await triviaApiRepository.addQuestion(triviaId, question);
      setTrivias((prev) =>
        prev.map((t) =>
          t.id === triviaId
            ? { ...t, questions: [...(t.questions || []), addedQuestion] }
            : t
        )
      );
      return true;
    } catch (err: any) {
      setError(err.message);
      return false;
    }
  };

  // 🔥 NUEVO: Eliminar pregunta de una trivia
  const deleteQuestionFromTrivia = async (triviaId: string, questionId: string) => {
    setError(null);
    try {
      await triviaApiRepository.deleteQuestion(triviaId, questionId);
      setTrivias((prev) =>
        prev.map((t) =>
          t.id === triviaId
            ? { ...t, questions: (t.questions || []).filter((q) => q.id !== questionId) }
            : t
        )
      );
      return true;
    } catch (err: any) {
      setError(err.message);
      return false;
    }
  };

  return { 
    trivias, 
    loading, 
    error, 
    fetchTrivias, 
    fetchTriviaDetails,
    createTrivia,
    addQuestionToTrivia,
    deleteQuestionFromTrivia
  };
};