<%--
	The navigation.jsp file contains the logic for populating the navigation area
	with the appropriate buttons in the appropriate states (enabled, disabled, selected).
--%>

<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>

<%-- Load the "navbar" image if the theme has one. --%>
<c:if test="${theme == 'turtle'}">
	<img class="navbar" src="<c:url value="/images/${theme}/navbar.jpg" />" />
</c:if>

<%-- Load the appropriate state (image and whether to hyperlink) for each button, depending on the "page" parameter. --%>
<%-- Main --%>
<c:choose>
	<c:when test="${param.page == 'main'}">
        <a href="
			<c:url value="/ElSea.jsp">
				<c:param name="page" value="main" />
				<c:param name="topic" value="${param.topic}" />
			</c:url>
		">
			<img class="buttonMain" src="<c:url value="/images/${theme}/buttonMainSelected.png" />" />
		</a>
	</c:when>
	<c:otherwise>
		<a href="
			<c:url value="/ElSea.jsp">
				<c:param name="page" value="main" />
				<c:param name="topic" value="${param.topic}" />
			</c:url>
		">
			<img class="buttonMain" src="<c:url value="/images/${theme}/buttonMainEnabled.png" />" />
		</a>
	</c:otherwise>
</c:choose>

<%-- Glossary --%>
<c:choose>
	<c:when test="${param.page == 'glossary'}">
		<img class="buttonGlossary" src="<c:url value="/images/${theme}/buttonGlossarySelected.png" />" />
	</c:when>
	<c:otherwise>
		<a href="
			<c:url value="/ElSea.jsp">
				<c:param name="page" value="glossary" />
				<c:param name="topic" value="${param.topic}" />
			</c:url>
		">
			<img class="buttonGlossary" src="<c:url value="/images/${theme}/buttonGlossaryEnabled.png" />" />
		</a>
	</c:otherwise>
</c:choose>

<%-- Letters --%>
<c:choose>
	<c:when test="${param.page == 'letters'}">
		<img class="buttonLetters" src="<c:url value="/images/${theme}/buttonLettersSelected.png" />" />
	</c:when>
	<c:otherwise>
		<a href="
			<c:url value="/ElSea.jsp">
				<c:param name="page" value="letters" />
				<c:param name="topic" value="${param.topic}" />
			</c:url>
		">
			<img class="buttonLetters" src="<c:url value="/images/${theme}/buttonLettersEnabled.png" />" />
		</a>
	</c:otherwise>
</c:choose>

<%-- Training --%>
<c:choose>
	<c:when test="${param.page == 'training'}">
    <img class="buttonTraining" src="<c:url value="/images/${theme}/buttonTrainingSelected.png" />" />
 	</c:when>
 	<c:otherwise>
		<a href="
			<c:url value="/ElSea.jsp">
				<c:param name="page" value="training" />
				<c:param name="topic" value="${param.topic}" />
			</c:url>
		">
			<img class="buttonTraining" src="<c:url value="/images/${theme}/buttonTrainingEnabled.png" />" />
		</a>
	</c:otherwise>
</c:choose>

<%-- Narratives --%>
<c:choose>
	<c:when test="${param.page == 'narratives'}">
    <img class="buttonNarratives" src="<c:url value="/images/${theme}/buttonNarrativesSelected.png" />" />
  </c:when>
  <c:when test="${param.page == 'search' || param.page == 'proceduresList' || param.page == 'procedures' || param.page == 'staff' || param.page == 'flowcharts' || param.page == 'policy' || param.page == 'risks'}">
    <a href="
    	<c:url value="/ElSea.jsp">
    		<c:param name="page" value="narratives" />
    		<c:param name="topic" value="${param.topic}" />
  		</c:url>
		">
    	<img class="buttonNarratives" src="<c:url value="/images/${theme}/buttonNarrativesEnabled.png" />" />
  	</a>
  </c:when>
  <c:otherwise>
  	<%-- Load the "disabled" image if the theme supports it. --%>
  	<c:if test="${theme == 'turtle' || theme == 'volcano'}">
			<img class="buttonNarratives" src="<c:url value="/images/${theme}/buttonNarrativesDisabled.png" />" />
		</c:if>
	</c:otherwise>
</c:choose>

