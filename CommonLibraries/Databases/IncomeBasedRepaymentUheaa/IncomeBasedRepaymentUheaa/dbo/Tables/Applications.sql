CREATE TABLE [dbo].[Applications] (
    [application_id]                         INT           IDENTITY (1, 1) NOT NULL,
    [e_application_id]                       CHAR (10)     NULL,
    [award_id]                               VARCHAR (50)  NULL,
    [spouse_id]                              INT           NULL,
    [application_source_id]                  INT           NOT NULL,
    [repayment_plan_status_id]               INT           NULL,
    [disclosure_date]                        DATE          NULL,
    [repayment_plan_date_entered]            DATE          NULL,
    [filing_status_id]                       INT           NULL,
    [family_size]                            INT           NULL,
    [tax_year]                               INT           NULL,
    [taxes_filed_flag]                       BIT           DEFAULT ((0)) NULL,
    [loans_at_other_servicers]               BIT           NULL,
    [joint_repayment_plan_request_indicator] BIT           NULL,
    [adjusted_grose_income]                  MONEY         NULL,
    [agi_reflects_current_income]            BIT           NULL,
    [manually_submitted_income_indicator]    BIT           NULL,
    [manually_submitted_income]              MONEY         NULL,
    [supporting_documentation_required]      BIT           NULL,
    [supporting_documentation_received_date] DATETIME      NULL,
    [requested_by_borrower_indicator]        BIT           NULL,
    [borrower_eligibility_id]                INT           NULL,
    [income_source_id]                       INT           NULL,
    [repayment_plan_reason_id]               INT           NULL,
    [repayment_plan_type_requested_id]       INT           NULL,
    [due_date_requested]                     CHAR (2)      NULL,
    [state]                                  CHAR (2)      NULL,
    [received_date]                          DATE          NULL,
    [borrower_selected_lowest_plan]          BIT           DEFAULT ((0)) NULL,
    [updated_at]                             DATETIME      NULL,
    [created_at]                             DATETIME      CONSTRAINT [DF_created_at] DEFAULT (getdate()) NOT NULL,
    [updated_by]                             VARCHAR (20)  NULL,
    [created_by]                             VARCHAR (20)  NOT NULL,
    [Active]                                 BIT           DEFAULT ((1)) NOT NULL,
    [RepaymentTypeProcessedNotSame]          BIT           NULL,
    [number_children]                        INT           NULL,
    [number_dependents]                      INT           NULL,
    [total_income]                           INT           NULL,
    [current_def_forb_id]                    INT           NULL,
    [public_service_employment]              VARCHAR (200) NULL,
    [reduced_payment_forbearance]            MONEY         NULL,
    [marital_status_id]                      INT           NULL,
    [GradeLevel]                             CHAR (1)      NULL,
    [IncludeSpouseInFamilySize] BIT NULL, 
    CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED ([application_id] ASC),
    CONSTRAINT [FK_Applications_Application_Source] FOREIGN KEY ([application_source_id]) REFERENCES [dbo].[Application_Source] ([application_source_id]),
    CONSTRAINT [FK_Applications_Borrower_Eligibility] FOREIGN KEY ([borrower_eligibility_id]) REFERENCES [dbo].[Borrower_Eligibility] ([eligibility_id]),
    CONSTRAINT [FK_Applications_Filing_Statuses] FOREIGN KEY ([filing_status_id]) REFERENCES [dbo].[Filing_Statuses] ([filing_status_id]),
    CONSTRAINT [FK_Applications_Income_Source] FOREIGN KEY ([income_source_id]) REFERENCES [dbo].[Income_Source] ([income_source_id]),
    CONSTRAINT [FK_Applications_Repayment_Plan_Type_Requested] FOREIGN KEY ([repayment_plan_type_requested_id]) REFERENCES [dbo].[Repayment_Plan_Type_Requested] ([repayment_plan_type_requested_id]),
    CONSTRAINT [FK_Applications_Spouses] FOREIGN KEY ([spouse_id]) REFERENCES [dbo].[Spouses] ([spouse_id])
);




GO
-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/20/2013
-- Description:	WILL INSERT DATA INTO THE BD,BE,AND BF TABLES FROM APPLICATIONS TABLE
-- =============================================
CREATE TRIGGER [dbo].[ReportingInfoInsert]
   ON  [dbo].[Applications]
   AFTER INSERT
