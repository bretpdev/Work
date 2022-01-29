DATA R4 (DROP=LN_SEQ);
	SET R4 END=LAST;
	LENGTH SEQLIST $200. ;
	BY DF_SPE_ACC_ID LN_SEQ;
	RETAIN SEQLIST;

	IF FIRST.DF_SPE_ACC_ID THEN 
		DO;
			SEQLIST = LEFT(PUT(LN_SEQ,2.));
		END;
	ELSE IF FIRST.LN_SEQ THEN
		DO;
			SEQLIST = CATX(',',TRIM(SEQLIST),LEFT(PUT(LN_SEQ,2.)));
		END;

	IF LAST.DF_SPE_ACC_ID THEN OUTPUT;
RUN;