using System;
using System.Collections.Generic;
using System.Linq;

namespace GunStoreIMS.Shared.Dto
{
    /// <summary>
    /// A simple wrapper for paged queries.
    /// </summary>
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? ErrorMessage { get; set; } // ADD THIS PROPERTY

        // Ensure you have this constructor (or similar)
        public PagedResultDto(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        // You might also need a default constructor for some frameworks
        public PagedResultDto()
        {
            Items = new List<T>();
        }
    }
}