<%-- Procedures --%>
<c:choose>
	<%-- If the page is actually "proceduresList" then the Procedures button is selected. --%>
	<c:when test="${param.page == 'proceduresList'}">
    <img class="buttonProcedures" src="<c:url value="/images/${theme}/buttonProceduresSelected.png" />" />
  </c:when>
  <%-- If we're on an actual procedure page, enable the Procedures button to allow the user to return to the list of procedures. --%>
  <c:when test="${param.page == 'procedures'}">
    <a href="
    	<c:url value="/ElSea.jsp">
    		<c:param name="page" value="proceduresList" />
    		<c:param name="topic" value="${param.topic}" />
  		</c:url>
	">
    	<img class="buttonProcedures" src="<c:url value="/images/${theme}/buttonProceduresSelected.png" />" />
  	</a>
  </c:when>
  <c:when test="${param.page == 'search' || param.page == 'narratives' || param.page == 'staff' || param.page == 'flowcharts' || param.page == 'policy' || param.page == 'risks'}">
  	<%-- If we've selected a procedure before, return to that procedure. --%>
  	<c:if test="${!empty param.procedure}">
	    <a href="
	    	<c:url value="/ElSea.jsp">
	    		<c:param name="page" value="procedures" />
	    		<c:param name="topic" value="${param.topic}" />
	    		<c:param name="procedure" value="${param.procedure}" />
	  		</c:url>
		">
	    	<img class="buttonProcedures" src="<c:url value="/images/${theme}/buttonProceduresEnabled.png" />" />
	  	</a>
		</c:if>
  	<%-- Otherwise, show the list of procedures for the selected topic. --%>
  	<c:if test="${empty param.procedure}">
	    <a href="
	    	<c:url value="/ElSea.jsp">
	    		<c:param name="page" value="proceduresList" />
	    		<c:param name="topic" value="${param.topic}" />
	  		</c:url>
		">
	    	<img class="buttonProcedures" src="<c:url value="/images/${theme}/buttonProceduresEnabled.png" />" />
	  	</a>
		</c:if>
  </c:when>
  <c:otherwise>
  	<%-- Load the "disabled" image if the theme supports it. --%>
  	<c:if test="${theme == 'turtle' || theme == 'volcano'}">
			<img class="buttonProcedures" src="<c:url value="/images/${theme}/buttonProceduresDisabled.png" />" />
		</c:if>
	</c:otherwise>
</c:choose>

<%-- Policy --%>
<c:choose>
	<c:when test="${param.page == 'policy'}">
		<img class="buttonPolicy" src="<c:url value="/images/${theme}/buttonPolicySelected.png" />" />
	</c:when>
	<c:when test="${param.page == 'search' || param.page == 'narratives' || param.page == 'proceduresList' || param.page == 'procedures' || param.page == 'staff' || param.page == 'flowcharts' || param.page == 'risks'}">
    <a href="
    	<c:url value="/ElSea.jsp">
    		<c:param name="page" value="policy" />
    		<c:param name="topic" value="${param.topic}" />
    		<%-- Include the "procedure" parameter if it's set. --%>
    		<c:if test="${!empty param.procedure}">
    			<c:param name="procedure" value="${param.procedure}" />
    		</c:if>
  		</c:url>
	">
    	<img class="buttonPolicy" src="<c:url value="/images/${theme}/buttonPolicyEnabled.png" />" />
  	</a>
  </c:when>
  <c:otherwise>
  	<%-- Load the "disabled" image if the theme supports it. --%>
  	<c:if test="${theme == 'turtle' || theme == 'volcano'}">
			<img class="buttonPolicy" src="<c:url value="/images/${theme}/buttonPolicyDisabled.png" />" />
		</c:if>
	</c:otherwise>
</c:choose>

<%-- Responsible Staff --%>
<c:choose>
	<c:when test="${param.page == 'staff'}">
		<img class="buttonStaff" src="<c:url value="/images/${theme}/buttonStaffSelected.png" />" />
	</c:when>
	<c:when test="${param.page == 'search' || param.page == 'narratives' || param.page == 'proceduresList' || param.page == 'procedures' || param.page == 'flowcharts' || param.page == 'policy' || param.page == 'risks'}">
    <a href="
    	<c:url value="/ElSea.jsp">
    		<c:param name="page" value="staff" />
    		<c:param name="topic" value="${param.topic}" />
    		<c:param name="procedure" value="${param.procedure}" />
  		</c:url>
	">
    	<img class="buttonStaff" src="<c:url value="/images/${theme}/buttonStaffEnabled.png" />" />
  	</a>
  </c:when>
  <c:otherwise>
  	<%-- Load the "disabled" image if the theme supports it. --%>
  	<c:if test="${theme == 'turtle' || theme == 'volcano'}">
			<img class="buttonStaff" src="<c:url value="/images/${theme}/buttonStaffDisabled.png" />" />
		</c:if>
	</c:otherwise>
</c:choose>

<%-- Flow Charts --%>
<c:choose>
	<c:when test="${param.page == 'flowcharts'}">
		<img class="buttonFlowChart" src="<c:url value="/images/${theme}/buttonFlowChartsSelected.png" />" />
	</c:when>
	<c:when test="${param.page == 'search' || param.page == 'narratives' || param.page == 'proceduresList' || param.page == 'procedures' || param.page == 'staff' || param.page == 'policy' || param.page == 'risks'}">
    <a href="
    	<c:url value="/ElSea.jsp">
    		<c:param name="page" value="flowcharts" />
    		<c:param name="topic" value="${param.topic}" />
    		<%-- Include the "procedure" parameter if it's set. --%>
    		<c:if test="${!empty param.procedure}">
    			<c:param name="procedure" value="${param.procedure}" />
    		</c:if>
  		</c:url>
	">
    	<img class="buttonFlowChart" src="<c:url value="/images/${theme}/buttonFlowChartsEnabled.png" />" />
  	</a>
  </c:when>
  <c:otherwise>
  	<%-- Load the "disabled" image if the theme supports it. --%>
  	<c:if test="${theme == 'turtle' || theme == 'volcano'}">
			<img class="buttonFlowChart" src="<c:url value="/images/${theme}/buttonFlowChartsDisabled.png" />" />
		</c:if>
	</c:otherwise>
