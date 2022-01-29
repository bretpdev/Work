*This example of the SIMPLE CASE expression prevents an extra space between
 first and last names when there is no middle initial;

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT distinct integer(A.bf_ssn)		as SSN,
case 
	when RTRIM(D.DM_PRS_MID) <> ' ' 
		then RTRIM(D.DM_PRS_1)||' '||RTRIM(D.DM_PRS_MID)||' '||RTRIM(D.DM_PRS_LST)
	when RTRIM(D.DM_PRS_MID) = ' ' 
		then RTRIM(D.DM_PRS_1)||' '||RTRIM(D.DM_PRS_LST)
end										
										AS NAME,
CASE 
		WHEN D.dn_alt_phn = ' ' THEN D.dn_alt_phn
		ELSE SUBSTR(D.dn_alt_phn,1,3)||'-'||SUBSTR(D.dn_alt_phn,4,3)
			 ||'-'||SUBSTR(D.dn_alt_phn,7,4)
END										AS ALTPHN
FROM  OLWHRM1.DC01_LON_CLM_INF A left outer join OLWHRM1.PD01_PDM_INF D
	on A.bf_ssn = D.DF_PRS_ID
WHERE A.bf_ssn in ('528212765','575885241')
);
DISCONNECT FROM DB2;
endrsubmit  ;

DATA DEMO; SET WORKLOCL.DEMO; RUN;

OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN;
LABEL	NAME = ' Name' ;
FORMAT SSN SSN11.		;
TITLE 'DEMOGRAPHICS INFORMATION';
RUN;