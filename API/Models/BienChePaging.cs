using System;

namespace API.Models
{
    public class BienChePaging
    {
        public IEnumerable<BienCheTreeDto> Items { get; set; }
    public int TotalRecords { get; set; }
    }
}
