using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ExpenseFlow.Infrastructure.DbContext;
using ExpenseFlow.Application.Cqrs.Commands;
using ExpenseFlow.Application.Cqrs.Queries;
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Application.Cqrs.Handlers
{
    public class AccountInfoHandler :
        IRequestHandler<CreateAccountInfoCommand, AccountInfoResponse>,
        IRequestHandler<UpdateAccountInfoCommand, Unit>,
        IRequestHandler<DeleteAccountInfoCommand, Unit>,
        IRequestHandler<GetAllAccountInfosQuery, List<AccountInfoResponse>>,
        IRequestHandler<GetAccountInfoByIdQuery, AccountInfoResponse>
    {
        private readonly ExpenseFlowDbContext _db;
        private readonly IMapper _mapper;

        public AccountInfoHandler(ExpenseFlowDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<AccountInfoResponse> Handle(CreateAccountInfoCommand command, CancellationToken cancellationToken)
        {
            bool personnelExists = await _db.Personnels
                .AnyAsync(p => p.Id == command.Request.PersonnelId, cancellationToken);
            if (!personnelExists)
                throw new KeyNotFoundException("Personnel not found.");

            bool exists = await _db.AccountInfos
                .AnyAsync(a => a.IBAN == command.Request.IBAN, cancellationToken);
            if (exists)
                throw new InvalidOperationException("AccountInfo with this IBAN already exists.");


            var entity = _mapper.Map<AccountInfo>(command.Request);
            entity.Id = Guid.NewGuid();

            _db.AccountInfos.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AccountInfoResponse>(entity);
        }

        public async Task<Unit> Handle(UpdateAccountInfoCommand command, CancellationToken cancellationToken)
        {
            var entity = await _db.AccountInfos
                .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException("AccountInfo not found.");

            _mapper.Map(command.Request, entity);

            await _db.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteAccountInfoCommand command, CancellationToken cancellationToken)
        {
            var entity = await _db.AccountInfos
                .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException("AccountInfo not found.");

            _db.AccountInfos.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        public async Task<List<AccountInfoResponse>> Handle(GetAllAccountInfosQuery query, CancellationToken cancellationToken)
        {
            var list = await _db.AccountInfos.ToListAsync(cancellationToken);
            return _mapper.Map<List<AccountInfoResponse>>(list);
        }

        public async Task<AccountInfoResponse> Handle(GetAccountInfoByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await _db.AccountInfos
                .FirstOrDefaultAsync(a => a.Id == query.Id, cancellationToken);

            if (entity == null)
                throw new KeyNotFoundException("AccountInfo not found.");

            return _mapper.Map<AccountInfoResponse>(entity);
        }
    }
}
