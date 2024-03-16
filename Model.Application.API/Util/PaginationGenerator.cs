using Model.Application.API.DTO;
using Model.Domain.Common;
using Model.Domain.Entities;

namespace Model.Application.API.Util
{
    public static class PaginationGenerator
    {
        public static PaginatedResponseDTO<T> GetPaginatedResponse<T>(PagedResult<T> paginationResult, 
            PaginationParametersDTO parameters) where T : class
        {
            bool hasNextPage = paginationResult.Items.Count() * parameters.PageNumber < paginationResult.TotalCount;

            var response = new PaginatedResponseDTO<T>()
            {
                PageCount = (int)Math.Ceiling((double)paginationResult.TotalCount / parameters.PageSize),
                TotalCount = paginationResult.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                HasPreviousPage = parameters.PageNumber > 1,
                HasNextPage = hasNextPage,
                Items = paginationResult.Items.ToList()
            };

            return response;
        }
    }
}
