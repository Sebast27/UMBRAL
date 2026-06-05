using MediatR;
using Microsoft.AspNetCore.Mvc;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Application.TriviaModule.Queries;

namespace Umbral.Adapters.API.Controllers.TriviaModule;

[ApiController]
[Route("api/[controller]")]
public class SessionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SessionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateSession([FromBody] CreateSessionCommand command)
    {
        var sessionId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetSession), new { id = sessionId }, sessionId);
    }

    [HttpGet]
    public async Task<ActionResult<Application.Common.PaginatedResult<Application.TriviaModule.Dtos.SessionListItemDto>>> ListSessions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? onlyPublic = null)
    {
        var result = await _mediator.Send(new ListSessionsQuery(page, pageSize, onlyPublic));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Domain.TriviaModule.Entities.Session>> GetSession(Guid id)
    {
        var session = await _mediator.Send(new GetSessionByIdQuery(id));
        if (session is null)
            return NotFound();

        return Ok(session);
    }

    [HttpPost("{id}/join")]
    public async Task<ActionResult<Guid>> JoinSession(Guid id, [FromBody] JoinSessionCommand command)
    {
        if (id != command.SessionId)
            return BadRequest("El ID de la sesión no coincide");

        var participantId = await _mediator.Send(command);
        return Ok(participantId);
    }

    [HttpPost("{id}/start")]
    public async Task<IActionResult> StartSession(Guid id)
    {
        await _mediator.Send(new StartSessionCommand(id));
        return Ok();
    }

    [HttpDelete("{id}/participants/{participantId}")]
    public async Task<IActionResult> RemoveParticipant(Guid id, Guid participantId)
    {
        await _mediator.Send(new RemoveParticipantCommand(id, participantId));
        return NoContent();
    }

    [HttpDelete("{id}/participants/{participantId}/leave")]
    public async Task<IActionResult> LeaveSession(Guid id, Guid participantId)
    {
        await _mediator.Send(new LeaveSessionCommand(id, participantId));
        return NoContent();
    }

    [HttpPost("{id}/answer")]
    public async Task<ActionResult<AnswerResultDto>> SubmitAnswer(Guid id, [FromBody] SubmitAnswerCommand command)
    {
        if (id != command.SessionId)
            return BadRequest("El ID de la sesión no coincide");

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}