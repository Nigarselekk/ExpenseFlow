
using ExpenseFlow.Application.Requests;
using ExpenseFlow.Application.Responses;
using ExpenseFlow.Domain.Entities;
using AutoMapper;

namespace ExpenseFlow.Application.Mapping;
public class MapperProfile : Profile
{
        public MapperProfile()
        {
                // Expense
                CreateMap<ExpenseRequest, Expense>();
                CreateMap<Expense, ExpenseResponse>()
                    .ForMember(e => e.PersonnelName,
                            e => e.MapFrom(p=> $"{p.Personnel.FirstName} {p.Personnel.LastName}"))
                    .ForMember(c => c.CategoryName,
                            c => c.MapFrom(c => c.ExpenseCategory.Name))
                    .ForMember(a => a.Attachments,
                            a => a.MapFrom(s => s.Attachments))
                    .ForMember(t => t.Transactions,
                            t => t.MapFrom(s => s.Transactions));

                //Category 
                CreateMap<ExpenseCategoryRequest, ExpenseCategory>();
                CreateMap<ExpenseCategory, ExpenseCategoryResponse>();

                // Attachment
                CreateMap<ExpenseAttachmentRequest, ExpenseAttachment>();
                CreateMap<ExpenseAttachment, ExpenseAttachmentResponse>();

                //PaymentTransaction 
                CreateMap<PaymentTransactionRequest, PaymentTransaction>();
                CreateMap<PaymentTransaction, PaymentTransactionResponse>();

                //  Personnel
                CreateMap<PersonnelRequest, Personnel>();
                CreateMap<Personnel, PersonnelResponse>();

                // AccountInfo 
                CreateMap<AccountInfoRequest, AccountInfo>();
                CreateMap<AccountInfo, AccountInfoResponse>();
        }
}

