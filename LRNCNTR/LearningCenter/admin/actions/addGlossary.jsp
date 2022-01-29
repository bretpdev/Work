		<script language="javascript">
        function addTerm()
        {
            try
			{ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");}
			catch (e)
			{ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");}
			var updateSQL = "insert into LCTR_DAT_Glossary (Term,Definition) values ('" + document.getElementById('term').value.replace(/'/g,"''") + "','" + document.getElementById('definition').value.replace(/\n/g,'<br>').replace(/'/g,"''") + "')"
			
			ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
			ajaxReqGetTopic.open("GET","./xml/insertUpdate.jsp?sql=" + updateSQL,true);
			ajaxReqGetTopic.send(null);
			document.frmaddGlossary.reset();
        }
		function HandlesOnReady4()
		{
			
			var xmlDoc;
			if (ajaxReqGetTopic.readyState == 4)
			{
			
				if (ajaxReqGetTopic.status == 200)
				{//Success
					xmlDoc = ajaxReqGetTopic.responseXML.documentElement;
					document.getElementById('adminMsg').innerHTML = xmlDoc.getElementsByTagName("Status")[0].childNodes[0].nodeValue;
				}
				else
				{//Failed
					xmlDoc = ajaxReqGetTopic.responseXML.documentElement;
					alert("The following error occured on the server " + ajaxReqGetTopic.status + ".");
					document.getElementById('adminMsg').innerHTML = xmlDoc.getElementsByTagName("Status")[0].childNodes[0].nodeValue;
				}
			}
		}
        </script>
        <h3>Add Glossary Term</h3>
    	<form action="admin.jsp?theme=$(theme)&page=main" method="post" name="frmaddGlossary">
        Glossary Term<br>
        <input name="term" type="text" size="50" maxlength="100" /><br>
        Definition<br>
        <textarea name="definition" cols="80" rows="10"></textarea>
        <br>
        <a href="#" onclick="addTerm()">Add</a>
        </form>