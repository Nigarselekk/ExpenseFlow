using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using ExpenseFlow.Application.Cqrs.Queries;

namespace ExpenseFlow.Api.Controllers;

public class AccountInfosController : ControllerBase 
{
        private readonly IMediator _mediator;
        public AccountInfosController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Authorize(Roles = "Admin,Personnel")]
        public Task<List<AccountInfoResponse>> GetAll() => 
            _mediator.Send(new GetAllAccountInfosQuery());

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Personnel")]
        public Task<AccountInfoResponse> GetById(Guid id) =>
            _mediator.Send(new GetAccountInfoByIdQuery(id));

        [HttpPost]
        [Authorize(Roles = "Personnel")]
        public async Task<IActionResult> Create([FromBody] AccountInfoRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            
            request.PersonnelId = Guid.Parse(userId);

            var created = await _mediator.Send(new CreateAccountInfoCommand(request));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Personnel")]
        public Task Update(Guid id, [FromBody] AccountInfoRequest request)
            => _mediator.Send(new UpdateAccountInfoCommand(id, request));

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public Task Delete(Guid id)
            => _mediator.Send(new DeleteAccountInfoCommand(id));
}

