<?xml version="1.0" encoding="ISO-8859-1" ?>
<%@ page isErrorPage="true" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Oh no!</title>
	<style type="text/css">
		body {background-color: black;}
		h1 {font-family: "chiller"; font-size: 300%; color: red;}
		h3 {color: yellow;}
		a:link, a:visited {color: green;}
	</style>
</head>
<body>
	<h1>Ack! That didn't work.</h1>
	<h3>
		Something went wrong with the page you were trying to get to.<br />
		The site administrator has been notified of the problem.<br /><br />
		Until it gets fixed, I invite you to
		<a href="<c:url value="/index.jsp" />">return to the main page</a><br />
		and continue to enjoy the rest of UHEAA's Learning Center.
	</h3>
	
	<%
		//Define the arguments for sendmail.
		String from = "Learning Center";
		String to = "dbeattie@utahsbr.edu;tpacker@utahsbr.edu";
		String cc = "";
		String bcc = "";
		String subject = "Error: " + exception.getMessage();
		String body =
		    "Requested URI: " + request.getRequestURI() + "\n"
		    + "\nPage variables from the end-user area:\n"
		    + "Page name: " + request.getParameter("page") + "\n"
		    + "Topic: " + request.getParameter("topic") + "\n"
		    + "Procedure: " + request.getParameter("procedure") + "\n"
		    + "\nPage variables from the admin area:\n"
		    + "\nCaused by: " + exception.getCause() + "\n"
		    ;
		String localfile = "";
		String attachName = "";
		//Send an e-mail using the above arguments.
		SMTP.sendmail.sendmail(from, to, cc, bcc, subject, body, localfile, attachName);
		
	%>
</body>
</html>
