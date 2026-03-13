using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;

namespace Application.ServiceInterfaces
{
    public interface IFeedService
    {
        Task<PagedResponseDto<List<PostResponseDto>>> GetAllFeedsAsync(int pageNumber, int pageSize);
    }
}