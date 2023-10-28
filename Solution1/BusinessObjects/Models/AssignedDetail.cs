using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class AssignedDetail
{
    public string EmployeeId { get; set; } = null!;

    public string TaskId { get; set; } = null!;

    public string AssignedDetailsId { get; set; } = null!;

    public virtual Account Employee { get; set; } = null!;

    public virtual FixTask Task { get; set; } = null!;
}
