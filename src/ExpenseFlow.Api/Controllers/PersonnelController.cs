using Microsoft.AspNetCore.Mvc;
using MediatR;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Cqrs.Queries;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ExpenseFlow.Infrastructure.Identity;

namespace ExpenseFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class PersonnelController : ControllerBase
    {
        private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;

    public PersonnelController(IMediator mediator,
                               UserManager<ApplicationUser> userManager)
    {
        _mediator    = mediator;
        _userManager = userManager;
    }

        [HttpGet]
        public Task<List<PersonnelResponse>> GetAll()
            => _mediator.Send(new GetAllPersonnelQuery());

        [HttpGet("{id:guid}")]
        public Task<PersonnelResponse> GetById(Guid id)
            => _mediator.Send(new GetPersonnelByIdQuery(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonnelRequest request)
    {
        
        var user = new ApplicationUser {
            UserName = request.Email,
            Email    = request.Email,
            FullName = $"{request.FirstName} {request.LastName}"
        };
        var createRes = await _userManager.CreateAsync(user, request.Password);
        if (!createRes.Succeeded)
            return BadRequest(createRes.Errors);
        
        await _userManager.AddToRoleAsync(user, "Personnel");
        
      var result = await _mediator.Send(new CreatePersonnelCommand(user.Id, request));
    
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }


        [HttpPut("{id:guid}")]
        public Task<Unit> Update(Guid id, [FromBody] PersonnelRequest request)
            => _mediator.Send(new UpdatePersonnelCommand(id, request));

        [HttpDelete("{id:guid}")]
        public Task<Unit> Delete(Guid id)
            => _mediator.Send(new DeletePersonnelCommand(id));
    }
}
