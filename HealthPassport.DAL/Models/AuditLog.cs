using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }

        public string TableName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;

        public string PrimaryKey { get; set; } = string.Empty;

        public string? ChangedColumns { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }

        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public object? Tag { get; set; }
    }
}
