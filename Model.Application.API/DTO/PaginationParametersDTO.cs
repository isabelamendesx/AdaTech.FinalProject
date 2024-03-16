namespace Model.Application.API.DTO
{
    public class PaginationParametersDTO
    {
        const int MaxPageSize = 50;
        private int _pageSize;

        public int PageNumber { get; set; }
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
