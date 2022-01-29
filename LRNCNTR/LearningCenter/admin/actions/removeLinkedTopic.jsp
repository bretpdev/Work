<script language="javascript">
		function deleteLink(tid,tid2){
			
            try
			{ajaxReqGetTopic = new ActiveXObject("Msxml2.XMLHTTP");}
			catch (e)
			{ajaxReqGetTopic = new ActiveXObject("Microsoft.XMLHTTP"); 	}
			ajaxReqGetTopic.onreadystatechange = HandlesOnReady4;
			var updateSQL = "delete from LCTR_DAT_LinkedTopics where ID = " + tid + " and ToID = " + tid2 ;

			ajaxReqGetTopic.open("GET","./xml/insertUpdate.jsp?sql=" + updateSQL,true);
			ajaxReqGetTopic.send(null);
			document.frmRemoveLinkedTopic.submit();
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
     	<h3>Remove Linked Topic</h3>
   	  <form action="admin.jsp?theme=$(theme)&page=main&subpage=removeLinkedTopic" method="post" name="frmRemoveLinkedTopic">
        <div id="nonMenu">
        <table class="nonMenu">
        <thead><tr><td>Topic Name</td><td>Procedure Name</td><td></td></tr></thead>
        <sql:query var="linkedtopic">
		  	select A.Name as Name1, C.Name as Name2, A.ID as ID1, C.ID as ID2 from LCTR_DAT_Topic A inner join LCTR_DAT_LinkedTopics B on A.ID = B.ID inner join LCTR_DAT_Topic C on B.ToID = C.ID
		</sql:query>
        <c:forEach items="${linkedtopic.rows}" var="record">
        <tr><td class="nonMenu">${record.Name1}</td>
        <td class="nonMenu">${record.Name2}</td>
        <td class="nonMenu"><a href="#" onclick="deleteLink(${record.ID1},${record.ID2})">Remove</a></td>
        </tr>
        </c:forEach>

        </table>
        </div>
   	  </form>