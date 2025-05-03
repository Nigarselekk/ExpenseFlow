using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ExpenseFlow.Infrastructure.DbContext;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Cqrs.Queries;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Application.Cqrs.Handlers;

public class ExpenseCategoryHandler :
    IRequestHandler<CreateExpenseCategoryCommand, ExpenseCategoryResponse>,
    IRequestHandler<UpdateExpenseCategoryCommand, Unit>,
    IRequestHandler<DeleteExpenseCategoryCommand, Unit>,
    IRequestHandler<GetAllExpenseCategoriesQuery, List<ExpenseCategoryResponse>>,
    IRequestHandler<GetExpenseCategoryByIdQuery, ExpenseCategoryResponse>
{
    private readonly ExpenseFlowDbContext _context;
    private readonly IMapper _mapper;

    public ExpenseCategoryHandler(ExpenseFlowDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ExpenseCategoryResponse> Handle(CreateExpenseCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ExpenseCategory>(command.Request);

        _context.ExpenseCategories.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ExpenseCategoryResponse>(entity);
    }

    public async Task<Unit> Handle(UpdateExpenseCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.ExpenseCategories
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException($"ExpenseCategory not found.");

        _mapper.Map(command.Request, entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteExpenseCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.ExpenseCategories
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException($"ExpenseCategory not found.");

        _context.ExpenseCategories.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

    public async Task<List<ExpenseCategoryResponse>> Handle(GetAllExpenseCategoriesQuery query, CancellationToken cancellationToken)
    {
        var list = await _context.ExpenseCategories.ToListAsync(cancellationToken);
        return _mapper.Map<List<ExpenseCategoryResponse>>(list);
    }

    public async Task<ExpenseCategoryResponse> Handle(GetExpenseCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await _context.ExpenseCategories
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException($"ExpenseCategory not found.");

        return _mapper.Map<ExpenseCategoryResponse>(entity);
    }
}

