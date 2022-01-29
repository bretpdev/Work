PROC SQL;
CONNECT TO ODBC AS BSYS (REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn; update_lock_type=nolock;");
CREATE TABLE PROMOTED AS
SELECT *
FROM CONNECTION TO BSYS (
SELECT A.SEQUENCE
	,A.CLASS
	,A.REQUEST
	,COALESCE(B.TITLE,C.TITLE) AS JOB
	,COALESCE(E.DESCRIPTION,F.DESCRIPTION) AS DESCRIPTION
	,A."BEGIN"
	,A."END"
	,A.STATUS
	,COALESCE(B.PRIORITY,C.PRIORITY) AS PRIORITY
	,COALESCE(B.SUMMARY,C.SUMMARY) AS SUMMARY
	,D."ORDER"
FROM SCKR_REF_STATUS A
LEFT OUTER JOIN SCKR_DAT_SASREQUESTS B
	ON A.REQUEST = B.REQUEST
	AND A.CLASS = B.CLASS
LEFT OUTER JOIN SCKR_DAT_SAS E
	ON E.JOB = B.JOB
LEFT OUTER JOIN SCKR_DAT_SCRIPTREQUESTS C
	ON A.REQUEST = C.REQUEST
	AND A.CLASS = C.CLASS
LEFT OUTER JOIN SCKR_DAT_SCRIPTS F
	ON C.SCRIPT = F.SCRIPT

LEFT OUTER JOIN SCKR_LST_STATUSES D
	ON A.STATUS = D.STATUS
);
DISCONNECT FROM BSYS;
QUIT;