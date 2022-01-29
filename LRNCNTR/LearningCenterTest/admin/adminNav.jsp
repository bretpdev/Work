<script language="javascript">
	var menuNumber = 5;
	document.onmouseover = hideAllMenusTest;
	
	function showMenuTest(menuNumber, eventObj) {
	    hideAllMenusTest();
		if(document.layers) {
			img = getImage("AImg" + menuNumber);
		 	x = getImagePageLeft(img);
		 	y = getImagePageTop(img);
		 	menuTop = y + 10; // LAYER TOP POSITION
			eval('document.layers["AMenu'+menuNumber+'"].top="'+menuTop+'"');
		 	eval('document.layers["AMenu'+menuNumber+'"].left="'+x+'"');
		}
		eventObj.cancelBubble = true;
	    var menuId = 'AMenu' + menuNumber;
	    if(changeObjectVisibility(menuId, 'visible')) {
			return true;
	    } else {
			return false;
	    }
	}
	
	function hideAllMenusTest() {
	    for(counter = 1; counter <= menuNumber; counter++) {
			changeObjectVisibility('AMenu' + counter, 'hidden');
	    }
	}
	
	function getStyleObject(objectId) {
	    // cross-browser function to get an object's style object given its id
	    if(document.getElementById && document.getElementById(objectId)) {
			// W3C DOM
			return document.getElementById(objectId).style;
	    } else if (document.all && document.all(objectId)) {
			// MSIE 4 DOM
			return document.all(objectId).style;
	    } else if (document.layers && document.layers[objectId]) {
			// NN 4 DOM.. note: this won't find nested layers
			return document.layers[objectId];
	    } else {
			return false;
	    }
	} // getStyleObject
	
	function changeObjectVisibility(objectId, newVisibility) {
	    // get a reference to the cross-browser style object and make sure the object exists
	    var styleObject = getStyleObject(objectId);
	    if(styleObject) {
			styleObject.visibility = newVisibility;
			return true;
	    } else {
			//we couldn't find the object, so we can't change its visibility
			return false;
	    }
	} // changeObjectVisibility
</script>

<div id="adminMenu">
	<H2>Welcome Admininstrator ${fname} ${lname}</H2>
	<table border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td width="150px" height="38" onMouseover="return !showMenuTest('1', event);">
				<img src="\LearningCenter\images\admin\nothing.gif" id="AImg1" name="AImg1" border="0"/>Topic
			</td>
			<td width="150px" onMouseover="return !showMenuTest('2', event);">
				<img src="\LearningCenter\images\admin\nothing.gif" id="AImg2" name="AImg2" border="0"/>Procedure
			</td>
			<td width="150px" onMouseover="return !showMenuTest('3', event);">
				<img src="\LearningCenter\images\admin\nothing.gif" id="AImg3" name="AImg3" border="0"/>Letter
			</td>
			<td width="150" onMouseover="return !showMenuTest('4', event);">
				<img src="\LearningCenter\images\admin\nothing.gif" id="AImg4" name="AImg4" border="0"/>Training
			</td>
			<td width="150" onMouseover="return !showMenuTest('5', event);">
				<img src="\LearningCenter\images\admin\nothing.gif" id="AImg5" name="AImg5" border="0"/>Glossary
			</td>
		</tr>
	</table>

	<div id="AMenu1" style="width: 250px; height: 52px; position:absolute; z-index:20; visibility:hidden"  onMouseOver="event.cancelBubble = true;">
		<table width="100%" border="0">
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=addTopic">Add</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=updateTopic">Update</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=linkProcedure">Link Procedure</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=removeLinkedProcedure">Remove Linked Procedure</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=linkedTopic">Link Topic</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=removeLinkedTopic">Remove Linked Topic</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=topicRisk">Risk</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=topicPolicy">Policy</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=topicFlowChart">Flow Chart</a></td>
			</tr>
		</table>
	</div>
	<div id="AMenu2" style="width: 250px; height: 52px; position:absolute; z-index:20; visibility:hidden; left: 150px; " onMouseOver="event.cancelBubble = true;">
		<table width="100%" border="0">
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=addProcedure">Add</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=updateProcedure">Update</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=procRisk">Risk</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=procPolicy">Policy</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=procFlowChart">Flow Chart</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=procStaff">Staff</a></td>
			</tr>
		</table>
	</div>
	<div id="AMenu3" style="width: 250px; height: 52px; position:absolute; z-index:20; visibility:hidden; left: 302px; " onMouseOver="event.cancelBubble = true;">
		<table width="100%" border="0">
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=addLetter">Add</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=updateLetter">Update</a></td>
			</tr>
		</table>
	</div>
	<div id="AMenu4" style="width: 250px; height: 52px; position:absolute; z-index:20; visibility:hidden; left: 453px; " onMouseOver="event.cancelBubble = true;">
		<table width="100%" border="0">
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=addTraining">Add</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=updateTraining">Update</a></td>
			</tr>
		</table>
	</div>
	<div id="AMenu5" style="width: 250px; height: 52px; position:absolute; z-index:20; visibility:hidden; left: 603px; " onMouseOver="event.cancelBubble = true;">
		<table width="100%" border="0">
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=addGlossary">Add</a></td>
			</tr>
			<tr>
				<td><a href="admin.jsp?theme=${theme}&amp;page=main&amp;subpage=updateGlossary">Update</a></td>
			</tr>
		</table>
	</div>
</div>