CREATE TABLE [dbo].[Letters] (
    [LetterId]          INT           IDENTITY (1, 1) NOT NULL,
    [Letter]            VARCHAR (10)  NOT NULL,
    [LetterTypeId]      INT           NOT NULL,
    [DocId]             VARCHAR (10)  NOT NULL,
    [Viewable]         CHAR (1)      NOT NULL,
    [ReportDescription] VARCHAR (60)  NOT NULL,
    [ReportName]        VARCHAR (17)  NOT NULL,
    [Viewed]            CHAR (1)      NOT NULL,
    [MainframeRegion]   VARCHAR (8)   NOT NULL,
    [SubjectLine]       VARCHAR (50)  NOT NULL,
    [DocSource]         VARCHAR (10)  NOT NULL,
    [DocComment]        VARCHAR (255) NOT NULL,
    [WorkFlow]          CHAR (1)      NOT NULL,
    [DocDelete]         CHAR (1)      NOT NULL,
    [Active] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_Letters] PRIMARY KEY CLUSTERED ([LetterId] ASC),
	CONSTRAINT [CK_DocumentDetails_DocId] CHECK (DocId COLLATE latin1_general_cs_as IN ('XEBL' ,'XEIL', 'XECL', 'XELT', 'XERO')), -- These are the only valid values 
    CONSTRAINT [FK_Letters_LetterTypes] FOREIGN KEY (LetterTypeId) REFERENCES [LetterTypes]([LetterTypeId]));

GO

CREATE TRIGGER [dbo].[Trigger_Letters]
    ON [dbo].[Letters]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON

		DECLARE @Count int =
			(SELECT
				COUNT(*)
			FROM
				Letters A
				INNER JOIN INSERTED I
					ON A.LetterId = I.LetterId
					AND A.Active = 1)
		IF (@Count > 1)
		BEGIN
			ROLLBACK TRANSACTION
			RAISERROR('Duplicate application names are not permitted.', 16, 1)
			RETURN
		END
    END

