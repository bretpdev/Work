CREATE TABLE [dbo].[Arc_Letter_Mapping] (
    [repayment_type_id]        INT NOT NULL,
    [repayment_type_status_id] INT NOT NULL,
    [mapping_id]               INT NOT NULL,
    CONSTRAINT [FK_Arc_Letter_Mapping_Arc_Letter_Data] FOREIGN KEY ([mapping_id]) REFERENCES [dbo].[Arc_Letter_Data] ([mapping_id]),
    CONSTRAINT [FK_Arc_Letter_Mapping_Repayment_Type] FOREIGN KEY ([repayment_type_id]) REFERENCES [dbo].[Repayment_Type] ([repayment_type_id]),
    CONSTRAINT [FK_Arc_Letter_Mapping_Repayment_Type_Status] FOREIGN KEY ([repayment_type_status_id]) REFERENCES [dbo].[Repayment_Type_Status] ([repayment_type_status_id])
);

