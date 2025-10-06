using System;

namespace API.Models
{
    public class BienCheTreePaging
    {
        public IEnumerable<BienCheTreeDto> Items { get; set; }
        public int TotalRecords { get; set; }
    }
}