AS 
BEGIN

	DECLARE @application_id INT = (SELECT application_id FROM inserted)
	DECLARE @e_application_id char(10) = (SELECT e_application_id FROM inserted)
	DECLARE @award_id char(21) = (SELECT award_id FROM inserted)
	DECLARE @application_rec_date date = (SELECT received_date FROM inserted)
	DECLARE @repayment_plan_reason_id INT = (SELECT repayment_plan_reason_id FROM inserted)
	DECLARE @loans_at_other_servicers bit = (SELECT loans_at_other_servicers FROM inserted)
	DECLARE @spouse_id INT = (SELECT spouse_id FROM inserted)
	DECLARE @joint_repayment_request bit = (SELECT joint_repayment_plan_request_indicator FROM inserted)
	DECLARE @family_size INT = (SELECT family_size FROM inserted)
	DECLARE @tax_year INT = (SELECT tax_year FROM inserted)
	DECLARE @filing_status_id INT = (SELECT filing_status_id FROM inserted)
	DECLARE @adjusted_gross_income money = (SELECT adjusted_grose_income FROM inserted)
	DECLARE @agi_reflects_current_income bit = (SELECT agi_reflects_current_income FROM inserted)
	DECLARE @manually_submitted_income_ind bit = (SELECT manually_submitted_income_indicator FROM inserted)
	DECLARE @manully_submitted_income money = (SELECT manually_submitted_income FROM inserted)
	DECLARE @sup_docs_req bit = (SELECT supporting_documentation_required FROM inserted)
	DECLARE @sup_docs_rec_date date = (SELECT supporting_documentation_received_date FROM inserted)
	DECLARE @taxes_filed_flag bit =  (SELECT taxes_filed_flag  FROM inserted)
	DECLARE @borrower_selected_lowest bit = (SELECT borrower_selected_lowest_plan  FROM inserted)
	DECLARE @updated_by varchar(50) = (SELECT updated_by FROM inserted)
	
	DECLARE @repayment_plan_type INT = (SELECT repayment_plan_type_requested_id FROM inserted )
													
	DECLARE @requested_by_borrower BIT = (SELECT requested_by_borrower_indicator FROM inserted)
	
	DECLARE @repayment_plan_type_status INT = (SELECT
													HIS.repayment_plan_type_status_mapping_id
												FROM 
													inserted I
												LEFT JOIN Repayment_Plan_Selected RPS
													ON RPS.application_id = I.application_id
												LEFT JOIN Repayment_Plan_Type_Status_History HIS
													ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id)
												
													
	DECLARE @repayment_plan_type_status_date date = (SELECT [disclosure_date] FROM inserted)
	DECLARE @repayment_plan_date_entered date = (SELECT repayment_plan_date_entered FROM inserted)

	DECLARE @repaymentTypeProcessedNotSame bit = (SELECT RepaymentTypeProcessedNotSame FROM inserted)
	DECLARE @number_children int = (SELECT number_children FROM inserted)
	DECLARE @number_dependents int = (SELECT number_dependents FROM inserted)
	DECLARE @total_income int = (SELECT total_income FROM inserted)
	DECLARE @current_def_forb_id int = (SELECT current_def_forb_id FROM inserted)
	DECLARE @public_service_employment int = (SELECT public_service_employment FROM inserted) 
	DECLARE @reduced_payment_forbearance int = (SELECT reduced_payment_forbearance FROM inserted)
	DECLARE @marital_status_id int = (SELECT marital_status_id FROM inserted)

	SET NOCOUNT ON;
	
	
	INSERT INTO BD_Data_History(application_id,e_application_id,award_id,application_received_date,repayment_plan_reason_id,loans_at_other_servicers,spouse_id,joint_repayment_plan_request,family_size,tax_year,filing_status_id,adjusted_gross_income,agi_reflects_current_income,manually_submitted_income_indicator,manually_submitted_income,supporting_documentation_required,supporting_documentation_received_date,updated_by, borrower_selected_lowest_plan, taxes_filed_flag, RepaymentTypeProcessedNotSame,
	number_children, number_dependents, public_service_employment, reduced_payment_forbearance, marital_status_id, current_def_forb_id)
	VALUES(@application_id,@e_application_id,@award_id,@application_rec_date,@repayment_plan_reason_id,@loans_at_other_servicers,@spouse_id,@joint_repayment_request,@family_size,@tax_year,@filing_status_id,@adjusted_gross_income,@agi_reflects_current_income,@manually_submitted_income_ind,@manully_submitted_income,@sup_docs_req,@sup_docs_rec_date,@updated_by, @borrower_selected_lowest, @taxes_filed_flag, @repaymentTypeProcessedNotSame,
	@number_children, @number_dependents, @public_service_employment, @reduced_payment_forbearance, @marital_status_id, @current_def_forb_id)
	
	INSERT INTO	dbo.BE_Data_History(application_id,award_id,repayment_plan_type,requested_by_borrower,substatus_mapping_id,repayment_application_status,created_by, total_income)
	VALUES(@application_id,@award_id,@repayment_plan_type,@requested_by_borrower,@repayment_plan_type_status, @repayment_plan_type_status_date, @updated_by, @total_income)
	
	INSERT INTO dbo.BF_Data_History(application_id,award_id,disclosure_date,created_by)
	VALUES(@application_id,@award_id,@repayment_plan_date_entered,@updated_by)

