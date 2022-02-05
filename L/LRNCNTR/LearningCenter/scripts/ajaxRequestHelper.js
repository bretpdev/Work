// This file is a near-exact copy of the RequestHelper.js code,
// listing 5.6.5 from "Web Development with JavaScript and Ajax Illuminated"
// by Richard Allen, Kai Qian, Lixin Tao, and Xiang Fu,
// copyright 2009 by Jones and Bartlett Publishers, LLC.

// Create the "lc" namespace if it's not yet defined.
if (typeof lc == "undefined") {
	lc = {};
}

/** An object to help with creating and using XMLHttpRequest. */
lc.requestHelper = (function() {
	// Array of MSXML XMLHTTP versions to try in IE browsers, ordered by preferability.
	var msxmlVersions = ["Msxml2.XMLHTTP.6.0", "Msxml2.XMLHTTP.3.0"];
	// Variable to store the correct XMLHttpRequest function for this browser.
	var xhrFunction = null;
	
	// Determine what XMLHttpRequest object is available.
	if (window.XMLHttpRequest) {
		xhrFunction = function() { return new XMLHttpRequest; };
	}
	else if (window.ActiveXObject) {
		// Try each XMLHTTP version until one works.
		for (var i = 0; i < msxmlVersions.length; i++) {
			xhrFunction = function() {
				return new ActiveXObject(msxmlVersions[i]);
			};
			try {
				xhrFunction();
			}
			catch (e) {
				continue;
			}
			break;
		}
	}
	
	// Return an object that defines the public API.
	return {
		/**
		 * Creates and returns a new XMLHttpRequest instance.
		 * Throws an error if this browser does not support XMLHttpRequest or XMLHTTP.
		 */
		createXHR: function() {
			var xhrObj = null;
			if (xhrFunction != null) {
				xhrObj = xhrFunction();
			}
			if (xhrObj == null) {
				// The browser does not support XMLHttpRequest, so throw an error.
				throw new Error("XMLHttpRequest not supported.");
			}
			return xhrObj;
		},
		
		/**
		 * Sends an asynchronous GET request to the given URL and invokes either
		 * the success callback or the failure callback on success or failure.
		 * A timeout value in milliseconds is used for timing out the request.
		 */
		 sendGet: function(url, successCallback, failureCallback, timeout) {
			// Create an XMLHttpRequest object.
			var request = lc.requestHelper.createXHR();
			// Set a timer to cancel the request after the given timeout.
			var abortTimer = setTimeout(function() { request.abort(); failureCallback(request); }, timeout);
			
			// Define the readyState event handler to process the response.
			request.onreadystatechange = function() {
				// We care only about state 4, indicating the response has been received.
				if (request.readyState == 4) {
					// Cancel the abort timer.
					clearTimeout(abortTimer);
					
					// Status 200 means the response was OK.
					// Status 304 means the response was pulled from the browser cache.
					if (request.status == 200 || request.status == 304) {
						successCallback(request);
					}
					else {
						failureCallback(request);
					}
				}
			}
			
			// Tell XMLHttpRequest what URL we want to get.
			request.open("GET", url, true);
			// Send the request.
			request.send(null);
		}
	};
})();