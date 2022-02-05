<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>

<%-- Show a header row from which the user can pick a letter to filter the results. --%>
<c:forTokens items="0-9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,All" delims="," var="current">
	&nbsp;<a class="header" href="
		<c:url value="/ElSea.jsp">
			<c:param name="page" value="training" />
			<c:param name="topic" value="${param.topic}" />
			<c:param name="filter" value="${current}" />
		</c:url>
	">
		${current}
	</a>
</c:forTokens>

<%-- Include a search box at the end of the header row. --%>
<br />
<form action="
	<c:url value="/ElSea.jsp">
		<c:param name="page" value="training" />
		<c:param name="topic" value="${param.topic}" />
	</c:url>
" method="POST">
	<input type="text" name="search" value="${param.search}" />
	<input type="submit" value="Search" />
</form>
<br />

<%-- Query for training documents, filtered by the "search" or "filter" parameter. --%>
<sql:query var="docs">
	SELECT Name, Path FROM LCTR_DAT_Docs
	WHERE Type = 'Training'
	<c:choose>
		<c:when test="${!empty param.search}">
			AND (SearchKey LIKE '%${param.search}%' OR Name LIKE '%${param.search}%')
		</c:when>
		<c:when test="${param.filter == 'All'}">
			<%-- No constraints. --%>
		</c:when>
		<c:when test="${(empty param.filter) || (param.filter == '0-9')}">
			AND (Name LIKE '0%' OR Name LIKE '1%' OR Name LIKE '2%' OR Name LIKE '3%' OR Name LIKE '4%' OR Name LIKE '5%' OR Name LIKE '6%' OR Name LIKE '7%' OR Name LIKE '8%' OR Name LIKE '9%')
		</c:when>
		<c:otherwise>
			AND Name LIKE '${param.filter}%'
		</c:otherwise>
	</c:choose>
	ORDER BY Name
</sql:query>

<%-- Show the results as links to the documents. --%>
<c:forEach items="${docs.rows}" var="record">
	<br /><a class="document" href="<c:url value="/docs/training/${record.Path}" />" target="_blank">${record.Name}</a>
</c:forEach>
