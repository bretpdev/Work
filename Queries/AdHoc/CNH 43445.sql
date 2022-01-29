--Can you please provide the total number of XA tasks created between X/XX - X/XX. The output file will need to list the date and the number of tasks created. (date requested) 


SELECT
	WD_ACT_REQ AS REQUEST_DATE,
	PF_REQ_ACT AS ARC,
	COUNT(DISTINCT BF_SSN) TASK_COUNT
FROM
(
SELECT DISTINCT
	WD_ACT_REQ,
	PF_REQ_ACT,
	BF_SSN
	--COUNT(WN_CTL_TSK) AS TASKS
FROM
	CDW..WQXX_TSK_QUE_HST
WHERE
	cast(WD_ACT_REQ as date) BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
	AND WF_QUE = 'XA'
	
) P

GROUP BY
	WD_ACT_REQ,
	PF_REQ_ACT
ORDER BY REQUEST_DATE