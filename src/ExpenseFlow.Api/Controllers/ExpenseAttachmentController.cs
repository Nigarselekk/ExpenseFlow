
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ExpenseFlow.Application.Cqrs.Queries;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ExpenseFlow.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpenseAttachmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ExpenseAttachmentsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Authorize(Roles = "Admin,Personnel")]
    public Task<List<ExpenseAttachmentResponse>> GetAll()
        => _mediator.Send(new GetAllExpenseAttachmentsQuery());

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Personnel")]
    public Task<ExpenseAttachmentResponse> GetById(Guid id)
        => _mediator.Send(new GetExpenseAttachmentByIdQuery(id));

    [HttpPost]
    [Authorize(Roles = "Personnel")]
    public async Task<IActionResult> Create([FromBody] ExpenseAttachmentRequest req)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var created = await _mediator.Send(new CreateExpenseAttachmentCommand(req));
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Personnel")]
    public Task Update(Guid id, [FromBody] ExpenseAttachmentRequest req)
        => _mediator.Send(new UpdateExpenseAttachmentCommand(id, req));

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,Personnel")]
    public Task Delete(Guid id)
        => _mediator.Send(new DeleteExpenseAttachmentCommand(id));
}
