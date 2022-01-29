*	This example selects an 8-bit character string as a date
	in the SQL environment so it can be used as a selection criterion.
;

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	integer(A.lf_prs_id)				as SSN,
		A.lc_dsb_typ,
		A.lc_dsb_sta,
		A.ld_rtn_psf,
case 
when A.ln_vou_trt_num = ' ' then null
when A.ln_vou_trt_num <> ' ' then 
		cast(substr(A.ln_vou_trt_num,1,2)||'-'||substr(A.ln_vou_trt_num,3,2)
		||'-'||substr(A.ln_vou_trt_num,5,4)	as date)
end											as trt_num
FROM  OLWHRM1.FD01_QUE_TAB A
WHERE A.ln_vou_trt_num <> ' '
AND cast(substr(A.ln_vou_trt_num,1,2)||'-'||substr(A.ln_vou_trt_num,3,2)
        ||'-'||substr(A.ln_vou_trt_num,5,4) as date) BETWEEN '04-10-2002' AND '04-12-2002'
ORDER BY A.lf_prs_id,A.lc_dsb_typ, A.ld_rtn_psf
);
DISCONNECT FROM DB2;
endrsubmit  ;

DATA DEMO; SET WORKLOCL.DEMO; RUN;
