﻿namespace Model.Application.API.DTO.Response
{
    public class PaginatedResponseDTO<T> where T : class
    {
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public required List<T> Items { get; set; }

    }
}
