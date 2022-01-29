/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CREATE TABLE DATA AS
		SELECT DISTINCT
			LN10.BF_SSN,
			LN10.LN_SEQ,
			LN10.IC_LON_PGM,
			coalesce(unsub.LD_LON_GTR, sub.LD_LON_GTR) as LD_LON_GTR,
			LN10.LD_LON_ACL_ADD,
			SUB.LD_FOR_BEG,
			SUB.LD_FOR_END

/*			,coalesce(UNSUB.LC_FOR_STA, SUB.LC_FOR_STA) as LC_FOR_STA */
/*			,coalesce(UNSUB.LC_STA_FOR10, SUB.LC_STA_FOR10) as LC_STA_FOR10*/
/*			,coalesce(UNSUB.LC_STA_LON60, SUB.LC_STA_LON60) as LC_STA_LON60*/

		FROM	
			OLWHRM1.LN10_LON LN10
			LEFT JOIN
			(
				SELECT distinct
					LN10.BF_SSN,
					LN10.LN_SEQ,
					LD_LON_GTR,
					sum(CASE
						WHEN FB10_UNSUB.BF_SSN IS NOT NULL THEN 1 
						ELSE 0
					END) AS HAS_FORB

/*					,LC_FOR_STA*/
/*					,LC_STA_FOR10*/
/*					,LC_STA_LON60*/

				FROM 
					OLWHRM1.LN10_LON LN10
					LEFT JOIN OLWHRM1.LN60_BR_FOR_APV LN60_UNSUB
						ON LN10.BF_SSN = LN60_UNSUB.BF_SSN
						AND LN10.LN_SEQ = LN60_UNSUB.LN_SEQ
						AND LN60_UNSUB.LC_STA_LON60 = 'A'
					LEFT JOIN OLWHRM1.FB10_BR_FOR_REQ FB10_UNSUB
						ON FB10_UNSUB.BF_SSN = LN60_UNSUB.BF_SSN
						AND FB10_UNSUB.LF_FOR_CTL_NUM = LN60_UNSUB.LF_FOR_CTL_NUM
						AND FB10_UNSUB.LC_FOR_TYP = '17'
						AND FB10_UNSUB.LC_FOR_STA = 'A'
						AND FB10_UNSUB.LC_STA_FOR10 = 'A'
				WHERE	
					IC_LON_PGM IN ('UNCNS','UNSPC')
				group by
					LN10.BF_SSN,
					LN10.LN_SEQ,
					LD_LON_GTR
					
			)UNSUB
				ON UNSUB.BF_SSN = LN10.BF_SSN
				AND UNSUB.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN
			(
				SELECT distinct
					LN10.BF_SSN,
					LN10.LN_SEQ,
					LD_LON_GTR,
					1 AS HAS_FORB,
					LD_FOR_BEG,
					LD_FOR_END

/*					,LC_FOR_STA*/
/*					,LC_STA_FOR10*/
/*					,LC_STA_LON60*/

				FROM 
					OLWHRM1.LN10_LON LN10
					INNER JOIN OLWHRM1.LN60_BR_FOR_APV LN60_SUB
						ON LN10.BF_SSN = LN60_SUB.BF_SSN
						AND LN10.LN_SEQ = LN60_SUB.LN_SEQ
						AND LN60_SUB.LC_STA_LON60 = 'A'
					INNER JOIN OLWHRM1.FB10_BR_FOR_REQ FB10_UNSUB
						ON FB10_UNSUB.BF_SSN = LN60_SUB.BF_SSN
						AND FB10_UNSUB.LF_FOR_CTL_NUM = LN60_SUB.LF_FOR_CTL_NUM
						AND FB10_UNSUB.LC_FOR_TYP = '17'
						AND FB10_UNSUB.LC_FOR_STA = 'A'
						AND FB10_UNSUB.LC_STA_FOR10 = 'A'
				WHERE	
					IC_LON_PGM IN ('SUBCNS','SUBSPC')
					
			)SUB
				ON SUB.BF_SSN = LN10.BF_SSN
				AND SUB.LN_SEQ = LN10.LN_SEQ
		WHERE
			(UNSUB.BF_SSN IS NOT NULL OR SUB.BF_SSN IS NOT NULL)
/*			(UNSUB.CLUID = SUB.CLUID OR UNSUB.GUID = SUB.GUID)*/
/*AND (SUB.HAS_FORB = 1 AND UNSUB.HAS_FORB = 0)*/
  and (UNSUB.HAS_FORB = 0 OR SUB.HAS_FORB = 1)
 and year(LD_LON_ACL_ADD) = 2016 
order by
	ln10.bf_ssn,
	ln10.ln_seq
			 
;
QUIT;

ENDRSUBMIT;

DATA DATA;
	SET DUSTER.DATA;
RUN;

proc sql;
	create table final as 
		select distinct
			d.*
		from
			data d
			inner join
			(
				select
					bf_ssn,
					LD_LON_GTR,
					count(*)
				from
					data
				group by
					bf_ssn,
					LD_LON_GTR
				having count(*) > 1
			) f
				on d.bf_ssn = f.bf_ssn
			inner join
			(
				select
					bf_ssn,
					ic_lon_pgm,
					count(*)
				from
					data
				group by
					bf_ssn,
					ic_lon_pgm
				having count(*) = 1
			)ff
				on d.bf_ssn = ff.bf_ssn
;
quit;


PROC EXPORT DATA = WORK.final 
            OUTFILE = "T:\SAS\NH 26324.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
