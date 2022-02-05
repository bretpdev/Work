/*
* Noble Systems Web Chat Visitor Portal Chatviewer v2.0.1
*
* Copyright (c) 2017 Noble Systems Corporation
*/

/* 
*   Slimmed down version of the kendo-cui-chatPanel for public consumption
*/

function refreshFunc(){
	window.location.reload();
}

var refreshTimerID;
var refreshTimerInterval = 60000;

(function($) {
    var simpleMinifiedTableCssFormat =  'table.gridtable{font-family:verdana,arial,sans-serif;font-size:11px;color:#333;border-width:1px;border-color:#666;border-collapse:collapse}table.gridtable th{border-width:1px;padding:8px;border-style:solid;border-color:#666;background-color:#dedede}table.gridtable td{border-width:1px;padding:8px;border-style:solid;border-color:#666;background-color:#fff}';
	
	
    $.fn.methods = function()
        {
            if ($(this).data('methods') == undefined)
            {
                $(this).data('methods', {});
            }

            return $(this).data('methods');
        };

    $.fn.chatPanel = function(options)
    {
        var chatMessage = [];
        var thisDialog = $(this);
        var self = this;
        var localizeString;
        var log = function (str, data) 
        { 
            if (options.debug)
            {
                if (data)
                {
                    console.log(str, data)
                }
                else
                {
                    console.log(str);
                }
            }
        };

        var periodsTimeout;
        var updateUserElementForState = function (resource, userAlias, state) {
            
            var headerText = localizeString('Messages')
            var columnTitleEl = $('#chatviewer-messagecolumn .chatviewer-columntitle b');

            clearInterval(periodsTimeout);
            
            if (state == 'composing') {

                columnTitleEl.text(headerText += " - " + userAlias + ' ' + localizeString('is typing.'));

                periodsTimeout = setInterval(function() { 

                    var indexTyping = columnTitleEl.text().indexOf(localizeString('is typing.'));
                    var indexEndTyping = indexTyping + 10;

                    if (indexTyping > -1) {
                        var periodCount = columnTitleEl.text().length - indexEndTyping;

                        if (periodCount >= 0 && periodCount <= 1) {
                            columnTitleEl.text(columnTitleEl.text() +'.');
                        }
                        else {
                            columnTitleEl.text(columnTitleEl.text().slice(0, indexEndTyping) ); 
                        }
                    }
                    
                }, 1000);
            }
            else {
                columnTitleEl.text(headerText);
            }
        };

        var addMessageElement = function (chatTimestamp, toUser, fromUserAlias, fromUserResource, messageBody) {

            var privateChat = toUser != 'ALL';

            if (privateChat)
            {
                return; //not supported in visitor portal
            }

            var parentEl = $('#chatviewer-messagelist');

            var messageHeaderClass = 'chatviewer-messageheader';
            var messageBorderClass = defaultMessageBorderClass;
            var userIconClass = "chatviewer-usericon";
            var defaultAgentMessageBorderClass = ' chatviewer-border-agent';
            var defaultCustomerMessageBorderClass = ' chatviewer-border-customer';
            var defaultManagerMessageBorderClass = ' chatviewer-border-manager';
            var defaultAdminMessageBorderClass = ' chatviewer-border-admin'
            var defaultMessageBorderClass = 'chatviewer-message';
            var defaultTableMessageBorderClass = 'chatviewer-message-table';

            if (fromUserResource == 'agent') {
                messageHeaderClass += ' chatviewer-color-agent';
                messageBorderClass += defaultAgentMessageBorderClass;
                userIconClass += (options.useBlockStyle == true) ? ' chatviewer-icon-agent' : ' chatviewer-icon-agent-table';
                fromUserAlias = "Agent " + fromUserAlias;
            }
            else if (fromUserResource === 'customer') {
                messageHeaderClass += ' chatviewer-color-customer';
                messageBorderClass += defaultCustomerMessageBorderClass;
                userIconClass += (options.useBlockStyle == true) ? ' chatviewer-icon-customer' : ' chatviewer-icon-customer-table';
            }
            else if (fromUserResource === 'manager') {
                messageHeaderClass += ' chatviewer-color-manager';
                messageBorderClass += defaultManagerMessageBorderClass;
                userIconClass += (options.useBlockStyle == true) ? ' chatviewer-icon-manager' : ' chatviewer-icon-manager-table';
            }
            else if (fromUserResource === 'admin') {
                messageHeaderClass += ' chatviewer-color-admin';
                messageBorderClass += defaultAdminMessageBorderClass;
                userIconClass += (options.useBlockStyle == true) ? ' chatviewer-icon-admin' : ' chatviewer-icon-admin-table';
            }

            if (options.useBlockStyle == false)
            {
                messageHeaderClass = 'chatviewer-messageheader-table';
                messageBorderClass = defaultTableMessageBorderClass;
            }

            var uniqueMsgId = chatTimestamp.replace(/\W+/g, "");

            if (parentEl.find('#' + uniqueMsgId).length == 0)
            {
                parentEl.append('<div id="' + uniqueMsgId + '" class="chatviewer-messagecontainer"></div>');

                var msgContainer = parentEl.find('#' + uniqueMsgId);
                
                var dateFormat = options.dateFormat;

                var chatDateTime = moment(chatTimestamp).format(dateFormat);
                if (options.useBlockStyle == true) {
                    msgContainer.append('<div class="' + messageHeaderClass + '"></div>');
                    var msgHeader = msgContainer.find('.' + messageHeaderClass.replace(/ /g, '.'));

                    msgHeader.append('<div class="' + userIconClass + '"></div>');

                    msgHeader.append('<label class="chatviewer-messagefrom">' + fromUserAlias + '</label>').append('<br />');

                    msgHeader.append('<label class="chatviewer-messagedate">' + (chatDateTime + '</label>'));

                    msgContainer.append('<div class="' + messageBorderClass + '">' + messageBody + '\r\n</div>');
                }
                else if (options.useBlockStyle == false) {
                    msgContainer.append('<div class="' + userIconClass + '"></div>');

                    msgContainer.append('<label class="chatviewer-messagefrom">' + fromUserAlias + '</label>');

                    msgContainer.append('<label class="chatviewer-messagedate">' + (chatDateTime + '</label>'));

                    msgContainer.append('<div class="' + messageBorderClass + '">' + messageBody + '\r\n</div>');
                }

                var messageListEl = $('#chatviewer-messagelist');

                messageListEl.scrollTop(messageListEl[0].scrollHeight);

                if (options.useBlockStyle == false) {
                    $('#chatviewer-messagecolumn').addClass('table');
                    parentEl.addClass('table');
                    $('.chatviewer-messagecontainer').addClass('table');
                    $('.chatviewer-usericon').removeClass('chatviewer-usericon').addClass('chatviewer-usericon-table');
                    parentEl.find('.chatviewer-messagefrom').removeClass('chatviewer-messagefrom').addClass('chatviewer-messagefrom-table');
                    parentEl.find('.chatviewer-messagedate').removeClass('chatviewer-messagedate').addClass('chatviewer-messagedate-table');
                }

                chatMessage.push({fromUserAlias: fromUserAlias, chatDateTime: chatDateTime, messageBody: messageBody});
            }
        };

        var addUserElement = function (resource, usernameStr) {

            var parentEl = $('#chatviewer-userlist');
            var colorClass;

            if (resource == 'agent') {
                colorClass = 'chatviewer-textcolor-agent';
            }
            else if (resource === 'customer') {
                colorClass = 'chatviewer-textcolor-customer';
            }
            else if (resource === 'manager') {
                colorClass = 'chatviewer-textcolor-manager';
            }
            else if (resource === 'admin') {
                colorClass = 'chatviewer-textcolor-admin';
            }

            var userId = usernameStr.replace(/\W+/g, "");

            if ($('div#' + userId).length == 0)
            {
                parentEl.append('<div id="' + userId + '"></div>');

                var userEl = $('div#' + userId);
                userEl.append('<b class="' + colorClass + '">[' + resource + ']</b>');
                userEl.append('<span class="username">' + usernameStr + '</span>');
            }
        };

        var removeUserElement = function (username) {

            var parentEl = $('#chatviewer-userlist');

            parentEl.find('#' + username.replace(/\W+/g, "")).remove();
        };

        var showConnectErrorMsg;

        var onUpdate = function (messages, userUpdates)
        {
            //each public message looks like: { timestamp: '20140925T21:09:52', from: { username: 'ALL', conference: 'conf' }, to: { username: 'user[alias]', resource: 'manager' }, message: 'this is a message'}
            //each private message looks like: { timestamp: '20140925T21:09:52', from: { username: 'user[alias]', resource: 'manager', conference: 'conf' }, to: { username: 'user[alias]', resource: 'manager' }, message: 'this is a message'}

            for (var i = 0; i < messages.length; i++)
            {
                var msg = messages[i];
                var toUserString = msg.to.username == 'ALL' ? 'ALL' : (msg.to.username + ' (' + msg.to.resource + ')');
                var fromUserString = msg.from.username + ' (' + msg.from.resource + ')';
                var chatTimestamp = msg.timestamp;
                var agentTerminated =  msg.from.agentTerminated;

                if (msg.error)
                {
                    log('chatPanel error occurred text "' + msg.error.text + '" code ' + msg.error.code + ' type ' + msg.error.type + ' while receiving message to user ' + toUserString + ' from user ' + fromUserString + ' in conference ' + msg.from.conference + ' message ' + msg.message);
                }
                else
                {
                    log('chatPanel received msg at ' + chatTimestamp + ' to user ' + toUserString + ' from user ' + fromUserString + ' in conference ' + msg.from.conference + ' message ' + msg.message);

                    var fromUserAlias = msg.from.username.substring(msg.from.username.lastIndexOf('[') + 1, msg.from.username.lastIndexOf(']'));
                    addMessageElement(chatTimestamp, msg.to.username, fromUserAlias, msg.from.resource, msg.message);
                    updateUserElementForState(msg.from.resource, fromUserAlias, 'available'); //set compsing icon back to available
                }

                // disables send button if agent terminated chat, else enable button
                if (agentTerminated == true)
                {
                     $('#chatviewer-messagesubmitbutton').attr('disabled', 'disabled');
                     $('#chatviewer-messagebar').attr('disabled', 'disabled');
                }
                else
                {
                    $('#chatviewer-messagesubmitbutton').removeAttr('disabled');
                    $('#chatviewer-messagebar').removeAttr('disabled');
                }
            }

            //each userUpdate looks like { from: { username: 'user[alias]', resource: 'manager', conference: 'conf' }, state: 0|1 }
            for (var i = 0; i < userUpdates.length; i++)
            {
                var userUpdate = userUpdates[i];

                if (userUpdate.error)
                {
                    var user = userUpdate.from.username;

                    //showConnectErrorMsg(userUpdate.error.text);

                    log('chatPanel error occurred text "' + userUpdate.error.text + '" code ' + userUpdate.error.code + ' type ' + userUpdate.error.type + ' while getting user update from user ' + user);
                }
                else
                {
                    log('chatPanel user ' + userUpdate.from.username + ' [' + userUpdate.from.resource + '] in conference ' + userUpdate.from.conference + ' is ' + userUpdate.state);
                    
                    //if available, add to list
                    var usernameStr = userUpdate.from.username.replace('[', ' [');
                    var resource = userUpdate.from.resource;
                    var fromUserAlias = userUpdate.from.username.substring(userUpdate.from.username.lastIndexOf('[') + 1, userUpdate.from.username.lastIndexOf(']'));
                    
					if((userUpdate.to && userUpdate.to.username != 'ALL')){
						//private. Don't use.
						continue;
					}
					
                    switch(userUpdate.state) {

                        case 'unavailable': 
                            removeUserElement(resource, usernameStr);
                            break;
                        case 'available': 
                            addUserElement(resource, usernameStr);
                            break
                        case 'composing': 
							if(resource != 'customer')
								updateUserElementForState(resource, fromUserAlias, userUpdate.state);
                            break;
                        case 'paused': 
							if(resource != 'customer')
								updateUserElementForState(resource, fromUserAlias, userUpdate.state);
                            break;
                    }
                }
            }
        };

        options.updateCallback = onUpdate;
        thisDialog.data('chatWrapper', new WebChatClient({ debug: options.debug, services: options.services, updateCallback: options.updateCallback }));

        thisDialog.data('chatWrapper').loadLocalizationFiles(function(l)
        {
            if (!l || l == undefined) {
                thisDialog.append('<div id="chatviewer-contentarea"></div>');

                var contentAreaEl = thisDialog.find('#chatviewer-contentarea');
                contentAreaEl.append('<div id="chatviewer-loadingarea"></div>');

                var loadingAreaEl = $('#chatviewer-loadingarea');
                loadingAreaEl.append('<img class="error" src="" alt=""></img>');
                loadingAreaEl.append('<br />');
                loadingAreaEl.append('<span id="text">Unable to connect to web chat gateway.</span>');

                return;
            }

            localizeString = l;

            thisDialog.methods().chatWrapper = function()
            {
                return thisDialog.data('chatWrapper');
            };

            showConnectErrorMsg = function(text)
            {
                var loadingAreaEl = $('#chatviewer-loadingarea');

                loadingAreaEl.show();
                loadingAreaEl.find('img').removeClass('loading').addClass('error');
                loadingAreaEl.find('#text').text(localizeString('Unable to connect to web chat gateway session.') + ': ' + text);
                loadingAreaEl.find('#btnCancelLoad').hide();
            };

            var showServerStatusErrorMsg = function(text)
            {
                var serverStatusEl = $('#chatviewer-serverstatus');

                serverStatusEl.find('> img').addClass('disconnected');
                serverStatusEl.find('> span').text(text);
                serverStatusEl.show();
            };

            var clearServerStatusErrorMsg = function()
            {
                var serverStatusEl = $('#chatviewer-serverstatus');

                serverStatusEl.find('> img').removeClass('disconnected');
                serverStatusEl.find('> span').text('');
                serverStatusEl.hide();
            };

            var showLoading = function () {

                var loadingAreaEl = $('#chatviewer-loadingarea');
                var chatPanelEl = $('#chatviewer-chatpanel');
                var conRoomContainerEl = $('#chatviewer-confroom-container');
                var messageBarContainerEl = $('#chatviewer-messagebarcontainer');

                chatPanelEl.hide();
                conRoomContainerEl.hide();
                messageBarContainerEl.hide();
                loadingAreaEl.show();
                loadingAreaEl.find('img').removeClass('error').addClass('loading')
                loadingAreaEl.append('<br />');
                loadingAreaEl.find('#text').text(localizeString('Loading') + '... ' + localizeString('If the page does not load in 5s, please refresh the page.'));
                loadingAreaEl.find('#btnCancelLoad').show();
            };

            var hideLoading = function () {
                $('#chatviewer-loadingarea').hide();
                var contentAreaEl = $('#chatviewer-contentarea');
                contentAreaEl.find('#btnCancelLoad').hide();

                hideLoadingSpinner();
            };

            var hideLoadingSpinner = function () {

                var loadingAreaEl = $('#chatviewer-loadingarea');

                loadingAreaEl.find('img').removeClass('loading');
                loadingAreaEl.find('#text').text('');
                loadingAreaEl.find('#btnCancelLoad').hide();
            };

            var applyDefaultOptions = function()
            {
                var setDefault = function(tgtOptions, optionName, defaultVal)
                {
                    if (tgtOptions[optionName] == undefined)
                    {
                        tgtOptions[optionName] = defaultVal;
                    }
                };

                setDefault(options, 'debug', true);
                setDefault(options, 'services', []);
                setDefault(options, 'user', {});
                setDefault(options, 'chat', {});
                setDefault(options.chat, 'type', '#barge');
                setDefault(options, 'toUser', {}); //might be blank
                setDefault(options, 'fromUser', {}); //might be blank

                //incoming options that need to be supplied:
                setDefault(options.fromUser, 'id', Math.floor(Math.random() * 1000000000000000000000));
                setDefault(options.fromUser, 'name', "NSC_CHAT_SERVICE");
                setDefault(options.fromUser, 'firstName', undefined);
                setDefault(options.fromUser, 'lastName', undefined);
                setDefault(options.fromUser, 'phone', undefined);
                setDefault(options.fromUser, 'resource', "customer");
                setDefault(options.fromUser, 'initDone', false);
                setDefault(options.toUser, 'id', null);
                setDefault(options.toUser, 'name', null);
                setDefault(options.toUser, 'resource', 'undefined');
                setDefault(options.chat, 'conference', null);

                //user defined chat queueId options:
                setDefault(options, 'queueId', undefined);
                setDefault(options, 'queueList', undefined);

                //user defined entry url and ip address
                setDefault(options, 'entryUrl', '');
                setDefault(options, 'terminationUrl', '');
                setDefault(options, 'ipAddress', '');

                // For Demo purposes only, swaps between Harmony chatviewer layout style (false) and table layout (true)
                setDefault(options, 'useBlockStyle', false);
                setDefault(options, 'chatTypingState', {});
                setDefault(options.chatTypingState, 'enabled', false);
                setDefault(options.chatTypingState, 'clearInMs', 30000);

                setDefault(options, 'dateFormat', 'MM/DD/YYYY HH:mm:ss');
            };

            var dialogLoaded = function () {
                // open animation has finished playing
                var contentAreaEl = thisDialog.find('#chatviewer-contentarea');
                var loadingAreaEl = $('#chatviewer-loadingarea');

                loadingAreaEl.css('z-index', 100000);
                loadingAreaEl.css('width', contentAreaEl.css('width'));
                loadingAreaEl.css('height', contentAreaEl.css('height'));
            };

            var buildDialog = function () {

                thisDialog.removeClass(); // remove all css from the div this widget was called on.

                // use our own CSS on the container div
                thisDialog.css({ "position": "relative", "width": "100%", "height": "100%", "min-height": "400px" });

                thisDialog.append('<div id="chatviewer-contentarea"></div>');

                var contentAreaEl = thisDialog.find('#chatviewer-contentarea');
                contentAreaEl.append('<div id="chatviewer-loadingarea"></div>');

                var loadingAreaEl = $('#chatviewer-loadingarea');
                loadingAreaEl.append('<img src="" alt=""></img>');
                loadingAreaEl.append('<span id="text"></span>');
                loadingAreaEl.append('<br/>');
                loadingAreaEl.append('<button type="text" id="btnCancelLoad" class="k-input">' + localizeString('Cancel') + '</button>')

                showLoading();

                    contentAreaEl.append('<div id="chatviewer-confroom-container"></div>');
                    var confRoomContainerEl = $('#chatviewer-confroom-container');
                    confRoomContainerEl.append('<div id="chatqueue-picker"></div>');
                    confRoomContainerEl.append('<div id="chatqueue-form"></div>');

                contentAreaEl.append('<div id="chatviewer-chatpanel"></div>');

                var chatPanelEl = $('#chatviewer-chatpanel');

                chatPanelEl.append('<div id="chatviewer-serverstatus"></div>');
                $('#chatviewer-serverstatus').append('<img src="" alt=""></img>');
                $('#chatviewer-serverstatus').append('<span></span>');
                $('#chatviewer-serverstatus').hide();

                chatPanelEl.append('<div id="chatviewer-usercolumn"></div>');

                var userColumnEl = $('#chatviewer-usercolumn');
                userColumnEl.append('<div class="chatviewer-columntitle"></div>');

                userColumnEl.find(' > .chatviewer-columntitle').append('<b>' + localizeString('Users') + '</b>');
                userColumnEl.append('<div id="chatviewer-userlist"></div>');

                chatPanelEl.append('<div id="chatviewer-messagecolumn"><div>');
                var messageColumnEl = $('#chatviewer-messagecolumn');
                messageColumnEl.append('<div class="chatviewer-columntitle"></div>');
                messageColumnEl.find(' > .chatviewer-columntitle').append('<b>' + localizeString('Messages') + '</b>');

                // actions buttons
                messageColumnEl.find(' > .chatviewer-columntitle').append('<div id="chatviewer-columntitle-actions"></div>');
                var actionsBar = $('#chatviewer-columntitle-actions');

                actionsBar.append('<button id="chatviewer-printbutton">' + localizeString('Print') + '</button>');
                //actionsBar.append('<button id="chatviewer-sendemailbutton">' + localizeString('Send Email') + '</button>');
                actionsBar.append('<button id="chatviewer-closebutton">' + localizeString('Close') + '</button>');

                messageColumnEl.append('<div id="chatviewer-messagelist"></div>');

                chatPanelEl.append('<div id="chatviewer-messagebarcontainer"></div>');

                var messageBarContainerEl = $('#chatviewer-messagebarcontainer');
                messageBarContainerEl.append('<input id="chatviewer-messagebar" type="text" maxlength="255" name="message" placeholder="' + localizeString('Enter message...') + '">');
                messageBarContainerEl.append('<button id="chatviewer-messagesubmitbutton" disabled="true">' + localizeString('Send') + '</div>'); // disable button on start (enable after typing)
                //messageBarContainerEl.append('<button id="chatviewer-sendemailbutton">' + localizeString('Send Email') + '</button>');
                //messageBarContainerEl.append('<br />').append('<button id="chatviewer-closebutton">' + localizeString('Close') + '</button>');

                var sendMessage = function () {
                    var msgBody = $('#chatviewer-messagebar').get(0).value;

                    thisDialog.methods().chatWrapper().sendMessageToConference(msgBody, options.chat.conference, function(data)
                    {
                        if (data.error)
                        {
                            showServerStatusErrorMsg(data.error.text);
                        }
                        else
                        {
                            clearServerStatusErrorMsg();
                        }
                    });

                    $('#chatviewer-messagebar').get(0).value = '';
                };

                var sendState = function (state) {
                    
                    thisDialog.methods().chatWrapper().sendChatStateToConference(state, options.chat.conference, function(data)
                    {
                        if (data.error)
                        {
                            showServerStatusErrorMsg(data.error.text);
                        }
                        else
                        {
                            clearServerStatusErrorMsg();
                        }
                    });
                };
                
                $('#chatviewer-printbutton').on('click', function () {
    
                    log('chatPanel print button clicked');
                    window.print();
                });

                $('#chatviewer-messagesubmitbutton').on('click', function () {
                    log('chatPanel submit button clicked');
                    
                    if ($('#chatviewer-messagebar').get(0).value.length > 0)
                        sendMessage();
                });

                $('#chatviewer-sendemailbutton').on('click', function () {
                    log('chatPanel send email button clicked');

                    var tableRows = '';
                    for (var i = 0; i < chatMessage.length; i++)
                    {
                        tableRows = tableRows  +  
                                    '<tr><td>' + chatMessage[i].fromUserAlias +
                                    '</td><td>' + chatMessage[i].chatDateTime + 
                                    '</td><td>' +  chatMessage[i].messageBody + 
                                    '</td></tr>';
                    }

                    var tableHead = '<table class="gridtable">' +
                        '<tr><th>User</th><th>Date Time</th><th>Message</th></tr>';

                    var tableTail = '</table>';

                    var allformattedMsgs = tableHead + tableRows + tableTail;

                    var style = '<head> <style type="text/css">' + simpleMinifiedTableCssFormat + '</style></head>';

                    var formattedMessageListHtml = '<html>' + style + allformattedMsgs +  '</html>';

                    thisDialog.methods().chatWrapper().sendEmail(options.fromUser.email, formattedMessageListHtml, function(data)
                    {
                        if (data.error)
                        {
                            showServerStatusErrorMsg(localizeString('Error sending web chat transcript email.') + ': ' + data.error.text);
                        }
                        else
                        {
                            clearServerStatusErrorMsg();
                        }
                    });  
                });

                $('#chatviewer-closebutton').on('click', function ()
                {
                    if (confirm("Really close this chat window?")) {
                        log('chatPanel close button clicked');
                        var confRoomContainerEl = $('#chatviewer-confroom-container');
                        var chatPanelEl = $('#chatviewer-chatpanel');
                        var messageBarContainerEl = $('#chatviewer-messagebarcontainer');

                        var messagesContainerEl = $('#chatviewer-messagelist');

                        thisDialog.methods().chatWrapper().leaveConference(options.chat.conference, options.fromUser, options.terminationUrl, true, function (data) {
                            if (data.error) {
                                log('chatPanel error occurred leaving conference ' + options.chat.conference + ': ' + data.error.text);
                            }

                            if (chatPanelFormConfig.recaptchaEnabled && grecaptcha) {
                                grecaptcha.reset();
                                $('#chatviewer-form-submit').prop("disabled", true);
                            }

                            options.chat.conference = null;
                            chatMessage = [];

                            $('#chatviewer-loadingarea').find('span#text').text('');

                            chatPanelEl.hide();
                            messageBarContainerEl.hide();
                            messagesContainerEl.empty();
                            confRoomContainerEl.show();
                        });

                        refreshTimerID = setInterval(refreshFunc, refreshTimerInterval);
                        window.close();
                    }
                });
                
                $('#chatviewer-messagebar').on('keyup', function(e) { // disables send button if messagebar blank, else enable button
                    $('#chatviewer-messagesubmitbutton').prop('disabled', $(this).val().length == 0);
                });

                $(window).bind("beforeunload", function() { 
                    chatMessage = [];
                    if(options.chat.conference) {
                        thisDialog.methods().chatWrapper().leaveConference(options.chat.conference, options.fromUser, options.terminationUrl, true);
                    }
                });            
                    

                var composingPausedTimeout;
                var keyPress = function(e) {
                    var code = e.keyCode || e.which; 
                      if (code  == 13) 
                      {               
                        log(e.target.id);
                        e.preventDefault();
                        //enter key pressed, send msg
                        if ($(this).val().length > 0)
                            sendMessage();
                      }		
                      else if (options.chatTypingState.enabled) {
                        clearTimeout(composingPausedTimeout);
                        sendState('composing');
                        composingPausedTimeout = setTimeout(function() { sendState('paused'); }, options.chatTypingState.clearInMs);
                    }		
                };

                var throttledKeyPress = _.throttle(keyPress, 1000);

                $('#chatviewer-messagebar').on('keypress', throttledKeyPress);

                $(window).resize();
            };

            var connectToXmppServerAndJoinRoom = function ()
            {
                var socketIoStatusCallback = function(msg)
                {
                    showServerStatusErrorMsg(localizeString(msg));
                };

                var xmppServerStatusCallback = function(data)
                {
                    if (data.state == '#connected')
                    {
                        clearServerStatusErrorMsg();
                    }
                    else if (data.state == '#disconnected')
                    {
                        showServerStatusErrorMsg(data.message);
                    }
                };

                thisDialog.methods().chatWrapper().connect(options.fromUser, socketIoStatusCallback, xmppServerStatusCallback, function (data)
                {
                    if (data.error) {
                        log('chatPanel An error occurred while connecting.');
                    }

                    if (data.status == '#connfail') 
                    {
                        log('chatPanel failed to connect.');
                    }
                    else if (data.status == '#disconnected') 
                    {
                        log('chatPanel disconnected.');
                    }
                    else if (data.status == '#connected') 
                    {
                        log('chatPanel connected.');

                        thisDialog.methods().chatWrapper().listAllConferences(function (data)
                        {
                            if (data.error) 
                            {
                                return log('list all conferences error occurred, code ' + data.error.code + ' type ' + data.error.type);
                            }

                            if (options.chat.conference || options.queueId)
                            {
                                var foundConference;

                                for (var i = 0; i < data.conferences.length; i++)
                                {
                                    var item = data.conferences[i];
                                    var queueId = item.id;
                                    var itemName = item.name;

                                    if (itemName == options.chat.conference || queueId == options.queueId)
                                    {
                                        foundConference = item;
                                        options.chat.conference = itemName; // if options.queueId is specified but options.chat.conference isn't
                                        
                                        if (item.state.toUpperCase() == "CLOSED" || item.state.toUpperCase() == "BADUSERCONFIGURATION") 
                                        {
                                            log('chatPanel found conference ' + itemName + ', but conference is closed.');
                                            showConnectErrorMsg(localizeString('The conference room') + '"' + item.description + '" ' + localizeString('is closed :') + item.closedMsg);
                                            return;
                                        }
                                        
                                        if (options.fromUser.initDone != true) 
                                        {
                                             // if user isn't defined, show chatviewer-form plugin
                                            log('chatPanel found conference ' + itemName + ', showing user data form...');
                                            
                                            var chatUserInfoForm = $('#chatqueue-form');                                            
                                            chatUserInfoForm.empty();
                                            chatUserInfoForm.chatPanelForm(chatPanelFormConfig);
                                            
                                            hideLoading();
                                            $('#chatviewer-confroom-container').show().before('<h2 style="text-align:center;">' + localizeString('Joining') + ' ' + item.description + '</h2>');
                                            
                                            chatUserInfoForm.off('formsubmit').on('formsubmit', function(e, data) {
                                                e.stopPropagation();
                                                parseFormDataAndJoin(data, foundConference);                                    
                                            });
                                        }
                                        else 
                                        {
                                            
                                            log('chatPanel found conference ' + itemName + ', joining...');

                                            var collectedData = 
                                            {
                                                firstName: options.fromUser.firstName,
                                                lastName: options.fromUser.lastName,
                                                email: options.fromUser.email,
                                                phone: options.fromUser.phone,
                                                chatData1: chatPanelFormConfig.userDefinedData.chatData1,
                                                chatData2: chatPanelFormConfig.userDefinedData.chatData2,
                                                chatData3: chatPanelFormConfig.userDefinedData.chatData3,
                                                chatData4: chatPanelFormConfig.userDefinedData.chatData4,
                                                entryUrl: options.entryUrl
                                            };

                                            parseFormDataAndJoin(collectedData, foundConference);  

                                        }
                                    }
                                }

                                if (!foundConference)
                                {
                                    log('chatPanel error - could not find conference ' + (options.queueId ? 'queueId ' + options.queueId : options.chat.conference));

                                    showConnectErrorMsg(localizeString('Could not find conference ') + (options.queueId ? 'queueId ' + options.queueId : options.chat.conference));
                                }
                            }
                            else
                            {
                                hideLoading();

                                //let user choose a conference
                                var confRoomContainerEl = $('#chatviewer-confroom-container');
                                var chatQueuePicker = $('#chatqueue-picker');
                                var chatUserInfoForm = $('#chatqueue-form');

                                var conference = data.conferences.length > 0 ? data.conferences[0] : null;
                                var conferencesToShow = filterConferenceList(data.conferences, options.queueList);
                                
                                if (!conferencesToShow || conferencesToShow.length < 1) {
                                    showConnectErrorMsg(localizeString('Could not load conference list.'));
                                    return;
                                }

                                confRoomContainerEl.show();

                                chatQueuePicker.empty();
                                chatQueuePicker.chatPanelQueue($.extend({ conferences: conferencesToShow }, chatPanelQueueConfig));
								
								//add setInterval to refresh
								if(refreshTimerID == null){
									refreshTimerID = setInterval(
									refreshFunc,
									refreshTimerInterval);								
								}
                                
                                chatUserInfoForm.empty();
                                chatUserInfoForm.chatPanelForm(chatPanelFormConfig);
                                chatUserInfoForm.hide(); // hide on first load

                                chatQueuePicker.off('queueChange').on('queueChange', function(e, data) {
                                    conference = data.conference;
                                });

                                chatQueuePicker.off('showUserInfoForm').on('showUserInfoForm', function (e, data) {
                                    //chatUserInfoForm.methods.show();
                                    chatUserInfoForm.show();
                                    //$(function () {
                                    //    if (window.location.origin.includes("beta.mycornerstoneloan.org")) {
                                    //        $('#inputFirstName').val('UI Test');
                                    //        $('#inputLastName').val('Please Ignore');
                                    //        $('#inputChatData1').val('1/1/1990');
                                    //    }
                                    //});
									if(refreshTimerID){
										clearInterval(refreshTimerID);
										refreshTimerID = null;
									}
                                });

                                chatQueuePicker.off('hideUserInfoForm').on('hideUserInfoForm', function (e, data) {
                                    //chatUserInfoForm.methods().hide();
                                    chatUserInfoForm.hide();
									
									if(!refreshTimerID){
										refreshTimerID = setInterval(refreshFunc, refreshTimerInterval);
									}
                                });

                                chatUserInfoForm.off('formsubmit').on('formsubmit', function(e, data) {
                                    e.stopPropagation();
									
									if(refreshTimerID){
										clearInterval(refreshTimerID);
										refreshTimerID = null;
									}
									
                                    conference = chatQueuePicker.methods().getSelectedQueue();
                                    parseFormDataAndJoin(data, conference);                                    
                                });
                                    
                                $('#btnCancelLoad').off('click').on('click', function()
                                {
                                    hideLoading();
                                    thisDialog.data('chatWrapper').cancelCustomerWaitForConference(options.chat.conference, options.fromUser, options.terminationUrl, false);
                                    options.chat.conference = null;
                                    confRoomContainerEl.show();
                                });
                            }
                        });
                    }
                    else if (data.status == '#connecting') {
                        log('chatPanel connecting...');
                    }
                });
            };
            
            var parseFormDataAndJoin = function(data, conference) 
            {
                var reqInfo;

                options.fromUser.id = data.lastName + (new Date()).getMilliseconds() + data.firstName;
                options.fromUser.firstName = data.firstName;
                options.fromUser.lastName = data.lastName;
                options.fromUser.email = data.email;
                options.fromUser.phone = data.phone;
                options.fromUser.resource = "customer";
                options.recaptchaData = data.recaptchaData;
                
                log('chatPanel selected customer conference ' + conference + ', joining...');

                showLoading();

                options.chat.conference = conference.name;

                thisDialog.methods().chatWrapper().joinConferenceAndWaitOnCustomerConference(options.chat.conference, options.fromUser, options.recaptchaData, reqInfo, function(data1)
                {                 
                    var confRoomContainerEl = $('#chatviewer-confroom-container');
                    
                    if (data1 && data1.error)
                    {
                        log('chatPanel error occurred joining conference ' + conference.name + ': ' + data1.error.text);
                        hideLoading();
                        options.chat.conference = null;
                        confRoomContainerEl.show();
                    }
                    else
                    {
                        this.conference = data1.conference;
                        var termUrl; // Note is leaving channel to join back his conference so we do not send termUrl private message to service

                        thisDialog.methods().chatWrapper().leaveConference(options.chat.conference, options.fromUser, termUrl, false, function(data2)
                        {
                            if (data2 && data2.error)
                            {
                                log('chatPanel error occurred leaving conference ' + conference.name + ': ' + data2.error.text);

                                hideLoading();
                                options.chat.conference = null;
                                confRoomContainerEl.show();
                            }
                            else
                            {
                                reqInfo = 
                                {
                                    filler1: data.chatData1,
                                    filler2: data.chatData2,
                                    filler3: data.chatData3,
                                    filler4: data.chatData4,
                                    entryUrl: data.entryUrl
                                };

                                options.chat.conference = this.conference.name;

                                thisDialog.methods().chatWrapper().joinConference(this.conference.name, options.fromUser, options.entryUrl, options.ipAddress, reqInfo, function(data3)
                                {
                                    if (data3 && data3.error)
                                    {
                                        log('chatPanel error occurred joining customer conference ' + conference.name + ': ' + data3.error.text);
                                    }
                                    else
                                    {
                                        updateUiElements(conference.emailEnabled);
                                    }
                                });
                            }
                        });
                    }
                });
            };
            
            /* This does an intersection of "data.conferences" and user-defined "options.queueList" based on the "queueId" property
                left-over items in user-defined queueList (meaning user defined a queueId that the server doesn't have),
                will still be included but will display an error to use when attempting to select */
            var filterConferenceList = function(serverList, userList) {
                if (!userList || !userList.length > 0)
                    return serverList;
                
                var queueIds = [];
                for (var i = 0; i < userList.length; i++)
                    queueIds.push(userList[i].queueId);

                conferencesToShow = [];

                // do intersection of "data.conferences" and user-defined "options.queueList" based on the "queueId" property
                conferencesToShow = serverList.filter(function(conf) { 
                    var queueId = conf.id;

                    var foundItem = userList.filter(function(item, index) { 
                        if (item.queueId == queueId) {
                            userList.splice(index, 1); // remove found item from options.queueList
                            return true;
                        } 
                    });

                    if (foundItem && foundItem.length > 0) {
                        conf.description = foundItem[0].description; // replace matched item's description with user's description
                        return true;
                    }
                });

                // if there are leftover user-defined queueIds, include them anyways but as "error" state
                if (userList.length > 0) { 
                    for (var i = 0; i < userList.length; i++) {
                        var errorMsg = localizeString("Unable to find queue: '") + userList[i].queueId + " / " + userList[i].description + localizeString("' in the system configuration. Please contact system administrator.");
                        conferencesToShow.push({
                            id: userList[i].queueId,
                            description: userList[i].description,
                            state: "BadUserConfiguration",
                            closedMsg: errorMsg
                        });
                    }
                }
                
                return conferencesToShow;
            };



            var updateUiElements = function (emailEnabled) {

                var chatPanelEl = $('#chatviewer-chatpanel');
                var messageBarContainerEl = $('#chatviewer-messagebarcontainer');
                var sendEmailEl = $('#chatviewer-sendemailbutton');

                hideLoading();
                chatPanelEl.show();
                messageBarContainerEl.show();

                if (emailEnabled)
                {
                    sendEmailEl.show();
                }
                else
                {
                    sendEmailEl.hide();
                }
            };

            applyDefaultOptions();

            buildDialog();
            showLoading();

            connectToXmppServerAndJoinRoom();
        });
    };
})
(jQuery);



