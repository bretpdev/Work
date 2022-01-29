--QUE,Date,Existing Inventory,Newly Received,Processed,Remaining Inventory,Business Days In Process,Compliance Deadline,Comments


IF OBJECT_ID('tempdb..##UheaaQueueData') IS NOT NULL
    DROP TABLE tempdb..##UheaaQueueData

DECLARE @UheaaQueues TABLE
(
	UheaaQueueId INT IDENTITY(1,1),
	BusinessUnit VARCHAR(30) NOT NULL,
	Que CHAR(2) NOT NULL,
	ComplianceDays INT DEFAULT(30)
)

INSERT INTO @UheaaQueues
	(
		BusinessUnit,
		Que
	)
VALUES
	('Asset Management','4K'),
	('Asset Management','4X'),
	('Asset Management','AD'),
	('Asset Management','AQ'),
	('Asset Management','BC'),
	('Asset Management','DR'),
	('Asset Management','ED'),
	('Asset Management','EE'),
	('Asset Management','F0'),
	('Asset Management','F2'),
	('Asset Management','FA'),
	('Asset Management','GF'),
	('Asset Management','GG'),
	('Asset Management','H0'),
	('Asset Management','H1'),
	('Asset Management','HB'),
	('Asset Management','HF'),
	('Asset Management','HV'),
	('Asset Management','NA'),
	('Asset Management','NB'),
	('Asset Management','O6'),
	('Asset Management','OF'),
	('Asset Management','OK'),
	('Asset Management','OU'),
	('Asset Management','OV'),
	('Asset Management','OW'),
	('Asset Management','OZ'),
	('Asset Management','P0'),
	('Asset Management','RB'),
	('Asset Management','RZ'),
	('Asset Management','T0'),
	('Asset Management','TW'),
	('Asset Management','TX'),
	('Asset Management','U0'),
	('Asset Management','U1'),
	('Asset Management','U2'),
	('Asset Management','U4'),
	('Asset Management','US'),
	('Asset Management','V5'),
	('Asset Management','YZ'),
	('Asset Management','Z9'),
	('Asset Management','ZA'),
	('Asset Management','3A'),
	('Asset Management','20'),
	('Asset Management','21'),
	('Asset Management','22'),
	('Asset Management','23'),
	('Asset Management','24'),
	('Asset Management','25'),
	('Asset Management','26'),
	('Asset Management','27'),
	('Asset Management','28'),
	('Asset Management','29'),
	('Asset Management','30'),
	('Asset Management','31'),
	('Asset Management','32'),
	('Asset Management','34'),
	('Asset Management','3B'),
	('Asset Management','3I'),
	('Asset Management','3K'),
	('Asset Management','3L'),
	('Asset Management','3W'),
	('Asset Management','3X'),
	('Asset Management','3Z'),
	('Asset Management','44'),
	('Asset Management','4P'),
	('Asset Management','OB'),
	('Asset Management','P1'),
	('Asset Management','T1'),
	('Asset Management','T2'),
	('Asset Management','XB'),
	('Asset Management','4D'),
	('Asset Management','ER'),
	('Asset Management','F3'),
	('Asset Management','F8'),
	('Asset Management','GT'),
	('Asset Management','H7'),
	('Asset Management','LR'),
	('Asset Management','NC'),
	('Asset Management','NE'),
	('Asset Management','NF'),
	('Asset Management','NG'),
	('Asset Management','NH'),
	('Asset Management','O9'),
	('Asset Management','OC'),
	('Asset Management','OH'),
	('Asset Management','OL'),
	('Asset Management','R4'),
	('Asset Management','SV'),
	('Asset Management','ZP'),
	('Asset Management','55'),
	('Asset Management','BK'),
	('Asset Management','CQ'),
	('Asset Management','TB'),
	('Asset Management','I9'),
	('Asset Management','3B'),
	('Skip','JN'),
	('Skip','K8'),
	('Skip','IE'),
	('Skip','1R'),
	('Skip','7A'),
	('Skip','B2'),
	('Skip','BI'),
	('Skip','CO'),
	('Skip','CU'),
	('Skip','DV'),
	('Skip','E1'),
	('Skip','FJ'),
	('Skip','FY'),
	('Skip','GV'),
	('Skip','I1'),
	('Skip','I2'),
	('Skip','IA'),
	('Skip','IB'),
	('Skip','IC'),
	('Skip','IG'),
	('Skip','IH'),
	('Skip','II'),
	('Skip','IP'),
	('Skip','IR'),
	('Skip','IT'),
	('Skip','JL'),
	('Skip','JR'),
	('Skip','LX'),
	('Skip','PK'),
	('Skip','PU'),
	('Skip','TC'),
	('Skip','V1'),
	('Skip','V9'),
	('Skip','IJ'),
	('Skip','IK'),
	('Skip','D1'),
	('Skip','IS'),
	('Skip','IM'),
	('Skip','IQ'),
	('Skip','IO'),
	('Skip','B1'),
	('Skip','PC'),
	('Skip','KE'),
	('Skip','SR'),
	('Loan Servicing','1P'),
	('Loan Servicing','2A'),
	('Loan Servicing','5C'),
	('Loan Servicing','99'),
	('Loan Servicing','AP'),
	('Loan Servicing','AT'),
	('Loan Servicing','AW'),
	('Loan Servicing','CN'),
	('Loan Servicing','PM'),
	('Loan Servicing','PR'),
	('Loan Servicing','R0'),
	('Loan Servicing','RO'),
	('Loan Servicing','R1'),
	('Loan Servicing','R8'),
	('Loan Servicing','RX'),
	('Loan Servicing','SA'),
	('Loan Servicing','SF'),
	('Loan Servicing','SI'),
	('Loan Servicing','TU'),
	('Loan Servicing','V8'),
	('Loan Servicing','VB'),
	('Loan Servicing','VC'),
	('Loan Servicing','VR'),
	('Loan Servicing','X9'),
	('Loan Servicing','3A'),
	('Loan Servicing','1M'),
	('Loan Servicing','2M'),
	('Loan Servicing','2P'),
	('Loan Servicing','36'),
	('Loan Servicing','37'),
	('Loan Servicing','BR'),
	('Loan Servicing','GP'),
	('Loan Servicing','SO'),
	('Loan Servicing','60'),
	('Loan Servicing','CP'),
	('Loan Servicing','CV'),
	('Loan Servicing','DU'),
	('Loan Servicing','G8'),
	('Loan Servicing','MF'),
	('Loan Servicing','MS'),
	('Loan Servicing','R7'),
	('Loan Servicing','RR'),
	('Loan Servicing','YB'),
	('Loan Servicing','AH'),
	('Loan Servicing','BD'),
	('Loan Servicing','NO'),
	('Loan Servicing','S4'),
	('Loan Servicing','IR') --UheaaQueues

