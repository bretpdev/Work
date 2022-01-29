USE NobleCalls

GO

DELETE FROM NobleCalls.nblclbck.Callback WHERE CAST(AddedAt AS DATE) <= CAST(DATEADD(MONTH,-12,GETDATE()) AS DATE) 

MERGE
	NobleCalls.nblclbck.Callback C
USING
(
	SELECT
		*
	FROM OPENQUERY(UHEAAAPP1,
	'
		SELECT
			*
		FROM
			Callback
	')
) NewData
	ON NewData.rowid = C.rowid
	AND NewData.cb_rowid = C.cb_rowid
	AND NewData.cb_appl = C.cb_appl
	AND NewData.cb_acode = C.cb_acode
	AND NewData.cb_phone = C.cb_phone
	AND NewData.cb_date = C.cb_date
	AND NewData.cb_tsr = C.cb_tsr
	AND CAST(C.AddedAt AS DATE) = CAST(GETDATE() AS DATE) --Review for accuracy
WHEN MATCHED THEN
	UPDATE SET
		C.cb_time = NewData.cb_time,
		C.cb_status = NewData.cb_status,
		C.cb_adate = NewData.cb_adate,
		C.cb_atime = NewData.cb_atime,
		C.cb_btries = NewData.cb_btries,
		C.cb_ntries = NewData.cb_ntries,
		C.cb_country_id = NewData.cb_country_id,
		C.cb_listid = NewData.cb_listid,
		C.cb_dnis = NewData.cb_dnis,
		C.has_lapsed = NewData.has_lapsed
WHEN NOT MATCHED THEN
	INSERT
	(
		rowid,
		cb_rowid,
		cb_appl,
		cb_acode,
		cb_phone,
		cb_date,
		cb_time,
		cb_tsr,
		cb_status,
		cb_adate,
		cb_atime,
		cb_btries,
		cb_ntries,
		cb_country_id,
		cb_listid,
		cb_dnis,
		has_lapsed,
		AddedAt
	)
	VALUES
	(
		NewData.rowid,
		NewData.cb_rowid,
		NewData.cb_appl,
		NewData.cb_acode,
		NewData.cb_phone,
		NewData.cb_date,
		NewData.cb_time,
		NewData.cb_tsr,
		NewData.cb_status,
		NewData.cb_adate,
		NewData.cb_atime,
		NewData.cb_btries,
		NewData.cb_ntries,
		NewData.cb_country_id,
		NewData.cb_listid,
		NewData.cb_dnis,
		NewData.has_lapsed,
		GETDATE()
	);