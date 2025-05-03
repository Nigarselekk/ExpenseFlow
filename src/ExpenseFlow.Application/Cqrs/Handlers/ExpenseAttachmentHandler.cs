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

public class ExpenseAttachmentHandler :
    IRequestHandler<CreateExpenseAttachmentCommand, ExpenseAttachmentResponse>,
    IRequestHandler<UpdateExpenseAttachmentCommand, Unit>,
    IRequestHandler<DeleteExpenseAttachmentCommand, Unit>,
    IRequestHandler<GetAllExpenseAttachmentsQuery, List<ExpenseAttachmentResponse>>,
    IRequestHandler<GetExpenseAttachmentByIdQuery, ExpenseAttachmentResponse>
{
    private readonly ExpenseFlowDbContext _db;
    private readonly IMapper _mapper;

    public ExpenseAttachmentHandler(ExpenseFlowDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ExpenseAttachmentResponse> Handle(CreateExpenseAttachmentCommand command, CancellationToken ct)
    {
        
        bool expenseExists = await _db.Expenses
            .AnyAsync(e => e.Id == command.Request.ExpenseId, ct);
        if (!expenseExists)
            throw new KeyNotFoundException("Expense not found.");

        
        var entity = _mapper.Map<ExpenseAttachment>(command.Request);
        entity.Id = Guid.NewGuid();
        entity.UploadedAt = DateTime.UtcNow;

        _db.ExpenseAttachments.Add(entity);
        await _db.SaveChangesAsync(ct);

        return _mapper.Map<ExpenseAttachmentResponse>(entity);
    }

    public async Task<Unit> Handle(UpdateExpenseAttachmentCommand command, CancellationToken cancellationToken)
    {
        var entity = await _db.ExpenseAttachments
            .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException("ExpenseAttachment not found.");

        
        entity.FileUrl  = command.Request.FileUrl;
        entity.FileType = command.Request.FileType;
        

        await _db.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteExpenseAttachmentCommand command, CancellationToken cancellationToken)
    {
        var entity = await _db.ExpenseAttachments
            .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException("ExpenseAttachment not found.");

        _db.ExpenseAttachments.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

    public async Task<List<ExpenseAttachmentResponse>> Handle(GetAllExpenseAttachmentsQuery query, CancellationToken cancellationToken)
    {
        var list = await _db.ExpenseAttachments.ToListAsync(cancellationToken);
        return _mapper.Map<List<ExpenseAttachmentResponse>>(list);
    }

    public async Task<ExpenseAttachmentResponse> Handle(GetExpenseAttachmentByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await _db.ExpenseAttachments
            .FirstOrDefaultAsync(a => a.Id == query.Id, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException("ExpenseAttachment not found.");

        return _mapper.Map<ExpenseAttachmentResponse>(entity);
    }
}
