<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>

<%-- Get the policy for the given procedure if the user has a procedure selected. --%>
<c:if test="${!empty param.procedure}">
	<h2>Policy for <em>${param.procedure}</em></h2>
	<sql:query var="result">
		SELECT A.Policy FROM LCTR_DAT_Policy A
			INNER JOIN LCTR_DAT_Procedures B
				ON A.ID = B.ID
		WHERE A.Type = 'Procedure'
			AND B.Name = '${param.procedure}'
	</sql:query>
</c:if>

<%-- Get the policy for the given topic if no procedure is selected. --%>
<c:if test="${empty param.procedure}">
	<h2>Policy for <em>${param.topic}</em></h2>
	<sql:query var="result">
		SELECT A.Policy FROM LCTR_DAT_Policy A
			INNER JOIN LCTR_DAT_Topic B
				ON A.ID = B.ID
		WHERE A.Type = 'Topic'
			AND B.Name = '${param.topic}'
	</sql:query>
</c:if>

<%-- Display the results --%>
${result.rows[0].Policy}
