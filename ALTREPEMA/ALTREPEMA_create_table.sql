--set up placeholder table for BU to specify values
USE CLS
GO

CREATE TABLE dbo.ALTREPEMA
(
	[Table Index] INT IDENTITY(1,1) PRIMARY KEY,
	Segment SMALLINT NOT NULL,
	[Max Number] INT NOT NULL,
	[Min Delq] SMALLINT NOT NULL,
	[Max Delq] SMALLINT NOT NULL,
	AddedAt DATETIME DEFAULT GETDATE(),
	AddedBy VARCHAR(50) DEFAULT SYSTEM_USER
);

INSERT INTO dbo.ALTREPEMA 
(	
	Segment,
	[Max Number],
	[Min Delq],
	[Max Delq]
)
VALUES 
	(1,2500,30,32767, GETDATE(), 'SASR 4356') 
	

