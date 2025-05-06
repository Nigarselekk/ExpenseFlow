using MediatR;
using ExpenseFlow.Application.Cqrs.Queries;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Responses;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Infrastructure.DbContext;
using ExpenseFlow.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace ExpenseFlow.Application.Cqrs.Handlers
{
    public class PaymentTransactionHandler : 
        IRequestHandler<GetAllPaymentTransactionsQuery, List<PaymentTransactionResponse>>,
        IRequestHandler<GetPaymentTransactionByIdQuery, PaymentTransactionResponse>,
        IRequestHandler<CreatePaymentTransactionCommand, PaymentTransactionResponse>,
        IRequestHandler<UpdatePaymentTransactionCommand, Unit>,
        IRequestHandler<DeletePaymentTransactionCommand, Unit>

    
    {
        private readonly ExpenseFlowDbContext _context;
        private readonly IMapper _mapper;

        public PaymentTransactionHandler(ExpenseFlowDbContext db, IMapper mapper)
        {
            _context = db;
            _mapper = mapper;
        }

        public async Task<List<PaymentTransactionResponse>> Handle(GetAllPaymentTransactionsQuery request, CancellationToken cancellationToken)
            => await _context.PaymentTransactions
                      .AsNoTracking()
                      .ProjectTo<PaymentTransactionResponse>(_mapper.ConfigurationProvider)
                      .ToListAsync(cancellationToken);

        public async Task<PaymentTransactionResponse> Handle(GetPaymentTransactionByIdQuery request, CancellationToken cancellationToken)
            => await _context.PaymentTransactions
                         .AsNoTracking()
                         .ProjectTo<PaymentTransactionResponse>(_mapper.ConfigurationProvider)
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                   ?? throw new KeyNotFoundException("PaymentTransaction not found");

        public async Task<PaymentTransactionResponse> Handle(CreatePaymentTransactionCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<PaymentTransaction>(request.Request);
            _context.PaymentTransactions.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<PaymentTransactionResponse>(entity);
        }

        public async Task<Unit> Handle(UpdatePaymentTransactionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.PaymentTransactions.FindAsync(new object[]{ request.Id }, cancellationToken)
                         ?? throw new KeyNotFoundException("PaymentTransaction not found");
            _mapper.Map(request.Request, entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeletePaymentTransactionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.PaymentTransactions.FindAsync(new object[]{ request.Id }, cancellationToken)
                         ?? throw new KeyNotFoundException("PaymentTransaction not found");
            _context.PaymentTransactions.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}