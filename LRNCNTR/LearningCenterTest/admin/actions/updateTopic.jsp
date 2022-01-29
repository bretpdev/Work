<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>

<script language="javascript">
	var ajaxReqGetTopic;
	
	function selectTopic() {
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e){
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
		}
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4selectTopic;
		myIdentifier=Math.round(Math.random()*10000);
		ajaxReqGetTopic.open("GET","./xml/getTopic.jsp?topicID=" + document.getElementById('topicName').value + "&uniqe=" + myIdentifier,true);
		ajaxReqGetTopic.send(null);
	}
	
	function HandlesOnReady4selectTopic() {
		var xmlDoc;
		if (ajaxReqGetTopic.readyState == 4) {
			if (ajaxReqGetTopic.status == 200) {
				//populate form elements
				xmlDoc = ajaxReqGetTopic.responseXML.documentElement;
				GetXMLVal(xmlDoc, "Narrative", null, document.getElementById('topicNarrative'));
				GetXMLVal(xmlDoc, "SearchKey", null, document.getElementById('topicKey'));
			}
			else {
				alert("The following error occured on the server " + ajaxReqGetTopic.status + ".");
			}
		}
	}
	
	function updateTopic() {
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
		}
		var updateSQL = "UPDATE LCTR_DAT_Topic SET Narrative = '" + document.getElementById('topicNarrative').value.replace(/\n/g,'<br>').replace(/'/g,"''") + "', SearchKey = '" + document.getElementById('topicKey').value.replace(/'/g,"''")  + "' WHERE ID = " + document.getElementById('topicName').value;
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
		ajaxReqGetTopic.open("GET","./xml/insertUpdate.jsp?sql=" + updateSQL, true);
		ajaxReqGetTopic.send(null);
		document.frmeditTopic.reset();
	}
	
	function deleteTopic() {
		if (!(confirm("Are you sure you want to delete this topic?"))) {
			return false;
		}
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
		}
		//var updateSQL = "delete from LCTR_DAT_Topic where ID = " + document.getElementById('topicName').value;
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
		ajaxReqGetTopic.open("GET","./xml/deleteTopic.jsp?id=" + document.getElementById('topicName').value, true);
		ajaxReqGetTopic.send(null);
		var el = document.getElementById("topicName");
		el.remove(el.selectedIndex);
		document.frmeditTopic.reset();
	}
	
	function HandlesOnReady4() {
		var xmlDoc;
		if (ajaxReqGetTopic.readyState == 4) {
			if (ajaxReqGetTopic.status == 200) {//Success
				xmlDoc = ajaxReqGetTopic.responseXML.documentElement;
				document.getElementById('adminMsg').innerHTML = xmlDoc.getElementsByTagName("Status")[0].childNodes[0].nodeValue;
			}
			else {//Failed
				xmlDoc = ajaxReqGetTopic.responseXML.documentElement;
				alert("The following error occured on the server " + ajaxReqGetTopic.status + ".");
				document.getElementById('adminMsg').innerHTML = xmlDoc.getElementsByTagName("Status")[0].childNodes[0].nodeValue;
			}
		}
	}
</script>

<h3>Update Topic</h3>
<form action="admin.jsp?theme=$(theme)&page=main" method="post" name="frmeditTopic">
	Topic Name<br />
	<sql:query var="topics">
		SELECT ID, Name FROM LCTR_DAT_Topic ORDER BY Name ASC
	</sql:query>
	<select name="topicName" onchange="selectTopic()">
		<c:forEach items="${topics.rows}" var="record">
			<option value ="${record.ID}">${record.Name}</option>
		</c:forEach>
	</select>
	<br />
	Narrative<br />
	<textarea name="topicNarrative" cols="80" rows="10"></textarea>
	<br />
	Search Key<br />
	<textarea name="topicKey" cols="80" rows="2"></textarea>
	<br />
	<a href="#" onclick="updateTopic()">Update</a><br /><br />
	<a href="#" onclick="deleteTopic()">Delete</a>
</form>
