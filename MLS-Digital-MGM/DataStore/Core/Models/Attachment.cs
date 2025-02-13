﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class Attachment: Meta
    {
        public Attachment()
        {
        
            ProBonos = new List<ProBono>();
            ProBonosApplications = new List<ProBonoApplication>();
            MemberQualifications = new List<MemberQualification>();
            LicenseApplications = new List<LicenseApplication>();
            ProBonoReports = new List<ProBonoReport>();
            CPDTrainingRegistrations = new List<CPDTrainingRegistration>();
            Penalties = new List<Penalty>();
            PenaltyPayments = new List<PenaltyPayment>();
            LevyDeclarations = new List<LevyDeclaration>();
            Stamps = new List<Stamp>();
            Signatures = new List<Signature>();
            NotariesPublic = new List<NotaryPublic>();
            CommunicationMessages = new List<CommunicationMessage>();
            GenericSignatures = new List<GenericSignature>();
            SubcommitteeMessages = new List<SubcommitteeMessage>();
            ApplicationUsers = new List<ApplicationUser>();
        }
        
        [Required]
        [StringLength(maximumLength:200)]
        public string FileName { get; set; }
        [Required]
        [StringLength(maximumLength:250)]
        public string FilePath { get; set; }
        [StringLength(maximumLength:200)]
        public string? PropertyName { get; set; }

        //add association to AttachmentType

        public int AttachmentTypeId { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public ICollection<ProBono> ProBonos { get; set; }
        public ICollection<ProBonoApplication> ProBonosApplications { get; set;}
        public ICollection<MemberQualification> MemberQualifications { get; set; } 
        public ICollection<LicenseApplication> LicenseApplications { get; set; }
        public ICollection<ProBonoReport> ProBonoReports { get; set; }
        public ICollection<CPDTraining> CPDTrainings { get; set; }
        public ICollection<CPDTrainingRegistration> CPDTrainingRegistrations { get; set; }
        public ICollection<Penalty> Penalties { get; set; }
        public ICollection<PenaltyPayment> PenaltyPayments { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<LevyDeclaration> LevyDeclarations { get; set; }
        public ICollection<Stamp> Stamps { get; set; }
        public ICollection<Signature> Signatures { get; set; }
        public ICollection<NotaryPublic> NotariesPublic { get; set; }
        public ICollection<CommunicationMessage> CommunicationMessages { get; set; }
        public ICollection<GenericSignature> GenericSignatures { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public ICollection<SubcommitteeMessage> SubcommitteeMessages { get; set; }

    }
}
