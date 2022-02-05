// Create the "lc" namespace if it's not yet defined.
if (typeof lc == "undefined") {
	lc = {};
}

/**
 * The adminAjax object forwards ajax requests to a given JSP page and returns the response.
 */
lc.adminAjax = (function() {
	//Some variables are needed for displaying the XML results.
	var displayObjects = [{"xmlNodeName": "", "displayElement": "", "displayElementIsComboBox" : false}];
	
	// The next three functions handle the server response for HTTP error, insert/delete/update, and select, respectively.
	var showError = function(httpResponse) {
		alert("The following error occured on the server " + httpResponse.status + ".");
	};
	
	var showXmlStatus = function(httpResponse) {
		document.getElementById("adminMsg").innerHTML = httpResponse.responseXML.documentElement.getElementsByTagName("Status")[0].childNodes[0].nodeValue;
	};

	var getXmlValue = function(httpResponse) {
		// Extract the desired XML element from the HTTP response object.
		var xmlNode = httpResponse.responseXML.documentElement.getElementsByTagName(displayObjects[0].xmlNodeName)[0];
		// Index of the item in the combo box that matches the value from the XML document.
		var currentIndex = 0;
		
		// See whether we're dealing with a combo box or a text box.
		if (displayObjects[0].displayElementIsComboBox) {
			// See if the XML doc has a value for the given tag.
			if (xmlNode == null) {
				currentIndex = 0;
			}
			else {
				// Find the item in the combo box that matches the value from the XML doc.
				while (displayObjects[0].displayElement.options[currentIndex].value != xmlNode.childNodes[0].nodeValue) {
					currentIndex += 1;
				}
			}
			displayObjects[0].displayElement.selectedIndex = currentIndex;
		}
		else {
			try {
				displayObjects[0].displayElement.value = "";
				// See if the XML doc has a value for the given tag.
				if (xmlNode.childNodes[0]) {
					displayObjects[0].displayElement.value = xmlNode.childNodes[0].nodeValue.replace("<br />", "\n");
				}
			}
			catch (e) {
				alert(e);
			}
		}
	};

	var getXmlValues = function(httpResponse) {
		for (var i = 0; i < displayObjects.length; i++) {
			// Extract the desired XML element from the HTTP response object.
			var xmlNode = httpResponse.responseXML.documentElement.getElementsByTagName(displayObjects[i].xmlNodeName)[0];
			// Index of the item in the combo box that matches the value from the XML document.
			var currentIndex = 0;
			
			// See whether we're dealing with a combo box or a text box.
			if (displayObjects[i].displayElementIsComboBox) {
				// See if the XML doc has a value for the given tag.
				if (xmlNode == null) {
					currentIndex = 0;
				}
				else {
					// Find the item in the combo box that matches the value from the XML doc.
					while (displayObjects[i].displayElement.options[currentIndex].value != xmlNode.childNodes[0].nodeValue) {
						currentIndex += 1;
					}
				}
				displayObjects[i].displayElement.selectedIndex = currentIndex;
			}
			else {
				try {
					displayObjects[i].displayElement.value = "";
					// See if the XML doc has a value for the given tag.
					if (xmlNode.childNodes[0]) {
						displayObjects[i].displayElement.value = xmlNode.childNodes[0].nodeValue.replace("<br />", "\n");
					}
				}
				catch (e) {
					alert(e);
				}
			}
		}
	};

	/**
	 * The API object contains a function that adds, updates, or deletes data,
	 * and a function that retrieves data.
	 * Both use the lc.requestHelper object to perform the ajax.
	 */
	return {
		/**
		 * Add, update, or delete data.
		 * xmlUrl: The URL of a JSP that will do the server-side processing.
		 */ 
		addUpdateOrDeleteItem : function(xmlUrl) {
			// Send the HTTP request and have it call back to the appropriate function. 
			lc.requestHelper.sendGet(xmlUrl, showXmlStatus, showError, 5000);
		},

		/**
		 * Retrieve data.
		 * xmlUrl: The URL of a JSP that will do the server-side processing.
		 * nodeName: The name of the node from the returned XML document
		 * 			that contains the data to be displayed.
		 * domElement: The node in the DOM tree that will display the fetched data.
		 * 			This can be either a text area or a combo box.
		 * isComboBox: A boolean flag indicating whether domElement is a combo box.
		 * 			If false, domElement is assumed to be a text area.
		 * 			Default is false.
		 */
		fetchItem : function(xmlUrl, nodeName, domElement, isComboBox) {
			// Set the private object so that it's ready to show the results.
			displayObjects[0].xmlNodeName = nodeName;
			displayObjects[0].displayElement = domElement;
			displayObjects[0].displayElementIsComboBox = isComboBox;
			// Send the HTTP request and have it call back to the appropriate function. 
			lc.requestHelper.sendGet(xmlUrl, getXmlValue, showError, 5000);
		},
		
		fetchItems : function(xmlUrl, objectArray) {
			// Set the private object so that it's ready to show the results.
			displayObjects = objectArray;
			// Send the HTTP request and have it call back to the appropriate function. 
			lc.requestHelper.sendGet(xmlUrl, getXmlValues, showError, 5000);
		}
	};
})(); // lc.adminAjax

/**
 * The dropDownMenu object provides handles for showing and hiding
 * a set number of objects with a particular ID naming scheme.
 */
lc.dropDownMenu = (function() {
	var numberOfMenus = 5;

	var getStyleObject = function(objectId) {
		if (document.getElementById && document.getElementById(objectId)) {
			return document.getElementById(objectId).style;
		}
		else if (document.all && document.all(objectId)) {
			return document.all(objectId).style;
		}
		else if (document.layers && document.layers[objectId]) {
			return document.layers[objectId];
		}
		else {
			return false;
		}
	};

	var changeObjectVisibility = function(objectId, newVisibility) {
		var styleObject = getStyleObject(objectId);
		if (styleObject) {
			styleObject.visibility = newVisibility;
			return true;
		}
		else {
			return false;
		}
	};

	/**
	 * The API object offers functions for showing and hiding menus.
	 */
	return {
		/**
		 * Show the object that follows the ID naming scheme ending with the given number.
		 * menuNumber: The number at the end of the object's ID name.
		 * eventObj:
		 */
		showMenu : function(menuNumber, eventObj) {
			var img;
			var x;
			var y;
			var menuTop;
			var menuId = 'AMenu' + menuNumber;
			
			lc.dropDownMenu.hideAllMenus();
			
			if (document.layers) {
				img = getImage("AImg" + menuNumber);
				x = getImagePageLeft(img);
				y = getImagePageTop(img);
				menuTop = y + 10;
				eval('document.layers["' + menuId + '"].top="' + menuTop + '"');
				eval('document.layers["' + menuId + '"].left="' + x + '"');
			}
			
			eventObj.cancelBubble = true;
			
			if (changeObjectVisibility(menuId, 'visible')) {
				return true;
			}
			else {
				return false;
			}
		},

		/**
		 * Hide all of the objects that follow the ID naming scheme
		 * and are within the number range defined by the menuNumber variable.
		 */
		hideAllMenus : function() {
			for (var counter = 1; counter <= numberOfMenus; counter++) {
				changeObjectVisibility('AMenu' + counter, 'hidden');
			}
		}
	};
})(); // lc.dropDownMenu
