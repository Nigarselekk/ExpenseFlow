using Microsoft.AspNetCore.Mvc;
using MediatR;
using ExpenseFlow.Application.Cqrs.Queries;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;

namespace ExpenseFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpenseCategoriesController(IMediator mediator)
            => _mediator = mediator;

        
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<ExpenseCategoryResponse>>> GetAll()
        {
            var list = await _mediator.Send(new GetAllExpenseCategoriesQuery());
            return Ok(list);
        }

        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ExpenseCategoryResponse>> GetById(int id)
        {
            var expenseCategory = await _mediator.Send(new GetExpenseCategoryByIdQuery(id));
            return Ok(expenseCategory);
        }

        
        [HttpPost]
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
        public async Task<IActionResult> Put(int id, [FromBody] ExpenseCategoryRequest req)
        {
            await _mediator.Send(new UpdateExpenseCategoryCommand(id, req));
            return NoContent();
        }

    
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteExpenseCategoryCommand(id));
            return NoContent();
        }
    }
}
