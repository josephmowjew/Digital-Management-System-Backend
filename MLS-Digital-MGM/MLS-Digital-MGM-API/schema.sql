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

CREATE TABLE `IdentityTypes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(150) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`)
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

CREATE TABLE `QBCustomers` (
    `Id` varchar(255) NOT NULL,
    `CustomerName` longtext NOT NULL,
    `FirstName` longtext NOT NULL,
    `MiddleName` longtext NOT NULL,
    `LastName` longtext NOT NULL,
    `Title` longtext NOT NULL,
    `JobTitle` longtext NOT NULL,
    `CompanyName` longtext NOT NULL,
    `BillingAddressLine1` longtext NOT NULL,
    `BillingAddressLine2` longtext NOT NULL,
    `BillingAddressLine3` longtext NOT NULL,
    `BillingAddressLine4` longtext NOT NULL,
    `BillingAddressLine5` longtext NOT NULL,
    `City` longtext NOT NULL,
    `State` longtext NOT NULL,
    `Province` longtext NOT NULL,
    `ActiveStatus` tinyint(1) NOT NULL,
    `EmailAddress` longtext NOT NULL,
    `PhoneNumber` longtext NOT NULL,
    `AccountBalance` decimal(18,2) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
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

CREATE TABLE `QBInvoices` (
    `Id` varchar(255) NOT NULL,
    `InvoiceNumber` longtext NOT NULL,
    `CustomerId` varchar(255) NOT NULL,
    `CustomerName` longtext NOT NULL,
    `InvoiceDate` datetime(6) NOT NULL,
    `InvoiceAmount` decimal(18,2) NOT NULL,
    `UnpaidAmount` decimal(18,2) NOT NULL,
    `InvoiceType` longtext NOT NULL,
    `ExpiryDate` datetime(6) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `InvoiceDescription` longtext NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_QBInvoices_QBCustomers_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `QBCustomers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `RoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(200) NOT NULL,
    `ClaimType` longtext NULL,
    `ClaimValue` longtext NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RoleClaims_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `LevyPercents` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `PercentageValue` double NOT NULL,
    `YearOfOperationId` int NOT NULL,
    `OperationStatus` longtext NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_LevyPercents_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `QBPayments` (
    `Id` varchar(255) NOT NULL,
    `CustomerId` varchar(255) NOT NULL,
    `InvoiceId` varchar(255) NOT NULL,
    `PaymentAmount` decimal(18,2) NOT NULL,
    `PaymentMethod` longtext NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_QBPayments_QBCustomers_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `QBCustomers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_QBPayments_QBInvoices_InvoiceId` FOREIGN KEY (`InvoiceId`) REFERENCES `QBInvoices` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `QBReceipts` (
    `Id` varchar(255) NOT NULL,
    `CustomerId` varchar(255) NOT NULL,
    `PaymentDate` datetime(6) NOT NULL,
    `ReceiptNumber` longtext NOT NULL,
    `TotalPaymentAmount` decimal(18,2) NOT NULL,
    `PaymentId` longtext NOT NULL,
    `InvoiceId` varchar(255) NOT NULL,
    `InvoiceNumber` longtext NOT NULL,
    `InvoiceDate` datetime(6) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_QBReceipts_QBCustomers_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `QBCustomers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_QBReceipts_QBInvoices_InvoiceId` FOREIGN KEY (`InvoiceId`) REFERENCES `QBInvoices` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AttachmentCPDTraining` (
    `AttachmentsId` int NOT NULL,
    `CPDTrainingsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `CPDTrainingsId`)
);

CREATE TABLE `AttachmentCPDTrainingRegistration` (
    `AttachmentsId` int NOT NULL,
    `CPDTrainingRegistrationsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `CPDTrainingRegistrationsId`)
);

CREATE TABLE `AttachmentLevyDeclaration` (
    `AttachmentsId` int NOT NULL,
    `LevyDeclarationsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `LevyDeclarationsId`)
);

CREATE TABLE `AttachmentLicenseApplication` (
    `AttachmentsId` int NOT NULL,
    `LicenseApplicationsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `LicenseApplicationsId`)
);

CREATE TABLE `AttachmentMemberQualification` (
    `AttachmentsId` int NOT NULL,
    `MemberQualificationsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `MemberQualificationsId`)
);

CREATE TABLE `AttachmentMessage` (
    `AttachmentsId` int NOT NULL,
    `MessagesId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `MessagesId`)
);

CREATE TABLE `AttachmentPenalty` (
    `AttachmentsId` int NOT NULL,
    `PenaltiesId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `PenaltiesId`)
);

