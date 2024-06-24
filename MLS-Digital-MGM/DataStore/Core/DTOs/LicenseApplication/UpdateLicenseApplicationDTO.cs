using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.LicenseApplication
{
    public class UpdateLicenseApplicationDTO
    {
        public int Id { get; set; }

        public int YearOfOperationId { get; set; }

        [Required]
        [StringLength(100)]
        public string ApplicationStatus { get; set; }

        public int CurrentApprovalLevelID { get; set; }

        public int MemberId { get; set; }

        public bool FirstApplicationForLicense { get; set; }
        public bool RenewedLicensePreviousYear { get; set; }
        public bool ObtainedLeaveToRenewLicenseOutOfTime { get; set; }
        public bool PaidAnnualSubscriptionToSociety { get; set; }
        [StringLength(250)]
        public string? ExplanationForNoAnnualSubscriptionToSociety { get; set; }
        public bool MadeContributionToFidelityFund { get; set; }
        [StringLength(250)]
        public string ExplanationForNoContributionToFidelityFund { get; set; }
        public bool RemittedSocietysLevy { get; set; }
        [StringLength(250)]
        public string ExplanationForNoSocietysLevy { get; set; }
        public bool MadeContributionToMLSBuildingProjectFund { get; set; }
        [StringLength(250)]
        public string ExplanationForNoContributionToMLSBuildingProjectFund { get; set; }
        public bool PerformedFullMandatoryProBonoWork { get; set; }
        [StringLength(250)]
        public string ExplanationForNoFullMandatoryProBonoWork { get; set; }
        public bool AttainedMinimumNumberOfCLEUnits { get; set; }
        [StringLength(250)]
        public string ExplanationForNoMinimumNumberOfCLEUnits { get; set; }
        public bool HasValidAnnualProfessionalIndemnityInsuranceCover { get; set; }
        [StringLength(250)]
        public string ExplanationForNoProfessionalIndemnityInsuranceCover { get; set; }
        public bool SubmittedValidTaxClearanceCertificate { get; set; }
        [StringLength(250)]
        public string ExplanationForNoValidTaxClearanceCertificate { get; set; }
        public bool SubmittedAccountantsCertificate { get; set; }
        [StringLength(250)]
        public string ExplanationForNoAccountantsCertificate { get; set; }
        public bool CompliedWithPenaltiesImposedUnderTheAct { get; set; }
        [StringLength(250)]
        public string ExplanationForNoComplianceWithPenalties { get; set; }
        public bool CertificateOfAdmission { get; set; }
        [StringLength(maximumLength: 250)]
        public string? ExplanationForNotSubmittingCertificateOfAdmission { get; set; }
    }
}
