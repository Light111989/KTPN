using System;
using System.ComponentModel.DataAnnotations;

namespace API.Domain
{
    public class ExportFilter
    {
        [Required]                 // bắt buộc
        public string Type { get; set; }   // "current" | "history"

        public Guid? KhoiId { get; set; }      // có thể null
        public Guid? LinhVucId { get; set; }   // có thể null

        public string? TenDonVi { get; set; }  // có thể null hoặc ""

        public DateTime? FromDate { get; set; } // có thể null
        public DateTime? ToDate { get; set; }   // có thể null
    }
}
