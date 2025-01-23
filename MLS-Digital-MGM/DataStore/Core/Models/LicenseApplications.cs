using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Core.DTOs.User;
using DataStore.Helpers;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.Models
{
    public class LicenseApplication : Meta
    {
        public LicenseApplication()
        {
            Attachments = new List<Attachment>();
            LicenseApprovalHistories = new List<LicenseApprovalHistory>();
        }

        public int YearOfOperationId { get; set; }
        public YearOfOperation YearOfOperation { get; set; }
        public string ApplicationStatus  { get; set; } = Lambda.Pending;
        public int CurrentApprovalLevelID { get; set; } 
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public  LicenseApprovalLevel CurrentApprovalLevel { get; set; }
        public int MemberId { get; set; }
        public  Member Member { get; set; }
        public License License { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public virtual List<Attachment> Attachments { get; set; }
        // Extra properties
        public bool FirstApplicationForLicense { get; set; }
        public bool RenewedLicensePreviousYear { get; set; }
        public bool ObtainedLeaveToRenewLicenseOutOfTime { get; set; }
        public bool PaidAnnualSubscriptionToSociety { get; set; }
        public string? ExplanationForNoAnnualSubscriptionToSociety { get; set; }
        public bool MadeContributionToFidelityFund { get; set; }
        [StringLength(maximumLength:250)]
        public string? ExplanationForNoContributionToFidelityFund { get; set; }
        public bool RemittedSocietysLevy { get; set; }
         [StringLength(maximumLength:250)]
        public string? ExplanationForNoSocietysLevy { get; set; }
        public bool MadeContributionToMLSBuildingProjectFund { get; set; }
         [StringLength(maximumLength:250)]
        public string? ExplanationForNoContributionToMLSBuildingProjectFund { get; set; }
        public bool PerformedFullMandatoryProBonoWork { get; set; }
         [StringLength(maximumLength:250)]
        public string? ExplanationForNoFullMandatoryProBonoWork { get; set; }
        public bool AttainedMinimumNumberOfCLEUnits { get; set; }
         [StringLength(maximumLength:250)]
        public string? ExplanationForNoMinimumNumberOfCLEUnits { get; set; }
        public string? ExplanationForMinimumNumberOfCLEUnits { get; set; }
        public bool HasValidAnnualProfessionalIndemnityInsuranceCover { get; set; }
         [StringLength(maximumLength:250)]
        public string? ExplanationForNoProfessionalIndemnityInsuranceCover { get; set; }
        public bool SubmittedValidTaxClearanceCertificate { get; set; }
         [StringLength(maximumLength:250)]
        public string? ExplanationForNoValidTaxClearanceCertificate { get; set; }
        public bool SubmittedAccountantsCertificate { get; set; }
         [StringLength(maximumLength:250)]
        public string? ExplanationForNoAccountantsCertificate { get; set; }
        public bool CompliedWithPenaltiesImposedUnderTheAct { get; set; }
         [StringLength(maximumLength:250)]
        public string? ExplanationForNoComplianceWithPenalties { get; set; }
        public virtual List<LicenseApprovalHistory> LicenseApprovalHistories { get; set; }
        public bool CertificateOfAdmission { get; set; }
        [StringLength(maximumLength: 250)]
        public string? ExplanationForNotSubmittingCertificateOfAdmission { get; set; }



    }


}
