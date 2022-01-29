﻿CREATE PROCEDURE [dbo].[spMD_GetEmployerData]
	@AccountNumber				VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	--get all Employer information
	SELECT 
		IN01.IM_IST_FUL AS Name,
		IN01.IX_GEN_STR_ADR_1 AS Addr1,
		IN01.IX_GEN_STR_ADR_2 AS Addr2,
		IN01.IM_GEN_CT AS City,
		IN01.IC_GEN_ST AS [State],
		IN01.IF_GEN_ZIP AS Zip,
		IN01.IN_GEN_PHN AS Phone
	FROM 
		ODW..BR02_BR_EMP BR02
		INNER JOIN ODW..PD01_PDM_INF PD01
			ON BR02.BF_SSN = PD01.DF_PRS_ID
		INNER JOIN ODW..IN01_LGS_IDM_MST IN01
			ON BR02.BF_EMP_ID_1 = IN01.IF_IST
	WHERE
		PD01.DF_SPE_ACC_ID = @AccountNumber
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetEmployerData] TO [UHEAA\Imaging Users]
    AS [dbo];

