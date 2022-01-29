<script language="javascript">
		function deleteLink(tid,pid){
			
            try
			{ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");}
			catch (e)
			{ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP"); 	}
			ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
			var updateSQL = "delete from LCTR_DAT_TopicProcedure where TopicID = " + tid + " and ProcedureID = " + pid ;

			ajaxReqGetTopic.open("GET","./xml/insertUpdate.jsp?sql=" + updateSQL,true);
			ajaxReqGetTopic.send(null);
			document.frmRemoveLinkedProcedure.submit();
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
     	<h3>Remove Linked Procedure</h3>
    	<form action="admin.jsp?theme=$(theme)&page=main&subpage=removeLinkedProcedure" method="post" name="frmRemoveLinkedProcedure">
        <div id="nonMenu">
        <table class="nonMenu">
        <thead><tr><td>Topic Name</td><td>Procedure Name</td><td></td></tr></thead>
        <sql:query var="linkedProc">
		  	select A.Name as TName, C.Name as PName, A.ID as TID, C.ID as PID from LCTR_DAT_Topic A inner join LCTR_DAT_TopicProcedure B on A.ID = B.TopicID inner join LCTR_DAT_Procedures C on B.ProcedureID = C.ID
		</sql:query>
        <c:forEach items="${linkedProc.rows}" var="record">
        <tr><td class="nonMenu">${record.TName}</td>
        <td class="nonMenu">${record.PName}</td>
        <td class="nonMenu"><a href="#" onclick="deleteLink(${record.TID},${record.PID})">Remove</a></td>
        </tr>
        </c:forEach>

        </table>
        </div>
    	</form>