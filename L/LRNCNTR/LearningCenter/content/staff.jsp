<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>

<%--
	Responsible Staff is specific to a procedure, so if there's no procedure selected,
	show a list of procedures related to the current topic.
--%>
<c:if test="${empty param.procedure}">
	<h3>Select a procedure for <em>${param.topic}</em> to see the staff responsible for that procedure.</h3>
	
	<%-- Get our list of procedures from the database. --%>
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
	
	<%-- Have the results link back to this page, with the "procedure" parameter set. --%>
	<c:forEach items="${list.rows}" var="listRow">
		<a href="
			<c:url value="/ElSea.jsp">
				<c:param name="page" value="staff" />
				<c:param name="topic" value="${param.topic}" />
				<c:param name="procedure" value="${listRow.Name}" />
			</c:url>
		">
			<br />${listRow.Name}
		</a>
	</c:forEach>
</c:if>

<%--
	If the "procedure" parameter is already set (either because this page set it above
	or because the user is coming from a page that sets it), get the responsible staff.
--%>
<c:if test="${!empty param.procedure}">
	<jsp:include page="/docs/staff/${param.procedure}.html" />
</c:if>
<br><br>
<a href="
    <c:url value="/ElSea.jsp">
        <c:param name="page" value="BUTree" />
        <c:param name="topic" value="${param.topic}" />
    </c:url>
">
    Business Unit Tree
</a>