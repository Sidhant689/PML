using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Shared.DTOs.Master.Menu
{
    public class MenuMasterDto
    {
        public long SlNo { get; set; }
        public string? Module { get; set; }
        public string? MenuCode { get; set; }
        public string? MenuText { get; set; }
        public string? FormName { get; set; }
        public string? MenuHandle { get; set; }
        public string? Handle { get; set; }
        public string? ParentCode { get; set; }
        public bool AddMenuSeparator { get; set; }
        public bool HasChild { get; set; }
        public string? ShortcutKey { get; set; }
        public string? TransactionType { get; set; }
        public string? ToolTip { get; set; }
        public bool ShortcutMenuEnable { get; set; }
        public bool IsCombineTransactionMenu { get; set; }
        public string? ImageName { get; set; }
        public string? SmallIcon { get; set; }
        public string? MediumIcon { get; set; }
        public string? LargeIcon { get; set; }
        public bool DefinedByUser { get; set; }
        public bool IsInActive { get; set; }
    }
}
