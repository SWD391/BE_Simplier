using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Enums
{
    public class Status
    {
        public enum FeedbackStatus
        {
            Pending,
            Rejected,
            Approved
        }

        public enum AssetStatus
        {
            Functional,
            UnderRepair,
            NonFunctional
        }

        public enum AccountRole
        {
            FeedbackPerson,
            Employee,
            AdministratorStaff,
            AdministratorManager            
        }

        public enum FixTaskStatus
        {
            Pending,
            Rejected,
            Accepted,
            Uncompleted,
            Completed
        }
    }
}
