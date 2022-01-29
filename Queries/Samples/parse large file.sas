data temp;
infile 'n:\largefile.001' firstobs=1 obs=25000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "s:\smallerfile.001" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;

