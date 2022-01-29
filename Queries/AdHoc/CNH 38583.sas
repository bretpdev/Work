libname d 'Y:\Development\ExternalDataSources\MonthlyDataSets\';

proc sql;
create table qtr_data as 
	select distinct
		'this is month and year of qtr' as label,
		XXXXXXXX as avg_in_repayment
	from
		d.pkub_dwXX_dw_clc_clu_XXXXXX
;

delete from qtr_data;
quit;



%macro getandinsertdata(tableX, tableX, tableX, qtr, mod);
proc sql;
create table dwXX_data as 
	select
		(sum(b_count) / &mod) as q_data
	from
		(

			select
				count(distinct bf_ssn) as b_count
			from 
				d.&tableX
			where 
				WC_DW_LON_STA in ('XX','XX','XX')	
			
			union	

			select
				count(distinct bf_ssn) as b_count
			from 
				d.&tableX
			where 
				WC_DW_LON_STA in ('XX','XX','XX')	

			union	

			select
				count(distinct bf_ssn) as b_count
			from 
				d.&tableX
			where 
				WC_DW_LON_STA in ('XX','XX','XX')	
		)
;

insert into qtr_data
select
	&qtr,
	q_data
from
	dwXX_data
;

quit;

%MEND getandinsertdata;


%macro getandinsertdatasql(tableX, tableX, tableX, qtr, mod);
proc sql;
create table dwXX_data as 
	select
		(sum(b_count) / &mod) as q_data
	from
		(

			select
				count(distinct bf_ssn) as b_count
			from 
				SQL.&tableX
			where 
				WC_DW_LON_STA in ('XX','XX','XX')	
			
			union	

			select
				count(distinct bf_ssn) as b_count
			from 
				SQL.&tableX
			where 
				WC_DW_LON_STA in ('XX','XX','XX')	

			union	

			select
				count(distinct bf_ssn) as b_count
			from 
				SQL.&tableX
			where 
				WC_DW_LON_STA in ('XX','XX','XX')	
		)
;

insert into qtr_data
select
	&qtr,
	q_data
from
	dwXX_data
;

quit;

%MEND getandinsertdatasql;

LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\AuditCDW.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= dbo;

%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);

%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);

%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);

%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdata(pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX,pkub_dwXX_dw_clc_clu_XXXXXX, 'XX/XXXX - XX/XXXX' ,X);



%getandinsertdatasql(DWXX_DW_CLC_CLU_JulXXXX,DWXX_DW_CLC_CLU_AugXXXX,DWXX_DW_CLC_CLU_SepXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdatasql(DWXX_DW_CLC_CLU_OctXXXX,DWXX_DW_CLC_CLU_NovXXXX,DWXX_DW_CLC_CLU_DecXXXX, 'XX/XXXX - XX/XXXX' ,X);

%getandinsertdatasql(DWXX_DW_CLC_CLU_JanXXXX,DWXX_DW_CLC_CLU_FebXXXX,DWXX_DW_CLC_CLU_MarXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdatasql(DWXX_DW_CLC_CLU_AprXXXX,DWXX_DW_CLC_CLU_MayXXXX,DWXX_DW_CLC_CLU_JunXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdatasql(DWXX_DW_CLC_CLU_JulXXXX,DWXX_DW_CLC_CLU_AugXXXX,DWXX_DW_CLC_CLU_SepXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdatasql(DWXX_DW_CLC_CLU_OctXXXX,DWXX_DW_CLC_CLU_NovXXXX,DWXX_DW_CLC_CLU_DecXXXX, 'XX/XXXX - XX/XXXX' ,X);


%getandinsertdatasql(DWXX_DW_CLC_CLU_JanXXXX,DWXX_DW_CLC_CLU_FebXXXX,DWXX_DW_CLC_CLU_MarXXXX, 'XX/XXXX - XX/XXXX' ,X);
%getandinsertdatasql(DWXX_DW_CLC_CLU_AprXXXX,DWXX_DW_CLC_CLU_MayXXXX,DWXX_DW_CLC_CLU_JunXXXX, 'XX/XXXX - XX/XXXX' ,X);




proc sql;
create table final as select * from qtr_data; quit;

PROC EXPORT 
	DATA= WORK.final
	OUTFILE= "T:\CNH XXXXX.xlsx" 
	DBMS=XLSX /*this is the correct DBMS for EXCEL XXXX */
	REPLACE /*comment out or delete this line and change the name of the sheet below if you want to add the output to a new tab in an existing spreadsheet*/
	; /*NOTE everything up to this semi-colon is actually one command, it has just been broken up on separate lines for readability*/
	SHEET="SheetX"; 
RUN;
