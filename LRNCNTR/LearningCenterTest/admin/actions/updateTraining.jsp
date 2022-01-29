<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>
<%@ page import="edu.utahsbr.*" %>

<script language="javascript">
	var ajaxReqGetTopic;
	
	function loadTraining(taction) {
		document.getElementById('docSearch').value = document.getElementById('docSearch').value.replace(/\n/g,'<br>').replace(/'/g,"''");
		document.getElementById('action').value = taction;
		document.frmupdateTraining.submit() ;
	}
	
	function selectTraining() {
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP"); 
		}
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4selectTraining;
		myIdentifier=Math.round(Math.random()*10000);
		ajaxReqGetTopic.open("GET","./xml/getXML.jsp?sp=dbo.spLCTR_getDocs (" + document.getElementById('docID').value + ",Training) &uniqe=" + myIdentifier, true);
		ajaxReqGetTopic.send(null);
	}
	
	function HandlesOnReady4selectTraining() {
		var xmlDoc;
		if (ajaxReqGetTopic.readyState == 4) {
			if (ajaxReqGetTopic.status == 200) {
				//populate form elements
				xmlDoc = ajaxReqGetTopic.responseXML.documentElement;
				GetXMLVal(xmlDoc, "SearchKey", null, document.getElementById('docSearch'));
				GetXMLVal(xmlDoc, "Name", null, document.getElementById('docName'));
				GetXMLVal(xmlDoc, "Path", null, document.getElementById('docPath'));
			}
			else {
				alert("The following error occured on the server " + ajaxReqGetTopic.status + ".");
			}
		}
	}
</script>

<h3>Update Training Document</h3>      
<form action="admin.jsp?theme=$(theme)&page=main&subpage=upload" method="post" name="frmupdateTraining" ENCTYPE="multipart/form-data">
	<div id="message"></div>
	Training Document Name<br />
	<sql:query var="Training">
		SELECT ID, Name FROM LCTR_DAT_Docs WHERE type = 'Training' ORDER BY Name ASC
	</sql:query>
	<select name="docID" onchange="selectTraining()">
		<option value=""></option>
		<c:forEach items="${Training.rows}" var="record">
			<option value ="${record.ID}">${record.Name}</option>
		</c:forEach>
	</select><br />
	Training<br />
	<input name="file1" type="file" />&nbsp;&nbsp;(Select the Training document to add.)<br />
	Key Words<br />
	<textarea name="docSearch" cols="80" rows="10"></textarea><br />
	<input name="test" type="hidden" value="<%= Globals.testString() %>" />
	<input name="type" type="hidden" value="training" />
	<input name="docName" type="hidden" value="" />
	<input name="docPath" type="hidden" value="" />
	<input name="action" type="hidden" value="update" />
	
	<a href="#" onclick="loadTraining('update');">Update</a><br />
	<a href="#" onclick="loadTraining('delete');">Delete</a><br />
</form>