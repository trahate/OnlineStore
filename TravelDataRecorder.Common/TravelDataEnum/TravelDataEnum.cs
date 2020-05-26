using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Common.TravelDataEnum
{
    public enum TravelRequestStatusEnum
    {
        [Display(Name = "submitted by User")]
        SubmittedByUser = 1,
        [Display(Name = "approved by PM")]
        ApprovedByProjectManager = 2,
        [Display(Name = "approved By Project Officer")]
        ApprovedByClient1 = 3,
        [Display(Name = "approved by Chief End User")]
        ApprovedByClient2 = 4,
        [Display(Name = "rejected by Program Manager")]
        RejectedByProjectManager = 5,
        [Display(Name = "rejected by Project Officer")]
        RejectedByClient1 = 6,
        [Display(Name = "rejected by Chief End User")]
        RejectedByClient2 = 7,
        [Display(Name = "Processing")]
        InProcess = 8,
        Rejected = 9,
        [Display(Name = "resubmitted by User")]
        Resubmitted = 10,
    }

    public enum UserRoleEnum
    {
        [Display(Name = "Program Manager")]
        ProjectManager = 1,
        [Display(Name = "User")]
        User = 2,
        [Display(Name = "Client 1")]
        Client1 = 3,
        [Display(Name = "Client 2")]
        Client2 = 4,
    }

    public enum ProcedureEnum
    {
        [Display(Name = "No Actions have been taken yet")]
        Default = 0,
        [Display(Name = "Procedural approval process")]
        Procedural = 1,
        [Display(Name = "Direct approval")]
        DirectFinalStep = 3,
    }

    public enum StepNotesEnum
    {
        [Display(Name = "Travel Request added by user")]
        TravelDetailAdded = 0,
        [Display(Name = "Travel request accepted by PM and procedural flow is selected")]
        TravelDetailAcceptedByPMForProc = 1,
        [Display(Name = "Travel request accepted by PM and sent for direct approval")]
        TravelDetailAcceptedByPMForDirect = 2,
        [Display(Name = "Travel Request rejected by PM")]
        TravelDetailRejectedByPM = 3,
        [Display(Name = "Travel request rejected by Client 1")]
        RejectedByClient1 = 4,
        [Display(Name = "Travel request rejected by Client 2")]
        RejectedByClient2 = 5,
        [Display(Name = "Travel request approved by Client 1")]
        ApprovedByClient1 = 6,
        [Display(Name = "Travel request approved by Client 2")]
        ApprovedByClient2 = 7,
    }

    public enum CriteriaForStringEnum
    {
        [Display(Name = "Select")]
        Select = 0,
        [Display(Name = "Is")]
        Is = 1,
        [Display(Name = "Contains")]
        Contains = 2,
    }
    public enum CriteriaForNumericEnum
    {
        [Display(Name = "Select")]
        Select = 0,
        [Display(Name = "Is")]
        Is = 1,
        [Display(Name = "Less Than")]
        LessThan = 2,
        [Display(Name = "Greater Than")]
        GreaterThan = 3,
    }
    public enum UserStatusEnum
    {
        Active = 1,
        Inactive = 2,
    }

    public enum PieChartEnum
    {
        Submitted = 0,
        Rejected = 1,
        InProgress = 2,
        Approved = 3,
        ReSubmitted=4,
    }
}