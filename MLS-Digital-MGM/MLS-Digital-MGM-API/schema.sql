CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

CREATE TABLE `AttachmentTypes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(200) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Countries` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(100) NOT NULL,
    `ShortCode` varchar(10) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Departments` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(50) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Firms` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(100) NOT NULL,
    `PostalAddress` varchar(250) NOT NULL,
    `PhysicalAddress` varchar(250) NOT NULL,
    `PrimaryContactPerson` varchar(100) NOT NULL,
    `PrimaryPhoneNumber` varchar(15) NOT NULL,
    `SecondaryContactPerson` varchar(100) NOT NULL,
    `SecondaryPhoneNumber` varchar(15) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `IdentityTypes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(150) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `QualificationTypes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(100) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Roles` (
    `Id` varchar(85) NOT NULL,
    `Discriminator` varchar(13) NOT NULL,
    `CreatedDate` datetime(6) NULL,
    `CreatedById` longtext NULL,
    `Status` longtext NULL,
    `UpdatedDate` datetime(6) NULL,
    `DeletedDate` datetime(6) NULL,
    `Name` varchar(256) NULL,
    `NormalizedName` varchar(85) NULL,
    `ConcurrencyStamp` longtext NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Titles` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(50) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `YearOfOperations` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `StartDate` datetime(6) NOT NULL,
    `EndDate` datetime(6) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Attachments` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `FileName` varchar(200) NOT NULL,
    `FilePath` varchar(250) NOT NULL,
    `AttachmentTypeId` int NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Attachments_AttachmentTypes_AttachmentTypeId` FOREIGN KEY (`AttachmentTypeId`) REFERENCES `AttachmentTypes` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `LicenseApprovalLevels` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Level` int NOT NULL,
    `DepartmentId` int NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_LicenseApprovalLevels_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `Departments` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `RoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(200) NOT NULL,
    `ClaimType` longtext NULL,
    `ClaimValue` longtext NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RoleClaims_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Users` (
    `Id` varchar(200) NOT NULL,
    `FirstName` varchar(100) NOT NULL,
    `LastName` varchar(100) NOT NULL,
    `OtherName` varchar(100) NOT NULL,
    `Gender` varchar(15) NOT NULL,
    `PhoneNumber` varchar(15) NOT NULL,
    `IdentityNumber` varchar(50) NOT NULL,
    `IdentityExpiryDate` datetime(6) NOT NULL,
    `DateOfBirth` datetime(6) NOT NULL,
    `DepartmentId` int NOT NULL,
    `IdentityTypeId` int NOT NULL,
    `TitleId` int NOT NULL,
    `CountryId` int NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `CreatedById` longtext NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    `LastLogin` datetime(6) NOT NULL,
    `Pin` int NULL,
    `FirmId` int NULL,
    `UserName` varchar(256) NULL,
    `NormalizedUserName` varchar(200) NULL,
    `Email` varchar(256) NULL,
    `NormalizedEmail` varchar(200) NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext NULL,
    `SecurityStamp` longtext NULL,
    `ConcurrencyStamp` varchar(200) NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Users_Countries_CountryId` FOREIGN KEY (`CountryId`) REFERENCES `Countries` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Users_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `Departments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Users_Firms_FirmId` FOREIGN KEY (`FirmId`) REFERENCES `Firms` (`Id`),
    CONSTRAINT `FK_Users_IdentityTypes_IdentityTypeId` FOREIGN KEY (`IdentityTypeId`) REFERENCES `IdentityTypes` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Users_Titles_TitleId` FOREIGN KEY (`TitleId`) REFERENCES `Titles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `ErrorLogs` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserFriendlyMessage` longtext NOT NULL,
    `DetailedMessage` longtext NOT NULL,
    `DateOccurred` datetime(6) NOT NULL,
    `CreatedById` varchar(200) NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ErrorLogs_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`)
);

