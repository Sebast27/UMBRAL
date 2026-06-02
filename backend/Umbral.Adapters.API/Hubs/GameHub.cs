using Microsoft.AspNetCore.SignalR;

namespace Umbral.Adapters.API.Hubs;

public class GameHub : Hub
{
    // Diccionario para rastrear qué conexión está en qué sesión
    private static readonly Dictionary<string, Guid> _connections = new();

    public async Task JoinSession(Guid sessionId, Guid participantId)
    {
        // Guardar la conexión
        _connections[Context.ConnectionId] = sessionId;
        
        // Unirse al grupo de la sesión
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId.ToString());
        
        // Notificar a todos en la sesión que alguien se unió
        await Clients.Group(sessionId.ToString()).SendAsync("ParticipantJoined", participantId);
    }

    public async Task LeaveSession(Guid sessionId)
    {
        // Salir del grupo
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId.ToString());
        
        // Remover la conexión
        if (_connections.ContainsKey(Context.ConnectionId))
        {
            _connections.Remove(Context.ConnectionId);
        }
    }

    public async Task StartGame(Guid sessionId)
    {
        // Notificar a todos que el juego comenzó
        await Clients.Group(sessionId.ToString()).SendAsync("GameStarted", sessionId);
    }

    public async Task SendQuestion(Guid sessionId, string question, string[] options, int timeLimit)
    {
        var questionData = new
        {
            Question = question,
            Options = options,
            TimeLimit = timeLimit,
            StartedAt = DateTime.UtcNow
        };
        
        await Clients.Group(sessionId.ToString()).SendAsync("NewQuestion", questionData);
    }

    public async Task SubmitAnswer(Guid sessionId, Guid participantId, int answer, int timeElapsed)
    {
        // Enviar la respuesta al operador (o a todos si quieres)
        await Clients.Group(sessionId.ToString()).SendAsync("AnswerSubmitted", participantId, answer, timeElapsed);
    }

    public async Task RoundWinner(Guid sessionId, Guid winnerId, int points)
    {
        await Clients.Group(sessionId.ToString()).SendAsync("RoundWinner", winnerId, points);
    }

    public async Task SessionFinished(Guid sessionId, object results)
    {
        await Clients.Group(sessionId.ToString()).SendAsync("SessionFinished", results);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Limpiar cuando un cliente se desconecta
        if (_connections.TryGetValue(Context.ConnectionId, out var sessionId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId.ToString());
            _connections.Remove(Context.ConnectionId);
        }
        
        await base.OnDisconnectedAsync(exception);
    }
}