USE UDW;
GO

DECLARE @TASKS TABLE (TAB TINYINT,TASK VARCHAR(5));
INSERT INTO @TASKS (TAB,TASK) VALUES
	(1,'TU-01'),
	(2,'RE-01'),
	(3,'R8-01'),
	(4,'R0-01'),
	(5,'HB-AB'),
	(6,'H1-R1'),
	(7,'GT-CT'),
	(8,'F2-A2'),
	(9,'EE-01'),
	(10,'AQ-A1'),
	(11,'55-01'),
	(12,'31-01');
--select * from @tasks

SELECT DISTINCT
	T.*,
	ALLPOP.WN_CTL_TSK,
	ALLPOP.SSN,
	ALLPOP.DateCreated,
	ALLPOP.DateCompleted
INTO
	#POP
FROM
	@TASKS T
	LEFT JOIN
	(--get open tasks
		SELECT DISTINCT
			BF_SSN AS SSN,
			WN_CTL_TSK,
			CONCAT(WF_QUE,'-',WF_SUB_QUE) AS Task,
			CAST(MIN(WF_CRT_DTS_WQ20) AS DATE) AS DateCreated,
			NULL AS DateCompleted
		FROM
			WQ20_TSK_QUE
		WHERE
			WC_STA_WQUE20 NOT IN ('C','X')
			AND CONVERT(DATE,WF_CRT_DTS_WQ20) >= CONVERT(DATE,'20200701')
			AND CONCAT(WF_QUE,'-',WF_SUB_QUE) IN
				(
					'TU-01',
					'RE-01',
					'R8-01',
					'R0-01',
					'HB-AB',
					'H1-R1',
					'GT-CT',
					'F2-A2',
					'EE-01',
					'AQ-A1',
					'55-01',
					'31-01'
				)
		GROUP BY
			BF_SSN,
			WN_CTL_TSK,
			WF_QUE,
			WF_SUB_QUE

		UNION

		--get completed tasks
		SELECT DISTINCT
			BF_SSN AS SSN,
			WN_CTL_TSK,
			CONCAT(WF_QUE,'-',WF_SUB_QUE) AS Task,
			CAST(MIN(WF_CRT_DTS_WQ20) AS DATE) AS DateCreated,
			CAST(MAX(WF_CRT_DTS_WQ21) AS DATE) AS DateCompleted
		FROM
			WQ21_TSK_QUE_HST
		WHERE
			WC_STA_WQUE20 IN ('C','X')
			AND CONVERT(DATE,WF_CRT_DTS_WQ20) >= CONVERT(DATE,'20200701')
			AND CONCAT(WF_QUE,'-',WF_SUB_QUE) IN
				(
					'TU-01',
					'RE-01',
					'R8-01',
					'R0-01',
					'HB-AB',
					'H1-R1',
					'GT-CT',
					'F2-A2',
					'EE-01',
					'AQ-A1',
					'55-01',
					'31-01'
				)
		GROUP BY
			BF_SSN,
			WN_CTL_TSK,
			WF_QUE,
			WF_SUB_QUE
	) ALLPOP
		ON ALLPOP.Task = T.TASK
;

SELECT
	*
FROM
	#POP
WHERE
	TAB=12 --cycle through the 12 tabs for output copy/paste into excel





--non-completed tasks currently outstanding
SELECT
	*
FROM
	(
		SELECT DISTINCT
			BF_SSN AS SSN,
			WN_CTL_TSK,
			CONCAT(WF_QUE,'-',WF_SUB_QUE) AS Task,
			WF_CRT_DTS_WQ20,
			WC_STA_WQUE20,
			CASE CONCAT(WF_QUE,'-',WF_SUB_QUE)
				WHEN 'TU-01' THEN 1
				WHEN 'RE-01' THEN 2
				WHEN 'R8-01' THEN 3
				WHEN 'R0-01' THEN 4
				WHEN 'HB-AB' THEN 5
				WHEN 'H1-R1' THEN 6
				WHEN 'GT-CT' THEN 7
				WHEN 'F2-A2' THEN 8
				WHEN 'EE-01' THEN 9
				WHEN 'AQ-A1' THEN 10
				WHEN '55-01' THEN 11
				WHEN '31-01' THEN 12
				ELSE NULL
			END AS TAB
		FROM
			WQ20_TSK_QUE
		WHERE
			WC_STA_WQUE20 NOT IN ('C','X')
			--AND CONVERT(DATE,WF_CRT_DTS_WQ20) <= CONVERT(DATE,'20200701')
			AND CONCAT(WF_QUE,'-',WF_SUB_QUE) IN
				(
					'TU-01',
					'RE-01',
					'R8-01',
					'R0-01',
					'HB-AB',
					'H1-R1',
					'GT-CT',
					'F2-A2',
					'EE-01',
					'AQ-A1',
					'55-01',
					'31-01'
				)
	) A
WHERE
	TAB = 12  --cycle through the 12 tabs for output copy/paste into excel
ORDER BY
	SSN
;
