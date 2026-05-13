using ims.Shared.Constants;

namespace ims.Application.DTOs;

    public class PagedRequestDto
    {
        public int PageNumber { get; set; } = AppConstants.DefaultPageNumber;
        public int PageSize { get; set; } = AppConstants.DefaultPageSize;
        public string? SearchTerm { get; set; }
    }

