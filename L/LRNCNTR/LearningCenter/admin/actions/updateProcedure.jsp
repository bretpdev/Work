<%@ page import="java.io.*"%>
<%@ page import="edu.utahsbr.*"%>

<script language="javascript">
		function updateProcedure() {
			try {
				ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
			}
			catch (e) {
				ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
			}
			
			ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
			ajaxReqGetTopic.open("POST","../admin/xml/addProcedure.jsp?htmlFile=" + document.getElementById('htmlFile').value + "&procedureName=" + document.getElementById('procedureName').options[document.getElementById('procedureName').selectedIndex].text + "&id=" + document.getElementById('procedureName').value + "&sKey=" + document.getElementById('SearchKey').value.replace(/'/g,"''") ,true);
			ajaxReqGetTopic.send(null);
			if (document.getElementById('htmlFile').value != ""){
				var el = document.getElementById("htmlFile");
				el.remove(el.selectedIndex);
			}
    		document.frmupdateProcedure.reset() ;
		}
		
		function deleteProcedure() {
			if (!confirm("Are you sure you want to delete this Procedure?")) {
                return false;
            }
			try {
				ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
			}
			catch (e) {
				ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
			}
			ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
			ajaxReqGetTopic.open("POST","../admin/xml/deleteProcedure.jsp?procedureName=" + document.getElementById('procedureName').options[document.getElementById('procedureName').selectedIndex].text + "&id=" + document.getElementById('procedureName').value ,true);
			var el = document.getElementById("procedureName");
			el.remove(el.selectedIndex);
			ajaxReqGetTopic.send(null);
    		document.frmupdateProcedure.reset() ;
		}
		
		function HandlesOnReady4() {
			var xmlDoc;
			if (ajaxReqGetTopic.readyState == 4) {
				if (ajaxReqGetTopic.status == 200) {
					//Success
					xmlDoc = ajaxReqGetTopic.responseXML.documentElement;
					document.getElementById('adminMsg').innerHTML = xmlDoc.getElementsByTagName("Status")[0].childNodes[0].nodeValue;
				}
				else {
					//Failed
					xmlDoc = ajaxReqGetTopic.responseXML.documentElement;
					alert("The following error occured on the server " + ajaxReqGetTopic.status + ".");
					document.getElementById('adminMsg').innerHTML = xmlDoc.getElementsByTagName("Status")[0].childNodes[0].nodeValue;
				}
			}
		}
		
		function selectProcedure() {
            try {
                ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
			}
			catch (e) {
				ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
			}
			ajaxReqGetTopic.onreadystatechange = HandlesOnReady4selectTopic;
			myIdentifier=Math.round(Math.random()*10000);
			ajaxReqGetTopic.open("GET","./xml/getProcedure.jsp?pID=" + document.getElementById('procedureName').value + "&uniqe=" + myIdentifier,true);
			ajaxReqGetTopic.send(null);
        }
        
		function HandlesOnReady4selectTopic() {
			var xmlDoc;
			if (ajaxReqGetTopic.readyState == 4) {
				if (ajaxReqGetTopic.status == 200) {
					//populate form elements
					xmlDoc = ajaxReqGetTopic.responseXML.documentElement;
					GetXMLVal(xmlDoc, "SearchKey", null, document.getElementById('SearchKey'));
				}
				else {
					alert("The following error occured on the server " + ajaxReqGetTopic.status + ".");
				}
			}
		}
</script>

<h3>Update Procedure</h3>
<form action="admin.jsp?theme=$(theme)&page=main" method="post" name="frmupdateProcedure">Procedure Name<br>
<sql:query var="topics">
	SELECT ID, Name FROM LCTR_DAT_Procedures order by Name ASC
</sql:query>
<select name="procedureName" onchange="selectProcedure()">
	<c:forEach items="${topics.rows}" var="record">
		<option value="${record.ID}">${record.Name}</option>
	</c:forEach>
</select>
<p>
	Search Key<br />
	<textarea name="SearchKey" cols="80" rows="2"></textarea>
</p>
<br />
Replace with<br />
<select name="htmlFile">
	<option></option>
<%
	File tempDirectory = new File("C:/Program Files/Apache Software Foundation/Tomcat 6.0/webapps/LearningCenter" + Globals.testString() + "/data");
	for (String fileName : tempDirectory.list()) {
		if (fileName.indexOf(".html") > 0) {
			out.println("<option value =" + fileName + ">" + fileName + "</option>");
		}
	}
%>
</select><br />
<a href="#" onclick="updateProcedure()">Update</a><br /><br />
<a href="#" onclick="deleteProcedure()">Delete</a>
</form>