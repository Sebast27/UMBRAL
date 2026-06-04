using MediatR;
using Microsoft.AspNetCore.Mvc;
using Umbral.Application.TriviaModule.Commands;
using Umbral.Application.TriviaModule.Queries;

namespace Umbral.Adapters.API.Controllers.TriviaModule;

[ApiController]
[Route("api/[controller]")]
public class TriviasController : ControllerBase
{
    private readonly IMediator _mediator;

    public TriviasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTrivia([FromBody] CreateTriviaCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTriviaById), new { id }, id);
    }

    [HttpPost("{id}/questions")]
    public async Task<ActionResult<Guid>> AddQuestion(Guid id, [FromBody] AddQuestionToTriviaCommand command)
    {
        if (id != command.TriviaId)
            return BadRequest("El ID de la trivia no coincide");

        var questionId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTriviaById), new { id }, questionId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrivia(Guid id, [FromBody] UpdateTriviaCommand command)
    {
        if (id != command.Id)
            return BadRequest("El ID de la ruta no coincide con el ID del comando");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrivia(Guid id)
    {
        await _mediator.Send(new DeleteTriviaCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Domain.TriviaModule.Entities.Trivia>> GetTriviaById(Guid id)
    {
        var trivia = await _mediator.Send(new GetTriviaByIdQuery(id));
        if (trivia is null)
            return NotFound();

        return Ok(trivia);
    }

    [HttpGet]
    public async Task<ActionResult<Application.Common.PaginatedResult<Application.TriviaModule.Dtos.TriviaListItemDto>>> ListTrivias(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(new ListTriviasQuery(page, pageSize, search));
        return Ok(result);
    }

    [HttpPut("{id}/questions/{questionId}")]
    public async Task<IActionResult> UpdateQuestion(Guid id, Guid questionId, [FromBody] UpdateQuestionCommand command)
    {
        if (id != command.TriviaId)
            return BadRequest("El ID de la trivia no coincide");
        if (questionId != command.QuestionId)
            return BadRequest("El ID de la pregunta no coincide");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}/questions/{questionId}")]
    public async Task<IActionResult> DeleteQuestion(Guid id, Guid questionId)
    {
        await _mediator.Send(new DeleteQuestionCommand(id, questionId));
        return NoContent();
    }
}