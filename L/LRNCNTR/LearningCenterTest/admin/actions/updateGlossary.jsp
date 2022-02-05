<script language="javascript">
	var ajaxReqGetTopic;
	
	function selectTerm() {
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
		}
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4selectTopic;
		myIdentifier=Math.round(Math.random()*10000);
		ajaxReqGetTopic.open("GET","./xml/getGlossaryTerm.jsp?TermID=" + document.getElementById('term').value + "&uniqe=" + myIdentifier, true);
		ajaxReqGetTopic.send(null);
	}
	
	function HandlesOnReady4selectTopic() {
		var xmlDoc;
		if (ajaxReqGetTopic.readyState == 4) {
		
			if (ajaxReqGetTopic.status == 200) {
				//populate form elements
				xmlDoc = ajaxReqGetTopic.responseXML.documentElement;
				GetXMLVal(xmlDoc, "Definition", null, document.getElementById('definition'));
			}
			else {
				alert("The following error occured on the server " + ajaxReqGetTopic.status + ".");
			}
		}
	}
	
	function updateTerm() {
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
		}
		var updateSQL = "UPDATE LCTR_DAT_Glossary SET Definition = '" + document.getElementById('definition').value.replace(/\n/g,'<br>').replace(/'/g,"''") + "' WHERE ID = " + document.getElementById('term').value;
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
		ajaxReqGetTopic.open("GET","./xml/insertUpdate.jsp?sql=" + updateSQL, true);
		ajaxReqGetTopic.send(null);
		document.frmeditTerm.reset() ;
	}
	
	function deleteTerm() {
		if (!(confirm("Are you sure you want to delete this Term?"))) {
			return false;
		}
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
		}
		var updateSQL = "DELETE FROM LCTR_DAT_Glossary WHERE ID = " + document.getElementById('term').value;
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
		ajaxReqGetTopic.open("GET","./xml/insertUpdate.jsp?sql=" + updateSQL, true);
		ajaxReqGetTopic.send(null);
		var el = document.getElementById("term");
		el.remove(el.selectedIndex);
		document.frmeditTerm.reset() ;
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

<h3>Update Glossary Term</h3>
<form action="admin.jsp?theme=$(theme)&page=main" method="post" name="frmeditTerm">
	Term<br />
	<sql:query var="terms">
		SELECT ID, Term FROM LCTR_DAT_Glossary ORDER BY Term ASC
	</sql:query>
	<select name="term" onchange="selectTerm()">
		<c:forEach items="${terms.rows}" var="record">
			<option value ="${record.ID}">${record.Term}</option>
		</c:forEach>
	</select>
	<br />
	Definition<br />
	<textarea name="definition" cols="80" rows="10"></textarea>
	<br />
	<a href="#" onclick="updateTerm()">Update</a><br /><br />
	<a href="#" onclick="deleteTerm()">Delete</a>
</form>