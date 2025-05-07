using MediatR;
using Microsoft.AspNetCore.Mvc;
using ExpenseFlow.Application.Cqrs.Queries.Reports;
using ExpenseFlow.Application.Dtos.Reports;

namespace ExpenseFlow.Api.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReportsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("by-category")]
        public async Task<ActionResult<List<ExpenseByCategoryDto>>> GetByCategory(
            [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var dto = await _mediator.Send(new GetExpensesByCategoryQuery(from, to));
            return Ok(dto);
        }
    }
}
