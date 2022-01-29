*BORROWER LEVEL ONLY Out of school/less than half time dates must be calculated this way
because the SD02 table is messed up, and you can't rely on the Active
SD02 row for valid information;
%LET BEGIN = '2001-09-28';
%LET END = '2001-09-30';

%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132 symbolgen;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE OOSCD AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	DISTINCT
		B.df_prs_id_stu					AS STUSSN,
		B.ld_stu_enr_sta				AS OOSCD
FROM  OLWHRM1.SD02_STU_ENR B
WHERE B.ld_stu_enr_sta between &BEGIN and &END
AND	B.ld_enr_cer = 
		(	select min(w.ld_enr_cer)
			from OLWHRM1.SD02_STU_ENR W
			where B.df_prs_id_stu = w.df_prs_id_stu
			and w.lc_stu_enr_sta = 'O'
			and w.ld_enr_cer > 
			(	select max(x.ld_enr_cer)
				from OLWHRM1.SD02_STU_ENR X
				where w.df_prs_id_stu = x.df_prs_id_stu
				and x.lc_stu_enr_sta = 'I'
			)
		)
AND exists (select y.lc_stu_enr_sta
			from OLWHRM1.SD02_STU_ENR Y
			where B.df_prs_id_stu = y.df_prs_id_stu
			and y.lc_stu_enr_sta = 'O'
			and y.ld_enr_cer = 
			(	select max(z.ld_enr_cer)
				from OLWHRM1.SD02_STU_ENR z
				where y.df_prs_id_stu = z.df_prs_id_stu
			)
		   )
);
DISCONNECT FROM DB2;
ENDRSUBMIT;
DATA OOSCD; SET WORKLOCL.OOSCD; RUN;