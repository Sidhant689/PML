using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Shared.Entities.Models.Master
{
    public class MenuMaster
    {
        [Key]
        [Column("SLNO")]
        public long SlNo { get; set; }

        [Column("MODULE")]
        [StringLength(20)]
        public string? Module { get; set; }

        [Column("MenuCode")]
        [StringLength(9)]
        public string? MenuCode { get; set; }

        [Column("MENU_Text")]
        [StringLength(200)]
        public string? MenuText { get; set; }

        [Column("Form_NAME")]
        [StringLength(100)]
        public string? FormName { get; set; }

        [Column("MENU_HANDLE")]
        [StringLength(100)]
        public string? MenuHandle { get; set; }

        [Column("HANDLE")]
        [StringLength(100)]
        public string? Handle { get; set; }

        [Column("PAR_Code")]
        [StringLength(9)]
        public string? ParentCode { get; set; }

        [Column("ADD_Menu_Separator")]
        public bool AddMenuSeparator { get; set; }

        [Column("HasChild")]
        public bool HasChild { get; set; }

        [Column("SHORTCUTKEY")]
        [StringLength(50)]
        public string? ShortcutKey { get; set; }

        [Column("TxNTYPE")]
        [StringLength(50)]
        public string? TransactionType { get; set; }

        [Column("ToolTip")]
        [StringLength(300)]
        public string? ToolTip { get; set; }

        [Column("ShortcutMenuEnable")]
        public bool ShortcutMenuEnable { get; set; }

        [Column("IsCombineTxN_Menu")]
        public bool IsCombineTransactionMenu { get; set; }

        [Column("ImageName")]
        [StringLength(200)]
        public string? ImageName { get; set; }

        [Column("Small_Icon")]
        [StringLength(200)]
        public string? SmallIcon { get; set; }

        [Column("Medium_Icon")]
        [StringLength(200)]
        public string? MediumIcon { get; set; }

        [Column("Large_Icon")]
        [StringLength(200)]
        public string? LargeIcon { get; set; }

        [Column("DefinedByUser")]
        public bool DefinedByUser { get; set; }

        [Column("IsInActive")]
        public bool IsInActive { get; set; }
    }
}