SELECT
	*
INTO 
	tempdb..##UheaaQueueData
FROM
	@UheaaQueues UQ
	LEFT OUTER JOIN OPENQUERY
	(DUSTER,
	'
		SELECT
			WQ20.WF_QUE AS QUE,
			DATE(AY10.LD_ATY_REQ_RCV) AS ReceivedDate,
			DATE(AY10.LD_ATY_RSP) AS ProcessedDate,
			SUM(CASE WHEN DATE(AY10.LD_ATY_REQ_RCV) < DATE(CURRENT DATE) THEN 1 ELSE 0 END) AS ExistingInventory,
			SUM(CASE WHEN DATE(AY10.LD_ATY_REQ_RCV) >= DATE(CURRENT DATE) THEN 1 ELSE 0 END) AS NewlyReceived,
			SUM(CASE WHEN WQ20.WF_QUE IS NULL OR WQ20.WC_STA_WQUE20 IN (''X'',''C'') THEN 1 ELSE 0 END) AS Processed,
			SUM(CASE WHEN WQ20.WC_STA_WQUE20 NOT IN (''X'',''C'') THEN 1 ELSE 0 END) AS RemainingInventory
		FROM
			OLWHRM1.AY10_BR_LON_ATY AY10
			LEFT OUTER JOIN OLWHRM1.WQ20_TSK_QUE WQ20
				ON WQ20.BF_SSN = AY10.BF_SSN
				AND WQ20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				AND WQ20.PF_REQ_ACT = AY10.PF_REQ_ACT
		GROUP BY
			WQ20.WF_QUE,
			DATE(AY10.LD_ATY_REQ_RCV),
			DATE(AY10.LD_ATY_RSP)
	'
	) AS UQH ON UQH.QUE = UQ.Que

