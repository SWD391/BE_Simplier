using System;
using System.Collections.Generic;
using static BusinessObjects.Enums.Status;

namespace BusinessObjects.Models;

public partial class Feedback
{
    public string FeedbackId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string AssetId { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public FeedbackStatus Status { get; set; }

    public DateTime? SubmitedDate { get; set; }

    public string CreatorId { get; set; } = null!;

    public virtual Asset Asset { get; set; } = null!;

    public virtual ICollection<FixTask> FixTasks { get; set; } = new List<FixTask>();
}
