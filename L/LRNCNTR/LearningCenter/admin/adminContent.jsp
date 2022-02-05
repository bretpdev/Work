<%--
	The adminContent.jsp file contains the logic for determining what should appear
	in the content area of the admin.jsp page. It is kept as a separate file to keep
	the main JSP page from getting too cluttered.
--%>

<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/functions" prefix="fn" %>

<script language="javascript">
	function GetXMLVal(XMLDoc, ValName, cbControl, tbControl) {
		if (cbControl != null) {
			var i = 0;
			if (XMLDoc.getElementsByTagName(ValName)[0] != null) {
				while (cbControl.options[i].value != XMLDoc.getElementsByTagName(ValName)[0].childNodes[0].nodeValue) {
					i = i + 1;
				}
			} 
			cbControl.selectedIndex = i;
		}
		else {
			try {
				tbControl.value = "";
				if (XMLDoc.getElementsByTagName(ValName)[0] != null) {
					tbControl.value = XMLDoc.getElementsByTagName(ValName)[0].childNodes[0].nodeValue.replace(/<br>/g,'\n');
				}
			}
			catch (e) {
			//alert(e);
			}
		}
	}
</script>

<c:if test="${!empty user}">
	<%@include file="/admin/adminNav.jsp" %>
	<div id="adminMsg"></div>
	<p>&nbsp;</p>
	
	<c:choose>        
	    <c:when test="${param.subpage == 'addTopic'}">
	    	<%@include file="/admin/actions/addTopic.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'updateTopic'}">
	    	<%@include file="/admin/actions/updateTopic.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'addProcedure'}">
	    	<%@include file="/admin/actions/addProcedure.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'updateProcedure'}">
	    	<%@include file="/admin/actions/updateProcedure.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'linkProcedure'}">
	    	<%@include file="/admin/actions/linkProcedure.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'removeLinkedProcedure'}">
	    	<%@include file="/admin/actions/removeLinkedProcedure.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'linkedTopic'}">
	    	<%@include file="/admin/actions/linkedTopic.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'removeLinkedTopic'}">
	    	<%@include file="/admin/actions/removeLinkedTopic.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'topicRisk'}">
	    	<%@include file="/admin/actions/topicRisk.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'topicPolicy'}">
	    	<%@include file="/admin/actions/topicPolicy.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'topicFlowChart'}">
	    	<%@include file="/admin/actions/topicFlowChart.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'procRisk'}">
	    	<%@include file="/admin/actions/procRisk.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'procPolicy'}">
	    	<%@include file="/admin/actions/procPolicy.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'procFlowChart'}">
	    	<%@include file="/admin/actions/procFlowChart.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'procStaff'}">
	    	<%@include file="/admin/actions/procStaff.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'addLetter'}">
	    	<%@include file="/admin/actions/addLetter.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'updateLetter'}">
	    	<%@include file="/admin/actions/updateLetter.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'addTraining'}">
	    	<%@include file="/admin/actions/addTraining.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'updateTraining'}">
	    	<%@include file="/admin/actions/updateTraining.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'addGlossary'}">
	    	<%@include file="/admin/actions/addGlossary.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'updateGlossary'}">
	    	<%@include file="/admin/actions/updateGlossary.jsp" %>
	    </c:when>
	    <c:when test="${param.subpage == 'upload'}">
	    	<%@include file="/admin/actions/upload.jsp" %>
	    </c:when>
	</c:choose>
</c:if>

