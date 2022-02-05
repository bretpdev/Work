<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>

<%-- Show a header row from which the user can pick a letter to filter the results. --%>
<c:forTokens items="0-9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,All" delims="," var="current">
	&nbsp;<a class="header" href="
		<c:url value="/ElSea.jsp">
			<c:param name="page" value="glossary" />
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
		<c:param name="page" value="glossary" />
		<c:param name="topic" value="${param.topic}" />
	</c:url>
" method="POST">
	<input type="text" name="search" value="${param.search}" />
	<input type="submit" value="Search the glossary" />
</form>
<br />

<%-- If we're not using the search box, query for glossary terms filtered by the "filter" parameter. --%>
<c:if test="${empty param.search}">
	<sql:query var="terms">
		SELECT * FROM LCTR_DAT_Glossary
		<c:choose>
			<c:when test="${param.filter == 'All'}">
				<%-- No constraints. --%>
			</c:when>
			<c:when test="${(empty param.filter) || (param.filter == '0-9')}">
				WHERE (Term LIKE '0%' OR Term LIKE '1%' OR Term LIKE '2%' OR Term LIKE '3%' OR Term LIKE '4%' OR Term LIKE '5%' OR Term LIKE '6%' OR Term LIKE '7%' OR Term LIKE '8%' OR Term LIKE '9%')
			</c:when>
			<c:otherwise>
				WHERE Term LIKE '${param.filter}%'
			</c:otherwise>
		</c:choose>
		ORDER BY Term
	</sql:query>
	
	<%-- Display the results as a definition list. --%>
	<dl>
		<c:forEach items="${terms.rows}" var="record">
			<dt>${record.Term}</dt>
			<dd>${record.Definition}</dd>
		</c:forEach>
	</dl>
</c:if>

<%-- If we're using the search box, query for glossary terms and definitions matching the search string. --%>
<c:if test="${!empty param.search}">
	<%-- First, the terms. --%>
	<sql:query var="terms">
		SELECT * FROM LCTR_DAT_Glossary
		WHERE Term LIKE '%${param.search}%'
		ORDER BY Term
	</sql:query>
	<br />
	Glossary terms matching "${param.search}":
	<dl>
		<c:forEach items="${terms.rows}" var="record">
			<dt>${record.Term}</dt>
			<dd>${record.Definition}</dd>
		</c:forEach>
	</dl>
	<c:if test="${empty terms.rows}">
		None
	</c:if>
	<%-- Draw a line to separate the Terms results from the Definitions results. --%>
	<hr />
	<%-- Now the definitions. --%>
	<sql:query var="definitions">
		SELECT * FROM LCTR_DAT_Glossary
		WHERE Definition LIKE '%${param.search}%'
			AND Term NOT LIKE '%${param.search}%'
		ORDER BY Term
	</sql:query>
	<br />
	Glossary definitions containing "${param.search}":
	<dl>
		<c:forEach items="${definitions.rows}" var="record">
			<dt>${record.Term}</dt>
			<dd>${record.Definition}</dd>
		</c:forEach>
	</dl>
	<c:if test="${empty definitions.rows}">
		None
	</c:if>
</c:if>
