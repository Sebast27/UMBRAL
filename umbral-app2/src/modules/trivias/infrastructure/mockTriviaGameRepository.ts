import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { ActiveQuestion, TriviaGameRepository } from '../domain/TriviaGame';

const API_URL = process.env.EXPO_PUBLIC_API_URL || 'http://192.168.250.5:5000/api';
const HUB_URL = process.env.EXPO_PUBLIC_HUB_URL || 'http://192.168.250.5:5000/hubs/game';

// Guardamos la conexión activa en memoria para este cliente
let globalConnection: HubConnection | null = null;

export const triviaGameApiRepository: TriviaGameRepository = {
  
  getCurrentQuestion: async (sessionId: string) => {
    // Si la pregunta actual se puede pedir por REST API si el jugador se reconecta
    const res = await fetch(`${API_URL}/Sessions/${sessionId}/currentQuestion`);
    if (!res.ok) return null;
    return await res.json();
  },

  submitAnswer: async (sessionId: string, participantId: string, answerId: string) => {
    // Enviamos la respuesta por API (también podrías hacerlo por signalR si el back lo exige)
    const res = await fetch(`${API_URL}/Sessions/${sessionId}/submitAnswer`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ participantId, answerId })
    });
    
    return res.ok;
  },

  // ⚡ LA MAGIA EN TIEMPO REAL
  subscribeToNextQuestion: (callback) => {
    // 1. Construimos la conexión si no existe
    if (!globalConnection) {
      globalConnection = new HubConnectionBuilder()
        .withUrl(HUB_URL)
        .configureLogging(LogLevel.Information)
        .withAutomaticReconnect()
        .build();
    }

    const connection = globalConnection;

    // 2. Definimos el evento (El nombre 'NewQuestion' debe ser EXACTAMENTE el que escupe C#)
    const onNewQuestion = (questionData: ActiveQuestion | null) => {
      callback(questionData);
    };

    connection.on('NewQuestion', onNewQuestion);

    // 3. Iniciamos la conexión si está apagada
    if (connection.state === 'Disconnected') {
      connection.start()
        .then(() => console.log('✅ App móvil conectada a SignalR'))
        .catch(err => console.error('❌ Error conectando SignalR en móvil:', err));
    }

    // 4. Retornamos la función de limpieza para que la pantalla la use al desmontarse
    return () => {
      connection.off('NewQuestion', onNewQuestion);
      if (connection.state === 'Connected') {
        connection.stop();
        globalConnection = null;
      }
    };
  }
};