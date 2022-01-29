<%-- Use the CookieBean to set a cookie for the theme; good for one year. --%>
<jsp:useBean id="cookieBean" class="edu.utahsbr.CookieBean" />
<jsp:setProperty name="cookieBean" property="name" value="theme" />
<jsp:setProperty name="cookieBean" property="value" value="${param.theme}" />
<jsp:setProperty name="cookieBean" property="maxAge" value="<%= (365*24*60*60) %>" />
<jsp:setProperty name="cookieBean" property="path" value="<%= request.getContextPath() %>" />
<jsp:setProperty name="cookieBean" property="cookieHeader" value="<%= response %>" />

<%-- We must redirect to the main page rather than forward for the cookie to take effect. --%>
<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<c:redirect url="/ElSea.jsp">
	<c:param name="page" value="${param.page}" />
	<c:param name="topic" value="${param.topic}" />
	<c:if test="${!empty param.procedure}">
		<c:param name="procedure" value="${param.procedure}" />
	</c:if>
</c:redirect>
