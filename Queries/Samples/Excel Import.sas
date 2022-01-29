PROC IMPORT OUT= WORK.SAS Dataset Name
            DATAFILE= "Filepath and Name of Import File.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Worksheet Name$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;
