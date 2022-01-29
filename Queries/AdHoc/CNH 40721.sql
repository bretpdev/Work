DECLARE @Temp TABLE(BF_SSN CHAR(X), LN_SEQ smallint)
INSERT INTO @Temp
VALUES
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',XX),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',XX),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X),
('XXXXXXXXX',X)

UPDATE 
	LNXX
SET
	LNXX.LC_STA_LONXX = 'X'
FROM
	AuditCDW..LNXX_LON_DLQ_HST_JanXXXX LNXX
	INNER JOIN
	(
		SELECT
			A.BF_SSN,
			A.LN_SEQ,
			MIN(A.LN_DLQ_SEQ) AS LN_DLQ_SEQ_Update
		FROM
			AuditCDW..LNXX_LON_DLQ_HST_JanXXXX A 
			INNER JOIN @Temp T 
				ON T.BF_SSN = A.BF_SSN 
				AND T.LN_SEQ = A.LN_SEQ
		WHERE
			A.LC_STA_LONXX = 'X'
		GROUP BY
			A.BF_SSN,
			A.LN_SEQ
	) Missed
		ON Missed.BF_SSN = LNXX.BF_SSN
		AND Missed.LN_SEQ = LNXX.LN_SEQ
		AND Missed.LN_DLQ_SEQ_Update = LNXX.LN_DLQ_SEQ