CREATE TABLE `Members` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(200) NOT NULL,
    `PostalAddress` varchar(250) NOT NULL,
    `PermanentAddress` varchar(250) NOT NULL,
    `ResidentialAddress` varchar(250) NOT NULL,
    `DateOfAdmissionToPractice` datetime(6) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Members_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `ProbonoClients` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(50) NOT NULL,
    `NationalId` varchar(200) NOT NULL,
    `PostalAddress` varchar(250) NOT NULL,
    `PermanentAddress` varchar(250) NOT NULL,
    `ResidentialAddress` varchar(250) NOT NULL,
    `PhoneNumber` varchar(20) NOT NULL,
    `OtherContacts` varchar(150) NOT NULL,
    `ApprovedDate` datetime(6) NOT NULL,
    `Occupation` varchar(150) NOT NULL,
    `AnnualIncome` decimal(18,2) NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ProbonoClients_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(200) NOT NULL,
    `ClaimType` longtext NULL,
    `ClaimValue` longtext NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_UserClaims_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserLogins` (
    `LoginProvider` varchar(85) NOT NULL,
    `ProviderKey` varchar(85) NOT NULL,
    `ProviderDisplayName` varchar(85) NULL,
    `UserId` varchar(200) NOT NULL,
    PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_UserLogins_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserRoles` (
    `UserId` varchar(85) NOT NULL,
    `RoleId` varchar(85) NOT NULL,
    PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_UserRoles_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_UserRoles_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserTokens` (
    `UserId` varchar(85) NOT NULL,
    `LoginProvider` varchar(85) NOT NULL,
    `Name` varchar(85) NOT NULL,
    `Value` varchar(200) NULL,
    PRIMARY KEY (`UserId`, `Name`),
    CONSTRAINT `FK_UserTokens_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `LicenseApplications` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `YearOfOperationId` int NOT NULL,
    `ApplicationStatus` longtext NOT NULL,
    `CurrentApprovalLevelID` int NOT NULL,
    `MemberId` int NOT NULL,
    `FirstApplicationForLicense` tinyint(1) NOT NULL,
    `RenewedLicensePreviousYear` tinyint(1) NOT NULL,
    `ObtainedLeaveToRenewLicenseOutOfTime` tinyint(1) NOT NULL,
    `PaidAnnualSubscriptionToSociety` tinyint(1) NOT NULL,
    `MadeContributionToFidelityFund` tinyint(1) NOT NULL,
    `ExplanationForNoContributionToFidelityFund` longtext NOT NULL,
    `RemittedSocietysLevy` tinyint(1) NOT NULL,
    `ExplanationForNoSocietysLevy` longtext NOT NULL,
    `MadeContributionToMLSBuildingProjectFund` tinyint(1) NOT NULL,
    `ExplanationForNoContributionToMLSBuildingProjectFund` longtext NOT NULL,
    `PerformedFullMandatoryProBonoWork` tinyint(1) NOT NULL,
    `ExplanationForNoFullMandatoryProBonoWork` longtext NOT NULL,
    `AttainedMinimumNumberOfCLEUnits` tinyint(1) NOT NULL,
    `ExplanationForNoMinimumNumberOfCLEUnits` longtext NOT NULL,
    `HasValidAnnualProfessionalIndemnityInsuranceCover` tinyint(1) NOT NULL,
    `ExplanationForNoProfessionalIndemnityInsuranceCover` longtext NOT NULL,
    `SubmittedValidTaxClearanceCertificate` tinyint(1) NOT NULL,
    `ExplanationForNoValidTaxClearanceCertificate` longtext NOT NULL,
    `SubmittedAccountantsCertificate` tinyint(1) NOT NULL,
    `ExplanationForNoAccountantsCertificate` longtext NOT NULL,
    `CompliedWithPenaltiesImposedUnderTheAct` tinyint(1) NOT NULL,
    `ExplanationForNoComplianceWithPenalties` longtext NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_LicenseApplications_LicenseApprovalLevels_CurrentApprovalLev~` FOREIGN KEY (`CurrentApprovalLevelID`) REFERENCES `LicenseApprovalLevels` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LicenseApplications_Members_MemberId` FOREIGN KEY (`MemberId`) REFERENCES `Members` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LicenseApplications_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `MemberQualifications` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext NOT NULL,
    `MemberId` int NOT NULL,
    `IssuingInstitution` varchar(250) NOT NULL,
    `DateObtained` date NOT NULL,
    `QualificationTypeId` int NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_MemberQualifications_Members_MemberId` FOREIGN KEY (`MemberId`) REFERENCES `Members` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_MemberQualifications_QualificationTypes_QualificationTypeId` FOREIGN KEY (`QualificationTypeId`) REFERENCES `QualificationTypes` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `MemberQualificationType` (
    `MembersId` int NOT NULL,
    `QualificationTypesId` int NOT NULL,
    PRIMARY KEY (`MembersId`, `QualificationTypesId`),
    CONSTRAINT `FK_MemberQualificationType_Members_MembersId` FOREIGN KEY (`MembersId`) REFERENCES `Members` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_MemberQualificationType_QualificationTypes_QualificationType~` FOREIGN KEY (`QualificationTypesId`) REFERENCES `QualificationTypes` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `ProBonoApplications` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `NatureOfDispute` varchar(200) NOT NULL,
    `CaseDetails` longtext NOT NULL,
    `CreatedById` varchar(200) NULL,
    `ProbonoClientId` int NOT NULL,
    `ApplicationStatus` longtext NOT NULL,
    `ApprovedDate` datetime(6) NULL,
    `DenialReason` varchar(200) NULL,
    `SummaryOfDispute` longtext NOT NULL,
    `YearOfOperationId` int NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ProBonoApplications_ProbonoClients_ProbonoClientId` FOREIGN KEY (`ProbonoClientId`) REFERENCES `ProbonoClients` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_ProBonoApplications_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`),
    CONSTRAINT `FK_ProBonoApplications_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AttachmentLicenseApplication` (
    `AttachmentsId` int NOT NULL,
    `LicenseApplicationsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `LicenseApplicationsId`),
    CONSTRAINT `FK_AttachmentLicenseApplication_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AttachmentLicenseApplication_LicenseApplications_LicenseAppl~` FOREIGN KEY (`LicenseApplicationsId`) REFERENCES `LicenseApplications` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `LicenseApplicationApprovals` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `LicenseApplicationID` int NOT NULL,
    `LicenseApprovalLevelID` int NOT NULL,
    `Approved` tinyint(1) NOT NULL,
    `Reason_for_Rejection` varchar(250) NOT NULL,
    `CreatedById` varchar(200) NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_LicenseApplicationApprovals_LicenseApplications_LicenseAppli~` FOREIGN KEY (`LicenseApplicationID`) REFERENCES `LicenseApplications` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LicenseApplicationApprovals_LicenseApprovalLevels_LicenseApp~` FOREIGN KEY (`LicenseApprovalLevelID`) REFERENCES `LicenseApprovalLevels` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LicenseApplicationApprovals_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`)
);

CREATE TABLE `Licenses` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `LicenseNumber` longtext NOT NULL,
    `MemberId` int NOT NULL,
    `ExpiryDate` date NOT NULL,
    `YearOfOperationId` int NOT NULL,
    `LicenseApplicationId` int NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Licenses_LicenseApplications_LicenseApplicationId` FOREIGN KEY (`LicenseApplicationId`) REFERENCES `LicenseApplications` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Licenses_Members_MemberId` FOREIGN KEY (`MemberId`) REFERENCES `Members` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Licenses_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AttachmentMemberQualification` (
    `AttachmentsId` int NOT NULL,
    `MemberQualificationsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `MemberQualificationsId`),
    CONSTRAINT `FK_AttachmentMemberQualification_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AttachmentMemberQualification_MemberQualifications_MemberQua~` FOREIGN KEY (`MemberQualificationsId`) REFERENCES `MemberQualifications` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AttachmentProBonoApplication` (
    `AttachmentsId` int NOT NULL,
    `ProBonosApplicationsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `ProBonosApplicationsId`),
    CONSTRAINT `FK_AttachmentProBonoApplication_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AttachmentProBonoApplication_ProBonoApplications_ProBonosApp~` FOREIGN KEY (`ProBonosApplicationsId`) REFERENCES `ProBonoApplications` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `ProBonos` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `FileNumber` varchar(100) NOT NULL,
    `NatureOfDispute` varchar(200) NOT NULL,
    `CaseDetails` longtext NOT NULL,
    `CreatedById` varchar(200) NULL,
    `ProbonoClientId` int NOT NULL,
    `ProBonoApplicationId` int NOT NULL,
    `SummaryOfDispute` longtext NOT NULL,
    `YearOfOperationId` int NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ProBonos_ProBonoApplications_ProBonoApplicationId` FOREIGN KEY (`ProBonoApplicationId`) REFERENCES `ProBonoApplications` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_ProBonos_ProbonoClients_ProbonoClientId` FOREIGN KEY (`ProbonoClientId`) REFERENCES `ProbonoClients` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_ProBonos_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`),
    CONSTRAINT `FK_ProBonos_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AttachmentProBono` (
    `AttachmentsId` int NOT NULL,
    `ProBonosId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `ProBonosId`),
    CONSTRAINT `FK_AttachmentProBono_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AttachmentProBono_ProBonos_ProBonosId` FOREIGN KEY (`ProBonosId`) REFERENCES `ProBonos` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `MemberProBono` (
    `MembersId` int NOT NULL,
    `ProBonosId` int NOT NULL,
    PRIMARY KEY (`MembersId`, `ProBonosId`),
    CONSTRAINT `FK_MemberProBono_Members_MembersId` FOREIGN KEY (`MembersId`) REFERENCES `Members` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_MemberProBono_ProBonos_ProBonosId` FOREIGN KEY (`ProBonosId`) REFERENCES `ProBonos` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `ProBonoReports` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ProBonoId` int NOT NULL,
    `ProBonoProposedHours` double NOT NULL,
    `ProBonoHours` double NOT NULL,
    `ReportStatus` longtext NOT NULL,
    `ApprovedById` varchar(200) NULL,
    `CreatedById` varchar(200) NOT NULL,
    `Description` varchar(250) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ProBonoReports_ProBonos_ProBonoId` FOREIGN KEY (`ProBonoId`) REFERENCES `ProBonos` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_ProBonoReports_Users_ApprovedById` FOREIGN KEY (`ApprovedById`) REFERENCES `Users` (`Id`),
    CONSTRAINT `FK_ProBonoReports_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AttachmentProBonoReport` (
    `AttachmentsId` int NOT NULL,
    `ProBonoReportsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `ProBonoReportsId`),
    CONSTRAINT `FK_AttachmentProBonoReport_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AttachmentProBonoReport_ProBonoReports_ProBonoReportsId` FOREIGN KEY (`ProBonoReportsId`) REFERENCES `ProBonoReports` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `PropBonoReportFeedbacks` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ProBonoReportId` int NOT NULL,
    `Feedback` varchar(250) NOT NULL,
    `FeedBackById` varchar(200) NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_PropBonoReportFeedbacks_ProBonoReports_ProBonoReportId` FOREIGN KEY (`ProBonoReportId`) REFERENCES `ProBonoReports` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_PropBonoReportFeedbacks_Users_FeedBackById` FOREIGN KEY (`FeedBackById`) REFERENCES `Users` (`Id`)
);

CREATE INDEX `IX_AttachmentLicenseApplication_LicenseApplicationsId` ON `AttachmentLicenseApplication` (`LicenseApplicationsId`);

CREATE INDEX `IX_AttachmentMemberQualification_MemberQualificationsId` ON `AttachmentMemberQualification` (`MemberQualificationsId`);

CREATE INDEX `IX_AttachmentProBono_ProBonosId` ON `AttachmentProBono` (`ProBonosId`);

CREATE INDEX `IX_AttachmentProBonoApplication_ProBonosApplicationsId` ON `AttachmentProBonoApplication` (`ProBonosApplicationsId`);

CREATE INDEX `IX_AttachmentProBonoReport_ProBonoReportsId` ON `AttachmentProBonoReport` (`ProBonoReportsId`);

CREATE INDEX `IX_Attachments_AttachmentTypeId` ON `Attachments` (`AttachmentTypeId`);

CREATE INDEX `IX_ErrorLogs_CreatedById` ON `ErrorLogs` (`CreatedById`);

CREATE INDEX `IX_LicenseApplicationApprovals_CreatedById` ON `LicenseApplicationApprovals` (`CreatedById`);

CREATE INDEX `IX_LicenseApplicationApprovals_LicenseApplicationID` ON `LicenseApplicationApprovals` (`LicenseApplicationID`);

CREATE INDEX `IX_LicenseApplicationApprovals_LicenseApprovalLevelID` ON `LicenseApplicationApprovals` (`LicenseApprovalLevelID`);

CREATE INDEX `IX_LicenseApplications_CurrentApprovalLevelID` ON `LicenseApplications` (`CurrentApprovalLevelID`);

CREATE INDEX `IX_LicenseApplications_MemberId` ON `LicenseApplications` (`MemberId`);

CREATE INDEX `IX_LicenseApplications_YearOfOperationId` ON `LicenseApplications` (`YearOfOperationId`);

CREATE INDEX `IX_LicenseApprovalLevels_DepartmentId` ON `LicenseApprovalLevels` (`DepartmentId`);

CREATE INDEX `IX_Licenses_LicenseApplicationId` ON `Licenses` (`LicenseApplicationId`);

CREATE INDEX `IX_Licenses_MemberId` ON `Licenses` (`MemberId`);

CREATE INDEX `IX_Licenses_YearOfOperationId` ON `Licenses` (`YearOfOperationId`);

CREATE INDEX `IX_MemberProBono_ProBonosId` ON `MemberProBono` (`ProBonosId`);

CREATE INDEX `IX_MemberQualifications_MemberId` ON `MemberQualifications` (`MemberId`);

CREATE INDEX `IX_MemberQualifications_QualificationTypeId` ON `MemberQualifications` (`QualificationTypeId`);

CREATE INDEX `IX_MemberQualificationType_QualificationTypesId` ON `MemberQualificationType` (`QualificationTypesId`);

CREATE INDEX `IX_Members_UserId` ON `Members` (`UserId`);

CREATE INDEX `IX_ProBonoApplications_CreatedById` ON `ProBonoApplications` (`CreatedById`);

CREATE INDEX `IX_ProBonoApplications_ProbonoClientId` ON `ProBonoApplications` (`ProbonoClientId`);

CREATE INDEX `IX_ProBonoApplications_YearOfOperationId` ON `ProBonoApplications` (`YearOfOperationId`);

CREATE INDEX `IX_ProbonoClients_CreatedById` ON `ProbonoClients` (`CreatedById`);

CREATE INDEX `IX_ProBonoReports_ApprovedById` ON `ProBonoReports` (`ApprovedById`);

CREATE INDEX `IX_ProBonoReports_CreatedById` ON `ProBonoReports` (`CreatedById`);

CREATE INDEX `IX_ProBonoReports_ProBonoId` ON `ProBonoReports` (`ProBonoId`);

CREATE INDEX `IX_ProBonos_CreatedById` ON `ProBonos` (`CreatedById`);

CREATE INDEX `IX_ProBonos_ProBonoApplicationId` ON `ProBonos` (`ProBonoApplicationId`);

CREATE INDEX `IX_ProBonos_ProbonoClientId` ON `ProBonos` (`ProbonoClientId`);

CREATE INDEX `IX_ProBonos_YearOfOperationId` ON `ProBonos` (`YearOfOperationId`);

CREATE INDEX `IX_PropBonoReportFeedbacks_FeedBackById` ON `PropBonoReportFeedbacks` (`FeedBackById`);

CREATE INDEX `IX_PropBonoReportFeedbacks_ProBonoReportId` ON `PropBonoReportFeedbacks` (`ProBonoReportId`);

CREATE INDEX `IX_RoleClaims_RoleId` ON `RoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `Roles` (`NormalizedName`);

CREATE INDEX `IX_UserClaims_UserId` ON `UserClaims` (`UserId`);

CREATE INDEX `IX_UserLogins_UserId` ON `UserLogins` (`UserId`);

CREATE INDEX `IX_UserRoles_RoleId` ON `UserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `Users` (`NormalizedEmail`);

CREATE INDEX `IX_Users_CountryId` ON `Users` (`CountryId`);

CREATE INDEX `IX_Users_DepartmentId` ON `Users` (`DepartmentId`);

CREATE INDEX `IX_Users_FirmId` ON `Users` (`FirmId`);

CREATE INDEX `IX_Users_IdentityTypeId` ON `Users` (`IdentityTypeId`);

CREATE INDEX `IX_Users_TitleId` ON `Users` (`TitleId`);

CREATE UNIQUE INDEX `UserNameIndex` ON `Users` (`NormalizedUserName`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240501123515_initial migration', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `MemberQualifications` MODIFY `DateObtained` datetime(6) NOT NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240501130229_updated member qualification date obtained data type to date time', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoValidTaxClearanceCertificate` varchar(250) NOT NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoSocietysLevy` varchar(250) NOT NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoProfessionalIndemnityInsuranceCover` varchar(250) NOT NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoMinimumNumberOfCLEUnits` varchar(250) NOT NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoFullMandatoryProBonoWork` varchar(250) NOT NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoContributionToMLSBuildingProjectFund` varchar(250) NOT NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoContributionToFidelityFund` varchar(250) NOT NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoAccountantsCertificate` varchar(250) NOT NULL;

ALTER TABLE `LicenseApplications` ADD `CreatedById` varchar(200) NOT NULL DEFAULT '';

CREATE INDEX `IX_LicenseApplications_CreatedById` ON `LicenseApplications` (`CreatedById`);

ALTER TABLE `LicenseApplications` ADD CONSTRAINT `FK_LicenseApplications_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240507085349_added created by reference to license application', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoValidTaxClearanceCertificate` varchar(250) NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoSocietysLevy` varchar(250) NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoProfessionalIndemnityInsuranceCover` varchar(250) NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoFullMandatoryProBonoWork` varchar(250) NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoContributionToMLSBuildingProjectFund` varchar(250) NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoContributionToFidelityFund` varchar(250) NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoComplianceWithPenalties` varchar(250) NULL;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoAccountantsCertificate` varchar(250) NULL;

ALTER TABLE `LicenseApplications` ADD `ExplanationForNoAnnualSubscriptionToSociety` longtext NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240508080524_updated license table to make explanation properties optional', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Attachments` ADD `PropertyName` varchar(200) NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240508103920_added propertyName to attachments to get extra meta data for attachements', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `LicenseApplications` MODIFY `ExplanationForNoMinimumNumberOfCLEUnits` varchar(250) NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240508111318_made explanation for not meeting minimum CLE units optional', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Firms` ADD `CreatedById` varchar(200) NULL;

CREATE INDEX `IX_Firms_CreatedById` ON `Firms` (`CreatedById`);

ALTER TABLE `Firms` ADD CONSTRAINT `FK_Firms_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240515113544_added created by reference to firm', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Firms` ADD `DenialReason` varchar(250) NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240515142211_added denial reason to firm', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Members` ADD `FirmId` int NULL;

CREATE INDEX `IX_Members_FirmId` ON `Members` (`FirmId`);

ALTER TABLE `Members` ADD CONSTRAINT `FK_Members_Firms_FirmId` FOREIGN KEY (`FirmId`) REFERENCES `Firms` (`Id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240515163059_added firm to member', '8.0.6');

COMMIT;

START TRANSACTION;

CREATE TABLE `LicenseApprovalHistories` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `LicenseApplicationId` int NOT NULL,
    `ChangeDate` datetime(6) NOT NULL,
    `ApprovalLevelId` int NOT NULL,
    `Status` longtext NOT NULL,
    `ChangedById` varchar(200) NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_LicenseApprovalHistories_LicenseApplications_LicenseApplicat~` FOREIGN KEY (`LicenseApplicationId`) REFERENCES `LicenseApplications` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LicenseApprovalHistories_LicenseApprovalLevels_ApprovalLevel~` FOREIGN KEY (`ApprovalLevelId`) REFERENCES `LicenseApprovalLevels` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LicenseApprovalHistories_Users_ChangedById` FOREIGN KEY (`ChangedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `LicenseApprovalComments` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ApprovalHistoryId` int NOT NULL,
    `Comment` varchar(1000) NOT NULL,
    `CommentedById` varchar(200) NOT NULL,
    `CommentDate` datetime(6) NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_LicenseApprovalComments_LicenseApprovalHistories_ApprovalHis~` FOREIGN KEY (`ApprovalHistoryId`) REFERENCES `LicenseApprovalHistories` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LicenseApprovalComments_Users_CommentedById` FOREIGN KEY (`CommentedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_LicenseApprovalComments_ApprovalHistoryId` ON `LicenseApprovalComments` (`ApprovalHistoryId`);

CREATE INDEX `IX_LicenseApprovalComments_CommentedById` ON `LicenseApprovalComments` (`CommentedById`);

CREATE INDEX `IX_LicenseApprovalHistories_ApprovalLevelId` ON `LicenseApprovalHistories` (`ApprovalLevelId`);

CREATE INDEX `IX_LicenseApprovalHistories_ChangedById` ON `LicenseApprovalHistories` (`ChangedById`);

CREATE INDEX `IX_LicenseApprovalHistories_LicenseApplicationId` ON `LicenseApprovalHistories` (`LicenseApplicationId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240517134458_added licence application status tracking tables', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `LicenseApprovalHistories` ADD `CreatedDate` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000';

ALTER TABLE `LicenseApprovalHistories` ADD `DeletedDate` datetime(6) NULL;

ALTER TABLE `LicenseApprovalHistories` ADD `UpdatedDate` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000';

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240520134357_added fields to license history', '8.0.6');

COMMIT;

START TRANSACTION;

DROP INDEX IX_Licenses_LicenseApplicationId ON Licenses;

ALTER TABLE `Licenses` MODIFY `ExpiryDate` datetime(6) NOT NULL;

CREATE UNIQUE INDEX `IX_Licenses_LicenseApplicationId` ON `Licenses` (`LicenseApplicationId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240520194810_updated license expiry date datatype', '8.0.6');

COMMIT;

START TRANSACTION;

CREATE TABLE `CPDTrainings` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(100) NOT NULL,
    `Description` varchar(250) NOT NULL,
    `Duration` double NOT NULL,
    `DateToBeConducted` datetime(6) NOT NULL,
    `ProposedUnits` int NOT NULL,
    `CPDUnitsAwarded` int NOT NULL,
    `AccreditingInstitution` varchar(200) NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `YearOfOperationId` int NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_CPDTrainings_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_CPDTrainings_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `CPDTrainingRegistrations` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `MemberId` int NOT NULL,
    `RegistrationStatus` varchar(100) NOT NULL,
    `CPDTrainingId` int NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_CPDTrainingRegistrations_CPDTrainings_CPDTrainingId` FOREIGN KEY (`CPDTrainingId`) REFERENCES `CPDTrainings` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_CPDTrainingRegistrations_Members_MemberId` FOREIGN KEY (`MemberId`) REFERENCES `Members` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_CPDTrainingRegistrations_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `CPDUnitsEarned` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `MemberId` int NOT NULL,
    `CPDTrainingId` int NOT NULL,
    `UnitsEarned` int NOT NULL,
    `YearOfOperationId` int NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_CPDUnitsEarned_CPDTrainings_CPDTrainingId` FOREIGN KEY (`CPDTrainingId`) REFERENCES `CPDTrainings` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_CPDUnitsEarned_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_CPDTrainingRegistrations_CPDTrainingId` ON `CPDTrainingRegistrations` (`CPDTrainingId`);

CREATE INDEX `IX_CPDTrainingRegistrations_CreatedById` ON `CPDTrainingRegistrations` (`CreatedById`);

CREATE INDEX `IX_CPDTrainingRegistrations_MemberId` ON `CPDTrainingRegistrations` (`MemberId`);

CREATE INDEX `IX_CPDTrainings_CreatedById` ON `CPDTrainings` (`CreatedById`);

CREATE INDEX `IX_CPDTrainings_YearOfOperationId` ON `CPDTrainings` (`YearOfOperationId`);

CREATE INDEX `IX_CPDUnitsEarned_CPDTrainingId` ON `CPDUnitsEarned` (`CPDTrainingId`);

CREATE INDEX `IX_CPDUnitsEarned_YearOfOperationId` ON `CPDUnitsEarned` (`YearOfOperationId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240521095754_added cpd units tables', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `CPDTrainings` ADD `ApprovalStatus` longtext NOT NULL;

CREATE TABLE `AttachmentCPDTraining` (
    `AttachmentsId` int NOT NULL,
    `CPDTrainingsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `CPDTrainingsId`),
    CONSTRAINT `FK_AttachmentCPDTraining_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AttachmentCPDTraining_CPDTrainings_CPDTrainingsId` FOREIGN KEY (`CPDTrainingsId`) REFERENCES `CPDTrainings` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_AttachmentCPDTraining_CPDTrainingsId` ON `AttachmentCPDTraining` (`CPDTrainingsId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240521122859_added attachment reference to CPD trainings', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `CPDTrainings` ADD `TrainingFee` double NULL;

ALTER TABLE `CPDTrainingRegistrations` ADD `AttachmentId` int NULL;

CREATE INDEX `IX_CPDTrainingRegistrations_AttachmentId` ON `CPDTrainingRegistrations` (`AttachmentId`);

ALTER TABLE `CPDTrainingRegistrations` ADD CONSTRAINT `FK_CPDTrainingRegistrations_Attachments_AttachmentId` FOREIGN KEY (`AttachmentId`) REFERENCES `Attachments` (`Id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240522081619_added training fee to cpd training', '8.0.6');

COMMIT;

START TRANSACTION;

DROP INDEX IX_CPDTrainingRegistrations_AttachmentId ON CPDTrainingRegistrations;

ALTER TABLE `CPDTrainingRegistrations` DROP COLUMN `AttachmentId`;

CREATE TABLE `AttachmentCPDTrainingRegistration` (
    `AttachmentsId` int NOT NULL,
    `CPDTrainingRegistrationsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `CPDTrainingRegistrationsId`),
    CONSTRAINT `FK_AttachmentCPDTrainingRegistration_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AttachmentCPDTrainingRegistration_CPDTrainingRegistrations_C~` FOREIGN KEY (`CPDTrainingRegistrationsId`) REFERENCES `CPDTrainingRegistrations` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_AttachmentCPDTrainingRegistration_CPDTrainingRegistrationsId` ON `AttachmentCPDTrainingRegistration` (`CPDTrainingRegistrationsId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240522113039_added many to many between cpd training registration and attachment', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `CPDTrainingRegistrations` ADD `DeniedReason` varchar(250) NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240525063103_added denial reason to cpd registration', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Users` MODIFY `OtherName` varchar(100) NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240525150415_made other names optional in users table', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `CPDTrainingRegistrations` ADD `DateOfPayment` datetime(6) NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240526104510_added date of payment to cpd registration', '8.0.6');

COMMIT;

START TRANSACTION;

CREATE TABLE `Penalties` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `MemberId` int NOT NULL,
    `PenaltyTypeId` int NOT NULL,
    `Reason` longtext NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Penalties_Members_MemberId` FOREIGN KEY (`MemberId`) REFERENCES `Members` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Penalties_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `PenaltyTypes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(100) NOT NULL,
    `Description` varchar(250) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

CREATE INDEX `IX_CPDUnitsEarned_MemberId` ON `CPDUnitsEarned` (`MemberId`);

CREATE INDEX `IX_Penalties_CreatedById` ON `Penalties` (`CreatedById`);

CREATE INDEX `IX_Penalties_MemberId` ON `Penalties` (`MemberId`);

ALTER TABLE `CPDUnitsEarned` ADD CONSTRAINT `FK_CPDUnitsEarned_Members_MemberId` FOREIGN KEY (`MemberId`) REFERENCES `Members` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240529081555_added penalty tables', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Penalties` ADD `Fee` double NOT NULL DEFAULT 0.0;

ALTER TABLE `Penalties` ADD `PenaltyStatus` varchar(100) NOT NULL DEFAULT '';

ALTER TABLE `Penalties` ADD `ResolutionComment` varchar(250) NULL;

ALTER TABLE `Penalties` ADD `YearOfOperationId` int NOT NULL DEFAULT 0;

ALTER TABLE `Attachments` ADD `PenaltyId` int NULL;

ALTER TABLE `Attachments` ADD `PenaltyPaymentId` int NULL;

CREATE TABLE `PenaltyPayments` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `PenaltyId` int NOT NULL,
    `PaymentStatus` longtext NOT NULL,
    `Description` longtext NULL,
    `Fee` double NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_PenaltyPayments_Penalties_PenaltyId` FOREIGN KEY (`PenaltyId`) REFERENCES `Penalties` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_Penalties_YearOfOperationId` ON `Penalties` (`YearOfOperationId`);

CREATE INDEX `IX_Attachments_PenaltyId` ON `Attachments` (`PenaltyId`);

CREATE INDEX `IX_Attachments_PenaltyPaymentId` ON `Attachments` (`PenaltyPaymentId`);

CREATE INDEX `IX_PenaltyPayments_PenaltyId` ON `PenaltyPayments` (`PenaltyId`);

ALTER TABLE `Attachments` ADD CONSTRAINT `FK_Attachments_Penalties_PenaltyId` FOREIGN KEY (`PenaltyId`) REFERENCES `Penalties` (`Id`);

ALTER TABLE `Attachments` ADD CONSTRAINT `FK_Attachments_PenaltyPayments_PenaltyPaymentId` FOREIGN KEY (`PenaltyPaymentId`) REFERENCES `PenaltyPayments` (`Id`);

ALTER TABLE `Penalties` ADD CONSTRAINT `FK_Penalties_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240529103137_added penalty payment table', '8.0.6');

COMMIT;

START TRANSACTION;

DROP INDEX IX_Attachments_PenaltyId ON Attachments;

DROP INDEX IX_Attachments_PenaltyPaymentId ON Attachments;

ALTER TABLE `Attachments` DROP COLUMN `PenaltyId`;

ALTER TABLE `Attachments` DROP COLUMN `PenaltyPaymentId`;

CREATE TABLE `AttachmentPenalty` (
    `AttachmentsId` int NOT NULL,
    `PenaltiesId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `PenaltiesId`),
    CONSTRAINT `FK_AttachmentPenalty_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AttachmentPenalty_Penalties_PenaltiesId` FOREIGN KEY (`PenaltiesId`) REFERENCES `Penalties` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AttachmentPenaltyPayment` (
    `AttachmentsId` int NOT NULL,
    `PenaltyPaymentsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `PenaltyPaymentsId`),
    CONSTRAINT `FK_AttachmentPenaltyPayment_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AttachmentPenaltyPayment_PenaltyPayments_PenaltyPaymentsId` FOREIGN KEY (`PenaltyPaymentsId`) REFERENCES `PenaltyPayments` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_AttachmentPenalty_PenaltiesId` ON `AttachmentPenalty` (`PenaltiesId`);

CREATE INDEX `IX_AttachmentPenaltyPayment_PenaltyPaymentsId` ON `AttachmentPenaltyPayment` (`PenaltyPaymentsId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240529103333_added penalty attachment tables', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE CPDTrainings RENAME COLUMN TrainingFee TO VirtualAttendanceFee;

ALTER TABLE `CPDTrainings` ADD `IsFree` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `CPDTrainings` ADD `NonMemberFee` double NULL;

ALTER TABLE `CPDTrainings` ADD `PhysicalAttendanceFee` double NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240605093732_added extra columns to cpdtraining', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `CPDTrainings` ADD `PhysicalVenue` varchar(250) NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240605102427_added physical attendance location', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `CPDTrainingRegistrations` ADD `Fee` double NOT NULL DEFAULT 0.0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240605120027_added fee to cpd registration', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `CPDTrainings` ADD `RegistrationDueDate` datetime(6) NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240605194620_added registration due date to cpd training', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE CPDTrainings RENAME COLUMN VirtualAttendanceFee TO NonMemberVirtualAttandanceFee;

ALTER TABLE CPDTrainings RENAME COLUMN PhysicalAttendanceFee TO NonMemberPhysicalAttendanceFee;

ALTER TABLE CPDTrainings RENAME COLUMN NonMemberFee TO MemberVirtualAttendanceFee;

ALTER TABLE `CPDTrainings` ADD `MemberPhysicalAttendanceFee` double NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240606075452_added updated fee attributes to cpd training', '8.0.6');

COMMIT;

START TRANSACTION;

CREATE TABLE `Committees` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CommitteeName` longtext NOT NULL,
    `Description` longtext NOT NULL,
    `CreationDate` datetime(6) NOT NULL,
    `MeetingSchedule` longtext NOT NULL,
    `ChairpersonID` int NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Committees_Members_ChairpersonID` FOREIGN KEY (`ChairpersonID`) REFERENCES `Members` (`Id`)
);

CREATE TABLE `CommitteeMembers` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CommitteeID` int NOT NULL,
    `MemberID` int NOT NULL,
    `JoinedDate` datetime(6) NOT NULL,
    `Role` longtext NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_CommitteeMembers_Committees_CommitteeID` FOREIGN KEY (`CommitteeID`) REFERENCES `Committees` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_CommitteeMembers_Members_MemberID` FOREIGN KEY (`MemberID`) REFERENCES `Members` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Threads` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CommitteeID` int NOT NULL,
    `Subject` longtext NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `CreatedBy` int NOT NULL,
    `CreatedByMemberId` int NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Threads_Committees_CommitteeID` FOREIGN KEY (`CommitteeID`) REFERENCES `Committees` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Threads_Members_CreatedByMemberId` FOREIGN KEY (`CreatedByMemberId`) REFERENCES `Members` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Messages` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CommitteeID` int NOT NULL,
    `SenderID` int NOT NULL,
    `Timestamp` datetime(6) NOT NULL,
    `Content` longtext NOT NULL,
    `ThreadID` int NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Messages_Committees_CommitteeID` FOREIGN KEY (`CommitteeID`) REFERENCES `Committees` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Messages_Members_SenderID` FOREIGN KEY (`SenderID`) REFERENCES `Members` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Messages_Threads_ThreadID` FOREIGN KEY (`ThreadID`) REFERENCES `Threads` (`Id`)
);

CREATE TABLE `AttachmentMessage` (
    `AttachmentsId` int NOT NULL,
    `MessagesId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `MessagesId`),
    CONSTRAINT `FK_AttachmentMessage_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AttachmentMessage_Messages_MessagesId` FOREIGN KEY (`MessagesId`) REFERENCES `Messages` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_AttachmentMessage_MessagesId` ON `AttachmentMessage` (`MessagesId`);

CREATE INDEX `IX_CommitteeMembers_CommitteeID` ON `CommitteeMembers` (`CommitteeID`);

CREATE INDEX `IX_CommitteeMembers_MemberID` ON `CommitteeMembers` (`MemberID`);

CREATE INDEX `IX_Committees_ChairpersonID` ON `Committees` (`ChairpersonID`);

CREATE INDEX `IX_Messages_CommitteeID` ON `Messages` (`CommitteeID`);

CREATE INDEX `IX_Messages_SenderID` ON `Messages` (`SenderID`);

CREATE INDEX `IX_Messages_ThreadID` ON `Messages` (`ThreadID`);

CREATE INDEX `IX_Threads_CommitteeID` ON `Threads` (`CommitteeID`);

CREATE INDEX `IX_Threads_CreatedByMemberId` ON `Threads` (`CreatedByMemberId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240606143103_added added messaging modules', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Committees` DROP COLUMN `MeetingSchedule`;

ALTER TABLE `CommitteeMembers` DROP COLUMN `Role`;

ALTER TABLE `Committees` ADD `CreatedById` varchar(200) NOT NULL DEFAULT '';

CREATE INDEX `IX_Committees_CreatedById` ON `Committees` (`CreatedById`);

ALTER TABLE `Committees` ADD CONSTRAINT `FK_Committees_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240606161311_updated messaging modules', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Committees` ADD `YearOfOperationId` int NOT NULL DEFAULT 0;

CREATE INDEX `IX_Committees_YearOfOperationId` ON `Committees` (`YearOfOperationId`);

ALTER TABLE `Committees` ADD CONSTRAINT `FK_Committees_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240606170716_updated committe by adding year of operation to the table', '8.0.6');

COMMIT;

START TRANSACTION;

DROP INDEX IX_Threads_CreatedByMemberId ON Threads;

ALTER TABLE `Threads` DROP COLUMN `CreatedBy`;

ALTER TABLE `Threads` DROP COLUMN `CreatedByMemberId`;

ALTER TABLE Threads RENAME COLUMN CommitteeID TO CommitteeId;

ALTER TABLE `Threads` RENAME INDEX `IX_Threads_CommitteeID` TO `IX_Threads_CommitteeId`;

ALTER TABLE `Threads` ADD `CreatedById` varchar(200) NOT NULL DEFAULT '';

CREATE INDEX `IX_Threads_CreatedById` ON `Threads` (`CreatedById`);

CREATE INDEX `IX_Penalties_PenaltyTypeId` ON `Penalties` (`PenaltyTypeId`);

ALTER TABLE `Penalties` ADD CONSTRAINT `FK_Penalties_PenaltyTypes_PenaltyTypeId` FOREIGN KEY (`PenaltyTypeId`) REFERENCES `PenaltyTypes` (`Id`) ON DELETE CASCADE;

ALTER TABLE `Threads` ADD CONSTRAINT `FK_Threads_Committees_CommitteeId` FOREIGN KEY (`CommitteeId`) REFERENCES `Committees` (`Id`) ON DELETE CASCADE;

ALTER TABLE `Threads` ADD CONSTRAINT `FK_Threads_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240607093846_updated threads to reference user than member', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Messages` DROP COLUMN `SenderID`;

ALTER TABLE Messages RENAME COLUMN ThreadID TO ThreadId;

ALTER TABLE `Messages` RENAME INDEX `IX_Messages_ThreadID` TO `IX_Messages_ThreadId`;

ALTER TABLE `Messages` ADD `CreatedById` varchar(200) NOT NULL DEFAULT '';

CREATE INDEX `IX_Messages_CreatedById` ON `Messages` (`CreatedById`);

ALTER TABLE `Messages` ADD CONSTRAINT `FK_Messages_Threads_ThreadId` FOREIGN KEY (`ThreadId`) REFERENCES `Threads` (`Id`);

ALTER TABLE `Messages` ADD CONSTRAINT `FK_Messages_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240607115233_updated message by adding created by to the table', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `CommitteeMembers` DROP COLUMN `MemberID`;

ALTER TABLE `CommitteeMembers` ADD `MemberShipId` varchar(200) NOT NULL DEFAULT '';

ALTER TABLE `CommitteeMembers` ADD `Role` varchar(150) NOT NULL DEFAULT '';

CREATE INDEX `IX_CommitteeMembers_MemberShipId` ON `CommitteeMembers` (`MemberShipId`);

ALTER TABLE `CommitteeMembers` ADD CONSTRAINT `FK_CommitteeMembers_Users_MemberShipId` FOREIGN KEY (`MemberShipId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240611134156_added role to committe membership', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `PenaltyPayments` ADD `DateApproved` datetime(6) NULL;

ALTER TABLE `PenaltyPayments` ADD `ReasonForDenial` varchar(250) NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240611141919_adding approved and denied date to penalty payments', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `PenaltyPayments` ADD `DateDenied` datetime(6) NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240611142352_adding denied date to penalty payments', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Penalties` ADD `AmountPaid` double NOT NULL DEFAULT 0.0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240612063219_adding amount paid to penalties', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Penalties` ADD `AmountRemaining` double NOT NULL DEFAULT 0.0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240612073820_adding amount remaining to penalties', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `CommitteeMembers` ADD `MemberShipStatus` longtext NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240612110411_added meshership status committe membership', '8.0.6');

COMMIT;

