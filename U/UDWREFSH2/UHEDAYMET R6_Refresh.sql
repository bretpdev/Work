--UHEDAYMET R6 Summary.rdl data set
BEGIN TRY
	BEGIN TRANSACTION

		INSERT INTO UDW.uhedaymet.R6
			(
				 ReportDate
				,Cat01Count
				,Cat02Count
				,Cat03Count
				,Cat04Count
				,Cat05Count
				,Cat06Count
				,Cat07Count
				,Cat08Count
				,Cat09Count
				,Cat10Count
				,Cat11Count
				,Cat12Count
				--,TotalCount --COMPUTED TABLE COLUMN
				,CreatedAt
				,CreatedBy
				--,DeletedAt --NULL by default
				--,DeletedBy --NULL by default
			)
		SELECT
			CategoryCount.CreatedAt AS ReportDate,
			MAX(CASE WHEN CategoryCount.PerformanceCategory = '01' THEN CategoryCount.BorrowerCount ELSE NULL END) AS Cat01Count, --'in In-School Status'
			MAX(CASE WHEN CategoryCount.PerformanceCategory = '02' THEN CategoryCount.BorrowerCount ELSE NULL END) AS Cat02Count, --'in Grace Status'
			MAX(CASE WHEN CategoryCount.PerformanceCategory = '03' THEN CategoryCount.BorrowerCount ELSE NULL END) AS Cat03Count, --'in Repayment'
			MAX(CASE WHEN CategoryCount.PerformanceCategory = '04' THEN CategoryCount.BorrowerCount ELSE NULL END) AS Cat04Count, --'as Service Members'
			MAX(CASE WHEN CategoryCount.PerformanceCategory = '05' THEN CategoryCount.BorrowerCount ELSE NULL END) AS Cat05Count, --'in Deferment'
			MAX(CASE WHEN CategoryCount.PerformanceCategory = '06' THEN CategoryCount.BorrowerCount ELSE NULL END) AS Cat06Count, --'in Forbearance'
			MAX(CASE WHEN CategoryCount.PerformanceCategory = '07' THEN CategoryCount.BorrowerCount ELSE NULL END) AS Cat07Count, --'6-30 Days Delinquent'
			MAX(CASE WHEN CategoryCount.PerformanceCategory = '08' THEN CategoryCount.BorrowerCount ELSE NULL END) AS Cat08Count, --'31-90 Days Delinquent'
			MAX(CASE WHEN CategoryCount.PerformanceCategory = '09' THEN CategoryCount.BorrowerCount ELSE NULL END) AS Cat09Count, --'91-150 Days Delinquent'
			MAX(CASE WHEN CategoryCount.PerformanceCategory = '10' THEN CategoryCount.BorrowerCount ELSE NULL END) AS Cat10Count, --'151-270 Days Delinquent'
			MAX(CASE WHEN CategoryCount.PerformanceCategory = '11' THEN CategoryCount.BorrowerCount ELSE NULL END) AS Cat11Count, --'271-360 Days Delinquent'
			MAX(CASE WHEN CategoryCount.PerformanceCategory = '12' THEN CategoryCount.BorrowerCount ELSE NULL END) AS Cat12Count, --'361+ Days Delinquent'
			CategoryCount.CreatedAt,
			SYSTEM_USER AS AddedBy
		FROM
			(--get counts of borrowers per category			
					SELECT
						BL.PerformanceCategory,
						COUNT(DISTINCT BL.BF_SSN) AS BorrowerCount,
						GETDATE() AS CreatedAt
					FROM
						UDW.uhedaymet.Daily_BorrowerLevel BL
					WHERE
						BL.PerformanceCategory NOT IN ('PIF','TRN','PIFPRV','TRNPRV','99')
					GROUP BY
						BL.PerformanceCategory
			) CategoryCount
			LEFT JOIN UDW.uhedaymet.R6 R6
				ON CONVERT(DATE,CategoryCount.CreatedAt) = CONVERT(DATE,R6.CreatedAt) --only want rows of any kind once per day
				AND R6.DeletedAt IS NULL
		WHERE
			R6.CreatedAt IS NULL
		GROUP BY
			CategoryCount.CreatedAt;

	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	PRINT 'UHEDAYMET R6_Refresh.sql transaction NOT committed.';
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;