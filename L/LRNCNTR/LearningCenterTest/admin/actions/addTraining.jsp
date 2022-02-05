<%@ page import="edu.utahsbr.*"%>
<script language="javascript">
	function loadTraining() {
		document.getElementById('docSearch').value = document.getElementById('docSearch').value.replace(/\n/g,'<br>').replace(/'/g,"''");
		document.frmaddTraining.submit() ;
	}
</script>
<h3>Add Training Document</h3>
<form action="admin.jsp?theme=$(theme)&page=main&subpage=upload" method="post" name="frmaddTraining" ENCTYPE="multipart/form-data">
	Training Document Name<br />
	<input name="docName" type="text" /><br />
	Document<br />
	<input name="file1" type="file" />&nbsp;&nbsp;(Select the Training Document to add.)<br />
	Key Words<br />
	<textarea name="docSearch" cols="80" rows="10"></textarea><br />
	<input name="test" type="hidden" value="<%=Globals.testString()%>" />
	<input name="type" type="hidden" value="training" />
	<input name="action" type="hidden" value="insert" />
	<a href="#" onclick="loadTraining(); return false;">Add</a><br />
</form>