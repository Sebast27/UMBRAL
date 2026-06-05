import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet, ActivityIndicator } from 'react-native';
import { usePlayTrivia } from '../modules/trivias/application/usePlayTrivia';

export default function TriviaScreen() {
  // Simulamos los IDs por ahora (en la versión final vendrán de la navegación de la HU-03)
  const { question, timeLeft, selectedAnswer, handleSelectAnswer, isSubmitting } = usePlayTrivia('TRIVIA-1234', 'player-1');

  if (!question) {
    return (
      <View style={styles.centerContainer}>
        <ActivityIndicator size="large" color="#2563eb" />
        <Text style={styles.loadingText}>Cargando pregunta...</Text>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Text style={styles.timerText}>{timeLeft}</Text>
      </View>

      <View style={styles.questionContainer}>
        <Text style={styles.questionText}>{question.text}</Text>
      </View>

      <View style={styles.optionsGrid}>
        {question.options.map((option) => {
          const isSelected = selectedAnswer === option.id;
          const isDisabled = selectedAnswer !== null || timeLeft === 0;

          return (
            <TouchableOpacity
              key={option.id}
              style={[
                styles.optionButton,
                { backgroundColor: option.color },
                isDisabled && !isSelected && styles.optionDisabled,
                isSelected && styles.optionSelected
              ]}
              onPress={() => handleSelectAnswer(option.id)}
              disabled={isDisabled}
            >
              <Text style={styles.optionText}>{option.text}</Text>
            </TouchableOpacity>
          );
        })}
      </View>

      {selectedAnswer && (
        <View style={styles.waitingContainer}>
          <Text style={styles.waitingText}>
            {isSubmitting ? 'Enviando...' : '¡Respuesta enviada! Esperando al resto...'}
          </Text>
        </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f8fafc', padding: 20 },
  centerContainer: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  loadingText: { marginTop: 16, fontSize: 16, color: '#64748b', fontWeight: '600' },
  header: { alignItems: 'center', marginVertical: 20 },
  timerText: { fontSize: 48, fontWeight: '900', color: '#0f172a' },
  questionContainer: { backgroundColor: 'white', padding: 24, borderRadius: 16, marginBottom: 30, shadowColor: '#000', shadowOffset: { width: 0, height: 2 }, shadowOpacity: 0.05, shadowRadius: 8, elevation: 2 },
  questionText: { fontSize: 22, fontWeight: '800', color: '#1e293b', textAlign: 'center', lineHeight: 30 },
  optionsGrid: { flex: 1, gap: 16 },
  optionButton: { padding: 24, borderRadius: 12, alignItems: 'center', justifyContent: 'center', minHeight: 90 },
  optionText: { color: 'white', fontSize: 18, fontWeight: '800', textAlign: 'center' },
  optionDisabled: { opacity: 0.4 },
  optionSelected: { borderWidth: 4, borderColor: '#0f172a', transform: [{ scale: 0.98 }] },
  waitingContainer: { padding: 20, alignItems: 'center' },
  waitingText: { fontSize: 16, fontWeight: '700', color: '#64748b' }
});