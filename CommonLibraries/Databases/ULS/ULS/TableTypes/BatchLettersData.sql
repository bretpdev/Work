CREATE TYPE [dbo].[BatchLettersData] AS TABLE
(
	BatchLettersId int,
	LetterId varchar(10),
	SasFilePattern varchar(50),
	StateFieldCodeName varchar(25),
	AccountNumberFieldName varchar(25),
	CostCenterFieldCodeName varchar(25),
	IsDuplex bit,
	OkIfMissing bit,
	ProcessAllFiles bit,
	Arc varchar(5),
	Comment varchar(1200),
	CreatedAt datetime,
	CreatedBy [varchar](250),
	UpdatedAt datetime,
	UpdatedBy varchar(250),
	Active bit  
)
