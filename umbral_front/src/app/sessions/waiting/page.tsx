"use client";

import { useState, useEffect, Suspense } from 'react';
import { useSearchParams, useRouter } from 'next/navigation';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import styles from './WaitingRoom.module.css'; 


// Definir el tipo de participante
interface Participant {
  id: string;
  name: string;
}

function WaitingRoomContent() {
  const searchParams = useSearchParams();
  const router = useRouter();
  
  const sessionId = searchParams.get('sessionId') || '';
  const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

  const [participants, setParticipants] = useState<Participant[]>([]);
  const [connection, setConnection] = useState<HubConnection | null>(null);
  const [loading, setLoading] = useState(true);

  // Obtener participantes del backend
  const fetchParticipants = async () => {
    try {
      const res = await fetch(`${API_URL}/api/sessions/${sessionId}/participants`);
      if (res.ok) {
        const data = await res.json();
        setParticipants(data);
      }
    } catch (error) {
      console.error('Error al cargar participantes:', error);
    }
  };

  // Conectar a SignalR
  useEffect(() => {
    if (!sessionId || sessionId === 'SESION-MOCK-123') return;

    const newConnection = new HubConnectionBuilder()
      .withUrl(`${API_URL}/hubs/game`)
      .withAutomaticReconnect()
      .build();

    newConnection.start()
      .then(() => {
        console.log('Conectado a SignalR');
        newConnection.invoke('JoinSession', sessionId, 'operador-id');
        setLoading(false);
      })
      .catch(err => {
        console.error('Error de conexión:', err);
        setLoading(false);
      });

    newConnection.on('ParticipantJoined', () => {
      fetchParticipants();
    });

    newConnection.on('ParticipantLeft', () => {
      fetchParticipants();
    });

    setConnection(newConnection);
    fetchParticipants();

    return () => {
      newConnection.stop();
    };
  }, [sessionId]);

  // Iniciar el juego
  const startGame = async () => {
    try {
      const res = await fetch(`${API_URL}/api/sessions/${sessionId}/start`, { method: 'POST' });
      if (res.ok) {
        connection?.invoke('StartGame', sessionId);
        router.push(`/sessions/play?sessionId=${sessionId}`);
      } else {
        alert('No se pudo iniciar la sesión');
      }
    } catch (error) {
      console.error('Error al iniciar:', error);
      alert('Error de conexión');
    }
  };

  // Si no hay sessionId válido
  if (!sessionId || sessionId === 'SESION-MOCK-123') {
    return (
      <div style={{ maxWidth: '600px', margin: '4rem auto', padding: '2rem', backgroundColor: '#1e293b', borderRadius: '12px', color: 'white', textAlign: 'center' }}>
        <h1 style={{ color: '#38bdf8' }}>Error</h1>
        <p>ID de sesión no válido. Asegúrate de usar el enlace correcto.</p>
      </div>
    );
  }

  if (loading) {
    return (
      <div style={{ maxWidth: '600px', margin: '4rem auto', padding: '2rem', backgroundColor: '#1e293b', borderRadius: '12px', color: 'white', textAlign: 'center' }}>
        <p>Conectando a la sala de espera...</p>
      </div>
    );
  }

  return (
    <div style={{ maxWidth: '600px', margin: '4rem auto', padding: '2rem', backgroundColor: '#1e293b', borderRadius: '12px', color: 'white' }}>
      <h1 style={{ textAlign: 'center', color: '#38bdf8' }}>Sala de Espera ⏳</h1>
      <p style={{ textAlign: 'center', color: '#94a3b8' }}>ID de Sesión: {sessionId}</p>

      <div style={{ margin: '2rem 0', padding: '1rem', backgroundColor: '#0f172a', borderRadius: '8px' }}>
        <h2>Jugadores Conectados: {participants.length}</h2>
        {participants.length === 0 ? (
          <p style={{ color: '#64748b' }}>Esperando a que se unan los jugadores...</p>
        ) : (
          <ul style={{ listStyleType: 'none', padding: 0 }}>
            {participants.map((player) => (
              <li key={player.id} style={{ padding: '0.5rem 0', borderBottom: '1px solid #334155' }}>
                🟢 {player.name}
              </li>
            ))}
          </ul>
        )}
      </div>

      <button 
        onClick={startGame}
        disabled={participants.length === 0}
        style={{ 
          width: '100%', 
          padding: '1rem', 
          backgroundColor: participants.length === 0 ? '#475569' : '#0284c7', 
          color: 'white', 
          border: 'none', 
          borderRadius: '8px', 
          cursor: participants.length === 0 ? 'not-allowed' : 'pointer',
          fontWeight: 'bold', 
          fontSize: '1.1rem' 
        }}
      >
        ¡Iniciar Trivia!
      </button>
    </div>
  );
}

export default function WaitingRoomPage() {
  return (
    <Suspense fallback={<p style={{ color: 'white', textAlign: 'center', marginTop: '2rem' }}>Cargando sala...</p>}>
      <WaitingRoomContent />
    </Suspense>
  );
}