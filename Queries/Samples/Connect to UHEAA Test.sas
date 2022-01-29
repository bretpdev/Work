/*To connect to QADBD004 (UHEAA Test):*/

/*FIRST.  Open a tunnel to host34.aessuccess.org (NOT host143.aessuccess.org)*/
/*        if you do not remember your password for that server,*/
/*		call AES and let them know you need a new password for QADBD004*/

signoff QADBD004;
%let QADBD004 = LOCALHOST 5555;
filename rlink 'X:\PADU\SAS\TCPUNIX_SSH_DUSTER.SCR'  ;
OPTIONS REMOTE=QADBD004;
SIGNON QADBD004;
LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK;
