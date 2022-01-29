--rX:
--ACCOUNT # ,BANKRUPTCY TYPE,AGE,       # MNTHS,BANKRUPTCY STATUS,RECVD DT,STATUS DATE,# LOANS,PBO,          IRB,    TOTAL
--XXXXXXXXXX,               ,< X MONTHS,       ,XX               ,        ,XX/XX/XXXX ,X      ,"$XXX,XXX.XX",$XXX.XX,"$XXX,XXX.XX"


SELECT * FROM CDW..PDXX_PRS_BKR WHERE DF_PRS_ID = 'XXXXXXXXX'

---see equivalent lines XX-XX in UTNWCXX.sas
;WITH C AS
(
	SELECT 
		DF_PRS_ID
		,MAX(DD_BKR_STA) AS DD_BKR_STA
		,SUBSTRING(DF_COU_DKT,X,X) AS DF_COU_DKT
	FROM 
		CDW..PDXX_PRS_BKR
	WHERE 
		DF_PRS_ID = 'XXXXXXXXX'
	GROUP BY 
		DF_PRS_ID, 
		SUBSTRING(DF_COU_DKT,X,X)
)
SELECT DISTINCT
	--D.DF_SPE_ACC_ID
	A.DF_PRS_ID
	--,coalesce(b.bf_ssn,a.df_prs_id) as bf_ssn
	,CASE
		WHEN A.DC_BKR_STA = 'XX' AND C.DD_BKR_STA > CONVERT(DATE,'XXXXXXXX')--&FINISH 
		THEN 'Suspended'
		WHEN A.DC_BKR_STA = 'XX' THEN 'Discharged'
		WHEN A.DC_BKR_STA = 'XX' THEN 'Suspended'
		ELSE ''
	END AS DISSED
	,A.DD_BKR_COR_X_RCV
	,C.DD_BKR_STA
	,A.DD_BKR_NTF
	,A.DD_BKR_FIL
	,CASE
		WHEN A.DC_BKR_STA = 'XX' AND C.DD_BKR_STA > CONVERT(DATE,'XXXXXXXX')--&FINISH 
		THEN 'XX'
		ELSE A.DC_BKR_STA
	END AS DC_BKR_STA
	,A.DC_BKR_TYP
	,A.DF_COU_DKT
	--,intck('month',A.DD_BKR_COR_X_RCV,&finish) as num_mon
FROM
	C
	INNER JOIN CDW..PDXX_PRS_BKR A
		ON A.DF_PRS_ID = C.DF_PRS_ID
		AND A.DD_BKR_STA = C.DD_BKR_STA
		AND SUBSTRING(A.DF_COU_DKT,X,X) = C.DF_COU_DKT






