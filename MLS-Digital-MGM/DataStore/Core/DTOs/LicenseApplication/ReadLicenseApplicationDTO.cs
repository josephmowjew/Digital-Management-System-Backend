using System;
using System.Collections.Generic;
using DataStore.Core.Models;
using DataStore.Helpers;

namespace DataStore.Core.DTOs.LicenseApplication
{
    public class ReadLicenseApplicationDTO
    {
        public int Id { get; set; }
        public int YearOfOperationId { get; set; }
        public string ApplicationStatus { get; set; } = Lambda.Pending;
        public int CurrentApprovalLevelID { get; set; }
        public int MemberId { get; set; }
        public DataStore.Core.Models.Member Member { get; set; }
        public DataStore.Core.DTOs.YearOfOperation.ReadYearOfOperationDTO YearOfOperation { get; set; } 
        public ICollection<DataStore.Core.Models.Attachment> Attachments { get; set; } = new List<DataStore.Core.Models.Attachment>();
        public int? FirmId { get; set; }
        public Firm Firm { get; set; }


        // Extra properties
        public bool FirstApplicationForLicense { get; set; }
        public bool RenewedLicensePreviousYear { get; set; }
        public bool ObtainedLeaveToRenewLicenseOutOfTime { get; set; }
        public bool PaidAnnualSubscriptionToSociety { get; set; }
        public bool MadeContributionToFidelityFund { get; set; }
        public string ExplanationForNoContributionToFidelityFund { get; set; }
        public bool RemittedSocietysLevy { get; set; }
        public string ExplanationForNoSocietysLevy { get; set; }
        public bool MadeContributionToMLSBuildingProjectFund { get; set; }
        public string ExplanationForNoContributionToMLSBuildingProjectFund { get; set; }
        public bool PerformedFullMandatoryProBonoWork { get; set; }
        public string ExplanationForNoFullMandatoryProBonoWork { get; set; }
        public bool AttainedMinimumNumberOfCLEUnits { get; set; }
        public string ExplanationForNoMinimumNumberOfCLEUnits { get; set; }
        public bool HasValidAnnualProfessionalIndemnityInsuranceCover { get; set; }
        public string ExplanationForNoProfessionalIndemnityInsuranceCover { get; set; }
        public bool SubmittedValidTaxClearanceCertificate { get; set; }
        public string ExplanationForNoValidTaxClearanceCertificate { get; set; }
        public bool SubmittedAccountantsCertificate { get; set; }
        public string ExplanationForNoAccountantsCertificate { get; set; }
        public bool CompliedWithPenaltiesImposedUnderTheAct { get; set; }
        public string ExplanationForNoComplianceWithPenalties { get; set; }
        
    }
}