CREATE TABLE `AttachmentPenaltyPayment` (
    `AttachmentsId` int NOT NULL,
    `PenaltyPaymentsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `PenaltyPaymentsId`)
);

CREATE TABLE `AttachmentProBono` (
    `AttachmentsId` int NOT NULL,
    `ProBonosId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `ProBonosId`)
);

CREATE TABLE `AttachmentProBonoApplication` (
    `AttachmentsId` int NOT NULL,
    `ProBonosApplicationsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `ProBonosApplicationsId`)
);

CREATE TABLE `AttachmentProBonoReport` (
    `AttachmentsId` int NOT NULL,
    `ProBonoReportsId` int NOT NULL,
    PRIMARY KEY (`AttachmentsId`, `ProBonoReportsId`)
);

CREATE TABLE `Attachments` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `FileName` varchar(200) NOT NULL,
    `FilePath` varchar(250) NOT NULL,
    `PropertyName` varchar(200) NULL,
    `AttachmentTypeId` int NOT NULL,
    `SubcommitteeMessageId` int NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Attachments_AttachmentTypes_AttachmentTypeId` FOREIGN KEY (`AttachmentTypeId`) REFERENCES `AttachmentTypes` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `CommitteeMembers` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CommitteeID` int NOT NULL,
    `MemberShipId` varchar(200) NOT NULL,
    `MemberShipStatus` longtext NULL,
    `JoinedDate` datetime(6) NOT NULL,
    `Role` varchar(150) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Committees` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CommitteeName` longtext NOT NULL,
    `Description` longtext NOT NULL,
    `CreationDate` datetime(6) NOT NULL,
    `ChairpersonID` int NULL,
    `YearOfOperationId` int NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Committees_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `CommunicationMessages` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Subject` longtext NOT NULL,
    `Body` longtext NOT NULL,
    `SentByUserId` varchar(200) NOT NULL,
    `SentDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `SentToAllUsers` tinyint(1) NOT NULL,
    `TargetedRolesJson` longtext NOT NULL,
    `TargetedDepartmentsJson` longtext NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `UpdatedDate` datetime(6) NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `CPDTrainingRegistrations` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `MemberId` int NOT NULL,
    `RegistrationStatus` varchar(100) NOT NULL,
    `Fee` double NOT NULL,
    `CPDTrainingId` int NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `DeniedReason` varchar(250) NULL,
    `DateOfPayment` datetime(6) NULL,
    `InvoiceRequestId` int NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `CPDTrainings` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(100) NOT NULL,
    `Description` varchar(250) NOT NULL,
    `Duration` double NOT NULL,
    `DateToBeConducted` datetime(6) NOT NULL,
    `PhysicalVenue` varchar(250) NULL,
    `ApprovalStatus` longtext NOT NULL,
    `ProposedUnits` int NOT NULL,
    `MemberPhysicalAttendanceFee` double NULL,
    `MemberVirtualAttendanceFee` double NULL,
    `NonMemberPhysicalAttendanceFee` double NULL,
    `NonMemberVirtualAttandanceFee` double NULL,
    `RegistrationDueDate` datetime(6) NOT NULL,
    `IsFree` tinyint(1) NOT NULL,
    `CPDUnitsAwarded` int NOT NULL,
    `AccreditingInstitution` varchar(200) NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `YearOfOperationId` int NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_CPDTrainings_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE
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
    `CreatedById` varchar(200) NULL,
    `DenialReason` varchar(250) NULL,
    `CustomerId` varchar(255) NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Firms_QBCustomers_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `QBCustomers` (`Id`)
);

CREATE TABLE `Users` (
    `Id` varchar(200) NOT NULL,
    `FirstName` varchar(100) NOT NULL,
    `LastName` varchar(100) NOT NULL,
    `OtherName` varchar(100) NULL,
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

CREATE TABLE `InvoiceRequests` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CreatedById` varchar(200) NULL,
    `Amount` double NOT NULL,
    `CustomerId` varchar(255) NOT NULL,
    `Status` varchar(100) NOT NULL,
    `YearOfOperationId` int NOT NULL,
    `ReferencedEntityType` longtext NOT NULL,
    `ReferencedEntityId` longtext NOT NULL,
    `Description` varchar(250) NULL,
    `QBInvoiceId` varchar(255) NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_InvoiceRequests_QBCustomers_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `QBCustomers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_InvoiceRequests_QBInvoices_QBInvoiceId` FOREIGN KEY (`QBInvoiceId`) REFERENCES `QBInvoices` (`Id`),
    CONSTRAINT `FK_InvoiceRequests_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`),
    CONSTRAINT `FK_InvoiceRequests_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Members` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(200) NOT NULL,
    `PostalAddress` varchar(250) NOT NULL,
    `PermanentAddress` varchar(250) NOT NULL,
    `ResidentialAddress` varchar(250) NOT NULL,
    `DateOfAdmissionToPractice` datetime(6) NOT NULL,
    `FirmId` int NULL,
    `CustomerId` varchar(255) NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Members_Firms_FirmId` FOREIGN KEY (`FirmId`) REFERENCES `Firms` (`Id`),
    CONSTRAINT `FK_Members_QBCustomers_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `QBCustomers` (`Id`),
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
    `deleteRequest` tinyint(1) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ProbonoClients_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Threads` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CommitteeId` int NOT NULL,
    `Subject` longtext NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Threads_Committees_CommitteeId` FOREIGN KEY (`CommitteeId`) REFERENCES `Committees` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Threads_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
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
    PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_UserTokens_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `LevyDeclarations` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Month` datetime(6) NOT NULL,
    `Revenue` decimal(18,2) NOT NULL,
    `LevyAmount` decimal(18,2) NOT NULL,
    `Percentage` decimal(18,2) NOT NULL,
    `FirmId` int NOT NULL,
    `InvoiceRequestId` int NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_LevyDeclarations_Firms_FirmId` FOREIGN KEY (`FirmId`) REFERENCES `Firms` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LevyDeclarations_InvoiceRequests_InvoiceRequestId` FOREIGN KEY (`InvoiceRequestId`) REFERENCES `InvoiceRequests` (`Id`)
);

