DECLARE @MON_DATA TABLE (MON VARCHAR(X), YR VARCHAR(X))
INSERT INTO @MON_DATA
VALUES
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('XX','XXXX')



DECLARE @cols AS NVARCHAR(MAX),
@colsnonnull AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX),
	@colsX AS NVARCHAR(MAX)

	
select @cols = STUFF((SELECT ',' + 'isnull(' + QUOTENAME(MON + '-' + YR) + ',X) ' + QUOTENAME(MON + '-' + YR) FROM @MON_DATA ORDER BY CAST(YR AS INT), CAST(MON AS INT) FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,X,X,'')

select @colsX = STUFF((SELECT ',' + QUOTENAME(MON + '-' + YR)  FROM @MON_DATA ORDER BY CAST(YR AS INT), CAST(MON AS INT) FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,X,X,'')


SELECT @QUERY = 
'SELECT
	LABEL AS LABEL,
	' + @cols + '
FROM
(
	SELECT
		CASE 
			WHEN AYXX.PF_REQ_ACT = ''DUEDT'' THEN ''Other Repay Plan Processing:''
			WHEN AYXX.PF_REQ_ACT = ''RSCHG'' THEN ''Non-IDR Plan Processing:''
			WHEN AYXX.PF_REQ_ACT IN (''RSCRA'',''MSCRA'', ''MILRN'', ''USCRA'', ''ISCRA'', ''SCRAA'',''SCRAD'') THEN ''Military Benefits:''
			WHEN AYXX.PF_REQ_ACT IN (''DFAPV'', ''DFDNY'', ''CTDXP'', ''MILRV'') THEN ''Deferment Processing:''
			WHEN AYXX.PF_REQ_ACT IN (''FBDNY'', ''FBAPV'') THEN ''Forbearance Processing:''
			WHEN AYXX.PF_REQ_ACT IN (''ADFCR'', ''FCDNY'', ''ALLRV'', ''DEFSA'', ''CSFSA'', ''ADCSH'', ''CSDNY'', ''ADDTH'', ''CSFSA'', ''DEDNY'',
			''DIDAS'',''IDAPB'', ''IDEFS'', ''IDAPE'', ''IDBFS'', ''IDMDB'', ''IDMDE'', ''TLFSA'', ''ADTLF'', ''TLDNY'', ''UPDNY'', ''UPFSA'',
			''ADUPR'', ''ADBKP'', ''DTHBR'',''DTHCB'', ''DTHST'') THEN ''Discharge Processing (Non-TPD):''
			WHEN AYXX.PF_REQ_ACT IN (''BPOCR'') THEN ''Bankruptcy POC Filing:''
			WHEN AYXX.PF_REQ_ACT IN (''BKOBL'') THEN ''Bankruptcy Escalate to FSA:''
			WHEN AYXX.PF_REQ_ACT IN (''BXXXA'', ''DBKRW'', ''BPOCX'', ''BDISC'', ''BDISM'') THEN ''Bankruptcy Other:''
			WHEN AYXX.PF_REQ_ACT IN (''RVCRD'', ''CRDSP'') THEN ''FCRA Credit Dispute Processing:''
			WHEN AYXX.PF_REQ_ACT IN (''ENOTE'') THEN ''NSLDS/Enrollment Updates:''
			WHEN AYXX.PF_REQ_ACT IN (''PMTAD'') THEN ''Payment Exception and Refund Processing:''
			WHEN AYXX.PF_REQ_ACT IN (''LSADJ'', ''LSKEY'', ''MXADD'') THEN ''Account Status Change and Manual Adjustments:''
			WHEN AYXX.PF_REQ_ACT IN (''APPRV'', ''TPDAP'') THEN ''TPD Approval Processing:''
			WHEN AYXX.PF_REQ_ACT IN (''TPDNY'', ''APPRJ'') THEN ''TPD Denial Processing: ''
			WHEN AYXX.PF_REQ_ACT IN (''DIBCR'',''DIDDC'',''DIDDE'',''DIOEC'',''DIREQ'') THEN ''Borrower Correspondence: ''
			WHEN AYXX.PF_REQ_ACT IN (''BDFSA'', ''RCBDF'') THEN ''Borrower Defense Initial Processing: ''
			WHEN AYXX.PF_REQ_ACT IN (''BDDNY'', ''BDINT'', ''BDRVD'') THEN ''Borrower Defense Post-Determination: ''
			WHEN AYXX.PF_REQ_ACT IN (''BDOPT'') THEN ''Borrower Defense Other: ''

		END AS LABEL,
		CAST(MONTH(AYXX.LD_ATY_REQ_RCV ) AS VARCHAR(X)) + ''-'' + CAST(YEAR(AYXX.LD_ATY_REQ_RCV ) AS VARCHAR(X)) AS TF,
		COUNT(*) AS [COUNT]
	FROM
		CDW..AYXX_BR_LON_ATY AYXX
	WHERE
		AYXX.LD_ATY_REQ_RCV BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX''
		AND AYXX.LC_STA_ACTYXX = ''A''
		AND AYXX.PF_REQ_ACT IN (''DUEDT'', ''RSCHG'', ''RSCRA'',''MSCRA'', ''MILRN'', ''USCRA'', ''ISCRA'',''DFAPV'', ''DFDNY'', ''CTDXP'', ''MILRV'',
		''FBDNY'', ''FBAPV'',''ADFCR'', ''FCDNY'', ''ALLRV'', ''DEFSA'', ''CSFSA'', ''ADCSH'', ''CSDNY'', ''ADDTH'', ''CSFSA'', ''DEDNY'',
			''DIDAS'',''IDAPB'', ''IDEFS'', ''IDAPE'', ''IDBFS'', ''IDMDB'', ''IDMDE'', ''TLFSA'', ''ADTLF'', ''TLDNY'', ''UPDNY'', ''UPFSA'',
			''ADUPR'', ''ADBKP'', ''DTHBR'',''DTHCB'', ''DTHST'',''BPOCR'',''BKOBL'',''BXXXA'', ''DBKRW'', ''BPOCX'', ''BDISC'', ''BDISM'',''RVCRD'', ''CRDSP'',
			''ENOTE'',''PMTAD'',''LSADJ'',''APPRV'', ''TPDAP'',''TPDNY'', ''APPRJ'', ''LSKEY'', ''MXADD'',
			''SCRAA'',''SCRAD'', ''DIBCR'',''DIDDC'',''DIDDE'',''DIOEC'',''DIREQ'',''BDFSA'', ''RCBDF'',''BDDNY'', ''BDINT'', ''BDRVD'', ''BDOPT'')
	GROUP BY
		PF_REQ_ACT,
		CAST(MONTH(AYXX.LD_ATY_REQ_RCV ) AS VARCHAR(X)) + ''-'' + CAST(YEAR(AYXX.LD_ATY_REQ_RCV ) AS VARCHAR(X))
	
	

) P
PIVOT
(
	SUM([COUNT])
	FOR TF IN (' + @colsX + ')
) p

UNION

SELECT
	LABEL AS LABEL,
	' + @cols + '
FROM
(
	SELECT DISTINCT
	CAST(MONTH(created_at ) AS VARCHAR(X)) + ''-'' + CAST(YEAR(created_at ) AS VARCHAR(X)) AS TF,
	CASE 
		WHEN repayment_plan_reason_id IN (X,X) THEN ''IDR NEW Apps:''
		WHEN repayment_plan_reason_id IN (X,X) THEN ''IDR Recerts:''
	END AS LABEL,
	count(*) as [COUNT]
FROM
	Income_Driven_Repayment..Applications A
WHERE
	created_at BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX''
GROUP BY
	CAST(MONTH(created_at ) AS VARCHAR(X)) + ''-'' + CAST(YEAR(created_at ) AS VARCHAR(X)),
	CASE 
		WHEN repayment_plan_reason_id IN (X,X) THEN ''IDR NEW Apps:''
		WHEN repayment_plan_reason_id IN (X,X) THEN ''IDR Recerts:''
	END

) P
PIVOT
(
	SUM([COUNT])
	FOR TF IN (' + @colsX + ')
) p
'

EXEC (@QUERY)





