LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=WORK;

PROC IMPORT
		OUT=BBC
		FILE='T:\SAS\Approved BB Corrections 110613.xlsx'
		REPLACE;
RUN;

DATA DUSTER.BBC; SET BBC; RUN;

RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

PROC SQL;
	CREATE TABLE BBCP AS
		SELECT
			BBC.*,
			LN54.LC_BBS_ELG,
			LN54.LN_BBS_STS_PCV_PAY,
			LN54.LD_BBS_DSQ
		FROM
			BBC
			LEFT JOIN OLWHRM1.LN54_LON_BBS LN54
				ON BBC.BR_SSN = LN54.BF_SSN
				AND BBC.COMP_LN_SEQ = LN54.LN_SEQ
				AND LN54.LC_STA_LN54 = 'A'			
	;

QUIT;

ENDRSUBMIT;

PROC EXPORT
		DATA=DUSTER.BBCP
		OUTFILE='T:\SAS\Approved BB Corrections 110613_LN54.XLSX'
		REPLACE;
RUN;

