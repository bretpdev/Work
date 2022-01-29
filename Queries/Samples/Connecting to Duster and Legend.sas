/*To run part of code and not all select it and then press [F3].*/

/*To connect to any region first open a tunnel to the desired region using AES Link or the code below.*/
/*The Tunnel for Legend Prod and Legend Test are the same.  In fact you can connect */
/*To both Simultaneously using the same tunnel.*/
/*Then run the connection code below.*/


/*LEGEND*/

/*To open a Tunnel to Legend from within the SAS Session:*/
OPTIONS NOXWAIT NOXSYNC;
DATA _NULL_;
	RC=SYSTEM("%BQUOTE("C:\Program Files (x86)\AES Link\PLINK") -L 5555:legend:4502 LEGEND"); 
RUN;

/*To connect to legend - prod:*/
signoff LEGEND;
%let LEGEND = LOCALHOST 5555;
%let region = 1 ;
filename rlink 'X:\PADU\SAS\TCPUNIX_SSH_LEGEND.SCR'  ;
OPTIONS REMOTE=LEGEND;
SIGNON LEGEND;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND  SLIBREF=WORK;


/*To connect to legend - test:*/
signoff LEGEND;
%let LEGEND = LOCALHOST 5555;
%let region = 2 ;
filename rlink 'X:\PADU\SAS\TCPUNIX_SSH_LEGEND.SCR'  ;
OPTIONS REMOTE=LEGEND;
SIGNON LEGEND;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND  SLIBREF=WORK;


/*DUSTER*/
/*To open a Tunnel to Duster from within the SAS Session:*/
OPTIONS NOXWAIT NOXSYNC;
DATA _NULL_;
	RC=SYSTEM("%BQUOTE("C:\Program Files (x86)\AES Link\PLINK") -L 5556:duster:4502 DUSTER"); 
RUN;

/*To connect to duster:*/

signoff DUSTER;
%let DUSTER = LOCALHOST 5555;
filename rlink 'X:\PADU\SAS\TCPUNIX_SSH_DUSTER.SCR'  ;
OPTIONS REMOTE=DUSTER;
SIGNON DUSTER;
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;




/*If you need to open a two tunnels: one to duster and one to legend, you can do this by opening one of the*/
/*tunnels to port 5556(not 5555).  If you take this route you will need to change the let statement for the*/
/*region you are connecting to.  AES Link always opens through port 5555.*/

/*DUSTER*/
OPTIONS NOXWAIT NOXSYNC;
DATA _NULL_;
	RC=SYSTEM("%BQUOTE("C:\Program Files (x86)\AES Link\PLINK") -L 5556:duster:4502 DUSTER"); 
RUN;

/*LEGEND*/
OPTIONS NOXWAIT NOXSYNC;
DATA _NULL_;
	RC=SYSTEM("%BQUOTE("C:\Program Files (x86)\AES Link\PLINK") -L 5556:legend:4502 LEGEND"); 
RUN;

%let DUSTER = LOCALHOST 5556;
%let LEGEND = LOCALHOST 5556;
%let LEGEND = LOCALHOST 5556;


