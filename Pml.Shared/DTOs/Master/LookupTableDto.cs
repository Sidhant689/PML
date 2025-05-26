using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Shared.DTOs.Master
{
    public class LookupTableDto
    {
        public string TableName { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; } // Optional: Human-readable purpose
    }
    public class MenuMasterDto
    {
        public string Module { get; set; }
        public string MenuText { get; set; }
        public long ParCode { get; set; } = 0;
        public string? FormName { get; set; }
        public string? MenuHandle { get; set; }
        public bool AddMenuSeparator { get; set; } = false;
        public bool HasChild { get; set; } = false;
        public string? Lvl { get; set; }
        public int TxNType { get; set; } = 0;
        public string? ToolTip { get; set; }
        public bool IsShortcutMenuEnable { get; set; } = false;
        public bool IsCombineTxnMenu { get; set; } = false;
        public bool DefinedByUser { get; set; } = false;
        public bool IsInactive { get; set; } = false;
        public string? UrlPath { get; set; }
        public int SortOrder { get; set; } = 0;
    }
    public class ShortcutKeyMasterDto
    {
        public string Description { get; set; }
        public int? ControlKey { get; set; }
        public int? Keys { get; set; }
    }
    public class MenuShortcutMappingDto
    {
        public long MenuCode { get; set; }
        public int ShortcutKey { get; set; }
        public bool IsActive { get; set; } = true;
    }
    public class AssetMasterDto
    {
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string AssetType { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public int? FileSize { get; set; }
        public string? MimeType { get; set; }
        public bool IsActive { get; set; } = true;
    }
    public class MenuAssetMappingDto
    {
        public long MenuCode { get; set; }
        public string? ImageName { get; set; }
        public string? SmallIcon { get; set; }
        public string? MediumIcon { get; set; }
        public string? LargeIcon { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
