using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Helpers;

namespace DataStore.Core.Models
{
    public class LicenseApplication : Meta
    {
        public LicenseApplication()
        {
            Attachments = new List<Attachment>();
        }

        public int YearOfOperationId { get; set; }
        public YearOfOperation YearOfOperation { get; set; }

        public string ApplicationStatus  { get; set; } = Lambda.Pending;

        public int CurrentApprovalLevelID { get; set; }

        public  LicenseApprovalLevel CurrentApprovalLevel { get; set; }

        public int MemberId { get; set; }
        public  Member Member { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }

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
