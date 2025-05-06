using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using ExpenseFlow.Application.Cqrs.Queries;


namespace ExpenseFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ExpensesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public Task<List<ExpenseResponse>> GetAll()
            => _mediator.Send(new GetAllExpensesQuery());

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Personnel")]
        public Task<ExpenseResponse> GetById(Guid id)
            => _mediator.Send(new GetExpenseByIdQuery(id));

        // [HttpGet("ByPersonnel/{personnelId:guid}")]
        // public Task<List<ExpenseResponse>> GetByPersonnel(Guid personnelId)
        //     => _mediator.Send(new GetExpensesByPersonnelQuery(personnelId));

        [HttpPost]
        [Authorize(Roles = "Personnel")]
        public async Task<IActionResult> Create([FromBody] ExpenseRequest expenseRequest)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            Guid personnelId = Guid.Parse(userId);
            expenseRequest.PersonnelId = personnelId;

            var expense = await _mediator.Send(new CreateExpenseCommand(expenseRequest.PersonnelId, expenseRequest));
            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Personnel")]
        public Task Put(Guid id, [FromBody] ExpenseRequest expenseRequest)
            => _mediator.Send(new UpdateExpenseCommand(id, expenseRequest));

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public Task Delete(Guid id)
            => _mediator.Send(new DeleteExpenseCommand(id));
    }
}
