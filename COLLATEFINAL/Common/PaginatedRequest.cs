using Microsoft.AspNetCore.Mvc;

namespace COLLATEFINAL.Common
{
    public class PaginatedRequest
    {
        public const int ITEMS_PER_PAGE = 9;

        [FromQuery(Name = "p")]
        public int PageNumber { get; set; } = 1;
        [FromQuery(Name = "s")]
        public string? SearchKeyword { get; set; }

    }
}
