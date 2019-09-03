using System;
using System.Collections.Generic;
using System.Text;

namespace Zrk.Common.Paged
{
    public class PagedResultDto<T>
    {
        public int TotalCount { get; set; }
        public IReadOnlyList<T> Items
        {
            get { return _items ?? (_items = new List<T>()); }
            set { _items = value; }
        }
        private IReadOnlyList<T> _items;

        public PagedResultDto(int totalCount, IReadOnlyList<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}
