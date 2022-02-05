/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


delete from [imgclimprt].DocumentTypes
delete from [imgclimprt].DocIds

declare @PLNOG int = 1
insert into [imgclimprt].DocIds (DocIdId, DocIdValue) values (@PLNOG, 'PLNOG')
declare @PLALA int = 2
insert into [imgclimprt].DocIds (DocIdId, DocIdValue) values (@PLALA, 'PLALA')
declare @PLMPN int = 3
insert into [imgclimprt].DocIds (DocIdId, DocIdValue) values (@PLMPN, 'PLMPN')

insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('AdverseAction', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('Application', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('AutoMergedPromissoryNote', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('AutoNotification', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('AwardLetter', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('BankStatement', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('BenefitAwardLetter', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('BillsMail', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('BirthCertificate', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('CertificateOfNaturalization', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('CheckCoverLetter', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('CosignerNoticeSigned', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('CosignerSpousalNoticeSigned', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('CustomerComplaint', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('DeathCertificate', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('Disclosure', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('DriversLicense', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('EnrollmentVerification', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('FederalIncomeTaxReturn', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('GIBillPayments', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('Graduation Verification', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('High School Transcript', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('IDTheftNotificationDocumentation', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('InquiryRemovalRequest', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('INSCard', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('INSExtensionLetter', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('IntentToDateDocumentBorrower', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('IntentToDateDocumentCosigner', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('LetterFromEmployer', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('LetterFromEmploymentVerificationProvider', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('LoanChangeRequest', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('LoanDocuments', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('LoanDocuments : BorrowerWelcomePack', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('LoanDocumentsESigned', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('LoanDocumentsESignedBorrower', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('LoanDocumentsESignedCosigner', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('LoanStatement', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('MilitaryID', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('MissingInformationNotice', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('MississippiParentalConsentSigned', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('NoticeOfIncompleteness', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('Other', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('Passport', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('PayoffAuthBorrower', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('PayoffAuthServicer', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('PayoffVerification', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('PayStub', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('PowerOfAttorney', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('PromissoryNoteSigned', @PLMPN)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('PromissoryNoteSigned : LoanDocsAppr', @PLMPN)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('PromissoryNoteSignedBorrower', @PLMPN)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('PromissoryNoteSignedCosigner', @PLMPN)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('ProtectedIncomeVerification', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('RBPDisclosure', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('RBPDisclosureBorrower', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('RBPDisclosureCosigner', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('RBPDisclosureCover', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('RefundFileForm', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('Reminder', @PLNOG)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('SchoolCertification', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('SelfCertificationSigned', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('SocialSecurityAdminLetter', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('SocialSecurityCard', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('SocialSecurityIncomeStatement', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('StateID', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('Subpoena', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('TaxDocument', @PLALA)
insert into [imgclimprt].DocumentTypes (DocumentTypeValue, DocIdId)values ('UnemploymentStatement', @PLALA)


--AutoMergedPromissoryNote
--Disclosure-ApprovalDisclosurePDF
--Disclosure-FinalDisclosure
--Disclosure-ReDisclosurePDF
--LoanDocumentsESigned


grant execute on [imgclimprt] to [db_executor]
EXEC sp_addrolemember '[db_executor]', '[Imaging Users]' 