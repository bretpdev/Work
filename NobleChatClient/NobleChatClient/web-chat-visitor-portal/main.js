/*
* Web Chat Visitor Portal Main JS Entry Point
*/

$(function() {
    
    /* 
    * Upon page load, read QueryString values from the URL and set config.js values as needed.
    */
    (function getValuesFromUrl() {
        
        // Reads URL QueryString and returns a JSON object of key (lowercased) and value
        function queryStringToJSON() {            
            var pairs = location.search.slice(1).split('&');
            var result = {};
            $.each(pairs, function(index, value) {
				if(value) {
					var pair = value.split('=');
					result[pair[0].toLowerCase()] = decodeURIComponent(pair[1] || '');
				}
            });

            return JSON.parse(JSON.stringify(result));
        }
        
        var queryParams = queryStringToJSON();
        
        if (queryParams.queueid)
            chatPanelConfig.queueId = queryParams.queueid;
        
        if (queryParams.chatdata1 || queryParams.chatdata2 || queryParams.chatdata3 || queryParams.chatdata4)
            setChatData(queryParams.chatdata1, queryParams.chatdata2, queryParams.chatdata3, queryParams.chatdata4);
        
        setFromUser(queryParams);

    })();
    
    /*
    * Sets the Chat Data fields.
    * This is the same as changing chatPanelFormConfig in config.js but this is a programmatic way of doing it.
    * Calling this function will set showChatDataFields to false.
    */
    function setChatData(chatData1, chatData2, chatData3, chatData4) {
        
        chatPanelFormConfig.showChatDataFields = false;
        
        if (chatData1)
            chatPanelFormConfig.userDefinedData.chatData1 = chatData1.substring(0, 256);
        else
            chatPanelFormConfig.userDefinedData.chatData1 = "";
        
        if (chatData2)
            chatPanelFormConfig.userDefinedData.chatData2 = chatData2.substring(0, 256);
        else
            chatPanelFormConfig.userDefinedData.chatData2 = "";
        
        if (!isNaN(chatData3))
            chatPanelFormConfig.userDefinedData.chatData3 = parseInt(chatData3);
        else
            chatPanelFormConfig.userDefinedData.chatData3 = 0;        
        
        if (!isNaN(chatData4))
            chatPanelFormConfig.userDefinedData.chatData4 = parseInt(chatData4);
        else
            chatPanelFormConfig.userDefinedData.chatData4 = 0;
    }

    /*
    * Sets the User Data fields.
    * 
    * 
    */
    function setFromUser(queryParams) {

        if (!chatPanelConfig.fromUser)
        {
            chatPanelConfig.fromUser = {};
            chatPanelFormConfig.userInfoData = {}
        }

        if (queryParams.firstname)
        {
            chatPanelConfig.fromUser.firstName = queryParams.firstname;
            chatPanelFormConfig.userInfoData.firstName = queryParams.firstname;
        }
        if (queryParams.lastname)
        {
            chatPanelConfig.fromUser.lastName = queryParams.lastname;
            chatPanelFormConfig.userInfoData.lastName = queryParams.lastname;
        }
        if (queryParams.useremail && queryParams.useremail.trim() != "" && 
                   /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(queryParams.useremail.trim()) == true)
        {
            chatPanelConfig.fromUser.email = queryParams.useremail;
            chatPanelFormConfig.userInfoData.email = queryParams.useremail;
        }

        if (queryParams.userphone)
        {
            chatPanelConfig.fromUser.phone = queryParams.userphone;
            chatPanelFormConfig.userInfoData.phone = queryParams.userphone;
        }

        if(chatPanelConfig.fromUser.firstName 
            && chatPanelConfig.fromUser.lastName
            && chatPanelConfig.fromUser.email)
        {
            chatPanelConfig.fromUser.initDone = true;
        }
    }

    /*
    * Sets the Entry Url and IP Address of the request.
    * Without this, the Web Chat Server automaticallys read entry url and ip address from http header.
    * This provides a way to manually enter information.
    */
    function setRequestInfo(entryUrl, ipAddress) {
        if (entryUrl)
            chatPanelConfig.entryUrl = entryUrl;
        if (ipAddress)
            chatPanelConfig.ipAddress = ipAddress;
    }

    /*
    * Sets the Termination Url.
    * 
    * 
    */
    function setTerminationUrl(termUrl) {
        if(termUrl)
            chatPanelConfig.terminationUrl = termUrl;
    }
    

    //setRequestInfo("http://localhost/samplesite/chat.html", "127.0.0.1");

    //setTerminationUrl("http://www.noblesys.com/company/careers.aspx")
    
    /* 
    * Starts the Web Chat Visitor Portal
    */
    $('#chat-panel').chatPanel(chatPanelConfig);
});


function recaptchaOnloadCallback() {
	
    grecaptcha.render('chatviewer-recaptcha', { callback: function() {

        $('#chatviewer-form-submit').prop("disabled", false);
    }, sitekey: chatPanelConfig.recaptcha.siteKey});
};