END

GO

-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/20/2013
-- Description:	WILL UPDATE AN EXISTING RECORD IN THE BD,BE, AND BF RECORDS WHEN THE APPLICATIONS TABLE IS UPDATED
-- =============================================
CREATE TRIGGER [dbo].[ReportingInfoUpdate]
   ON  [dbo].[Applications]
   AFTER UPDATE
AS 
BEGIN

	DECLARE @application_id_new INT = (SELECT application_id FROM inserted)
	DECLARE @e_application_id_new char(10) = (SELECT e_application_id FROM inserted)
	DECLARE @award_id_new char(21) = (SELECT award_id FROM inserted)
	DECLARE @application_rec_date_new date = (SELECT received_date FROM inserted)
	DECLARE @repayment_plan_reason_id_new INT = (SELECT repayment_plan_reason_id FROM inserted)
	DECLARE @loans_at_other_servicers_new bit = (SELECT loans_at_other_servicers FROM inserted)
	DECLARE @spouse_id_new INT = (SELECT spouse_id FROM inserted)
	DECLARE @joint_repayment_request_new bit = (SELECT joint_repayment_plan_request_indicator FROM inserted)
	DECLARE @family_size_new INT = (SELECT family_size FROM inserted)
	DECLARE @tax_year_new INT = (SELECT tax_year FROM inserted)
	DECLARE @filing_status_id_new INT = (SELECT filing_status_id FROM inserted)
	DECLARE @adjusted_gross_income_new money = (SELECT adjusted_grose_income FROM inserted)
	DECLARE @agi_reflects_current_income_new bit = (SELECT agi_reflects_current_income FROM inserted)
	DECLARE @manually_submitted_income_ind_new bit = (SELECT manually_submitted_income_indicator FROM inserted)
	DECLARE @manully_submitted_income_new money = (SELECT manually_submitted_income FROM inserted)
	DECLARE @sup_docs_req_new bit = (SELECT supporting_documentation_required FROM inserted)
	DECLARE @sup_docs_rec_date_new date = (SELECT supporting_documentation_received_date FROM inserted)
	DECLARE @updated_by varchar(50) = (SELECT updated_by FROM inserted)
	DECLARE @repayment_plan_type_requested_id_new INT = (SELECT repayment_plan_type_requested_id FROM inserted)
	
	DECLARE @e_application_id_old char(10) = (SELECT e_application_id FROM deleted)
	DECLARE @award_id_old char(21) = (SELECT award_id FROM deleted)
	DECLARE @application_rec_date_old date = (SELECT received_date FROM deleted)
	DECLARE @repayment_plan_reason_id_old INT = (SELECT repayment_plan_reason_id FROM deleted)
	DECLARE @loans_at_other_servicers_old bit = (SELECT loans_at_other_servicers FROM deleted)
	DECLARE @spouse_id_old INT = (SELECT spouse_id FROM deleted)
	DECLARE @joint_repayment_request_old bit = (SELECT joint_repayment_plan_request_indicator FROM deleted)
	DECLARE @family_size_old INT = (SELECT family_size FROM deleted)
	DECLARE @tax_year_old INT = (SELECT tax_year FROM deleted)
	DECLARE @filing_status_id_old INT = (SELECT filing_status_id FROM deleted)
	DECLARE @adjusted_gross_income_old money = (SELECT adjusted_grose_income FROM deleted)
	DECLARE @agi_reflects_current_income_old bit = (SELECT agi_reflects_current_income FROM deleted)
	DECLARE @manually_submitted_income_ind_old bit = (SELECT manually_submitted_income_indicator FROM deleted)
	DECLARE @manully_submitted_income_old money = (SELECT manually_submitted_income FROM deleted)
	DECLARE @sup_docs_req_old bit = (SELECT supporting_documentation_required FROM deleted)
	DECLARE @sup_docs_rec_date_old date = (SELECT supporting_documentation_received_date FROM deleted)
	DECLARE @repayment_plan_type_requested_id_old INT = (SELECT repayment_plan_type_requested_id FROM deleted)
													
													
	DECLARE @requested_by_borrower_new BIT = (SELECT requested_by_borrower_indicator FROM inserted)
	DECLARE @requested_by_borrower_old BIT = (SELECT requested_by_borrower_indicator FROM deleted)
													
	DECLARE @repayment_plan_type_status_date_new date = (SELECT [disclosure_date] FROM inserted)
	DECLARE @repayment_plan_type_status_date_old date = (SELECT [disclosure_date] FROM deleted)
	DECLARE @disclosure_date_new date = (SELECT disclosure_date FROM inserted)
	DECLARE @disclosure_date_old date = (SELECT disclosure_date FROM deleted)
	DECLARE @borrower_selected_lowest_new bit = (SELECT borrower_selected_lowest_plan FROM inserted)
	DECLARE @borrower_selected_lowest_old bit = (SELECT borrower_selected_lowest_plan FROM deleted)
	DECLARE @taxes_filed_flag_new bit = (SELECT taxes_filed_flag FROM inserted)
	DECLARE @taxes_filed_flag_old bit = (SELECT taxes_filed_flag FROM deleted)
	
	DECLARE @repayment_plan_type_status_new INT = (SELECT top 1
														HIS.repayment_plan_type_status_mapping_id
													FROM 
														dbo.Applications I
													LEFT JOIN Repayment_Plan_Selected RPS
														ON RPS.application_id = I.application_id
													LEFT JOIN Repayment_Plan_Type_Status_History HIS
														ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
													WHERE I.application_id = @application_id_new
													order by repayment_plan_type_status_history_id desc)
													
	DECLARE @repayment_plan_type_status_old INT = (SELECT substatus_mapping_id FROM dbo.BE_Data_History WHERE application_id = @application_id_new)

	DECLARE @NewrepaymentTypeProcessedNotSame bit = (SELECT RepaymentTypeProcessedNotSame FROM inserted)
	DECLARE @OldrepaymentTypeProcessedNotSame bit = (SELECT RepaymentTypeProcessedNotSame FROM deleted)

	DECLARE @number_children_new int = (SELECT number_children FROM inserted)
	DECLARE @number_dependents_new  int = (SELECT number_dependents FROM inserted)
	DECLARE @total_income_new  int = (SELECT total_income FROM inserted)
	DECLARE @current_def_forb_id_new  int = (SELECT current_def_forb_id FROM inserted)
	DECLARE @public_service_employment_new  int = (SELECT public_service_employment FROM inserted) 
	DECLARE @reduced_payment_forbearance_new  int = (SELECT reduced_payment_forbearance FROM inserted)
	DECLARE @marital_status_id_new  int = (SELECT marital_status_id FROM inserted)

	DECLARE @number_children_old int = (SELECT number_children FROM deleted)
	DECLARE @number_dependents_old  int = (SELECT number_dependents FROM deleted)
	DECLARE @total_income_old  int = (SELECT total_income FROM deleted)
	DECLARE @current_def_forb_id_old  int = (SELECT current_def_forb_id FROM deleted)
	DECLARE @public_service_employment_old  int = (SELECT public_service_employment FROM deleted) 
	DECLARE @reduced_payment_forbearance_old  int = (SELECT reduced_payment_forbearance FROM deleted)
	DECLARE @marital_status_id_old  int = (SELECT marital_status_id FROM deleted)

	
	
	DECLARE @bd_needs_update bit = 0
	DECLARE @be_needs_update bit = 0
	DECLARE @bf_needs_update bit = 0

	IF(@number_children_new != @number_children_old)
		SET @bd_needs_update = 1
	IF(@number_dependents_new != @number_dependents_old)
		SET @bd_needs_update = 1
	IF(@total_income_new != @total_income_old)
		SET @be_needs_update = 1
	IF(@current_def_forb_id_new != @current_def_forb_id_old)
		SET @bd_needs_update = 1
	IF(@public_service_employment_new != @public_service_employment_old)
		SET @bd_needs_update = 1
	IF(@reduced_payment_forbearance_new != @reduced_payment_forbearance_old)
		SET @bd_needs_update = 1
	IF(@marital_status_id_new != @marital_status_id_old)
		SET @bd_needs_update = 1
	
	SET NOCOUNT ON;

    IF(@e_application_id_new != @e_application_id_old)
		SET @bd_needs_update = 1
	ELSE IF (@NewrepaymentTypeProcessedNotSame != @OldrepaymentTypeProcessedNotSame)
		SET @bd_needs_update = 1
	ELSE IF (@taxes_filed_flag_new != @taxes_filed_flag_old)
		SET @bd_needs_update = 1
	ELSE IF (@borrower_selected_lowest_new != @borrower_selected_lowest_old)
		SET @bd_needs_update = 1
	ELSE IF (@award_id_new != @award_id_old)
		SET @bd_needs_update = 1
	ELSE IF (@application_rec_date_new != @application_rec_date_old)
		SET @bd_needs_update = 1
	ELSE IF (@repayment_plan_reason_id_new != @repayment_plan_reason_id_old)
		SET @bd_needs_update = 1
	ELSE IF (@loans_at_other_servicers_new != @loans_at_other_servicers_old)
		SET @bd_needs_update = 1
	ELSE IF (@spouse_id_new != @spouse_id_old)
		SET @bd_needs_update = 1
	ELSE IF (@joint_repayment_request_new != @joint_repayment_request_old)
		SET @bd_needs_update = 1
	ELSE IF (@family_size_new != @family_size_old)
		SET @bd_needs_update = 1
	ELSE IF (@tax_year_new != @tax_year_old)
		SET @bd_needs_update = 1
	ELSE IF (@filing_status_id_new != @filing_status_id_old)
		SET @bd_needs_update = 1
	ELSE IF (@adjusted_gross_income_new != @adjusted_gross_income_old)
		SET @bd_needs_update = 1
	ELSE IF (@agi_reflects_current_income_new != @agi_reflects_current_income_old)
		SET @bd_needs_update = 1
	ELSE IF (@manually_submitted_income_ind_new != @manually_submitted_income_ind_old)
		SET @bd_needs_update = 1
	ELSE IF (@manully_submitted_income_new != @manully_submitted_income_old)
		SET @bd_needs_update = 1
	ELSE IF (@sup_docs_req_new != @sup_docs_req_old)
		SET @bd_needs_update = 1	
	ELSE IF (@sup_docs_rec_date_new != @sup_docs_rec_date_old)
		SET @bd_needs_update = 1
	ELSE IF (@repayment_plan_type_requested_id_new != @repayment_plan_type_requested_id_old)
		SET @be_needs_update = 1
	ELSE IF (@joint_repayment_request_new IS NULL AND @joint_repayment_request_old IS NOT NULL)	
		SET @bd_needs_update = 1
		
	IF (@joint_repayment_request_old IS NULL AND @joint_repayment_request_new IS NOT NULL)
		SET @bd_needs_update = 1
		
		
	IF(@e_application_id_new IS NOT NULL AND @e_application_id_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@award_id_new IS NOT NULL AND @award_id_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@application_rec_date_new IS NOT NULL AND @application_rec_date_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@repayment_plan_reason_id_new IS NOT NULL AND @repayment_plan_reason_id_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@loans_at_other_servicers_new IS NOT NULL AND @loans_at_other_servicers_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@spouse_id_new IS NOT NULL AND @spouse_id_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@joint_repayment_request_new IS NOT NULL AND @joint_repayment_request_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@family_size_new IS NOT NULL AND @family_size_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@tax_year_new IS NOT NULL AND @tax_year_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@filing_status_id_new IS NOT NULL AND @filing_status_id_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@adjusted_gross_income_new IS NOT NULL AND @adjusted_gross_income_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@agi_reflects_current_income_new IS NOT NULL AND @agi_reflects_current_income_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@manually_submitted_income_ind_new IS NOT NULL AND @manually_submitted_income_ind_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@manully_submitted_income_new IS NOT NULL AND @manully_submitted_income_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@sup_docs_req_new IS NOT NULL AND @sup_docs_req_old IS NULL)
		SET @bd_needs_update = 1	
	ELSE IF (@sup_docs_rec_date_new IS NOT NULL AND @sup_docs_rec_date_old IS NULL)
		SET @bd_needs_update = 1
	ELSE IF (@repayment_plan_type_requested_id_new IS NOT NULL AND @repayment_plan_type_requested_id_old IS NULL)
		SET @be_needs_update = 1
		
	IF(@spouse_id_new IS NULL AND @spouse_id_old IS NOT NULL)
		SET @bd_needs_update = 1
		
		
	IF(@bd_needs_update = 1)
		UPDATE
			 dbo.BD_Data_History
		SET
			e_application_id = @e_application_id_new,
			award_id = @award_id_new,
			application_received_date = @application_rec_date_new,
			repayment_plan_reason_id = @repayment_plan_reason_id_new,
			loans_at_other_servicers = @loans_at_other_servicers_new,
			spouse_id = @spouse_id_new,
			joint_repayment_plan_request = @joint_repayment_request_new,
			family_size = @family_size_new,
			tax_year = @tax_year_new,
			filing_status_id = @filing_status_id_new,
			adjusted_gross_income = @adjusted_gross_income_new,
			agi_reflects_current_income =@agi_reflects_current_income_new,
			manually_submitted_income_indicator = @manually_submitted_income_ind_new,
			manually_submitted_income = @manully_submitted_income_new,
			supporting_documentation_required = @sup_docs_req_new,
			supporting_documentation_received_date = @sup_docs_rec_date_new,
			updated_by = @updated_by,
			created_at	= GETDATE(),
			number_children = @number_children_new,
			number_dependents = @number_dependents_new,
			current_def_forb_id = @current_def_forb_id_new,
			public_service_employment = @public_service_employment_new,
			reduced_payment_forbearance = @reduced_payment_forbearance_new,
			marital_status_id = @marital_status_id_new
		WHERE application_id = @application_id_new
			
	IF (@award_id_new != @award_id_old)
		SET @be_needs_update = 1
	ELSE IF (@requested_by_borrower_new != @requested_by_borrower_old)
		SET @be_needs_update = 1
	ELSE IF (@repayment_plan_type_status_date_new != @repayment_plan_type_status_date_old)
		SET @be_needs_update = 1
	ELSE IF (@repayment_plan_type_requested_id_new != @repayment_plan_type_requested_id_old)
		SET @be_needs_update = 1
		
	IF(@repayment_plan_type_status_new != @repayment_plan_type_status_old)
		SET @be_needs_update = 1
		
	IF(@be_needs_update = 1)
		UPDATE
			dbo.BE_Data_History
		SET 
			award_id = @award_id_new,
			repayment_plan_type = @repayment_plan_type_requested_id_new,
			requested_by_borrower = @requested_by_borrower_new,
			repayment_application_status = @repayment_plan_type_status_date_new,
			substatus_mapping_id = @repayment_plan_type_status_new,
			created_by = @updated_by,
			created_at	= GETDATE()	,
			total_income = @total_income_new
		WHERE application_id = @application_id_new
			
	IF (@award_id_new != @award_id_old)
		SET @bf_needs_update = 1
	ELSE IF (@disclosure_date_new != @disclosure_date_old)
		SET @bf_needs_update = 1
	ELSE IF (@disclosure_date_new IS NULL AND @disclosure_date_old is not null)
		SET @bf_needs_update = 1
		
	IF (@disclosure_date_old IS NULL AND @disclosure_date_new IS NOT NULL)
		SET @bf_needs_update = 1
		
	IF(@bf_needs_update = 1)
		UPDATE 
			dbo.BF_Data_History
		SET
			award_id = @award_id_new,
			disclosure_date = @disclosure_date_new,
			created_at	= GETDATE(),	
			created_by = @updated_by
		WHERE application_id = @application_id_new
		
	

END
