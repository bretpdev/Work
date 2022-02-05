<%@ page import="edu.utahsbr.*" %>

<script language="javascript">
	function loadLetter() {
		document.getElementById('docSearch').value = document.getElementById('docSearch').value.replace(/\n/g,'<br>').replace(/'/g,"''");
		document.frmaddLetter.submit() ;
	}
</script>

<h3>Add Letter</h3>
<form action="admin.jsp?theme=$(theme)&page=main&subpage=upload" method="post" name="frmaddLetter" ENCTYPE="multipart/form-data">
	Letter Name<br />
	<input name="docName" type="text" /><br />
	Letter<br />
	<input name="file1" type="file" />&nbsp;&nbsp;(Select the Letter to add.)<br />
	Key Words<br />
	<textarea name="docSearch" cols="80" rows="10"></textarea><br />
	<input name="test" type="hidden" value="<%= Globals.testString() %>" />
	<input name="type" type="hidden" value="letter" />
	<input name="action" type="hidden" value="insert" />
	<a href="#" onclick="return loadLetter();">Add</a><br />
</form>