CREATE TABLE `LicenseApplications` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `YearOfOperationId` int NOT NULL,
    `ApplicationStatus` longtext NOT NULL,
    `CurrentApprovalLevelID` int NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `MemberId` int NOT NULL,
    `FirstApplicationForLicense` tinyint(1) NOT NULL,
    `RenewedLicensePreviousYear` tinyint(1) NOT NULL,
    `ObtainedLeaveToRenewLicenseOutOfTime` tinyint(1) NOT NULL,
    `PaidAnnualSubscriptionToSociety` tinyint(1) NOT NULL,
    `ExplanationForNoAnnualSubscriptionToSociety` longtext NULL,
    `MadeContributionToFidelityFund` tinyint(1) NOT NULL,
    `ExplanationForNoContributionToFidelityFund` varchar(250) NULL,
    `RemittedSocietysLevy` tinyint(1) NOT NULL,
    `ExplanationForNoSocietysLevy` varchar(250) NULL,
    `MadeContributionToMLSBuildingProjectFund` tinyint(1) NOT NULL,
    `ExplanationForNoContributionToMLSBuildingProjectFund` varchar(250) NULL,
    `PerformedFullMandatoryProBonoWork` tinyint(1) NOT NULL,
    `ExplanationForNoFullMandatoryProBonoWork` varchar(250) NULL,
    `AttainedMinimumNumberOfCLEUnits` tinyint(1) NOT NULL,
    `ExplanationForNoMinimumNumberOfCLEUnits` varchar(250) NULL,
    `HasValidAnnualProfessionalIndemnityInsuranceCover` tinyint(1) NOT NULL,
    `ExplanationForNoProfessionalIndemnityInsuranceCover` varchar(250) NULL,
    `SubmittedValidTaxClearanceCertificate` tinyint(1) NOT NULL,
    `ExplanationForNoValidTaxClearanceCertificate` varchar(250) NULL,
    `SubmittedAccountantsCertificate` tinyint(1) NOT NULL,
    `ExplanationForNoAccountantsCertificate` varchar(250) NULL,
    `CompliedWithPenaltiesImposedUnderTheAct` tinyint(1) NOT NULL,
    `ExplanationForNoComplianceWithPenalties` varchar(250) NULL,
    `CertificateOfAdmission` tinyint(1) NOT NULL,
    `ExplanationForNotSubmittingCertificateOfAdmission` varchar(250) NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_LicenseApplications_LicenseApprovalLevels_CurrentApprovalLev~` FOREIGN KEY (`CurrentApprovalLevelID`) REFERENCES `LicenseApprovalLevels` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LicenseApplications_Members_MemberId` FOREIGN KEY (`MemberId`) REFERENCES `Members` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LicenseApplications_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LicenseApplications_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `MemberQualifications` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext NOT NULL,
    `MemberId` int NOT NULL,
    `IssuingInstitution` varchar(250) NOT NULL,
    `DateObtained` datetime(6) NOT NULL,
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

