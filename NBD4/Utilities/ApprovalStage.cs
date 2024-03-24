using System.ComponentModel.DataAnnotations;

namespace NBD4.Utilities
{
    public enum ApprovalStage
    {
        [Display(Name="New")]
        NEW,
        [Display(Name = "Waiting for Management")]
        WAITING_FOR_MANAGEMENT,
        [Display(Name = "Waiting for Client")]
        WAITING_FOR_CLIENT,
        [Display(Name = "Approved")]
        APPROVED
    }
}
