<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">


<%@ page contentType="text/html; charset=UTF-8" %>
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
	<script language='javascript' type='text/javascript'>
		function toggleHidden(expander, target, tracker) {
			var expanderControl = document.getElementById(expander);
			var targetControl = document.getElementById(target);
			if (expanderControl != null && targetControl != null) {
				if (!typeof(document.getElementById) == "undefined") {
					return;
				}
				if (typeof(window.event) != "undefined" && typeof(window.event.returnValue) != "undefined") 	{
					window.event.returnValue = false;
				}
				if (targetControl.style.display == "none") {
					targetControl.style.display = "";
				}
				else {
					targetControl.style.display = "none";
				}
				return false;
			}
		}
		
		function selectTopic() {
			window.location="ElSea.jsp?page=narratives&topic="+document.getElementById('topicName').value;
	    }
	</script>	

	<title>UHEAA Learning Center</title>
	<link rel="stylesheet" type="text/css" href="<c:url value="/css/${theme}/general.css" />" />
	<link rel="stylesheet" type="text/css" href="<c:url value="/css/${theme}/${param.page}.css" />" />
	<link rel="icon" href="LC.ico" type="image/ico" />
	<link rel="SHORTCUT ICON" href="LC.ico" />
</head>

<body>
	<%-- Different themes need the content loaded at different times.
		For themes that need it loaded early, we do so here. --%>
	<c:if test="${theme == 'beach' || theme == 'volcano'}">
		<div id="content">
			<%-- The content area is loaded at run time for the same reasons as the navigation area. --%>
			<jsp:include page="/content/${param.page}.jsp" />
		</div>
	</c:if>
	<div id="background">
		<img src="<c:url value='/images/${theme}/background.jpg' />" />
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
			<%-- The content area is loaded at run time for the same reasons as the navigation area. --%>
			<jsp:include page="/content/${param.page}.jsp" />
		</div>
	</c:if>
</body>
</html>
