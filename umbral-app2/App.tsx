// App.tsx
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import React from 'react';
import JoinScreen from './src/screens/JoinScreen';
import TriviaScreen from './src/screens/TriviaScreen';

// Definimos los nombres de las pantallas para TypeScript
export type RootStackParamList = {
  Join: undefined;
  Trivia: { sessionId: string; participantId: string; playerName: string };
};

const Stack = createNativeStackNavigator<RootStackParamList>();

export default function App() {
  return (
    <NavigationContainer>
      {/* screenOptions={{ headerShown: false }} oculta la barra fea de arriba */}
      <Stack.Navigator initialRouteName="Join" screenOptions={{ headerShown: false }}>
        <Stack.Screen name="Join" component={JoinScreen} />
        <Stack.Screen name="Trivia" component={TriviaScreen} />
      </Stack.Navigator>
    </NavigationContainer>
  );
}