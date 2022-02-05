CREATE PROCEDURE [dbo].[InsertEstimate]
	@RequestType VARCHAR(10),
	@RequestNumber VARCHAR(50),
	@EstimatedHours DECIMAL(18,2),
	@TestHours DECIMAL(18,2) ,
	@ReasonForAdjustment VARCHAR(MAX) = NULL,
	@AttachmentFileName VARCHAR(500) = NULL,
	@AdditionalHrs DECIMAL(18,2) = NULL,
	@Employee VARCHAR(500)
AS
	INSERT INTO Estimates(RequestType, RequestNumber, EstimatedHours, ReasonForAdjustment, AttachmentFileName, Employee, CreatedAt, AdditionalHrs, TestingFixes)
	VALUES(@RequestType, @RequestNumber, @EstimatedHours, @ReasonForAdjustment, @AttachmentFileName, @Employee, GETDATE(), @AdditionalHrs, @TestHours)
RETURN 0
