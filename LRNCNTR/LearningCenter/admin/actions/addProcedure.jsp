<%@ page import="java.io.*"%>
<%@ page import="edu.utahsbr.*"%>

<script language="javascript">
	function addProcedure() {
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
		}
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
		ajaxReqGetTopic.open("POST","../admin/xml/addProcedure.jsp?htmlFile=" + document.getElementById('htmlFile').value + "&procedureName=" + document.getElementById('procedureName').value.replace(/'/g,"''") + "&sKey=" + document.getElementById('SearchKey').value.replace(/'/g,"''") ,true);
		ajaxReqGetTopic.send(null);
		var el = document.getElementById("htmlFile");
		el.remove(el.selectedIndex);
   		document.frmaddProcedure.reset() ;
	}
	
	function HandlesOnReady4() {
		var xmlDoc;
		if (ajaxReqGetTopic.readyState == 4){
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
</script>

<h3>Add Procedure</h3>
<form action="admin.jsp?theme=$(theme)&page=main" method="post" name="frmaddProcedure">
<p>
	Procedure Name<br />
	<input name="procedureName" type="text" size="50" maxlength="50" /><br />
	<p>
		Search Key<br />
		<textarea name="SearchKey" cols="80" rows="2"></textarea>
	</p>
	<select name="htmlFile">
		<option value=""></option>
<%
	File tempDirectory = new File("C:/Program Files/Apache Software Foundation/Tomcat 6.0/webapps/LearningCenter" + Globals.testString() + "/data");
	for (String fileName : tempDirectory.list()) {
		if (fileName.indexOf(".html") > 0) {
			out.println("<option value =" + fileName + ">" + fileName + "</option>");
		}
	}
%>
	</select>
	<br />
	<a href="#" onclick="addProcedure()">Add</a>
</p>
</form>