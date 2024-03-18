using Model.Application.API.DTO.Request;
using Model.Application.API.DTO.Response;
using Model.Domain.Common;

namespace Model.Application.API.Util
{
    public static class PaginationResponseGenerator
    {
        public static PaginatedResponseDTO<T> GetPaginatedResponse<T>(PaginatedResult<T> paginationResult, 
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
