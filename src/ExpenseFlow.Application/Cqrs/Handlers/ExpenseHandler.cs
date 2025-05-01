using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Cqrs.Queries;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Infrastructure.DbContext;

namespace ExpenseFlow.Application.Cqrs.Handlers;

    public class ExpenseHandler :
        IRequestHandler<CreateExpenseCommand, ExpenseResponse>,
        IRequestHandler<UpdateExpenseCommand, Unit>,
        IRequestHandler<DeleteExpenseCommand, Unit>,
        IRequestHandler<GetAllExpensesQuery, List<ExpenseResponse>>,
        IRequestHandler<GetExpenseByIdQuery, ExpenseResponse>,
        IRequestHandler<GetExpensesByPersonnelQuery, List<ExpenseResponse>>
    {
        private readonly ExpenseFlowDbContext _db;
        private readonly IMapper _mapper;

        public ExpenseHandler(ExpenseFlowDbContext db, IMapper mapper)
        {
            _db     = db;
            _mapper = mapper;
        }

        // CREATE 
        public async Task<ExpenseResponse> Handle(CreateExpenseCommand cmd, CancellationToken ct)
        {
            var entity = _mapper.Map<Expense>(cmd.Request);
            _db.Expenses.Add(entity);
            await _db.SaveChangesAsync(ct);

            // Load related entities
            await _db.Entry(entity).Reference(x => x.Personnel).LoadAsync(ct);
            await _db.Entry(entity).Reference(x => x.ExpenseCategory).LoadAsync(ct);
            await _db.Entry(entity).Collection(x => x.Attachments).LoadAsync(ct);
            await _db.Entry(entity).Collection(x => x.Transactions).LoadAsync(ct);

            return _mapper.Map<ExpenseResponse>(entity);
        }

        // UPDATE
        public async Task<Unit> Handle(UpdateExpenseCommand cmd, CancellationToken ct)
        {
            var entity = await _db.Expenses.FindAsync(cmd.Id, ct);
            if (entity == null)
                throw new KeyNotFoundException("Expense not found");

            _mapper.Map(cmd.Request, entity);
            await _db.SaveChangesAsync(ct);
            return Unit.Value;
        }

        // DELETE
        public async Task<Unit> Handle(DeleteExpenseCommand cmd, CancellationToken ct)
        {
            var entity = await _db.Expenses.FindAsync(cmd.Id, ct);
            if (entity == null)
                throw new KeyNotFoundException("Expense not found");

            _db.Expenses.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return Unit.Value;
        }

        // GET ALL
        public async Task<List<ExpenseResponse>> Handle(GetAllExpensesQuery req, CancellationToken ct)
        {
            var list = await _db.Expenses
                .Include(x => x.Personnel)
                .Include(x => x.ExpenseCategory)
                .Include(x => x.Attachments)
                .Include(x => x.Transactions)
                .ToListAsync(ct);

            return _mapper.Map<List<ExpenseResponse>>(list);
        }

        // GET BY ID
        public async Task<ExpenseResponse> Handle(GetExpenseByIdQuery req, CancellationToken ct)
        {
            var entity = await _db.Expenses
                .Include(x => x.Personnel)
                .Include(x => x.ExpenseCategory)
                .Include(x => x.Attachments)
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

            if (entity == null)
                throw new KeyNotFoundException("Expense not found");

            return _mapper.Map<ExpenseResponse>(entity);
        }

        // GET BY PERSONNEL
        public async Task<List<ExpenseResponse>> Handle(GetExpensesByPersonnelQuery req, CancellationToken ct)
        {
            var list = await _db.Expenses
                .Where(x => x.PersonnelId == req.PersonnelId)
                .Include(x => x.Personnel)
                .Include(x => x.ExpenseCategory)
                .Include(x => x.Attachments)
                .Include(x => x.Transactions)
                .ToListAsync(ct);

            return _mapper.Map<List<ExpenseResponse>>(list);
        }
    }

