using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class PagedList<T>
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public int totalPages => (int)Math.Ceiling((double)totalCount / pageSize);
        public List<T> results { get; set; }

        public PagedList()
        {
            pageNumber = 1;
            pageSize = 10;
            results = new List<T>();
        }

        public PagedList(List<T> items, int pageNumber, int pageSize, int totalCount)
        {
            results = items;
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
            this.totalCount = totalCount;
        }
    }
}
