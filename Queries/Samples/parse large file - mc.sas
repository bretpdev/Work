%let infile = p:\UHEAA.UNIPAC.LENDER.MANIFEST.DT03082002;
%let outfile = s:\dbq\files\UHEAA.UNIPAC.LENDER.MANIFEST.DT03082002;

data temp;
infile "&infile" firstobs=1 obs=25000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..001" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;

data temp;
infile "&infile" firstobs=25001 obs=50000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..002" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;
data temp;
infile "&infile" firstobs=50001 obs=75000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..003" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;

data temp;
infile "&infile" firstobs=75001 obs=100000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..004" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;
data temp;
infile "&infile" firstobs=100001 obs=125000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..005" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;
data temp;
infile "&infile" firstobs=125001 obs=150000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..006" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;

data temp;
infile "&infile" firstobs=150001 obs=175000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..007" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;
data temp;
infile "&infile" firstobs=175001 obs=200000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..008" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;

data temp;
infile "&infile" firstobs=200001 obs=225000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..009" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;
data temp;
infile "&infile" firstobs=225001 obs=250000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..010" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;

data temp;
infile "&infile" firstobs=250001 obs=275000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..011" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;
data temp;
infile "&infile" firstobs=275001 obs=300000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..012" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;
data temp;
infile "&infile" firstobs=300001 obs=325000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..013" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;
data temp;
infile "&infile" firstobs=325001 obs=350000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..014" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;

data temp;
infile "&infile" firstobs=350001 obs=375000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "&outfile..015" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;
data temp;
infile "&infile" firstobs=375001 obs=300000 lrecl=32769 TRUNCOVER;
input data $ 1-640;
run;

PROC EXPORT DATA= temp
            OUTFILE= "s:\dbq\files\&outfile.016" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;
