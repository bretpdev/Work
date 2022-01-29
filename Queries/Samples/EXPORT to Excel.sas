PROC EXPORT DATA= WORK.Current_forbs_detail_r 
            OUTFILE= "T:\SAS\CURRENT_FORBS_DETAIL.xls" 
            DBMS=EXCEL REPLACE;
     SHEET="A"; 
RUN;
