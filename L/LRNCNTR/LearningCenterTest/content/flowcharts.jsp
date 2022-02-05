<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>

<%-- Show the flow chart for the given procedure if the user has a procedure selected. --%>
<c:if test="${!empty param.procedure}">
	<jsp:include page="/docs/flowcharts/procedure/${param.procedure}/${param.procedure}.html" />
	<%-- Get the flow chart's description from the database. --%>
	<sql:query var="result">
		SELECT A.Description FROM LCTR_DAT_FlowChart A
			INNER JOIN LCTR_DAT_Procedures B
				ON A.ID = B.ID
		WHERE A.Type = 'Procedure'
			AND B.Name = '${param.procedure}'
	</sql:query>
</c:if>

<%-- Get the flow chart for the given topic if no procedure is selected. --%>
<c:if test="${empty param.procedure}">
	<jsp:include page="/docs/flowcharts/topic/${param.topic}/${param.topic}.html" />
	<%-- Get the flow chart's description from the database. --%>
	<sql:query var="result">
		SELECT A.Description FROM LCTR_DAT_FlowChart A
			INNER JOIN LCTR_DAT_Topic B
				ON A.ID = B.ID
		WHERE A.Type = 'Topic'
			AND B.Name = '${param.topic}'
	</sql:query>
</c:if>

<%-- Show the flow chart's description. --%>
${result.rows[0].Description}
