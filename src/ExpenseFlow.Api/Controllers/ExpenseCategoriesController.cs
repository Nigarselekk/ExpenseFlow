using Microsoft.AspNetCore.Mvc;
using MediatR;
using ExpenseFlow.Application.Cqrs.Queries;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpenseCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpenseCategoriesController(IMediator mediator)
            => _mediator = mediator;


        [HttpGet()]
        [Authorize(Roles = "Admin,Personnel")]
        public async Task<ActionResult<List<ExpenseCategoryResponse>>> GetAll()
        {
            var list = await _mediator.Send(new GetAllExpenseCategoriesQuery());
            return Ok(list);
        }


        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Personnel")]
        public async Task<ActionResult<ExpenseCategoryResponse>> GetById(int id)
        {
            var expenseCategory = await _mediator.Send(new GetExpenseCategoryByIdQuery(id));
            return Ok(expenseCategory);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ExpenseCategoryResponse>> Post([FromBody] ExpenseCategoryRequest expenseCategoryRequest)
        {
            if (expenseCategoryRequest == null)
            {
                return BadRequest("Expense category request cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(expenseCategoryRequest.Name))
            {
                return BadRequest("Expense category name cannot be empty.");
            }
            if (expenseCategoryRequest.Name.Length > 100 || expenseCategoryRequest.Name.Length < 2)
            {
                return BadRequest("Expense category name must be between 2 and 100 characters.");
            }

            var created = await _mediator.Send(new CreateExpenseCategoryCommand(expenseCategoryRequest));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] ExpenseCategoryRequest request)
        {
            await _mediator.Send(new UpdateExpenseCategoryCommand(id, request));
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteExpenseCategoryCommand(id));
            return NoContent();
        }
    }
}
