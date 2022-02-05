<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>

<%--
	If we're not displaying a search result, show the narrative for the selected topic,
	followed by a list of related topics and a list of procedures for the current topic.
--%>
<c:if test="${empty param.search}">
	<%-- Show the topic name as a header. --%>
	<h2>${param.topic}</h2>
	
	<%-- Get the narrative from the database. --%>
	<sql:query var="results">
		SELECT Narrative FROM LCTR_DAT_Topic
		WHERE Name = '${param.topic}'
	</sql:query>
	${results.rows[0].Narrative}
	<hr />
	
	<%-- Get related topics and show them as links to that topic's "narratives" page. --%>
	Other topics related to ${param.topic}:
	<sql:query var="related">
		SELECT Name FROM LCTR_DAT_Topic
		WHERE ID IN
			(
				SELECT B.ToID FROM LCTR_DAT_LinkedTopics B
				JOIN LCTR_DAT_Topic A
					ON B.ID = A.ID
				WHERE A.Name = '${param.topic}'
			)
		ORDER BY Name
	</sql:query>
	<c:forEach items="${related.rows}" var="relatedRow">
		<a href="
			<c:url value="/ElSea.jsp">
				<c:param name="page" value="narratives" />
				<c:param name="topic" value="${relatedRow.Name}" />
			</c:url>
		">
			<br />${relatedRow.Name}
		</a>
	</c:forEach>
	
	<%-- If there are no related topics, say so. --%>
	<c:if test="${empty related.rows}">
		<br />None
	</c:if>
	<hr />
	
	<%-- Get procedures belonging to this topic, and show them as links to the "procedures" page. --%>
	Procedures related to ${param.topic}:
	<sql:query var="procedures">
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
	<c:forEach items="${procedures.rows}" var="proceduresRow">
		<a class="listItem" href="
			<c:url value="/ElSea.jsp">
				<c:param name="page" value="procedures" />
				<c:param name="topic" value="${param.topic}" />
				<c:param name="procedure" value="${proceduresRow.Name}" />
			</c:url>
		">
			<br />${proceduresRow.Name}
		</a>
	</c:forEach>
	
	<%-- If there are no procedures for this topic, say so. --%>
	<c:if test="${empty procedures.rows}">
		<br />None
	</c:if>
</c:if>

<%-- For search results, show all topics, procedures, letters, and training matching the search term. --%>
<c:if test="${!empty param.search}">
	<%-- Topics --%>
	<a id="topicLink" onclick="toggleHidden('topicLink', 'topicList', 'topicLink')" href="#">
		<h3>Topics related to "${param.search}"</h3>
	</a>
	<sql:query var="topicSearch">
		SELECT * FROM LCTR_DAT_Topic
		WHERE (Name LIKE '%${param.search}%' OR Narrative LIKE '%${param.search}%' OR SearchKey LIKE '%${param.search}%')
	</sql:query>
	<span id="topicList">
		<dl>
			<c:forEach items="${topicSearch.rows}" var="searchRow">
				<%-- List the topics out in a definition list, with the term linking to that topic's "narratives" page. --%>
				<dt>
					<a class="listItem" href="
						<c:url value="/ElSea.jsp">
							<c:param name="page" value="narratives" />
							<c:param name="topic" value="${searchRow.Name}" />
						</c:url>
					">
						${searchRow.Name}
					</a>
				</dt>
				<dd>
					${searchRow.Narrative}
				</dd>
			</c:forEach>
		</dl>
	</span>
	
	<%-- If nothing matches the search term, say so. --%>
	<c:if test="${empty topicSearch.rows}">
		No results for "${param.search}"
	</c:if>
	<hr />
	
	<%-- Procedures --%>
	<a id="procedureLink" onclick="toggleHidden('procedureLink', 'procedureList', 'procedureLink')" href="#">
		<h3>Procedures related to "${param.search}"</h3>
	</a>
	<sql:query var="procedureSearch">
		SELECT * FROM LCTR_DAT_Procedures
		WHERE (Name LIKE '%${param.search}%' OR SearchKey LIKE '%${param.search}%')
	</sql:query>
	<span id="procedureList">
		<dl>
			<c:forEach items="${procedureSearch.rows}" var="searchRow">
				<%-- List the topics out in a definition list, with the term linking to that topic's "narratives" page. --%>
				<dt>
					<a class="listItem" href="
						<c:url value="/ElSea.jsp">
							<c:param name="page" value="procedures" />
							<c:param name="topic" value="${param.topic}" />
							<c:param name="procedure" value="${searchRow.Name}" />
						</c:url>
					">
						${searchRow.Name}
					</a>
				</dt>
			</c:forEach>
		</dl>
	</span>
	
	<%-- If nothing matches the search term, say so. --%>
	<c:if test="${empty procedureSearch.rows}">
		No results for "${param.search}"
	</c:if>
	<hr />
	
	<%-- Letters --%>
	<a id="letterLink" onclick="toggleHidden('letterLink', 'letterList', 'letterLink')" href="#">
		<h3>Letters related to "${param.search}"</h3>
	</a>
	<sql:query var="letterSearch">
		SELECT * FROM LCTR_DAT_Docs
		WHERE ((Name LIKE '%${param.search}%' OR SearchKey LIKE '%${param.search}%') AND Type = 'letter')
	</sql:query>
	<span id="letterList">
		<dl>
			<c:forEach items="${letterSearch.rows}" var="searchRow">
				<%-- List the topics out in a definition list, with the term linking to that topic's "narratives" page. --%>
				<dt>
					<a class="document" href="<c:url value="/docs/letter/${searchRow.Path}" />" target="_blank">${searchRow.Name}</a>
				</dt>
			</c:forEach>
		</dl>
	</span>
	
	<%-- If nothing matches the search term, say so. --%>
	<c:if test="${empty letterSearch.rows}">
		No results for "${param.search}"
	</c:if>
	<hr />
	
	<%-- Training --%>
	<a id="trainingLink" onclick="toggleHidden('trainingLink', 'trainingList', 'trainingLink')" href="#">
		<h3>Training related to "${param.search}"</h3>
	</a>
	<sql:query var="trainingSearch">
		SELECT * FROM LCTR_DAT_Docs
		WHERE ((Name LIKE '%${param.search}%' OR SearchKey LIKE '%${param.search}%') AND Type = 'training')
	</sql:query>
	<span id="trainingList">
		<dl>
			<c:forEach items="${trainingSearch.rows}" var="searchRow">
				<%-- List the topics out in a definition list, with the term linking to that topic's "narratives" page. --%>
				<dt>
					<a class="document" href="<c:url value="/docs/training/${searchRow.Path}" />" target="_blank">${searchRow.Name}</a>
				</dt>
			</c:forEach>
		</dl>
	</span>
	
	<%-- If nothing matches the search term, say so. --%>
	<c:if test="${empty trainingSearch.rows}">
		No results for "${param.search}"
	</c:if>
</c:if>

<%-- Include a search box at the end of the page. --%>
<hr />
<form action="
	<c:url value="/ElSea.jsp">
		<c:param name="page" value="narratives" />
		<c:param name="topic" value="${param.topic}" />
	</c:url>
" method="POST">
	<input type="text" name="search" value="${param.search}" />
	<input type="submit" value="Search" />
</form>
