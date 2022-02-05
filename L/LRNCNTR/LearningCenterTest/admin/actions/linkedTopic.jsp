<script language="javascript">
	function linkTopic() {
		if (document.getElementById('topic1').value == document.getElementById('topic2').value) {
			alert("You can not Link a Topic to itself!");
			return false;
		}
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
		}
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
		var updateSQL = "Insert into LCTR_DAT_LinkedTopics (ID,ToID) values (" + document.getElementById('topic1').value + "," + document.getElementById('topic2').value + ")";
		ajaxReqGetTopic.open("GET","./xml/insertUpdate.jsp?sql=" + updateSQL,true);
		ajaxReqGetTopic.send(null);
		document.frmlinkTopic.reset() ;
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
<h3>Link Topic to Topic</h3>
<br />
Be sure not to link a topic to itself or to a previously related topic.
<form action="admin.jsp?theme=$(theme)&page=main" method="post"	name="frmlinkTopic">
	Topic Name 1<br />
	<sql:query var="topics">
		SELECT ID, Name FROM LCTR_DAT_Topic ORDER BY Name ASC
	</sql:query>
	<select name="topic1">
		<c:forEach items="${topics.rows}" var="record">
			<option value="${record.ID}">${record.Name}</option>
		</c:forEach>
	</select><br />

	Topic Name 2<br />
	<sql:query var="topics2">
	  	SELECT ID, Name FROM LCTR_DAT_Topic order by Name ASC
	</sql:query>
	<select name="topic2">
		<c:forEach items="${topics2.rows}" var="record">
			<option value="${record.ID}">${record.Name}</option>
		</c:forEach>
	</select><br />
	<a href="#" onclick="linkTopic()">Link</a><br /><br />
</form>