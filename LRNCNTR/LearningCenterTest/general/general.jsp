<%
////////////////////////////////////////////////
//////////TEST MODE VARIABLES///////////////////
boolean TestMode = false;
////////////////////////////////////////////////

String CookieName = "userLoginCookie";
String BSYS_DRIVER = "com.microsoft.jdbc.sqlserver.SQLServerDriver";
String Conn_USERNAME = "bsweb";
String Conn_PASSWORD = "ProcAuto!";
String testStr = "";


String BSYS_STRING = "jdbc:microsoft:sqlserver://BART:1143;DatabaseName=BSYS";

if (TestMode){
	BSYS_STRING = "jdbc:microsoft:sqlserver://BART:1143;DatabaseName=BSYS";
	CookieName = "userLoginCookieTest";
	testStr = "Test";
}
else{
	BSYS_STRING = "jdbc:microsoft:sqlserver://NOCHOUSE:1433;DatabaseName=BSYS";		
}

%>
