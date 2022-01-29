CREATE PROCEDURE [i1i2schltr].[GetSchoolDemographics]
	@SchoolId VARCHAR(8),
	@Borrower VARCHAR(9)
AS
	SELECT
		SC01.IF_IST AS School,
		SC01.IM_IST_FUL AS SchoolName,
		SC01.IX_GEN_STR_ADR_1 AS SchoolAddress,
		SC01.IX_GEN_STR_ADR_2 AS SchoolAddress2,
		SC01.IX_GEN_STR_ADR_3 AS SchoolAddress3,
		SC01.IM_GEN_CT AS SchoolCity,
		SC01.IC_GEN_ST AS SchoolState,
		SC01.IF_GEN_ZIP AS SchoolZip,
		CostCenter.Lender AS Lender
	FROM
		ODW..SC01_LGS_SCL_INF SC01
		--We want to use the 112 department code if one is available and the blank one otherwise
		INNER JOIN
		(
			SELECT
				SC01.IF_IST,
				SC01.IC_IST_DPT,
				ROW_NUMBER() OVER (PARTITION BY SC01.IF_IST ORDER BY CASE WHEN SC01.IC_IST_DPT = '112' THEN 0 ELSE 1 END) AS ROWNUM
			FROM
				ODW..SC01_LGS_SCL_INF SC01
			WHERE
				SC01.IF_IST = @SchoolId
				AND RTRIM(SC01.IC_IST_DPT) IN ('112','')
		) SC01_PRIO
			ON SC01_PRIO.IF_IST = SC01.IF_IST
			AND SC01_PRIO.IC_IST_DPT = SC01.IC_IST_DPT
			AND ROWNUM = 1
		LEFT JOIN
		(
			SELECT
				GA01.AF_CUR_APL_OPS_LDR AS Lender
			FROM
				ODW..GA01_APP GA01
				INNER JOIN ODW..GA10_LON_APP GA10
					ON GA10.AF_APL_ID = GA01.AF_APL_ID
				INNER JOIN ODW..GA14_LON_STA GA14
					ON GA14.AF_APL_ID = GA01.AF_APL_ID
			WHERE
				GA01.DF_PRS_ID_BR = @Borrower
				AND GA14.AC_LON_STA_TYP NOT IN ('CP','CR') --Invalid loan statuses
				AND ISNULL(AA_CUR_PRI,0) > 0
		) CostCenter
		 ON 1 = 1 --This should be joined to only one record so this just gives a lender level school demographics
	WHERE
		SC01.IF_IST = @SchoolId
RETURN 0
