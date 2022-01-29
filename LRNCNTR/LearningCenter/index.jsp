<?xml version="1.0" encoding="ISO-8859-1" ?>
<%@ page language="java" contentType="text/html; charset=ISO-8859-1"
    pageEncoding="ISO-8859-1"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
	<title>Learning Center</title>
</head>

<body>
	<%-- Start the application by calling ElSea.jsp with an argument to bring up the main page. --%>
	<c:redirect url="/ElSea.jsp">
		<c:param name="page" value="main" />
		<c:param name="topic" value="none" />
	</c:redirect>
</body>
</html>