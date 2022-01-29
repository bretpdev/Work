
filename mymail email "Jay Davis"
         subject="My autoexec.sas file"
         attach="C:\Program Files\SAS Institute\SAS\V8\autoexec.sas"
EMAILID="Novell Default Settings"
/*EMAILPW="" */
;
data _null_;
  file mymail;
  put 'Jay,';
  put ' ';
  put "I've attached my SAS autoexec file.  "@@;
  put 'Install it in C:\Program Files\SAS Institute\SAS\V8 '@@;
  put 'and you should be able to sign in to Cyprus using '@@;
  put 'the Run -> Signon menu selection.';
  put ' ';
  put 'Oh by the way, this email was generated automatically '@@;
  put 'in its entirety by SAS code.  Pretty slick, eh?';
  put ' ';
  put 'Matt';
run;


/*****************************************/

filename mymail email 
		 to = "mcowley@utahsbr.edu"
         subject= TESTMAIL
         attach="C:\WINDOWS\TEMP\sample1.txt"
		 EMAILID="Novell Default Settings";

data _null_;
  file mymail;
  put 'This message was generated automatically by SAS.';
run;