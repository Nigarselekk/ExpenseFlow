using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ExpenseFlow.Infrastructure.DbContext;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Cqrs.Queries;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using ExpenseFlow.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ExpenseFlow.Application.Cqrs.Handlers;

    public class PersonnelHandler :
        IRequestHandler<CreatePersonnelCommand, PersonnelResponse>,
        IRequestHandler<UpdatePersonnelCommand, Unit>,
        IRequestHandler<DeletePersonnelCommand, Unit>,
        IRequestHandler<GetAllPersonnelQuery, List<PersonnelResponse>>,
        IRequestHandler<GetPersonnelByIdQuery, PersonnelResponse>
    {
        private readonly ExpenseFlowDbContext _context;
        private readonly IMapper             _mapper;

        public PersonnelHandler(ExpenseFlowDbContext db, IMapper mapper)
        {
            _context     = db;
            _mapper = mapper;
        }

        public async Task<PersonnelResponse> Handle(CreatePersonnelCommand command, CancellationToken ct)
        {
            var entity = _mapper.Map<Personnel>(command.Request);
            entity.Id = Guid.NewGuid();
            _context.Personnels.Add(entity);
            await _context.SaveChangesAsync(ct);
            return _mapper.Map<PersonnelResponse>(entity);
        }

        public async Task<Unit> Handle(UpdatePersonnelCommand command, CancellationToken ct)
        {
            var entity = await _context.Set<Personnel>().FirstOrDefaultAsync(x => x.Id == command.Id, ct);
            if (entity == null) throw new KeyNotFoundException("Personnel not found.");

            // Check if the personnel has any expenses before updating
            // var hasExpenses = await _context.Set<Expense>()
            //     .AnyAsync(e => e.PersonnelId == command.Id, ct);
            // if (hasExpenses)
            // {
            //     throw new InvalidOperationException("Cannot update personnel with existing expenses.");
            // }
            _mapper.Map(command.Request, entity);

            await _context.SaveChangesAsync(ct);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeletePersonnelCommand command, CancellationToken ct)
        {

            var entity = await _context.Set<Personnel>().FirstOrDefaultAsync(x => x.Id == command.Id, ct);
            if (entity == null) throw new KeyNotFoundException("Personnel not found.");

            // Check if the personnel has any expenses before deleting
            var hasExpenses = await _context.Set<Expense>()
                .AnyAsync(e => e.PersonnelId == command.Id, ct);
            if (hasExpenses)
            {
                throw new InvalidOperationException("Cannot delete personnel with existing expenses.");
            }
            _context.Personnels.Remove(entity);
            await _context.SaveChangesAsync(ct);
            return Unit.Value;
        }

        public async Task<List<PersonnelResponse>> Handle(GetAllPersonnelQuery query, CancellationToken ct)
        {
            var entities = await _context.Set<Personnel>().ToListAsync(ct);
            return _mapper.Map<List<PersonnelResponse>>(entities);
        }

        // public async Task<PersonnelResponse> Handle(GetPersonnelByIdQuery query, CancellationToken ct)
        // {
        //     var entity = await _context.Personnels.FindAsync(new object[]{ query.Id }, ct);
        //     if (entity == null) throw new KeyNotFoundException("Personnel not found.");
        //     return _mapper.Map<PersonnelResponse>(entity);
        // }

        public async Task<PersonnelResponse> Handle(GetPersonnelByIdQuery query, CancellationToken ct)
        {
            var entity = await _context.Set<Personnel>()
                .Include(p => p.Accounts)
                .Include(p => p.Expenses)
                .FirstOrDefaultAsync(p => p.Id == query.Id, ct);

            if (entity == null) throw new KeyNotFoundException("Personnel not found.");
            return _mapper.Map<PersonnelResponse>(entity);
        }
    }


