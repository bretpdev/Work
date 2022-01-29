     /* This step creates two macro variables called beg and end. */
     /* These variables will resolve to two dates, one today's date*/
     /* and the other a date 30 days ago. They will look like:     */
     /* Beg='02/12/2001'   End='03/12/2001'                        */
DATA _NULL_;
	CALL SYMPUT('END',"'"||put(INTNX('DAY',DATE(),0),mmddyy10.)||"'");
    CALL SYMPUT('BEG',"'"||put(INTNX('DAY',DATE(),-30),mmddyy10.)||"'");
	      
rsubmit;
PROC SQL;
connect to db2(database=dlgsutwh);
CREATE TABLE one AS
select *
from connection to db2(
SELECT  a.bf_ssn
       ,A.LC_TRX_TYP
       ,A.LC_RCI_TYP
       ,A.LA_TRX   
FROM OLWHRM1.DC11_LON_FAT A                 
WHERE  A.ld_sta_dc10 between &beg and &end  
        );
        disconnect from db2;
QUIT;
endrsubmit;

