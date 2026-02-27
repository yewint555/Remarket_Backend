using Application.Dtos;
using Domain.Entities;
using Domain.Enum;

namespace Application.Mappings;

public class AuthMapping
{
    public static Users MapRegisterToUserEntity(RegisterRequestDto dto)
    {
        return new Users
        {
            Id = Guid.NewGuid(),
            UserName = dto.UserName,
            Email = dto.Email,
            Phone = dto.PhoneNumber,
            GoogleId = dto.GoogleId,
            AccountType = Enum.TryParse<AccountTypeStatus>(dto.AccountType, true, out var accountType) ? accountType : AccountTypeStatus.Seller,
            PasswordHash = dto.Password
        };
    }
    
}