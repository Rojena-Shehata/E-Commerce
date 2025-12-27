using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared
{
    public record  PaginatedResult<TEntity>(int PageIndex, int PageSize, int Count, IEnumerable<TEntity> Data);
//    public class PaginatedResult<TEntity>
//    {
//        public PaginatedResult(int pageIndex, int pageSize, int count, IEnumerable<TEntity> data)
//        {
//            PageIndex = pageIndex;
//            PageSize = pageSize;
//            Count = count;
//            Data = data;
//        }

//        public int PageIndex { get; set; }
//        public int PageSize { get; set; }
//        public int Count { get; set; }//Count of all elements in Data base
//        public IEnumerable<TEntity> Data { get; set; }
//    }
}