SELECT
	UQ.BusinessUnit,
	UQD.Que,
	UQD.ExistingInventory,
	UQD.NewlyReceived,
	UQD.Processed,
	UQD.RemainingInventory,
	CentralData.dbo.BusinessDaysDiff(MIN(UQD.ReceivedDate), GETDATE()) [BusinessDaysInProcess],
	CAST(UQ.ComplianceDays AS VARCHAR(3)) + ' Days' [ComplianceDeadline],
	'' [Comments]
FROM
	tempdb..##UheaaQueueData UQD
	INNER JOIN @UheaaQueues UQ ON UQ.[Que] = UQD.QUE
GROUP BY
	UQ.BusinessUnit,
	UQD.Que,
	UQD.ExistingInventory,
	UQD.NewlyReceived,
	UQD.Processed,
	UQD.RemainingInventory,
	UQ.ComplianceDays
ORDER BY
	BusinessUnit,
	Que




	select * from openquery(DUSTER,
	'
		SELECT
			WQ20.WF_QUE AS QUE,
			DATE(AY10.LD_ATY_REQ_RCV) AS ReceivedDate,
			DATE(AY10.LD_ATY_RSP) AS ProcessedDate,
			SUM(CASE WHEN DATE(AY10.LD_ATY_REQ_RCV) < DATE(CURRENT DATE) AND AY10.LD_ATY_RSP IS NULL THEN 1 ELSE 0 END) AS ExistingInventory,
			SUM(CASE WHEN DATE(AY10.LD_ATY_REQ_RCV) >= DATE(CURRENT DATE) THEN 1 ELSE 0 END) AS NewlyReceived,
			SUM(CASE WHEN WQ20.WF_QUE IS NULL THEN 1 
					 WHEN WQ20.WC_STA_WQUE20 IN (''X'',''C'') THEN 1
					 ELSE 0 
			END) AS Processed,
			SUM(CASE WHEN WQ20.WC_STA_WQUE20 NOT IN (''X'',''C'') THEN 1 ELSE 0 END) AS RemainingInventory
		FROM
			OLWHRM1.AY10_BR_LON_ATY AY10
			LEFT OUTER JOIN OLWHRM1.WQ20_TSK_QUE WQ20
				ON WQ20.BF_SSN = AY10.BF_SSN
				AND WQ20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				AND WQ20.PF_REQ_ACT = AY10.PF_REQ_ACT
		WHERE
			DAYS(CURRENT DATE) BETWEEN DAYS(AY10.LD_ATY_REQ_RCV) - 7 AND DAYS(AY10.LD_ATY_REQ_RCV)
			OR DAYS(CURRENT DATE) BETWEEN DAYS(AY10.LD_ATY_RSP) - 7 AND DAYS(AY10.LD_ATY_RSP)
		GROUP BY
			WQ20.WF_QUE,
			DATE(AY10.LD_ATY_REQ_RCV),
			DATE(AY10.LD_ATY_RSP)
	'
	) 