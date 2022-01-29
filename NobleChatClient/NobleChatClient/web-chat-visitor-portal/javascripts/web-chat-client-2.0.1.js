/*
* Noble Systems Web Chat Visitor Portal Web-Chat-Client v2.0.1
*
* Copyright (c) 2017 Noble Systems Corporation
*/

//this is the client interface to the web chat interface on the server
var connectionId;
var socket;

var WebChatClient = function (options) {
    var self = this;

    this.debug = options.debug;
    this.services = options.services;

    this.log = function (msg, data) {
        if (this.debug) {
            if (data) {
                console.log(msg, data);
            }
            else {
                console.log(msg);
            }
        }
    };

    this.updateCallback = options.updateCallback;
    this.log('WebChatClient initialized.');
};

var loopToFindService = function(self, continueCallback, failCallback)
{
    var total = self.services.length;
    var count = 0;

    //loop through services and connect to the one that gives us a successful ping
    var loop = function()
    {
        var service = self.services[count];
        var chatAppUrl = (service.useSsl ? 'https' : 'http') + '://' + service.webChatGatewayHost + ':' + service.webChatGatewayPort;
        service.chatApiUrl = chatAppUrl + '/chatApi/'

        //required for ie10 or it doesn't even send the ajax call
        $.support.cors = true;

        var ping = $.ajax({
          url: chatAppUrl,
          timeout: 10000
        });

        count++;
            
        ping.done(function(page, code) {
            self.log('webChatClient successful ping of service at url ' + chatAppUrl);

            continueCallback(service);
        })
          .fail(function(obj, code) {
            self.log('webChatClient warning: cannot ping service at url ' + chatAppUrl);

            if (count < total)
            {
               loop(); 
            }
            else
            {
                failCallback();
            }
          });
    };

    loop();
};

WebChatClient.prototype.loadLocalizationFiles = function(callback)
{
    var self = this;

    var getLocalization = function(service)
    {
        var url = (service.useSsl ? 'https' : 'http') + '://' + service.webChatGatewayHost + ':' + service.webChatGatewayPort + '/locales/{{lng}}/{{ns}}.json';
        var backendOptions = {
            loadPath: url,
            crossDomain: true,
            withCredentials: false
        };

        i18next.use(i18nextBrowserLanguageDetector).use(i18nextXHRBackend);

        i18next.init({
                debug: true,
                fallbackLng: 'es-US',
                load: 'currentOnly', //just load the detected language
                backend: backendOptions
            }, function(err, l) { callback(l); });
    };

    loopToFindService(this, getLocalization, function(err)
    {
        self.log('webChatClient could not find a valid service for localization.');
        callback();
    });
};

WebChatClient.prototype.connect = function (fromUser, socketIoStatusCallback, xmppServerStatusCallback, callback) {
    var self = this;

    var setupSocketIOAndPostConnect = function(service)
    {
        //we communicate via socket io on a 1:1 channel with the namespace of the user logged in
        socket = io.connect((service.useSsl ? 'https' : 'http') + '://' + service.webChatGatewayHost + ':' + service.webChatGatewayPort + '/' + fromUser.id + '/' + fromUser.resource);

        self.service = service;
        
        self.log('webChatClient connect sent from: ', fromUser);

        socket.on('connect', function (s) {
            self.log('webChatClient connected to socket io (' + this.io.engine.id + ').');
        });

        socket.on('disconnect', function (s) {
            self.log('webChatClient disconnected from socket io (' + this.io.engine.id + ').');
            socketIoStatusCallback('The browser has been disconnected from the chat gateway.');
        });

         socket.on('xmpp status', function (data) {
             self.log('webChatClient received xmpp status through socket io (' + this.io.engine.id + '): ', data.state);
             xmppServerStatusCallback(data);
        });

        socket.on('chat message', function (msg) {
            self.log('webChatClient received through socket io (' + this.io.engine.id + ') message: ', msg);
            self.updateCallback([msg], []);
        });

        socket.on('chat presence', function (msg) {
            self.log('webChatClient received through socket io (' + this.io.engine.id + ') presence: ', msg);
            self.updateCallback([], [msg]);
        });

        socket.on('chat status', function (msg) {
            self.log('webChatClient received through socket io (' + this.io.engine.id + ') status: ', msg);
            self.updateCallback([], [msg]);
        });

        $.post(service.chatApiUrl + "connect", { connectionId: connectionId, user: fromUser }, function (data) {
            connectionId = data.connectionId;

            callback(data);
        });
    };

    loopToFindService(self, setupSocketIOAndPostConnect, function(err)
    {
        self.log('webChatClient could not find a valid service for connect.');
    });

};