CREATE TABLE `Penalties` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `MemberId` int NOT NULL,
    `PenaltyTypeId` int NOT NULL,
    `Fee` double NOT NULL,
    `Reason` longtext NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `YearOfOperationId` int NOT NULL,
    `AmountPaid` double NOT NULL,
    `AmountRemaining` double NOT NULL,
    `PenaltyStatus` varchar(100) NOT NULL,
    `ResolutionComment` varchar(250) NULL,
    `InvoiceRequestId` int NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Penalties_InvoiceRequests_InvoiceRequestId` FOREIGN KEY (`InvoiceRequestId`) REFERENCES `InvoiceRequests` (`Id`),
    CONSTRAINT `FK_Penalties_Members_MemberId` FOREIGN KEY (`MemberId`) REFERENCES `Members` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Penalties_PenaltyTypes_PenaltyTypeId` FOREIGN KEY (`PenaltyTypeId`) REFERENCES `PenaltyTypes` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Penalties_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Penalties_YearOfOperations_YearOfOperationId` FOREIGN KEY (`YearOfOperationId`) REFERENCES `YearOfOperations` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Subcommittees` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `SubcommitteeName` longtext NOT NULL,
    `Description` longtext NOT NULL,
    `ChairpersonId` int NULL,
    `CommitteeId` int NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Subcommittees_Committees_CommitteeId` FOREIGN KEY (`CommitteeId`) REFERENCES `Committees` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Subcommittees_Members_ChairpersonId` FOREIGN KEY (`ChairpersonId`) REFERENCES `Members` (`Id`),
    CONSTRAINT `FK_Subcommittees_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
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

CREATE TABLE `Messages` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CommitteeID` int NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `Timestamp` datetime(6) NOT NULL,
    `Content` longtext NOT NULL,
    `ThreadId` int NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Messages_Committees_CommitteeID` FOREIGN KEY (`CommitteeID`) REFERENCES `Committees` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Messages_Threads_ThreadId` FOREIGN KEY (`ThreadId`) REFERENCES `Threads` (`Id`),
    CONSTRAINT `FK_Messages_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
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

CREATE TABLE `LicenseApprovalHistories` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `LicenseApplicationId` int NOT NULL,
    `ChangeDate` datetime(6) NOT NULL,
    `ApprovalLevelId` int NOT NULL,
    `Status` longtext NOT NULL,
    `ChangedById` varchar(200) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_LicenseApprovalHistories_LicenseApplications_LicenseApplicat~` FOREIGN KEY (`LicenseApplicationId`) REFERENCES `LicenseApplications` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LicenseApprovalHistories_LicenseApprovalLevels_ApprovalLevel~` FOREIGN KEY (`ApprovalLevelId`) REFERENCES `LicenseApprovalLevels` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_LicenseApprovalHistories_Users_ChangedById` FOREIGN KEY (`ChangedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Licenses` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `LicenseNumber` longtext NOT NULL,
    `MemberId` int NOT NULL,
    `ExpiryDate` datetime(6) NOT NULL,
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

CREATE TABLE `PenaltyPayments` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `PenaltyId` int NOT NULL,
    `PaymentStatus` longtext NOT NULL,
    `Description` longtext NULL,
    `Fee` double NOT NULL,
    `DateApproved` datetime(6) NULL,
    `DateDenied` datetime(6) NULL,
    `ReasonForDenial` varchar(250) NULL,
    `QBInvoiceId` varchar(255) NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_PenaltyPayments_Penalties_PenaltyId` FOREIGN KEY (`PenaltyId`) REFERENCES `Penalties` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_PenaltyPayments_QBInvoices_QBInvoiceId` FOREIGN KEY (`QBInvoiceId`) REFERENCES `QBInvoices` (`Id`)
);

