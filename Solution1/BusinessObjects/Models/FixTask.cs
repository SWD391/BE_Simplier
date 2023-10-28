using System;
using System.Collections.Generic;
using static BusinessObjects.Enums.Status;

namespace BusinessObjects.Models;

public partial class FixTask
{
    public string TaskId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string AuthorId { get; set; } = null!;

    public FixTaskStatus Status { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public DateTime? ProcessedDate { get; set; }

    public string FeedbackId { get; set; } = null!;

    public DateTime Deadline { get; set; }

    public virtual ICollection<AssignedDetail> AssignedDetails { get; set; } = new List<AssignedDetail>();

    public virtual Account Author { get; set; } = null!;

    public virtual Feedback Feedback { get; set; } = null!;
}
