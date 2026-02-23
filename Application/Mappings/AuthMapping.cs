using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class AuthMapping
    {
        public static Users MapRegisterV1ToUserEntity(RegisterV1RequestDto dto)
        {
            return new Users
            {
                Id = Guid.NewGuid(),
                UserName = dto.UserName,
                Email = dto.Email,
                GoogleId = dto.GoogleId,
                AccountType = Enum.P
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };
        }
    }
}