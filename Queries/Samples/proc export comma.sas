PROC EXPORT DATA= TEMP.Schinfo 
            OUTFILE= "C:\My Documents\My SAS Files\V8\defaultdat.txt" 
            DBMS=DLM REPLACE;
     DELIMITER='2C'x; 
RUN;
