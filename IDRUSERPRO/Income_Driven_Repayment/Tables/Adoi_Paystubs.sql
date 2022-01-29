CREATE TABLE [dbo].[Adoi_Paystubs]
(
	[adoi_paystub_id] INT NOT NULL PRIMARY KEY IDENTITY,
	[application_id] INT NOT NULL,
	[ftw] decimal(10, 2) NOT NULL,
	[gross] decimal(10, 2) NOT NULL,
	[pre_tax_deductions] decimal(10, 2) NOT NULL,
	[bonus] decimal(10, 2) NOT NULL,
	[overtime] decimal(10, 2) NOT NULL,
	[adoi_paystub_frequency_id] INT NOT NULL, 
    [employer_name] VARCHAR(50) NULL, 
	[is_spouse] BIT NOT NULL,
    CONSTRAINT [FK_Adoi_Paystubs_Adoi_Paystub_Frequencies] FOREIGN KEY ([adoi_paystub_frequency_id]) REFERENCES [Adoi_Paystub_Frequencies]([adoi_paystub_frequency_id]), 
    CONSTRAINT [FK_Adoi_Paystubs_Adoi_Application] FOREIGN KEY ([application_id]) REFERENCES [Applications]([application_id]) ON DELETE CASCADE
)
