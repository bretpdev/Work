CREATE TYPE [dbo].[AdoiPaystubs] AS TABLE
(
	[ftw] decimal(10, 2) NOT NULL,
	[gross] decimal(10, 2) NOT NULL,
	[pre_tax_deductions] decimal(10, 2) NOT NULL,
	[bonus] decimal(10, 2) NOT NULL,
	[overtime] decimal(10, 2) NOT NULL,
	[adoi_paystub_frequency_id] INT NOT NULL,
	[employer_name] VARCHAR(50) NOT NULL
)
