CREATE TABLE [dbo].[LTDB_SystemLettersStoredProcedures]
(
	[SystemLettersStoredProcedureId] INT NOT NULL PRIMARY KEY IDENTITY,
	LetterId INT not null 
		CONSTRAINT FK_LETTER_ID_SP
			REFERENCES  LTDB_DAT_DocDetail(DocDetailId),
	StoredProcedureName varchar(100) not null,
	[ReturnTypeId] INT not null
		CONSTRAINT FK_ReturnType
			REFERENCES  LTDB_SystemLettersReturnType(ReturnTypeId), 
    [Active] BIT NOT NULL DEFAULT 1,
)
