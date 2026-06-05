import { useEffect, useState } from 'react';
import { ActiveQuestion } from '../domain/TriviaGame';
import { triviaGameApiRepository as repository } from '../infrastructure/mockTriviaGameRepository';

export const usePlayTrivia = (sessionId: string, participantId: string) => {
  const [question, setQuestion] = useState<ActiveQuestion | null>(null);
  const [timeLeft, setTimeLeft] = useState(0);
  const [selectedAnswer, setSelectedAnswer] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  // Efecto 1: Carga inicial y suscripción a SignalR
  useEffect(() => {
    // 1. Buscamos la pregunta actual al entrar
    const fetchInitial = async () => {
      const q = await repository.getCurrentQuestion(sessionId);
      setQuestion(q);
      setTimeLeft(q.timeLimit);
    };
    fetchInitial();

    // 2. Nos suscribimos para escuchar cuando el operador lance la siguiente
    const unsubscribe = repository.subscribeToNextQuestion((nextQ) => {
      setQuestion(nextQ);
      if (nextQ) {
        setTimeLeft(nextQ.timeLimit);
        setSelectedAnswer(null); // Desbloqueamos los botones
      }
    });

    // Limpiamos la suscripción al desmontar
    return () => unsubscribe();
  }, [sessionId]);

  // Efecto 2: El temporizador local de la UI
  useEffect(() => {
    if (timeLeft <= 0) return;
    const timer = setInterval(() => setTimeLeft(prev => prev - 1), 1000);
    return () => clearInterval(timer);
  }, [timeLeft]);

  const handleSelectAnswer = async (answerId: string) => {
    if (selectedAnswer || timeLeft === 0) return;
    
    setSelectedAnswer(answerId);
    setIsSubmitting(true);
    await repository.submitAnswer(sessionId, participantId, answerId);
    setIsSubmitting(false);
  };

  return { 
    question, 
    timeLeft, 
    selectedAnswer, 
    handleSelectAnswer, 
    isSubmitting,
    isGameOver: question === null && timeLeft <= 0
  };
};