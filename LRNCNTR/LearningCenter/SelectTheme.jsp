<?xml version="1.0" encoding="ISO-8859-1" ?>
<%@ page language="java" contentType="text/html; charset=ISO-8859-1"
    pageEncoding="ISO-8859-1"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
	<title>Learning Center Themes</title>
	<style type="text/css">
		td
		{
			padding-left: 2em;
			padding-right: 2em;
			font: 200% Times;
		}
	</style>
</head>
<body>
	<h1>Select a theme&nbsp;</h1>
	<h3>(requires cookies to be enabled)</h3>
	<table>
		<tr>
			<td>
				<a href="
					<c:url value="/setCookie.jsp">
						<c:param name="page" value="${param.page}" />
						<c:param name="topic" value="${param.topic}" />
						<c:if test="${!empty param.procedure}">
							<c:param name="procedure" value="${param.procedure}" />
						</c:if>
						<c:param name="theme" value="beach" />
					</c:url>
				">
					Beach
				</a>
			</td>
			<td>
				<a href="
					<c:url value="/setCookie.jsp">
						<c:param name="page" value="${param.page}" />
						<c:param name="topic" value="${param.topic}" />
						<c:if test="${!empty param.procedure}">
							<c:param name="procedure" value="${param.procedure}" />
						</c:if>
						<c:param name="theme" value="beach" />
					</c:url>
				">
					<img src="<c:url value="/images/beach.jpg" />" />
				</a>
			</td>
		</tr>
		<tr>
			<td>
				<a href="
					<c:url value="/setCookie.jsp">
						<c:param name="page" value="${param.page}" />
						<c:param name="topic" value="${param.topic}" />
						<c:if test="${!empty param.procedure}">
							<c:param name="procedure" value="${param.procedure}" />
						</c:if>
						<c:param name="theme" value="turtle" />
					</c:url>
				">
					Turtles
				</a>
			</td>
			<td>
				<a href="
					<c:url value="/setCookie.jsp">
						<c:param name="page" value="${param.page}" />
						<c:param name="topic" value="${param.topic}" />
						<c:if test="${!empty param.procedure}">
							<c:param name="procedure" value="${param.procedure}" />
						</c:if>
						<c:param name="theme" value="turtle" />
					</c:url>
				">
					<img src="<c:url value="/images/turtle.jpg" />" />
				</a>
			</td>
		</tr>
        <tr>
			<td>
				<a href="
					<c:url value="/setCookie.jsp">
						<c:param name="page" value="${param.page}" />
						<c:param name="topic" value="${param.topic}" />
						<c:if test="${!empty param.procedure}">
							<c:param name="procedure" value="${param.procedure}" />
						</c:if>
						<c:param name="theme" value="volcano" />
					</c:url>
				">
					Volcano
				</a>
			</td>
			<td>
				<a href="
					<c:url value="/setCookie.jsp">
						<c:param name="page" value="${param.page}" />
						<c:param name="topic" value="${param.topic}" />
						<c:if test="${!empty param.procedure}">
							<c:param name="procedure" value="${param.procedure}" />
						</c:if>
						<c:param name="theme" value="volcano" />
					</c:url>
				">
					<img src="<c:url value="/images/volcano.jpg" />" />
				</a>
			</td>
		</tr>
	</table>
</body>
</html>