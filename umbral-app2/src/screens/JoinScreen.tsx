import { useNavigation } from '@react-navigation/native';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import React, { useState } from 'react';
import { Alert, StyleSheet, Text, TextInput, TouchableOpacity, View } from 'react-native';
import { RootStackParamList } from '../../App';
import { useJoinSession } from '../modules/sessions/application/useJoinSession';

export default function JoinScreen() {
  const [playerName, setPlayerName] = useState('');
  const [sessionCode, setSessionCode] = useState('');
  
  // Usamos el hook de nuestra Arquitectura Hexagonal
  const { join, loading, error } = useJoinSession();
  const navigation = useNavigation<NativeStackNavigationProp<RootStackParamList>>();

  const handleJoin = async () => {
    if (!playerName || !sessionCode) {
      Alert.alert('Datos incompletos', 'Por favor ingresa tu nombre y el código de la sala');
      return;
    }

    const joinedParticipant = await join(sessionCode, playerName);

    if (joinedParticipant) {
    // ¡AQUÍ OCURRE LA MAGIA! Pasamos los datos reales del participante a la siguiente pantalla
    navigation.navigate('Trivia', {
      sessionId: joinedParticipant.sessionId,
      participantId: joinedParticipant.id,
      playerName: joinedParticipant.name
    });
  } else if (error) {
    Alert.alert('Error', error);
  }
};

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Proyecto Umbral</Text>
      <Text style={styles.subtitle}>Ingresa a la trivia en vivo</Text>

      <View style={styles.card}>
        <Text style={styles.label}>Tu Nombre o Equipo</Text>
        <TextInput
          style={styles.input}
          placeholder="Ej: Alpha Team"
          value={playerName}
          onChangeText={setPlayerName}
          autoCapitalize="words"
        />

        <Text style={styles.label}>Código de Acceso</Text>
        <TextInput
          style={styles.input}
          placeholder="Ej: TRIVIA-1234"
          value={sessionCode}
          onChangeText={setSessionCode}
          autoCapitalize="characters"
        />

        {error && <Text style={styles.errorText}>{error}</Text>}

        <TouchableOpacity 
          style={[styles.button, loading && styles.buttonDisabled]} 
          onPress={handleJoin}
          disabled={loading}
        >
          <Text style={styles.buttonText}>
            {loading ? 'Validando código...' : 'ENTRAR AL JUEGO'}
          </Text>
        </TouchableOpacity>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f8fafc', justifyContent: 'center', padding: 24 },
  title: { fontSize: 36, fontWeight: '900', color: '#1e293b', textAlign: 'center', marginBottom: 8 },
  subtitle: { fontSize: 16, color: '#64748b', textAlign: 'center', marginBottom: 40, textTransform: 'uppercase', letterSpacing: 1 },
  card: { backgroundColor: 'white', padding: 24, borderRadius: 16, shadowColor: '#000', shadowOffset: { width: 0, height: 4 }, shadowOpacity: 0.1, shadowRadius: 12, elevation: 5 },
  label: { fontSize: 14, fontWeight: '700', color: '#334155', marginBottom: 8 },
  input: { borderWidth: 1, borderColor: '#e2e8f0', borderRadius: 10, padding: 16, fontSize: 16, marginBottom: 20, backgroundColor: '#f8fafc' },
  errorText: { color: '#ef4444', marginBottom: 15, textAlign: 'center', fontWeight: '600' },
  button: { backgroundColor: '#2563eb', padding: 18, borderRadius: 50, alignItems: 'center', marginTop: 10 },
  buttonDisabled: { backgroundColor: '#93c5fd' },
  buttonText: { color: 'white', fontSize: 16, fontWeight: '800', letterSpacing: 1 },
});