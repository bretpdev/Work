USE ODW
GO

DECLARE @QUEUE VARCHAR(8) = 'ENRLPCA'
DECLARE @SOURCEFILE VARCHAR(25) = 'ULWDP8.LWDP8R2'
DECLARE @TODAY DATETIME = GETDATE()

INSERT INTO OLS.olqtskbldr.Queues(TargetId, QueueName, InstitutionId, InstitutionType, DateDue, TimeDue, Comment, SourceFilename, AddedAt, AddedBy)
SELECT DISTINCT
	DC01.BF_SSN, --TargetId
	@QUEUE, --QueueName
	NULL, --InstitutionId
	NULL, --InstitutionType
	NULL, --DateDue
	NULL, --TimeDue
	CONCAT('EGD = ', FORMAT(SD02.LD_XPC_GRD, 'd', 'us'), '; CERT = ', FORMAT(SD02.LD_ENR_CER, 'd', 'us')), --Comment
	@SOURCEFILE, --SourceFilename
	@TODAY, --AddedAt
	USER_NAME() --AddedBy
FROM
	ODW..DC01_LON_CLM_INF DC01
	INNER JOIN ODW..SD02_STU_ENR SD02
		ON DC01.BF_SSN = SD02.DF_PRS_ID_STU
	LEFT JOIN CT30_CALL_QUE CT30
		ON CT30.DF_PRS_ID_BR = DC01.BF_SSN
		AND CT30.IF_WRK_GRP = @QUEUE
		AND CT30.IC_TSK_STA IN ('A','W')
	LEFT JOIN OLS.olqtskbldr.Queues Q
		ON Q.TargetId = DC01.BF_SSN
		AND Q.QueueName = @QUEUE
		AND Q.ProcessedAt IS NULL
		AND Q.DeletedAt IS NULL
WHERE
	DC01.LC_STA_DC10 = '01'
	AND DC01.LC_PCL_REA IN ('DB','DF','DQ')
	AND SD02.LC_STA_SD02 = 'A'
	AND SD02.LC_STU_ENR_STA = 'I'
	AND CAST(SD02.LD_XPC_GRD AS DATE) > CAST(@TODAY AS DATE)
	AND CT30.DF_PRS_ID_BR IS NULL --Make sure there is not an active task
	AND Q.TargetId IS NULL --Check if the record is active in the Queues table so there are no dups
ORDER BY
	DC01.BF_SSN