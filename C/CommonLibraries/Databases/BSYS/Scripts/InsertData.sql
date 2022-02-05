USE BSYS
GO

INSERT INTO [dbo].[LTDB_LST_HeaderTypes]([HeaderType],[CreatedBy])
VALUES('NAME','DCR'),
('CITY_STATE_ZIP','DCR'),
('DATA_TABLE','DCR'),
('FORM_FIELD','DCR')

GO

INSERT INTO [dbo].[LTDB_File_Headers]([Header],[CreatedBy])
VALUES('ACCT NO','DCR'),
('FIRST NAME','DCR'),
('MIDDLE INITIAL','DCR'),
('LAST NAME','DCR'),
('STREET 1','DCR'),
('STREET 2','DCR'),
('CITY','DCR'),
('STATE','DCR'),
('ZIP','DCR'),
('FOREIGN COUNTRY','DCR'),
('FOREIGN STATE','DCR')