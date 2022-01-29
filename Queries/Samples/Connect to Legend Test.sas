
/*To connect to legend - Test prod:*/
/*1. open a tunnel to host203.aessuccess.org (the host is the same for prod and test)*/
/*2. run this job*/
/*3. In the job you want to run, comment out "%LET DB = DNFPUTDL;" and uncomment the %LET statement for the test database you want: DNFPRQUT = VUK1 test, DNFPRUUT = VUK3 test*/



signoff LEGEND;
%let LEGEND = LOCALHOST 5555;
%let region = 2 ; *region 1 = prod, region 2 = test;
filename rlink 'X:\PADU\SAS\TCPUNIX_SSH_LEGEND.SCR'  ;
OPTIONS REMOTE=LEGEND;
SIGNON LEGEND;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND  SLIBREF=WORK;
