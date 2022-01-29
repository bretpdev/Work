USE MauiDUDE
GO
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE calls.Tmp_Categories
	(
	CategoryId int NOT NULL IDENTITY (X, X),
	Title varchar(XX) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE calls.Tmp_Categories SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT calls.Tmp_Categories ON
GO
IF EXISTS(SELECT * FROM calls.Categories)
	 EXEC('INSERT INTO calls.Tmp_Categories (CategoryId, Title)
		SELECT CategoryId, Title FROM calls.Categories WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT calls.Tmp_Categories OFF
GO
ALTER TABLE calls.Reasons
	DROP CONSTRAINT FK_Reasons_Categories
GO
DROP TABLE calls.Categories
GO
EXECUTE sp_rename N'calls.Tmp_Categories', N'Categories', 'OBJECT' 
GO
ALTER TABLE calls.Categories ADD CONSTRAINT
	PK__Categori__XXXXXAXBXDXXXXAX PRIMARY KEY CLUSTERED 
	(
	CategoryId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE calls.Reasons ADD CONSTRAINT
	FK_Reasons_Categories FOREIGN KEY
	(
	CategoryId
	) REFERENCES calls.Categories
	(
	CategoryId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE calls.Reasons SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE	@CategoryId INT

INSERT INTO MauiDUDE.calls.Categories
	(
		Title
	)
VALUES
	(
		'Borrower Defense'
	)

SET @CategoryId = SCOPE_IDENTITY()

INSERT INTO	MauiDUDE.calls.Reasons
	(
		CategoryId, 
		ReasonText, 
		Uheaa, 
		Cornerstone, 
		Inbound, 
		Outbound,
		[Enabled]
	)
VALUES
(
		@CategoryId,
		'Borrower Defense and add a new Reason: Borrower claims school misconduct(borrower defense to repayment)',
		X,
		X,
		X,
		X,
		X
)	

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = X AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
