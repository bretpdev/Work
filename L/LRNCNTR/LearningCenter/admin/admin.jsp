<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">

<%@ page contentType="text/html" import="java.sql.*" %>
<%@ page import="com.oreilly.servlet.MultipartRequest" %>
<%@ page import="java.io.*" %>
<%@ page import="java.util.*" %>
<%@ page import="java.sql.ResultSet" %>
<%@ page import="java.sql.Statement" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>

<%-- Set the theme to the one specified in the cookie, or default to "turtle" if no cookie is set. --%>
<c:choose>
	<c:when test="${empty cookie.theme.value}">
		<c:set var="theme" value="turtle" scope="session" />
        
	</c:when>
	<c:otherwise>
		<c:set var="theme" value="${cookie.theme.value}" scope="session" />
	</c:otherwise>
</c:choose>

<head>
	<title>UHEAA Knowledge Base</title>
	<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
	<link rel="stylesheet" type="text/css" href="/LearningCenter/css/${theme}/general.css" />
	<link rel="stylesheet" type="text/css" href="/LearningCenter/css/${theme}/${param.page}.css" />
</head>

<body>
	<%-- Different themes need the content loaded at different times.
		For themes that need it loaded early, we do so here. --%>
	<c:if test="${theme == 'beach' || theme == 'volcano'}">
		<div id="content">
			<%-- The content area is loaded at compile time from the content.jsp file
				for the same reasons as the navigation area. --%>
			<%@include file="adminContent.jsp" %>
		</div>
	</c:if>
	<div id="background">
		<img src="/LearningCenter/images/${theme}/background.jpg" />
	</div>
	<div id="nav">
		<%-- The navigation area takes a bit of logic to create a proper presentation.
			Rather than clutter this file, we use an include directive to load the logic
			from a separate JSP at compile time. --%>
		<%@include file="/navigation.jsp" %>
	</div>
	<%-- For themes that need the content loaded last, we do so here. --%>
	<c:if test="${theme == 'turtle'}">
		<div id="content">
			<%-- The content area is loaded at compile time from the content.jsp file
				for the same reasons as the navigation area. --%>
			<%@include file="adminContent.jsp" %>
		</div>
	</c:if>
</body>
</html>
