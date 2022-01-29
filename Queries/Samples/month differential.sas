*ATUT = NUMBER OF MONTHS BETWEEN POFDT AND PSTDT;

DATA DEMO ; 
SET DEMO; 
ATUT = INTCK('MONTH',POFDT,PSTDT);
*ATUT = (month(pstdt) - month(pofdt)) + (year(pstdt) - year(pofdt))*12;
RUN;


