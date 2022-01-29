CREATE TABLE [dbo].[Repayment_Plan_Type_Status_History] (
    [repayment_plan_type_status_history_id] INT          IDENTITY (1, 1) NOT NULL,
    [repayment_plan_type_id]                INT          NOT NULL,
    [repayment_plan_type_status_mapping_id] INT          NOT NULL,
    [created_at]                            DATETIME     CONSTRAINT [DF__Repayment__creat__25869641] DEFAULT (getdate()) NOT NULL,
    [created_by]                            VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_Repayment_Plan_Type_Status_History] PRIMARY KEY CLUSTERED ([repayment_plan_type_status_history_id] ASC),
    CONSTRAINT [FK_Repayment_Plan_Type_Status_History_Repayment_Plan_Type] FOREIGN KEY ([repayment_plan_type_id]) REFERENCES [dbo].[Repayment_Plan_Selected] ([repayment_plan_type_id]),
    CONSTRAINT [FK_Repayment_Plan_Type_Status_History_Repayment_Plan_Type_Substatus] FOREIGN KEY ([repayment_plan_type_status_mapping_id]) REFERENCES [dbo].[Repayment_Plan_Type_Substatus] ([repayment_plan_type_substatus_id])
);


GO
-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/20/2013
-- Description:	WILL INSERT THE be RECORD TABLE WHEN THERE IS AN UPDATE
-- =============================================
CREATE TRIGGER [dbo].[InsertBeRecordHis]
   ON  [dbo].[Repayment_Plan_Type_Status_History]
   AFTER INSERT
AS 
BEGIN

	DECLARE @app_id INT = (SELECT DISTINCT
							RPS.application_id
						FROM
							inserted HIS
						INNER JOIN dbo.Repayment_Plan_Selected RPS
							ON RPS.repayment_plan_type_id = HIS.repayment_plan_type_id)
							
	DECLARE @mapping_id_new INT = (SELECT repayment_plan_type_status_mapping_id FROM inserted)
	

	SET NOCOUNT ON;

		UPDATE dbo.BE_Data_History
		SET substatus_mapping_id = @mapping_id_new
		WHERE application_id = @app_id

END
GO
-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/20/2013
-- Description:	WILL UPDATE THE be RECORD TABLE WHEN THERE IS AN UPDATE
-- =============================================
CREATE TRIGGER [dbo].[UpdateBeRecord]
   ON  [dbo].[Repayment_Plan_Type_Status_History]
   AFTER UPDATE
AS 
BEGIN

	DECLARE @app_id INT = (SELECT
							RPS.application_id
						FROM
							dbo.Repayment_Plan_Type_Status_History HIS
						INNER JOIN dbo.Repayment_Plan_Selected RPS
							ON RPS.repayment_plan_type_id = HIS.repayment_plan_type_id)
							
	DECLARE @mapping_id_new INT = (SELECT repayment_plan_type_status_mapping_id FROM inserted)
	DECLARE @mapping_id_old INT = (SELECT repayment_plan_type_status_mapping_id FROM deleted)
	

	SET NOCOUNT ON;

    IF(@mapping_id_new != @mapping_id_old)
		UPDATE dbo.BE_Data_History
		SET substatus_mapping_id = @mapping_id_new,
			created_at	= GETDATE()	
		WHERE application_id = @app_id

END