CREATE TABLE `SubcommitteeMemberships` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `SubcommitteeID` int NOT NULL,
    `MemberShipId` varchar(200) NOT NULL,
    `MemberShipStatus` longtext NULL,
    `JoinedDate` datetime(6) NOT NULL,
    `Role` varchar(150) NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_SubcommitteeMemberships_Subcommittees_SubcommitteeID` FOREIGN KEY (`SubcommitteeID`) REFERENCES `Subcommittees` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_SubcommitteeMemberships_Users_MemberShipId` FOREIGN KEY (`MemberShipId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `SubcommitteeThreads` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `SubcommitteeId` int NOT NULL,
    `Subject` longtext NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_SubcommitteeThreads_Subcommittees_SubcommitteeId` FOREIGN KEY (`SubcommitteeId`) REFERENCES `Subcommittees` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_SubcommitteeThreads_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
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

CREATE TABLE `SubcommitteeMessages` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `SubcommitteeID` int NOT NULL,
    `CreatedById` varchar(200) NOT NULL,
    `Timestamp` datetime(6) NOT NULL,
    `Content` longtext NOT NULL,
    `SubcommitteeThreadId` int NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `Status` longtext NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    `DeletedDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_SubcommitteeMessages_SubcommitteeThreads_SubcommitteeThreadId` FOREIGN KEY (`SubcommitteeThreadId`) REFERENCES `SubcommitteeThreads` (`Id`),
    CONSTRAINT `FK_SubcommitteeMessages_Subcommittees_SubcommitteeID` FOREIGN KEY (`SubcommitteeID`) REFERENCES `Subcommittees` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_SubcommitteeMessages_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
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

CREATE INDEX `IX_AttachmentCPDTraining_CPDTrainingsId` ON `AttachmentCPDTraining` (`CPDTrainingsId`);

CREATE INDEX `IX_AttachmentCPDTrainingRegistration_CPDTrainingRegistrationsId` ON `AttachmentCPDTrainingRegistration` (`CPDTrainingRegistrationsId`);

CREATE INDEX `IX_AttachmentLevyDeclaration_LevyDeclarationsId` ON `AttachmentLevyDeclaration` (`LevyDeclarationsId`);

CREATE INDEX `IX_AttachmentLicenseApplication_LicenseApplicationsId` ON `AttachmentLicenseApplication` (`LicenseApplicationsId`);

CREATE INDEX `IX_AttachmentMemberQualification_MemberQualificationsId` ON `AttachmentMemberQualification` (`MemberQualificationsId`);

CREATE INDEX `IX_AttachmentMessage_MessagesId` ON `AttachmentMessage` (`MessagesId`);

CREATE INDEX `IX_AttachmentPenalty_PenaltiesId` ON `AttachmentPenalty` (`PenaltiesId`);

CREATE INDEX `IX_AttachmentPenaltyPayment_PenaltyPaymentsId` ON `AttachmentPenaltyPayment` (`PenaltyPaymentsId`);

CREATE INDEX `IX_AttachmentProBono_ProBonosId` ON `AttachmentProBono` (`ProBonosId`);

CREATE INDEX `IX_AttachmentProBonoApplication_ProBonosApplicationsId` ON `AttachmentProBonoApplication` (`ProBonosApplicationsId`);

CREATE INDEX `IX_AttachmentProBonoReport_ProBonoReportsId` ON `AttachmentProBonoReport` (`ProBonoReportsId`);

CREATE INDEX `IX_Attachments_AttachmentTypeId` ON `Attachments` (`AttachmentTypeId`);

CREATE INDEX `IX_Attachments_SubcommitteeMessageId` ON `Attachments` (`SubcommitteeMessageId`);

CREATE INDEX `IX_CommitteeMembers_CommitteeID` ON `CommitteeMembers` (`CommitteeID`);

CREATE INDEX `IX_CommitteeMembers_MemberShipId` ON `CommitteeMembers` (`MemberShipId`);

CREATE INDEX `IX_Committees_ChairpersonID` ON `Committees` (`ChairpersonID`);

CREATE INDEX `IX_Committees_CreatedById` ON `Committees` (`CreatedById`);

CREATE INDEX `IX_Committees_YearOfOperationId` ON `Committees` (`YearOfOperationId`);

CREATE INDEX `IX_CommunicationMessages_SentByUserId` ON `CommunicationMessages` (`SentByUserId`);

CREATE INDEX `IX_CPDTrainingRegistrations_CPDTrainingId` ON `CPDTrainingRegistrations` (`CPDTrainingId`);

CREATE INDEX `IX_CPDTrainingRegistrations_CreatedById` ON `CPDTrainingRegistrations` (`CreatedById`);

CREATE INDEX `IX_CPDTrainingRegistrations_InvoiceRequestId` ON `CPDTrainingRegistrations` (`InvoiceRequestId`);

CREATE INDEX `IX_CPDTrainingRegistrations_MemberId` ON `CPDTrainingRegistrations` (`MemberId`);

CREATE INDEX `IX_CPDTrainings_CreatedById` ON `CPDTrainings` (`CreatedById`);

CREATE INDEX `IX_CPDTrainings_YearOfOperationId` ON `CPDTrainings` (`YearOfOperationId`);

CREATE INDEX `IX_CPDUnitsEarned_CPDTrainingId` ON `CPDUnitsEarned` (`CPDTrainingId`);

CREATE INDEX `IX_CPDUnitsEarned_MemberId` ON `CPDUnitsEarned` (`MemberId`);

CREATE INDEX `IX_CPDUnitsEarned_YearOfOperationId` ON `CPDUnitsEarned` (`YearOfOperationId`);

CREATE INDEX `IX_ErrorLogs_CreatedById` ON `ErrorLogs` (`CreatedById`);

CREATE INDEX `IX_Firms_CreatedById` ON `Firms` (`CreatedById`);

CREATE INDEX `IX_Firms_CustomerId` ON `Firms` (`CustomerId`);

CREATE INDEX `IX_InvoiceRequests_CreatedById` ON `InvoiceRequests` (`CreatedById`);

CREATE INDEX `IX_InvoiceRequests_CustomerId` ON `InvoiceRequests` (`CustomerId`);

CREATE INDEX `IX_InvoiceRequests_QBInvoiceId` ON `InvoiceRequests` (`QBInvoiceId`);

CREATE INDEX `IX_InvoiceRequests_YearOfOperationId` ON `InvoiceRequests` (`YearOfOperationId`);

CREATE INDEX `IX_LevyDeclarations_FirmId` ON `LevyDeclarations` (`FirmId`);

CREATE INDEX `IX_LevyDeclarations_InvoiceRequestId` ON `LevyDeclarations` (`InvoiceRequestId`);

CREATE INDEX `IX_LevyPercents_YearOfOperationId` ON `LevyPercents` (`YearOfOperationId`);

CREATE INDEX `IX_LicenseApplicationApprovals_CreatedById` ON `LicenseApplicationApprovals` (`CreatedById`);

CREATE INDEX `IX_LicenseApplicationApprovals_LicenseApplicationID` ON `LicenseApplicationApprovals` (`LicenseApplicationID`);

CREATE INDEX `IX_LicenseApplicationApprovals_LicenseApprovalLevelID` ON `LicenseApplicationApprovals` (`LicenseApprovalLevelID`);

CREATE INDEX `IX_LicenseApplications_CreatedById` ON `LicenseApplications` (`CreatedById`);

CREATE INDEX `IX_LicenseApplications_CurrentApprovalLevelID` ON `LicenseApplications` (`CurrentApprovalLevelID`);

CREATE INDEX `IX_LicenseApplications_MemberId` ON `LicenseApplications` (`MemberId`);

CREATE INDEX `IX_LicenseApplications_YearOfOperationId` ON `LicenseApplications` (`YearOfOperationId`);

CREATE INDEX `IX_LicenseApprovalComments_ApprovalHistoryId` ON `LicenseApprovalComments` (`ApprovalHistoryId`);

CREATE INDEX `IX_LicenseApprovalComments_CommentedById` ON `LicenseApprovalComments` (`CommentedById`);

CREATE INDEX `IX_LicenseApprovalHistories_ApprovalLevelId` ON `LicenseApprovalHistories` (`ApprovalLevelId`);

CREATE INDEX `IX_LicenseApprovalHistories_ChangedById` ON `LicenseApprovalHistories` (`ChangedById`);

CREATE INDEX `IX_LicenseApprovalHistories_LicenseApplicationId` ON `LicenseApprovalHistories` (`LicenseApplicationId`);

CREATE INDEX `IX_LicenseApprovalLevels_DepartmentId` ON `LicenseApprovalLevels` (`DepartmentId`);

CREATE UNIQUE INDEX `IX_Licenses_LicenseApplicationId` ON `Licenses` (`LicenseApplicationId`);

CREATE INDEX `IX_Licenses_MemberId` ON `Licenses` (`MemberId`);

CREATE INDEX `IX_Licenses_YearOfOperationId` ON `Licenses` (`YearOfOperationId`);

CREATE INDEX `IX_MemberProBono_ProBonosId` ON `MemberProBono` (`ProBonosId`);

CREATE INDEX `IX_MemberQualifications_MemberId` ON `MemberQualifications` (`MemberId`);

CREATE INDEX `IX_MemberQualifications_QualificationTypeId` ON `MemberQualifications` (`QualificationTypeId`);

CREATE INDEX `IX_MemberQualificationType_QualificationTypesId` ON `MemberQualificationType` (`QualificationTypesId`);

CREATE INDEX `IX_Members_CustomerId` ON `Members` (`CustomerId`);

CREATE INDEX `IX_Members_FirmId` ON `Members` (`FirmId`);

CREATE INDEX `IX_Members_UserId` ON `Members` (`UserId`);

CREATE INDEX `IX_Messages_CommitteeID` ON `Messages` (`CommitteeID`);

CREATE INDEX `IX_Messages_CreatedById` ON `Messages` (`CreatedById`);

CREATE INDEX `IX_Messages_ThreadId` ON `Messages` (`ThreadId`);

CREATE INDEX `IX_Penalties_CreatedById` ON `Penalties` (`CreatedById`);

CREATE INDEX `IX_Penalties_InvoiceRequestId` ON `Penalties` (`InvoiceRequestId`);

CREATE INDEX `IX_Penalties_MemberId` ON `Penalties` (`MemberId`);

CREATE INDEX `IX_Penalties_PenaltyTypeId` ON `Penalties` (`PenaltyTypeId`);

CREATE INDEX `IX_Penalties_YearOfOperationId` ON `Penalties` (`YearOfOperationId`);

CREATE INDEX `IX_PenaltyPayments_PenaltyId` ON `PenaltyPayments` (`PenaltyId`);

CREATE INDEX `IX_PenaltyPayments_QBInvoiceId` ON `PenaltyPayments` (`QBInvoiceId`);

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

CREATE INDEX `IX_QBInvoices_CustomerId` ON `QBInvoices` (`CustomerId`);

CREATE INDEX `IX_QBPayments_CustomerId` ON `QBPayments` (`CustomerId`);

CREATE INDEX `IX_QBPayments_InvoiceId` ON `QBPayments` (`InvoiceId`);

CREATE INDEX `IX_QBReceipts_CustomerId` ON `QBReceipts` (`CustomerId`);

CREATE INDEX `IX_QBReceipts_InvoiceId` ON `QBReceipts` (`InvoiceId`);

CREATE INDEX `IX_RoleClaims_RoleId` ON `RoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `Roles` (`NormalizedName`);

CREATE INDEX `IX_SubcommitteeMemberships_MemberShipId` ON `SubcommitteeMemberships` (`MemberShipId`);

CREATE INDEX `IX_SubcommitteeMemberships_SubcommitteeID` ON `SubcommitteeMemberships` (`SubcommitteeID`);

CREATE INDEX `IX_SubcommitteeMessages_CreatedById` ON `SubcommitteeMessages` (`CreatedById`);

CREATE INDEX `IX_SubcommitteeMessages_SubcommitteeID` ON `SubcommitteeMessages` (`SubcommitteeID`);

CREATE INDEX `IX_SubcommitteeMessages_SubcommitteeThreadId` ON `SubcommitteeMessages` (`SubcommitteeThreadId`);

CREATE INDEX `IX_Subcommittees_ChairpersonId` ON `Subcommittees` (`ChairpersonId`);

CREATE INDEX `IX_Subcommittees_CommitteeId` ON `Subcommittees` (`CommitteeId`);

CREATE INDEX `IX_Subcommittees_CreatedById` ON `Subcommittees` (`CreatedById`);

CREATE INDEX `IX_SubcommitteeThreads_CreatedById` ON `SubcommitteeThreads` (`CreatedById`);

CREATE INDEX `IX_SubcommitteeThreads_SubcommitteeId` ON `SubcommitteeThreads` (`SubcommitteeId`);

CREATE INDEX `IX_Threads_CommitteeId` ON `Threads` (`CommitteeId`);

CREATE INDEX `IX_Threads_CreatedById` ON `Threads` (`CreatedById`);

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

ALTER TABLE `AttachmentCPDTraining` ADD CONSTRAINT `FK_AttachmentCPDTraining_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentCPDTraining` ADD CONSTRAINT `FK_AttachmentCPDTraining_CPDTrainings_CPDTrainingsId` FOREIGN KEY (`CPDTrainingsId`) REFERENCES `CPDTrainings` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentCPDTrainingRegistration` ADD CONSTRAINT `FK_AttachmentCPDTrainingRegistration_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentCPDTrainingRegistration` ADD CONSTRAINT `FK_AttachmentCPDTrainingRegistration_CPDTrainingRegistrations_C~` FOREIGN KEY (`CPDTrainingRegistrationsId`) REFERENCES `CPDTrainingRegistrations` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentLevyDeclaration` ADD CONSTRAINT `FK_AttachmentLevyDeclaration_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentLevyDeclaration` ADD CONSTRAINT `FK_AttachmentLevyDeclaration_LevyDeclarations_LevyDeclarationsId` FOREIGN KEY (`LevyDeclarationsId`) REFERENCES `LevyDeclarations` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentLicenseApplication` ADD CONSTRAINT `FK_AttachmentLicenseApplication_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentLicenseApplication` ADD CONSTRAINT `FK_AttachmentLicenseApplication_LicenseApplications_LicenseAppl~` FOREIGN KEY (`LicenseApplicationsId`) REFERENCES `LicenseApplications` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentMemberQualification` ADD CONSTRAINT `FK_AttachmentMemberQualification_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentMemberQualification` ADD CONSTRAINT `FK_AttachmentMemberQualification_MemberQualifications_MemberQua~` FOREIGN KEY (`MemberQualificationsId`) REFERENCES `MemberQualifications` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentMessage` ADD CONSTRAINT `FK_AttachmentMessage_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentMessage` ADD CONSTRAINT `FK_AttachmentMessage_Messages_MessagesId` FOREIGN KEY (`MessagesId`) REFERENCES `Messages` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentPenalty` ADD CONSTRAINT `FK_AttachmentPenalty_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentPenalty` ADD CONSTRAINT `FK_AttachmentPenalty_Penalties_PenaltiesId` FOREIGN KEY (`PenaltiesId`) REFERENCES `Penalties` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentPenaltyPayment` ADD CONSTRAINT `FK_AttachmentPenaltyPayment_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentPenaltyPayment` ADD CONSTRAINT `FK_AttachmentPenaltyPayment_PenaltyPayments_PenaltyPaymentsId` FOREIGN KEY (`PenaltyPaymentsId`) REFERENCES `PenaltyPayments` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentProBono` ADD CONSTRAINT `FK_AttachmentProBono_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentProBono` ADD CONSTRAINT `FK_AttachmentProBono_ProBonos_ProBonosId` FOREIGN KEY (`ProBonosId`) REFERENCES `ProBonos` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentProBonoApplication` ADD CONSTRAINT `FK_AttachmentProBonoApplication_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentProBonoApplication` ADD CONSTRAINT `FK_AttachmentProBonoApplication_ProBonoApplications_ProBonosApp~` FOREIGN KEY (`ProBonosApplicationsId`) REFERENCES `ProBonoApplications` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentProBonoReport` ADD CONSTRAINT `FK_AttachmentProBonoReport_Attachments_AttachmentsId` FOREIGN KEY (`AttachmentsId`) REFERENCES `Attachments` (`Id`) ON DELETE CASCADE;

ALTER TABLE `AttachmentProBonoReport` ADD CONSTRAINT `FK_AttachmentProBonoReport_ProBonoReports_ProBonoReportsId` FOREIGN KEY (`ProBonoReportsId`) REFERENCES `ProBonoReports` (`Id`) ON DELETE CASCADE;

ALTER TABLE `Attachments` ADD CONSTRAINT `FK_Attachments_SubcommitteeMessages_SubcommitteeMessageId` FOREIGN KEY (`SubcommitteeMessageId`) REFERENCES `SubcommitteeMessages` (`Id`);

ALTER TABLE `CommitteeMembers` ADD CONSTRAINT `FK_CommitteeMembers_Committees_CommitteeID` FOREIGN KEY (`CommitteeID`) REFERENCES `Committees` (`Id`) ON DELETE CASCADE;

ALTER TABLE `CommitteeMembers` ADD CONSTRAINT `FK_CommitteeMembers_Users_MemberShipId` FOREIGN KEY (`MemberShipId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

ALTER TABLE `Committees` ADD CONSTRAINT `FK_Committees_Members_ChairpersonID` FOREIGN KEY (`ChairpersonID`) REFERENCES `Members` (`Id`);

ALTER TABLE `Committees` ADD CONSTRAINT `FK_Committees_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

ALTER TABLE `CommunicationMessages` ADD CONSTRAINT `FK_CommunicationMessages_Users_SentByUserId` FOREIGN KEY (`SentByUserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

ALTER TABLE `CPDTrainingRegistrations` ADD CONSTRAINT `FK_CPDTrainingRegistrations_CPDTrainings_CPDTrainingId` FOREIGN KEY (`CPDTrainingId`) REFERENCES `CPDTrainings` (`Id`) ON DELETE CASCADE;

ALTER TABLE `CPDTrainingRegistrations` ADD CONSTRAINT `FK_CPDTrainingRegistrations_InvoiceRequests_InvoiceRequestId` FOREIGN KEY (`InvoiceRequestId`) REFERENCES `InvoiceRequests` (`Id`);

ALTER TABLE `CPDTrainingRegistrations` ADD CONSTRAINT `FK_CPDTrainingRegistrations_Members_MemberId` FOREIGN KEY (`MemberId`) REFERENCES `Members` (`Id`) ON DELETE CASCADE;

ALTER TABLE `CPDTrainingRegistrations` ADD CONSTRAINT `FK_CPDTrainingRegistrations_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

ALTER TABLE `CPDTrainings` ADD CONSTRAINT `FK_CPDTrainings_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;

ALTER TABLE `CPDUnitsEarned` ADD CONSTRAINT `FK_CPDUnitsEarned_Members_MemberId` FOREIGN KEY (`MemberId`) REFERENCES `Members` (`Id`) ON DELETE CASCADE;

ALTER TABLE `ErrorLogs` ADD CONSTRAINT `FK_ErrorLogs_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`);

ALTER TABLE `Firms` ADD CONSTRAINT `FK_Firms_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `Users` (`Id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240729133303_initial migration', '8.0.6');

COMMIT;

