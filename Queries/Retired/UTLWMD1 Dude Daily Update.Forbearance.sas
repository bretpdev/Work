PROC SQL;
CREATE TABLE NO_FB10_DELTA AS
SELECT DISTINCT A.DF_SPE_ACC_ID
	,A.LN_SEQ
	,A.LF_FOR_CTL_NUM
	,A.LN_FOR_OCC_SEQ
FROM FB10 A
INNER JOIN DUDE.FORBEARANCE B
	ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
	AND A.LN_SEQ = B.LN_SEQ
	AND A.LF_FOR_CTL_NUM = B.LF_FOR_CTL_NUM
	AND A.LN_FOR_OCC_SEQ = B.LN_FOR_OCC_SEQ
	AND A.LC_FOR_TYP = B.LC_FOR_TYP
	AND A.FOR_TYP = B.FOR_TYP
	AND A.LD_FOR_INF_CER = B.LD_FOR_INF_CER
	AND A.LD_FOR_BEG = B.LD_FOR_BEG
	AND A.LD_FOR_END = B.LD_FOR_END
	AND A.LI_CAP_FOR_INT_REQ = B.LI_CAP_FOR_INT_REQ
	AND A.LC_STA_LON60 = B.LC_STA_LON60
	AND A.LC_FOR_STA = B.LC_FOR_STA
	AND A.LC_STA_FOR10 = B.LC_STA_FOR10
	AND A.MONTHS_USED = B.MONTHS_USED
	AND A.LA_REQ_RDC_PAY = B.LA_REQ_RDC_PAY;

CREATE TABLE FB10_DELTAS AS
SELECT DISTINCT A.*
FROM FB10 A
LEFT JOIN NO_FB10_DELTA B
      ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
      AND A.LN_SEQ = B.LN_SEQ
	  AND A.LF_FOR_CTL_NUM = B.LF_FOR_CTL_NUM
	  AND A.LN_FOR_OCC_SEQ = B.LN_FOR_OCC_SEQ
WHERE B.DF_SPE_ACC_ID IS NULL
	AND B.LN_SEQ IS NULL
	AND B.LF_FOR_CTL_NUM IS NULL
	AND B.LN_FOR_OCC_SEQ IS NULL;
QUIT;

DATA FORBEARANCE_DELTAS (KEEP=DF_SPE_ACC_ID LN_SEQ LF_FOR_CTL_NUM LN_FOR_OCC_SEQ);
	SET FB10_DELTAS;
RUN;
PROC SORT DATA=FORBEARANCE_DELTAS NODUPKEY;
	BY DF_SPE_ACC_ID LN_SEQ LF_FOR_CTL_NUM LN_FOR_OCC_SEQ;
RUN;  

PROC SQL;
CREATE TABLE LOCL_FORBEARANCE_DELTAS AS
      SELECT A.* 
      FROM DUDE.FORBEARANCE A
      INNER JOIN FORBEARANCE_DELTAS B
	      ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
	      AND A.LN_SEQ = B.LN_SEQ
		  AND A.LF_FOR_CTL_NUM = B.LF_FOR_CTL_NUM
		  AND A.LN_FOR_OCC_SEQ = B.LN_FOR_OCC_SEQ;

INSERT INTO DUDE.FORBEARANCE_DELETE
      SELECT DF_SPE_ACC_ID
	  		,LN_SEQ
			,LF_FOR_CTL_NUM
			,LN_FOR_OCC_SEQ 
      FROM LOCL_FORBEARANCE_DELTAS; 
QUIT;

%UPDATE_DATA(LOCL_FORBEARANCE_DELTAS,FB10_DELTAS,%STR(DF_SPE_ACC_ID LN_SEQ LF_FOR_CTL_NUM 
		LN_FOR_OCC_SEQ));
PROC SQL NOPRINT;
CONNECT TO ODBC AS MD (&MD);
SELECT COUNT(*) 
FROM CONNECTION TO MD (
      DELETE FORBEARANCE
      FROM FORBEARANCE A
      INNER JOIN FORBEARANCE_DELETE B
            ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
			AND A.LN_SEQ = B.LN_SEQ
			AND A.LF_FOR_CTL_NUM = B.LF_FOR_CTL_NUM
			AND A.LN_FOR_OCC_SEQ = B.LN_FOR_OCC_SEQ
      );
DISCONNECT FROM MD;
QUIT;

PROC SQL ;
INSERT INTO DUDE.FORBEARANCE
      SELECT * 
      FROM LOCL_FORBEARANCE_DELTAS
	  WHERE LC_STA_LON60 = 'A'
	  	AND LC_FOR_STA = 'A'
		AND LC_STA_FOR10 = 'A'; 
QUIT;

OPTIONS NOSOURCE;
%GOODBYE_NULLCHAR(FORBEARANCE,LC_FOR_TYP);
%GOODBYE_NULLCHAR(FORBEARANCE,FOR_TYP);
%GOODBYE_NULLCHAR(FORBEARANCE,LD_FOR_INF_CER);
%GOODBYE_NULLCHAR(FORBEARANCE,LD_FOR_BEG);
%GOODBYE_NULLCHAR(FORBEARANCE,LD_FOR_END);
%GOODBYE_NULLCHAR(FORBEARANCE,LI_CAP_FOR_INT_REQ);
%GOODBYE_NULLCHAR(FORBEARANCE,LC_STA_LON60);
%GOODBYE_NULLCHAR(FORBEARANCE,LC_FOR_STA);
%GOODBYE_NULLCHAR(FORBEARANCE,LC_STA_FOR10);
%GOODBYE_NULL(FORBEARANCE,MONTHS_USED);	
%GOODBYE_NULL(FORBEARANCE,LA_REQ_RDC_PAY);

PROC SQL NOPRINT;
CONNECT TO ODBC AS MD (&MD);
SELECT COUNT(*) 
FROM CONNECTION TO MD (
      DELETE 
      FROM FORBEARANCE_DELETE 
      );
QUIT;
OPTIONS SOURCE;
