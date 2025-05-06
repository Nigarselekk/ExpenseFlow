
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
                    .ForMember(dest => dest.PersonnelName,
                        opt => opt.MapFrom(src => $"{src.Personnel.FirstName} {src.Personnel.LastName}"))
                    .ForMember(dest => dest.CategoryName,
                        opt => opt.MapFrom(src => src.ExpenseCategory.Name))
                    .ForMember(dest => dest.Attachments,
                        opt => opt.MapFrom(src => src.Attachments))
                    .ForMember(dest => dest.Transactions,
                        opt => opt.MapFrom(src => src.Transactions));


                //Category 
                CreateMap<ExpenseCategoryRequest, ExpenseCategory>();
                CreateMap<ExpenseCategory, ExpenseCategoryResponse>();

                // Attachment
                CreateMap<ExpenseAttachmentRequest, ExpenseAttachment>();
                CreateMap<ExpenseAttachment, ExpenseAttachmentResponse>();

                //PaymentTransaction 
                CreateMap<PaymentTransactionRequest, PaymentTransaction>();
                CreateMap<PaymentTransaction, PaymentTransactionResponse>();

                CreateMap<PersonnelRequest, Personnel>();

                CreateMap<Personnel, PersonnelResponse>()
                    .ForMember(dest => dest.ApplicationUserId,
                               opt => opt.MapFrom(src => src.ApplicationUserId));

                CreateMap<PersonnelRequest, Personnel>();
                CreateMap<Personnel, PersonnelResponse>();

                // AccountInfo 
                CreateMap<AccountInfoRequest, AccountInfo>();
                CreateMap<AccountInfo, AccountInfoResponse>();
        }
}

