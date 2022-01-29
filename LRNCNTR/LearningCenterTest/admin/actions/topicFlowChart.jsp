<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>
<%@ page import="java.io.*" %>
<%@ page import="edu.utahsbr.*" %>

<script language="javascript">
	function updateFlow() {
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
		}
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
		ajaxReqGetTopic.open("POST","../admin/xml/addFlowChart.jsp?htmlFile=" + document.getElementById('htmlFile').value + "&topicName=" + document.getElementById('topicName').options[document.getElementById('topicName').selectedIndex].text.replace(/'/g,"''") + "&id=" + document.getElementById('topicName').value + "&type=Topic&description=" + document.getElementById('flowDescription').value.replace(/\n/g,'<br>').replace(/'/g,"''"), true);
		ajaxReqGetTopic.send(null);
		var el = document.getElementById("htmlFile");
		el.remove(el.selectedIndex);
		document.frmupdateFlow.reset() ;
	}
	
	function deleteFlow() {
		if (!(confirm("Are you sure you want to delete this Flow Chart?"))) {
			return false;
		}
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
		}
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
		ajaxReqGetTopic.open("POST","../admin/xml/deleteFlowChart.jsp?topicName=" + document.getElementById('topicName').options[document.getElementById('topicName').selectedIndex].text + "&id=" + document.getElementById('topicName').value +"&type=Topic", true);
		ajaxReqGetTopic.send(null);
		document.frmupdateFlow.reset() ;
	}
	
	function selectTopic() {
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP"); 
		}
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4selectTopic;
		myIdentifier=Math.round(Math.random()*10000);
		ajaxReqGetTopic.open("GET","./xml/getRiskPolicy.jsp?ID=" + document.getElementById('topicName').value + "&type=Topic&uniqe=" + myIdentifier, true);
		ajaxReqGetTopic.send(null);
	}
	
	function HandlesOnReady4selectTopic() {
		var xmlDoc;
		if (ajaxReqGetTopic.readyState == 4) {
			if (ajaxReqGetTopic.status == 200) {
				//populate form elements
				xmlDoc = ajaxReqGetTopic.responseXML.documentElement;
				GetXMLVal(xmlDoc, "Description", null, document.getElementById('flowDescription'));
			}
			else {
				alert("The following error occured on the server " + ajaxReqGetTopic.status + ".");
			}
		}
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

<h3>Update Topic Flow Chart</h3>      
<form action="admin.jsp?theme=$(theme)&page=main" method="post" name="frmupdateFlow">
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
	Replace with<br />
	<select name="htmlFile">
		<option value ="${record.ID}">${record.Name}</option>
		<% 
			File f = new File("C:/Program Files/Apache Software Foundation/Tomcat 6.0/webapps/LearningCenter" + Globals.testString() + "/data");
			File[] flist = f.listFiles();
			for (int i = 0; i < flist.length;i++){
				if (flist[i].getName().indexOf(".html") > 0){
					out.println("<option value =" + flist[i].getName() + ">" + flist[i].getName() + "</option>");
				}
			}
		%>
	</select><br />
	Flow Chart Description<br />
	<textarea name="flowDescription" cols="80" rows="10"></textarea>
	<br />
	<a href="#" onclick="updateFlow()">Update</a><br /><br />
	<a href="#" onclick="deleteFlow()">Delete</a>
</form>