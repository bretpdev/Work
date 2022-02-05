CREATE PROCEDURE [lnderlettr].[GetAvailableLetterData]
	@TestMode bit
AS
DECLARE @Query VARCHAR(Max)
IF @TestMode = 0
	SET @Query = 'INSERT INTO [lnderlettr].Letters(BF_SSN, II_LDR_VLD_ADR, InLenderList, WF_ORG_LDR) SELECT D.* FROM OPENQUERY(DUSTER,'
ELSE 
	SET @Query = 'INSERT INTO [lnderlettr].Letters(BF_SSN, II_LDR_VLD_ADR, InLenderList, WF_ORG_LDR) SELECT D.* FROM OPENQUERY(QADBD004,'

SELECT @Query = @Query + 
		'''
			SELECT DISTINCT
				AY10.BF_SSN,
				LR25.II_LDR_VLD_ADR,
				CASE
					WHEN GR10.WF_ORG_LDR IN (''''814817'''',''''818334'''',''''824421'''',''''826079'''',''''831495'''',''''832733'''',''''801871'''',''''802176'''',''''805317'''',''''806746'''',''''807674'''',''''811735'''',''''814817'''') THEN ''''1''''
					ELSE ''''0''''
				END AS InLenderList,
				GR10.WF_ORG_LDR
			FROM
				OLWHRM1.AY10_BR_LON_ATY AY10
				INNER JOIN OLWHRM1.GR10_RPT_LON_APL GR10
					ON GR10.BF_SSN = AY10.BF_SSN
				INNER JOIN OLWHRM1.LR25_LDR_DPT LR25
					ON LR25.IF_DOE_LDR = GR10.WF_ORG_LDR
					AND LR25.IC_LDR_DPT = ''''000''''
			WHERE
				AY10.PF_REQ_ACT IN (''''S3OLR'''',''''S6OLR'''')
				AND AY10.LC_STA_ACTY10 = ''''A''''
				AND AY10.PF_RSP_ACT = ''''''''
				AND LR25.II_LDR_VLD_ADR IS NOT NULL
				AND LR25.II_LDR_VLD_ADR <> ''''''''
				AND (CASE
						WHEN LR25.II_LDR_VLD_ADR = ''''N'''' AND LR25.IX_LDR_STR_ADR_1 LIKE ''''X-CLOSED-%'''' THEN LR25.II_LDR_VLD_ADR
						WHEN LR25.II_LDR_VLD_ADR = ''''Y'''' THEN LR25.II_LDR_VLD_ADR
					END) IS NOT NULL
			GROUP BY
				AY10.BF_SSN,
				LR25.II_LDR_VLD_ADR,
				GR10.WF_ORG_LDR
			'') D
		LEFT JOIN [lnderlettr].[Letters] L
			ON D.BF_SSN = L.BF_SSN
			AND CAST(L.AddedAt AS DATE) = ''' + CONVERT(VARCHAR(10), GETDATE(), 101) + '''
			AND L.DeletedAt IS NULL
		WHERE
			L.BF_SSN IS NULL'
EXEC (@Query)
GO
GRANT EXECUTE
    ON OBJECT::[lnderlettr].[GetAvailableLetterData] TO [db_executor]
    AS [dbo];

