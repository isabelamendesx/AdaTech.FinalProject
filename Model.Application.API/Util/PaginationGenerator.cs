using Model.Application.API.DTO;
using Model.Domain.Entities;

namespace Model.Application.API.Util
{
    public static class PaginationGenerator
    {
        public static PaginatedResponseDTO<T> GetPaginatedResponse<T>(PaginationParametersDTO paginationParameters, 
                        IEnumerable<T?> originalList) where T : class
        {
            var paginatedList = originalList
                                    .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                                    .Take(paginationParameters.PageSize)
                                    .ToList();

            bool hasNextPage = paginatedList.Count() * paginationParameters.PageNumber < originalList.Count();
            int totalCount = originalList.Count();

            var response = new PaginatedResponseDTO<T>()
            {
                PageCount = (int)Math.Ceiling((double)totalCount / paginationParameters.PageSize),
                TotalCount = totalCount,
                PageNumber = paginationParameters.PageNumber,
                PageSize = paginationParameters.PageSize,
                HasPreviousPage = paginationParameters.PageNumber > 1,
                HasNextPage = hasNextPage,
                Items = paginatedList
            };

            return response;
        }
    }
}
