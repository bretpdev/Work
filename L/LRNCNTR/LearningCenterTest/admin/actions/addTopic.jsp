		<script language="javascript">
        function addTopic()
        {
            try
			{ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");}
			catch (e)
			{ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP");}
			var updateSQL = "insert into LCTR_DAT_Topic (Name,Narrative,SearchKey) values ('" + document.getElementById('topicName').value.replace(/'/g,"''") + "','" + document.getElementById('topicNarrative').value.replace(/\n/g,'<br>').replace(/'/g,"''") + "','" + document.getElementById('topicKey').value.replace(/'/g,"''")  + "')"
			
			ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
			ajaxReqGetTopic.open("GET","./xml/insertUpdate.jsp?sql=" + updateSQL,true);
			ajaxReqGetTopic.send(null);
    		//document.frmaddTopic.submit();
			document.frmaddTopic.reset();
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
        <h3>Add Topic</h3>
    	<form action="admin.jsp?theme=$(theme)&page=main" method="post" name="frmaddTopic">
        Topic Name<br>
        <input name="topicName" type="text" size="50" maxlength="50" /><br>
        Narrative<br>
        <textarea name="topicNarrative" cols="80" rows="10"></textarea>
        <br>
        Search Key<br>
        <textarea name="topicKey" cols="80" rows="2"></textarea>
        <br>
        <a href="#" onclick="addTopic()">Add</a>
        </form>