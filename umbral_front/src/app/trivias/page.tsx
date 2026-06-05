"use client";

import { useEffect, useState } from 'react';
import { useTrivias } from '../../modules/trivias/application/useTrivia';
import { Question, Trivia } from '../../modules/trivias/domain/Trivia';
import { useRouter } from 'next/navigation'; // Hook de navegación nativo
import styles from './Trivias.module.css';

export default function TriviasPage() {
  const { 
    trivias, 
    loading, 
    error, 
    fetchTrivias, 
    fetchTriviaDetails,
    createTrivia, 
    addQuestionToTrivia, 
    deleteQuestionFromTrivia 
  } = useTrivias();


// Función para abrir el editor
const openQuestionsEditor = async (triviaId: string) => {
  await fetchTriviaDetails(triviaId);
  setActiveTriviaId(triviaId);
};
  console.log('Trivias recibidas:', trivias);
  const router = useRouter(); // Instancia para navegar entre pantallas

  // Estados del Formulario de Trivia (Metadata)
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  // Estado para controlar qué trivia tiene abierto el editor de preguntas
  const [activeTriviaId, setActiveTriviaId] = useState<string | null>(null);

  // Estados del Formulario de Preguntas
  const [qText, setQText] = useState('');
  const [qOptions, setQOptions] = useState<[string, string, string, string]>(['', '', '', '']);
  const [qCorrect, setQCorrect] = useState<0 | 1 | 2 | 3>(0);
  const [qPoints, setQPoints] = useState<number>(100);
  const [isAddingQuestion, setIsAddingQuestion] = useState(false);

  useEffect(() => {
    fetchTrivias();
  }, [fetchTrivias]);

  const handleTriviaSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name.trim() || name.length > 100) return;
    if (description.length > 500) return;

    setIsSubmitting(true);
    const success = await createTrivia(name.trim(), description.trim());
    if (success) {
      setName('');
      setDescription('');
    }
    setIsSubmitting(false);
  };

  const handleQuestionSubmit = async (e: React.FormEvent, triviaId: string) => {
    e.preventDefault();
    if (!qText.trim() || qOptions.some(opt => !opt.trim())) {
      alert("Por favor, llena la pregunta y las 4 opciones obligatorias.");
      return;
    }
    if (qPoints < 1 || qPoints > 1000) {
      alert("Los puntos deben estar entre 1 y 1000.");
      return;
    }

    setIsAddingQuestion(true);
    const newQuestion: Question = {
      triviaId: triviaId,
      text: qText.trim(),
      Options: [...qOptions] as [string, string, string, string],
      correctOption: qCorrect,
      points: qPoints
    };

    const success = await addQuestionToTrivia(triviaId, newQuestion);
    if (success) {
      setQText('');
      setQOptions(['', '', '', '']);
      setQCorrect(0);
      setQPoints(100);
    }
    setIsAddingQuestion(false);
  };

  return (
    <div className={styles.container}>
      <div className={styles.contentWrapper}>
        
        <h1 className={styles.title}>Administración de Trivias</h1>

        {/* Formulario para Crear Trivia (CA-01) */}
        <div className={styles.card}>
          <h2 className={styles.sectionTitle}>✨ Crear Nueva Trivia</h2>
          <form onSubmit={handleTriviaSubmit} className={styles.form}>
            <input
              type="text"
              placeholder="Nombre de la Trivia (ej: Cultura General 2026)"
              value={name}
              onChange={(e) => setName(e.target.value)}
              maxLength={100}
              required
              className={styles.input}
            />
            <textarea
              placeholder="Descripción (opcional, máximo 500 caracteres)"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              maxLength={500}
              className={styles.textarea}
            />
            
            {error && <p className={styles.error}>⚠️ {error}</p>}
            
            <button
              type="submit"
              disabled={isSubmitting || !name.trim()}
              className={styles.btnPrimary}
            >
              {isSubmitting ? 'Guardando cambios...' : 'Guardar Trivia'}
            </button>
          </form>
        </div>

        {/* Listado de Trivias (CA-05) */}
        <h2 className={styles.sectionTitle}>📚 Mis Trivias</h2>
        
        {loading ? (
          <p className={styles.statusMessage}>Cargando panel de trivias...</p>
        ) : trivias.length === 0 ? (
          <p className={styles.statusMessage}>No tienes ninguna trivia configurada todavía.</p>
        ) : (
          <div className={styles.grid}>
            {trivias.map((trivia) => {
              const isEditingQuestions = activeTriviaId === trivia.id;
              
              return (
                <div key={trivia.id} className={styles.triviaItem}>
                  <div className={styles.triviaHeader}>
                    <div>
                      <h3 style={{ fontSize: '1.25rem', fontWeight: 700, margin: '0 0 0.25rem 0' }}>{trivia.name}</h3>
                      {trivia.description && <p style={{ color: '#94a3b8', fontSize: '0.9rem', margin: '0 0 0.5rem 0' }}>{trivia.description}</p>}
                      <span style={{ fontSize: '0.75rem', color: '#475569' }}>
                        Preguntas registradas: {trivia.questions?.length || 0}
                      </span>
                    </div>
                    
                    {/* Panel lateral de botones de acción */}
                    <div style={{ display: 'flex', gap: '0.75rem', alignItems: 'center' }}>
                      {/* Botón de Navegación Cruzada hacia Lanzar Trivia */}
                      <button 
                        onClick={() => router.push(`/sessions/create?triviaId=${trivia.id}`)}
                        className={styles.btnPrimary}
                        style={{ padding: '0.625rem 1.25rem', fontSize: '0.875rem', boxShadow: 'none', margin: 0 }}
                      >
                        🚀 Lanzar En Vivo
                      </button>

                      <button 
                        onClick={() => openQuestionsEditor( trivia.id)}
                        className={styles.btnSecondary}
                      >
                        {isEditingQuestions ? '❌ Cerrar Editor' : '⚙️ Gestionar Preguntas'}
                      </button>
                    </div>
                  </div>

                  {/* SUBPANEL: Gestión de Preguntas de esta Trivia */}
                  {isEditingQuestions && (
                    <div className={styles.questionsManagerSection}>
                      <h4 className={styles.sectionTitle} style={{ fontSize: '1.05rem', color: '#818cf8' }}>
                        📝 Preguntas de la Trivia
                      </h4>

                      {/* Lista de Preguntas Existentes */}
                      {(!trivia.questions || trivia.questions.length === 0) ? (
                        <p style={{ color: '#64748b', fontSize: '0.9rem', marginBottom: '1.5rem' }}>
                          Esta trivia no tiene preguntas todavía. ¡Añade la primera abajo!
                        </p>
                      ) : (
                        <div style={{ marginBottom: '1.5rem' }}>
                          {trivia.questions.map((q, idx) => (
                            <div key={q.id || idx} className={styles.questionItem}>
                              <div style={{ paddingRight: '1rem' }}>
                                <p style={{ fontWeight: '600', margin: '0 0 0.25rem 0' }}>{idx + 1}. {q.text}</p>
                                <span style={{ fontSize: '0.8rem', color: '#64748b', marginRight: '1rem' }}>
                                  💎 {q.points} pts
                                </span>
                                <span className={styles.correctBadge}>
                                      Correcta: {q.Options && q.Options.length > 0 ? q.Options[q.correctOption] : 'N/A'}
                                </span>
                              </div>
                              <button 
                                onClick={() => q.id && deleteQuestionFromTrivia(trivia.id, q.id)}
                                className={styles.btnDanger}
                              >
                                Eliminar
                              </button>
                            </div>
                          ))}
                        </div>
                      )}

                      {/* Formulario para Añadir Pregunta */}
                      <form onSubmit={(e) => handleQuestionSubmit(e, trivia.id)} className={styles.form} style={{ background: 'rgba(2,6,23,0.3)', padding: '1.25rem', borderRadius: '0.75rem', border: '1px dashed #334155' }}>
                        <p style={{ fontSize: '0.9rem', fontWeight: 'bold', color: '#cbd5e1', margin: 0 }}>➕ Nueva Pregunta</p>
                        
                        <input 
                          type="text"
                          placeholder="Enunciado de la pregunta (ej: ¿Qué Hook se utiliza para guardar estados?)"
                          value={qText}
                          onChange={(e) => setQText(e.target.value)}
                          required
                          className={styles.input}
                        />

                        {/* Las 4 opciones obligatorias requeridas por el Dominio */}
                        <div className={styles.optionsGrid}>
                          {qOptions.map((option, index) => (
                            <input 
                              key={index}
                              type="text"
                              placeholder={`Opción ${index + 1}`}
                              value={option}
                              onChange={(e) => {
                                const updatedOptions = qOptions.map((opt, i) => i === index ? e.target.value : opt) as [string, string, string, string];
                                setQOptions(updatedOptions);
                              }}
                              required
                              className={styles.input}
                              style={{ padding: '0.75rem' }}
                            />
                          ))}
                        </div>

                        {/* Selección de Respuesta Correcta y Puntos */}
                        <div style={{ display: 'flex', gap: '1rem', flexWrap: 'wrap' }}>
                          <div style={{ flex: 1, minWidth: '150px' }}>
                            <label style={{ fontSize: '0.8rem', color: '#94a3b8', display: 'block', marginBottom: '0.25rem' }}>Opción Correcta</label>
                            <select 
                              value={qCorrect} 
                              onChange={(e) => setQCorrect(Number(e.target.value) as 0|1|2|3)}
                              className={styles.select}
                              style={{ width: '100%', padding: '0.75rem' }}
                            >
                              <option value={0}>Opción 1</option>
                              <option value={1}>Opción 2</option>
                              <option value={2}>Opción 3</option>
                              <option value={3}>Opción 4</option>
                            </select>
                          </div>

                          <div style={{ flex: 1, minWidth: '150px' }}>
                            <label style={{ fontSize: '0.8rem', color: '#94a3b8', display: 'block', marginBottom: '0.25rem' }}>Puntaje (1 - 1000)</label>
                            <input 
                              type="number" 
                              min={1} 
                              max={1000}
                              value={qPoints}
                              onChange={(e) => setQPoints(Number(e.target.value))}
                              className={styles.input}
                              style={{ width: '100%', padding: '0.75rem' }}
                            />
                          </div>
                        </div>

                        <button 
                          type="submit" 
                          disabled={isAddingQuestion || !qText.trim()}
                          className={styles.btnPrimary}
                          style={{ padding: '0.75rem', fontSize: '0.9rem' }}
                        >
                          {isAddingQuestion ? 'Añadiendo...' : 'Añadir Pregunta a la Lista'}
                        </button>
                      </form>
                    </div>
                  )}
                </div>
              );
            })}
          </div>
        )}
        
      </div>
    </div>
  );
}