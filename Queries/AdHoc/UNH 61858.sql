BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

--Update requests
	UPDATE 
		BSYS..LTDB_DAT_Requests
	SET 
		CurrentStatus = 'Tester Assignment',
		StatusDate = CAST (GETDATE() AS DATE),
		Court = 'Wendy Hack',
		CourtDate = CAST (GETDATE() AS DATE),
		History = CONCAT('UNH 61858 - ',CONVERT(varchar, GETDATE(), 22), ' - Tester Assignment', CHAR(13), CHAR(10), 'Request moved by DCR to Wendy Hack for tester assignment', CHAR(13), CHAR(10), CHAR(13), CHAR(10), History)
	FROM
		BSYS..LTDB_DAT_Requests
	WHERE
		Request IN (7607,7606,7608,7697,7708,7709,7715,7716,7717,7718,7719,7720,7721,7722,7723,7724,7725,7726,7727,7728,7729,7730,7731,7732,7733,7737,7738,7739,7740,7741,7742,7743,7744,7745,7746,7747,7748,7749,7750,7751,7752,7753,7754,7755,7756,7757,7758,7759,7760,7761,7762,7763,7764,7765,7766,7767,7768,7769,7770,7771,7772,7773,7774,7775,7776,7777,7778,7779,7780,7781,7782,7783,7785,7786,7787,7788,7789,7790,7791,7792,7793,7794,7795,7796,7797,7798,7799,7800)
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	
--set end date of open REF_Status record to current date to close that status record
	UPDATE
		BSYS..LTDB_REF_Status
	SET
		[End] = CAST(GETDATE() AS DATE)
	WHERE
		Request IN (7607,7606,7608,7697,7708,7709,7715,7716,7717,7718,7719,7720,7721,7722,7723,7724,7725,7726,7727,7728,7729,7730,7731,7732,7733,7737,7738,7739,7740,7741,7742,7743,7744,7745,7746,7747,7748,7749,7750,7751,7752,7753,7754,7755,7756,7757,7758,7759,7760,7761,7762,7763,7764,7765,7766,7767,7768,7769,7770,7771,7772,7773,7774,7775,7776,7777,7778,7779,7780,7781,7782,7783,7785,7786,7787,7788,7789,7790,7791,7792,7793,7794,7795,7796,7797,7798,7799,7800)
		AND [End] IS NULL
	---- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


--insert new status records into REF_Status
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7607,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7606,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7608,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7697,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7708,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7709,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7715,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7716,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7717,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7718,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7719,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7720,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7721,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7722,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7723,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7724,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7725,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7726,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7727,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7728,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7729,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7730,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7731,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7732,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7733,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7737,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7738,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7739,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7740,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7741,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7742,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7743,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7744,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7745,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7746,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7747,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7748,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7749,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7750,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7751,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7752,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7753,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7754,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7755,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7756,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7757,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7758,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7759,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7760,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7761,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7762,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7763,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7764,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7765,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7766,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7767,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7768,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7769,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7770,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7771,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7772,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7773,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7774,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7775,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7776,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7777,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7778,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7779,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7780,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7781,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7782,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7783,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7785,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7786,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7787,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7788,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7789,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7790,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7791,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7792,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7793,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7794,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7795,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7796,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7797,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7798,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7799,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS..LTDB_REF_Status (Request, [Status], [Begin], [Court]) VALUES (7800,'Tester Assignment',CAST(GETDATE() AS DATE), 'Wendy Hack') SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


IF @ROWCOUNT = 264 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END