<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/sql" prefix="sql" %>
<%@ page import="java.io.*" %>
<%@ page import="edu.utahsbr.*" %>
<script language="javascript">
	function updateStaff(taction) {
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");
		}
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
		document.getElementById('action').value = taction;
		ajaxReqGetTopic.open("POST","../admin/xml/updateStaff.jsp?htmlFile=" + document.getElementById('htmlFile').value + "&docName=" + document.getElementById('docName').value.replace(/'/g,"''") + "&action=" + taction , true);
		ajaxReqGetTopic.send(null);
		var el = document.getElementById("htmlFile");
		el.remove(el.selectedIndex);
		document.frmStaff.reset() ;
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

<h3>Update Procedure's Staff page</h3>      
<form action="admin.jsp?theme=$(theme)&page=main&subpage=upload" method="post" name="frmStaff">
	Procedure Name<br />
	<sql:query var="topics">
		SELECT ID, Name FROM LCTR_DAT_Procedures ORDER BY Name ASC
	</sql:query>
	<select name="docName">
		<c:forEach items="${topics.rows}" var="record">
			<option value ="${record.Name}">${record.Name}</option>
		</c:forEach>
	</select>
	<br />
	Staff HTML File<br />
	<select name="htmlFile">
		<option value =""></option>
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
	<input name="test" type="hidden" value="<%= Globals.testString() %>" />
	<input name="type" type="hidden" value="staff" />
	<input name="action" type="hidden" value="update" />
	
	<a href="#" onclick="updateStaff('update')">Update</a><br /><br />
	<a href="#" onclick="updateStaff('delete')">Delete</a><br />
</form>