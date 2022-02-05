<script language="javascript">
	var ajaxReqGetTopic;
	
	function selectTopic() {
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP"); 
		}
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4selectPolicy;
		myIdentifier=Math.round(Math.random()*10000);
		ajaxReqGetTopic.open("GET","./xml/getRiskPolicy.jsp?ID=" + document.getElementById('procedureName').value + "&type=Procedure&uniqe=" + myIdentifier, true);
		ajaxReqGetTopic.send(null);
	}
	
	function HandlesOnReady4selectPolicy() {
		var xmlDoc;
		if (ajaxReqGetTopic.readyState == 4) {
			if (ajaxReqGetTopic.status == 200) {
				//populate form elements
				xmlDoc = ajaxReqGetTopic.responseXML.documentElement;
				GetXMLVal(xmlDoc, "Policy", null, document.getElementById('topicPolicy'));
			}
			else {
				alert("The following error occured on the server " + ajaxReqGetTopic.status + ".");
			}
		}
	}
	
	function updatePolicy() {
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
		}
		var insertSQL = "INSERT INTO LCTR_DAT_Policy (ID, Type, Policy) VALUES (" + document.getElementById('procedureName').value + ",'Procedure','" + document.getElementById('topicPolicy').value.replace(/\n/g,'<br>').replace(/'/g,"''") + "')";
		var deleteSQL = "DELETE FROM LCTR_DAT_Policy WHERE ID = " + document.getElementById('procedureName').value + " AND Type = 'Procedure'";
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
		ajaxReqGetTopic.open("GET","./xml/addRiskPolicy.jsp?isql=" + insertSQL + "&dsql=" + deleteSQL,true);
		ajaxReqGetTopic.send(null);
		document.frmTopicPolicy.reset();
	}
	
	function deletePolicy() {
		if (!(confirm("Are you sure you want to delete this risk?"))) {
			return false;
		}
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
		}
		var deleteSQL = "DELETE FROM LCTR_DAT_Policy WHERE ID = " + document.getElementById('procedureName').value + " AND Type = 'Procedure'";
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
		ajaxReqGetTopic.open("GET","./xml/insertUpdate.jsp?sql=" + deleteSQL, true);
		ajaxReqGetTopic.send(null);
		document.frmTopicPolicy.reset() ;
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

<h3>Update Procedure Policy</h3>
<form action="admin.jsp?theme=$(theme)&page=main" method="post" name="frmTopicPolicy">
	Procedure Name<br />
	<sql:query var="topics">
		SELECT ID, Name FROM LCTR_DAT_Procedures ORDER BY Name ASC
	</sql:query>
	<select name="procedureName" onchange="selectTopic()">
		<c:forEach items="${topics.rows}" var="record">
			<option value ="${record.ID}">${record.Name}</option>
		</c:forEach>
	</select>
	<br />
	Policy<br />
	<textarea name="topicPolicy" cols="80" rows="10"></textarea>
	<br />
	<a href="#" onclick="updatePolicy()">Update</a><br /><br />
	<a href="#" onclick="deletePolicy()">Delete</a>
</form>