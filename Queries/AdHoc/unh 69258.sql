USE UDW
GO

DECLARE @DATA TABLE (WF_QUE CHAR(2), WF_SUB_QUE CHAR(2))
INSERT INTO @DATA VALUES
('RB','01'),
('RB','02'),
('RB','03'),
('RB','04'),
('RB','05'),
('RB','06'),
('RB','07'),
('RB','08'),
('RB','09'),
('RB','10'),
('RB','11'),
('RB','12'),
('RB','13'),
('RB','14'),
('RB','15'),
('RB','16'),
('RB','17'),
('RB','18'),
('RB','19'),
('RB','20'),
('ER','01'),
('ER','02'),
('ER','03'),
('ER','04'),
('ER','05'),
('ER','06'),
('ER','07'),
('ER','08'),
('ER','09'),
('ER','10'),
('ER','11'),
('ER','12'),
('ER','13'),
('ER','14'),
('ER','15'),
('ER','16'),
('ER','17'),
('ER','18'),
('ER','19'),
('ER','20'),
('ER','21'),
('ER','22'),
('ER','23')


SELECT DISTINCT
	WQ21.WF_QUE,
	WQ21.WF_SUB_QUE,
	WQ21.BF_SSN,
	WQ21.WF_CRT_DTS_WQ20 AS TASK_CREATE_DATE,
	MAX(CASE	
		WHEN WQ21.WC_STA_WQUE20 = 'C' THEN WQ21.WF_CRT_DTS_WQ21
	END) OVER (PARTITION BY WQ21.WF_QUE, WQ21.WF_SUB_QUE, WQ21.WN_CTL_TSK, WQ21.PF_REQ_ACT) AS COMPLETE_DATE
FROM
	UDW..WQ21_TSK_QUE_HST WQ21
	INNER JOIN @DATA D
		ON D.WF_QUE = WQ21.WF_QUE
		AND D.WF_SUB_QUE = WQ21.WF_SUB_QUE
WHERE
	WQ21.WF_CRT_DTS_WQ20 >= '07/01/2020'
ORDER BY 
	WQ21.BF_SSN,
	TASK_CREATE_DATE