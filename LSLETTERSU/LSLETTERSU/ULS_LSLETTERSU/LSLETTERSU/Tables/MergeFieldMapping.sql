CREATE TABLE [lslettersu].[MergeFieldMapping]
(
	[MergeFieldMappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MergeFieldsId] INT NOT NULL, 
    [FormFieldsId] INT NOT NULL, 
    [LoanServicingLettersId] INT NOT NULL, 
    CONSTRAINT [FK_MergeFieldMapping_MergeFields] FOREIGN KEY ([MergeFieldsId]) REFERENCES [lslettersu].[MergeFields]([MergeFieldsId]), 
    CONSTRAINT [FK_MergeFieldMapping_FormFields] FOREIGN KEY ([FormFieldsId]) REFERENCES [lslettersu].[FormFields]([FormFieldsId]), 
    CONSTRAINT [FK_MergeFieldMapping_LoanServicingLetters] FOREIGN KEY ([LoanServicingLettersId]) REFERENCES [lslettersu].[LoanServicingLetters]([LoanServicingLettersId])
)
