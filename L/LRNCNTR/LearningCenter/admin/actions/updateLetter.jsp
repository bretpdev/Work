<%@ page import="edu.utahsbr.*"%>

<script language="javascript">
	function loadLetter(taction) {
		document.getElementById('docSearch').value = document.getElementById('docSearch').value.replace(/\n/g,'<br>').replace(/'/g,"''");
		document.getElementById('action').value = taction;
		document.frmaddLetter.submit() ;
	}
	
	var ajaxReqGetTopic;
	function selectLetter() {
		try {
			ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP"); 
		}
		ajaxReqGetTopic.onreadystatechange = HandlesOnReady4selectLetter;
		myIdentifier=Math.round(Math.random()*10000);
		ajaxReqGetTopic.open("GET","./xml/getXML.jsp?sp=dbo.spLCTR_getDocs (" + document.getElementById('docID').value + ",Letter) &uniqe=" + myIdentifier,true);
		ajaxReqGetTopic.send(null);
	}
	
	function HandlesOnReady4selectLetter() {
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

<h3>Update Letter</h3>
<form action="admin.jsp?theme=$(theme)&page=main&subpage=upload" method="post" name="frmaddLetter" ENCTYPE="multipart/form-data">
<div id="message"></div>
Letter Name<br />
<sql:query var="letters">
	SELECT ID, Name FROM LCTR_DAT_Docs where type = 'Letter' order by Name ASC
</sql:query>
<select name="docID" onchange="selectLetter()">
	<c:forEach items="${letters.rows}" var="record">
		<option value="${record.ID}">${record.Name}</option>
	</c:forEach>
</select><br />
Letter<br />
<input name="file1" type="file">&nbsp;&nbsp;(Select the Letter to add.)<br />
Key Words<br />
<textarea name="docSearch" cols="80" rows="10"></textarea><br />
<input name="test" type="hidden" value="<%=Globals.testString()%>" />
<input name="type" type="hidden" value="letter" /> <input name="docName" type="hidden" value="" />
<input name="docPath" type="hidden" value="" />
<input name="action" type="hidden" value="update" />
<a href="#" onclick="loadLetter('update');">Update</a><br />
<a href="#" onclick="loadLetter('delete');">Delete</a>
</form>