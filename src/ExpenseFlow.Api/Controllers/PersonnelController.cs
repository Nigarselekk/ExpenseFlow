using Microsoft.AspNetCore.Mvc;
using MediatR;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Cqrs.Queries;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;

namespace ExpenseFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonnelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PersonnelController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet]
        public Task<List<PersonnelResponse>> GetAll()
            => _mediator.Send(new GetAllPersonnelQuery());

        [HttpGet("{id:guid}")]
        public Task<PersonnelResponse> GetById(Guid id)
            => _mediator.Send(new GetPersonnelByIdQuery(id));

        [HttpPost]
        public Task<PersonnelResponse> Create([FromBody] PersonnelRequest req)
            => _mediator.Send(new CreatePersonnelCommand(req));

        [HttpPut("{id:guid}")]
        public Task<Unit> Update(Guid id, [FromBody] PersonnelRequest req)
            => _mediator.Send(new UpdatePersonnelCommand(id, req));

        [HttpDelete("{id:guid}")]
        public Task<Unit> Delete(Guid id)
            => _mediator.Send(new DeletePersonnelCommand(id));
    }
}
