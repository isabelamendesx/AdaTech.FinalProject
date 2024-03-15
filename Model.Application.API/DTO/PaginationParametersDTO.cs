namespace Model.Application.API.DTO
{
    public class PaginationParametersDTO
    {
        const int MaxPageSize = 50;
        private int _pageSize = 0;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { 
                return _pageSize; 
            }
            set { 
                _pageSize = (value > MaxPageSize) ? MaxPageSize : value; 
            }
        }
    }
}
