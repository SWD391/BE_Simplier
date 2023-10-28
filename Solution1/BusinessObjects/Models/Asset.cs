using System;
using System.Collections.Generic;
using static BusinessObjects.Enums.Status;

namespace BusinessObjects.Models;

public partial class Asset
{
    public string AssetId { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string Type { get; set; } = null!;

    public AssetStatus Status { get; set; }

    public double Price { get; set; }

    public DateTime ImportedDate { get; set; }

    public string Description { get; set; } = null!;

    public string ImporterId { get; set; } = null!;

    public string Location { get; set; } = null!;

    public virtual ICollection<AssetHistory> AssetHistories { get; set; } = new List<AssetHistory>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
