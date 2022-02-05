<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>

<%-- Display a page header featuring the topic name. --%>
<h2>Procedures for ${param.topic}</h2>

<%-- Get the list of procedures for this topic. --%>
<sql:query var="list">
		SELECT Name FROM LCTR_DAT_Procedures
		WHERE ID IN
			(
				SELECT ProcedureID FROM LCTR_DAT_TopicProcedure B
				JOIN LCTR_DAT_Topic C
					ON B.TopicID = C.ID
				WHERE C.Name = '${param.topic}'
			)
		ORDER BY Name
</sql:query>

<%-- Show the list of procedures as links to the "procedures" page. --%>
<%--
	TODO: Once the list becomes more realistic, we'll probably want to
	only show it if there's more than one procedure for the given topic.
	Otherwise, if there's only one procedure, maybe jump straight to it;
	if there are none, provide a message to that effect.
--%>
<c:forEach items="${list.rows}" var="listRow">
	<a href="
		<c:url value="/ElSea.jsp">
			<c:param name="page" value="procedures" />
			<c:param name="topic" value="${param.topic}" />
			<c:param name="procedure" value="${listRow.Name}" />
		</c:url>
	">
		<br />${listRow.Name}
	</a>
</c:forEach>
