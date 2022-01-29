USE CDW
GO


--SELECT
--	WF_QUE,
--	WF_SUB_QUE,
--	COUNT(*) AS TASK_COUNT
--FROM
--(
--SELECT DISTINCT
--	WF_QUE,
--	WF_SUB_QUE,
--	WN_CTL_TSK,
--	PF_REQ_ACT,
--	WF_CRT_DTS_WQXX
--FROM
--	CDW..WQXX_TSK_QUE_HST
--WHERE
--	WF_CRT_DTS_WQXX BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
--	AND WF_QUE = 'RB'
--	AND WF_SUB_QUE IN ('XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX')
--) P
--GROUP BY
--	WF_QUE,
--	WF_SUB_QUE

--SELECT
--	WF_QUE,
--	WF_SUB_QUE,
--	PF_REQ_ACT,
--	COUNT(*) AS TASK_COUNT
--FROM
--(
--SELECT DISTINCT
--	WF_QUE,
--	WF_SUB_QUE,
--	WN_CTL_TSK,
--	PF_REQ_ACT,
--	WF_CRT_DTS_WQXX
--FROM
--	CDW..WQXX_TSK_QUE_HST
--WHERE
--	WF_CRT_DTS_WQXX BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
--	AND WF_QUE = 'ML'
--	AND WF_SUB_QUE = 'XX'
--	AND PF_REQ_ACT IN ('MILRN','MILRV','RSCRA')
--) P
--GROUP BY
--	WF_QUE,
--	WF_SUB_QUE,
--	PF_REQ_ACT

--SELECT
--	WF_QUE,
--	WF_SUB_QUE,
--	PF_REQ_ACT,
--	COUNT(*) AS TASK_COUNT
--FROM
--(
--SELECT DISTINCT
--	WF_QUE,
--	WF_SUB_QUE,
--	WN_CTL_TSK,
--	PF_REQ_ACT,
--	WF_CRT_DTS_WQXX
--FROM
--	CDW..WQXX_TSK_QUE_HST
--WHERE
--	WF_CRT_DTS_WQXX BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
--	AND WF_QUE = 'SB'
--	AND WF_SUB_QUE = 'XX'
--	AND PF_REQ_ACT IN ('WRDDC','RPSFM','RPAYE','ISSRF','EMFRM')
--) P
--GROUP BY
--	WF_QUE,
--	WF_SUB_QUE,
--	PF_REQ_ACT

DECLARE @DATA TABLE (WF_QUE CHAR(X))
INSERT INTO @DATA VALUES
('XX'),
('XX'),
('XX'),
('XX'),
('XX'),
('XX'),
('XX'),
('XP'),
('XA'),
('XB'),
('XP'),
('XI'),
('XR'),
('BY'),
('DR'),
('EX'),
('GP'),
('MF'),
('ML'),
('NX'),
('NX'),
('PX'),
('PX'),
('PX'),
('PX'),
('PX'),
('PA'),
('PD'),
('PR'),
('RX'),
('RX'),
('RG'),
('RO'),
('RZ'),
('SX'),
('SX'),
('SD'),
('SE'),
('SF'),
('SO'),
('SU'),
('VB'),
('VR'),
('XX')

SELECT
	D.WF_QUE,
	COUNT(*) AS TASK_COUNT
FROM
	@DATA D
LEFT JOIN
(
SELECT DISTINCT
	WQXX.WF_QUE,
	WF_SUB_QUE,
	WN_CTL_TSK,
	PF_REQ_ACT,
	WF_CRT_DTS_WQXX
FROM
	CDW..WQXX_TSK_QUE_HST WQXX
	INNER JOIN @DATA D
		ON D.WF_QUE = WQXX.WF_QUE
WHERE
	WF_CRT_DTS_WQXX BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
) P
	ON P.WF_QUE = D.WF_QUE

GROUP BY
	D.WF_QUE
ORDER BY 
	D.WF_QUE