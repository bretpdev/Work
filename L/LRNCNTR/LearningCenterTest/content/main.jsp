<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>

<h1>Welcome to the UHEAA Learning Center!<br /></h1>
<h3>
	Use the search box to find topics, procedures, letters, or training material,
	or choose a topic from the drop-down list.<br />
	You can also browse around using the navigation buttons on the left.<br />
</h3>

<%-- Query the default database (defined in web.xml) for our topic list. --%>
<sql:query var="topics">
  	SELECT * FROM LCTR_DAT_Topic
  	ORDER BY Name
</sql:query>

<%-- Provide a search box. --%>
<form action="
	<c:url value="/ElSea.jsp">
		<c:param name="page" value="narratives" />
		<c:param name="topic" value="${param.topic}" />
	</c:url>
" method="POST">
	<input type="text" name="search" />
	<input type="submit" value="Search" />
	&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
	<select name="topicName" onchange="selectTopic()">
		<option value="none">--- Select a topic ---</option>
		<c:forEach items="${topics.rows}" var="record">
	    <option value="${record.Name}">${record.Name}</option>
	  </c:forEach>
	</select>
</form>

<%-- The following code block was originally used to display a table of topics. We don't do that any more, so it's going away.	
<c:set var="firstIndex" value="0" />

<table cellspacing=6>
<c:forEach items="${topics.rows}" begin="0" step="3">
	<tr>
	<c:forEach begin="${firstIndex}" end="${firstIndex + 2}" var="record" items="${topics.rows}">
		<a href="
			<c:url value="/ElSea.jsp">
				<c:param name="page" value="narratives" />
				<c:param name="topic" value="${record.Name}" />
			</c:url>
		">
			<td>${record.Name}</td>
		</a>
	</c:forEach>
	</tr>
	
	<c:set var="firstIndex" value="${firstIndex + 3}" />
</c:forEach>
</table>
--%>