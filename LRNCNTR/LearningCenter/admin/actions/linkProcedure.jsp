<script language="javascript">
		function linkProcedureTopic(){
			
            try
			{ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");}
			catch (e)
			{ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP"); 	}
			ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
			var updateSQL = "Insert into LCTR_DAT_TopicProcedure (TopicID,ProcedureID) values (" + document.getElementById('topicName').value + "," + document.getElementById('procedureName').value + ")";

			ajaxReqGetTopic.open("GET","./xml/insertUpdate.jsp?sql=" + updateSQL,true);
			ajaxReqGetTopic.send(null);
			document.frmupdateProcedure.reset() ;
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
    	<h3>Link Procedure to Topic</h3>      
    	<form action="admin.jsp?theme=$(theme)&page=main" method="post" name="frmupdateProcedure" >
        Topic Name<br>
        <sql:query var="topics">
		  	SELECT ID, Name FROM LCTR_DAT_Topic order by Name ASC
		</sql:query>
        <select name="topicName" >
        <c:forEach items="${topics.rows}" var="record">
        <option value ="${record.ID}">${record.Name}</option>
        </c:forEach>
        </select>
        <br>
        Procedure Name<br>
        <sql:query var="procedures">
		  	SELECT ID, Name FROM LCTR_DAT_Procedures order by Name ASC
		</sql:query>
        <select name="procedureName" >
        <c:forEach items="${procedures.rows}" var="record">
        <option value ="${record.ID}">${record.Name}</option>
        </c:forEach>
        </select><br>
        <a href="#" onclick="linkProcedureTopic()">Link</a><br><br>
        </form>