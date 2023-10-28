using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class AssetHistory
{
    public string HistoryId { get; set; } = null!;

    public string AssetId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual Asset Asset { get; set; } = null!;
}
