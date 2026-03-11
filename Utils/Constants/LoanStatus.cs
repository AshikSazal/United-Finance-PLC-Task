using Microsoft.AspNetCore.Mvc.Rendering;

namespace Loan_Procedure.Utils.Constants
{
    public enum LoanStatus : byte
    {
        Draft = 0,
        Submitted = 1,
        Approved = 2,
        Rejected = 3
    }
    public static class LoanStatusList
    {
        public static readonly List<SelectListItem> Status = new()
        {
            new() { Value = ((byte)LoanStatus.Draft).ToString(), Text = "Draft" },
            new() { Value = ((byte)LoanStatus.Submitted).ToString(), Text = "Submitted" },
            new() { Value = ((byte)LoanStatus.Approved).ToString(), Text = "Approved" },
            new() { Value = ((byte)LoanStatus.Rejected).ToString(), Text = "Rejected" }
        };
    }
}
