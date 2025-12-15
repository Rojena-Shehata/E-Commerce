using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.ProductDTOs
{
    public class ProductQueryParameters
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Search { get; set; }//SearchByName
        public ProductSortingOptions Sort { get; set; }
        private int _pageIndex=1;

        public int PageNumber
        {
            get {
                return _pageIndex; 
            }
            set 
            {
                _pageIndex = (value <= 0) ? 1 : value;
                    
            }
        }

        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;
        private int _pageSize = 5;

        public int PageSize
        {
            get {
                return _pageSize; 
            }
            set 
            {
                _pageSize = (value <= 0) ? 5 :value>MaxPageSize?MaxPageSize :value;
                    
            }
        }



    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
   public enum ProductSortingOptions
    {
        NameAsc=1,
        NameDesc=2,
        PriceAsc=3,
        PriceDesc=4,

    }
}