var sendMessage = function(self, message, paramObject, callback)
{
    var response = $.post(self.service.chatApiUrl + message, paramObject, function (data) {
        if (callback)
        {
            callback(data);
        }
    });

    response.fail(function(data) {
        if (callback && data.responseText)
        {
            callback(JSON.parse(data.responseText));
        }
    });
};

WebChatClient.prototype.disconnect = function (callback) {
    sendMessage(this, "disconnect", { connectionId: connectionId }, callback);
};

WebChatClient.prototype.listAllConferences = function (callback) {
    sendMessage(this, "listAllConferences", { connectionId: connectionId }, callback);
};

WebChatClient.prototype.sendCustomerMessage = function (fromUser, callback) {
    sendMessage(this, "sendCustomerMessage", { connectionId: connectionId, user: fromUser }, callback);
};

WebChatClient.prototype.joinConference = function (conferenceName, fromUser, entryUrl, ipAddress, reqInfo, callback) {
    sendMessage(this, "joinConference", { connectionId: connectionId, conferenceName: conferenceName, user: fromUser, entryUrl: entryUrl, ipAddress: ipAddress, userRequestInfo: reqInfo  }, callback);
};

WebChatClient.prototype.joinConferenceAndWaitOnCustomerConference = function (conferenceName, fromUser, recaptchaData,  reqInfo, callback) {

    sendMessage(this, "joinConferenceAndWaitOnCustomerConference", { connectionId: connectionId, conferenceName: conferenceName, user: fromUser, recaptchaData: recaptchaData, userRequestInfo: reqInfo }, callback);
};

WebChatClient.prototype.cancelCustomerWaitForConference = function (conferenceName, fromUser, termUrl, sendHeaders, callback) {
    sendMessage(this, "cancelCustomerWaitForConference", { connectionId: connectionId, conferenceName: conferenceName, user: fromUser, termUrl: termUrl, sendHeaders: sendHeaders  }, callback);
};

WebChatClient.prototype.leaveConference = function (conferenceName, fromUser, termUrl, sendHeaders, callback) {
    sendMessage(this, "leaveConference", { connectionId: connectionId, conferenceName: conferenceName, user: fromUser, termUrl: termUrl, sendHeaders: sendHeaders }, callback);
};

WebChatClient.prototype.sendMessageToConference = function (message, conferenceName, callback) {
    sendMessage(this, "sendMessageToConference", { connectionId: connectionId, message: message, conferenceName: conferenceName }, callback);
};

WebChatClient.prototype.sendPrivateMessageToUser = function (message, conferenceName, toUser, callback) {
    sendMessage(this, "sendPrivateMessageToUser", { connectionId: connectionId, message: message, conferenceName: conferenceName, user: toUser }, callback);
};

WebChatClient.prototype.sendChatStateToConference = function (state, conferenceName, callback) {
    sendMessage(this, "sendChatStateToConference", { connectionId: connectionId, state: state, conferenceName: conferenceName }, callback);
};

WebChatClient.prototype.sendChatStateToUser = function (state, conferenceName, toUser, callback) {
    sendMessage(this, "sendChatStateToUser", { connectionId: connectionId, state: state, conferenceName: conferenceName, user: toUser }, callback);
};

WebChatClient.prototype.sendEmail = function (emailAddress, messageList, callback) {
    sendMessage(this, "sendEmail", { connectionId: connectionId, emailAddress: emailAddress, messageList: messageList }, callback);
};