</c:choose>

<%-- Risks --%>
<c:choose>
	<c:when test="${param.page == 'risks'}">
		<img class="buttonRisks" src="<c:url value="/images/${theme}/buttonRisksSelected.png" />" />
	</c:when>
	<c:when test="${param.page == 'search' || param.page == 'narratives' || param.page == 'proceduresList' || param.page == 'procedures' || param.page == 'staff' || param.page == 'flowcharts' || param.page == 'policy'}">
    <a href="
    	<c:url value="/ElSea.jsp">
    		<c:param name="page" value="risks" />
    		<c:param name="topic" value="${param.topic}" />
    		<%-- Include the "procedure" parameter if it's set. --%>
    		<c:if test="${!empty param.procedure}">
    			<c:param name="procedure" value="${param.procedure}" />
    		</c:if>
  		</c:url>
	">
    	<img class="buttonRisks" src="<c:url value="/images/${theme}/buttonRisksEnabled.png" />" />
  	</a>
  </c:when>
  <c:otherwise>
  	<%-- Load the "disabled" image if the theme supports it. --%>
  	<c:if test="${theme == 'turtle' || theme == 'volcano'}">
			<img class="buttonRisks" src="<c:url value="/images/${theme}/buttonRisksDisabled.png" />" />
		</c:if>
	</c:otherwise>
</c:choose>

<%-- Load the "endcap" image, which is typically just a nice way to complete the navigation area. --%>
<img class="endcap" src="<c:url value="/images/${theme}/endcap.jpg" />" />

<%-- Provide a search box. --%>
<form action="
	<c:url value="/ElSea.jsp">
		<c:param name="page" value="narratives" />
		<c:param name="topic" value="${param.topic}" />
	</c:url>
" method="POST">
	<input type="text" name="search" />
	<input type="submit" value="Search" />
</form>

<%-- Provide a link to the page that allows users to choose a theme. --%>
<a class="selectTheme" href="
	<c:url value="/SelectTheme.jsp">
		<c:param name="page" value="${param.page}" />
		<c:param name="topic" value="${param.topic}" />
		<c:if test="${!empty param.procedure}">
			<c:param name="procedure" value="${param.procedure}" />
		</c:if>
	</c:url>
">
	Change Theme
</a>

<%-- Provide a link to the admin sign-in page. --%>
<c:choose>
	<c:when test="${initParam['TestMode'] == 'True'}">
		<c:choose>
			<c:when test="${empty cookie.userLoginCookieTest.value}">
				<c:set var="user" value="" scope="session" />
			</c:when>
			<c:otherwise>
				<c:set var="usert" value="${cookie.userLoginCookieTest.value}" scope="request" />
                <sql:query var="getName">
                    SELECT FirstName, LastName FROM SYSA_LST_Users A INNER JOIN GENR_REF_BU_Agent_Xref B ON A.WindowsUserName = B.WindowsUserID WHERE A.WindowsUserName = '${usert}' AND B.BusinessUnit IN ('Operations Support', 'Systems Support', 'Process Automation') AND B.Role = 'Member Of'
                </sql:query>
                <c:if test="${!empty getName.rows}">
	                <c:set var="fname" value="${getName.rows[0].FirstName}" scope="session" />
	                <c:set var="lname" value="${getName.rows[0].LastName}" scope="session" />
			    	<a class="selectAdmin" href="/LearningCenterTest/admin/admin.jsp?theme=${theme}&page=main">
						Administrator
					</a>
					<c:set var="user" value="${cookie.userLoginCookieTest.value}" scope="session" />
                </c:if>
			</c:otherwise>
		</c:choose>
	</c:when>
	<c:otherwise>
		<c:choose>
			<c:when test="${empty cookie.userLoginCookie.value}">
				<c:set var="user" value="" scope="session" />
			</c:when>
			<c:otherwise>
            	<c:set var="usert" value="${cookie.userLoginCookie.value}" scope="session" />
            	<c:set var="businessUnits" value="'Operations Support'" scope="page" />
            	<sql:query var="getName">
                    SELECT FirstName, LastName FROM SYSA_LST_Users A INNER JOIN GENR_REF_BU_Agent_Xref B ON A.WindowsUserName = B.WindowsUserID WHERE A.WindowsUserName = '${usert}' AND B.BusinessUnit = 'Operations Support' AND B.Role = 'Member Of'
                </sql:query>
                <c:if test="${!empty getName.rows}">
	                <c:set var="fname" value="${getName.rows[0].FirstName}" scope="session" />
	                <c:set var="lname" value="${getName.rows[0].LastName}" scope="session" />
			    	<a class="selectAdmin" href="/LearningCenter/admin/admin.jsp?page=main">
						Administrator
					</a>
					<c:set var="user" value="${cookie.userLoginCookie.value}" scope="session" />
                </c:if>
			</c:otherwise>
    	</c:choose>
	</c:otherwise>
</c:choose>
