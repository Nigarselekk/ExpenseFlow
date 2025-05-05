using Microsoft.AspNetCore.Mvc;
using MediatR;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Cqrs.Queries;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpensesController(IMediator mediator)
            => _mediator = mediator;

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
        public Task<ExpenseResponse> Create([FromBody] ExpenseRequest req)
            => _mediator.Send(new CreateExpenseCommand(req));

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Personnel")]
        public Task Put(Guid id, [FromBody] ExpenseRequest req)
            => _mediator.Send(new UpdateExpenseCommand(id, req));

        [HttpDelete("{id:guid}")]
            [Authorize(Roles = "Admin")]
        public Task Delete(Guid id)
            => _mediator.Send(new DeleteExpenseCommand(id));
    }
}
