using Application.Dtos;
using Domain.Entities;
using Domain.Enum;

namespace Application.Mappings;

public class AuthMapping
{
    public static Users MapRegisterV1ToUserEntity(RegisterRequestDto dto)
    {
        return new Users
        {
            Id = Guid.NewGuid(),
            UserName = dto.UserName,
            Email = dto.Email,
            Phone = dto.PhoneNumber,
            AccountType = Enum.TryParse<AccountTypeStatus>(dto.AccountType, true, out var accountType) ? accountType : AccountTypeStatus.Seller,
            PasswordHash = dto.Password
        };
    }

    public static Users MapRegisterV2ToUserEntity(RegisterRequestDto dto)
    {
        return new Users
        {
            Id = Guid.NewGuid(),
            UserName = dto.UserName,
            Email = dto.Email,
            Phone = dto.PhoneNumber,
            AccountType = Enum.TryParse<AccountTypeStatus>(dto.AccountType, true, out var accountType) ? accountType : AccountTypeStatus.Buyer,
            PasswordHash = dto.Password
        };
    }
    
}