/*
* Noble Systems Web Chat Visitor Portal Chatviewer Queue v1.0.0
*
* Copyright (c) 2014 Noble Systems Corporation
*/

(function ($) {
    
    "use strict";
    
    $.fn.chatPanelQueue = function(userOptions) {
        
        var options = $.extend(true, {}, $.fn.chatPanelQueue.defaultOptions, userOptions);
        
        var container = $(this);
        
        var htmlStructure =
            '<div id="chatviewer-queue-container">' +
                '<table id="chatviewer-queue-table">' +
                    '<tbody>' +
                        '<tr>' +
                            '<td colspan="2"><label id="chatviewer-queue-welcome-message">' + options.welcomeMessage + '</label></td>' +
                        '</tr>' +
                        '<tr id="confroom-selection-row">' +
                            '<td colspan="2"><select id="chatviewer-queue-selection-picker"></select></td>' +
                        '</tr>' +
                        '<tr id="chatviewer-queue-message-row" style="display:none; font-size: 16px; color: #00457b;">' +
                            '<td><label style="font-weight: bold;">Message: </label></td>' +
                            '<td><label id="chatviewer-queue-message-label"></label></td>' +
                        '</tr>' +
                    '</tbody>' +
                '</table>' +
            '</div>';
        
        if (!localizeString)
            var localizeString = function(string) { return string; } // if function doesn't exist, just return same text instead of throw error
        
        var conferences = {};
        var selectedConference = options.conferences.length > 0 ? options.conferences[0] : null;
        
        function init() {
            container.append(htmlStructure);
            
            var conferencePicker = $('#chatviewer-queue-selection-picker');
            conferencePicker.append('<option value="-1">' + localizeString(options.placeholderItemText) + '</option>'); // placeholder item

            for (var i = 0; i < options.conferences.length; i++) {
                var item = options.conferences[i];

				if(item.type == "dnis") {

					conferences[item.id] = item;

					if (conferencePicker.find('option[value="'+ item.id + '"]').length == 0)
					{
						var listItem = $('<option value="' + item.id + '">' + item.description + '</option>');
						listItem.data('conferenceRoom', item);
						conferencePicker.append(listItem);
					}
				}
            }
            
            conferencePicker.data(conferences).on('change', function(e) {
                selectedConference = $(this).data()[this.value];
                container.trigger('queueChange', { conference: selectedConference });

                if (!selectedConference) {
                    $('#chatviewer-queue-message-row').hide();
                    $('#chatviewer-queue-message-label').text('');

                    container.trigger('hideUserInfoForm');
                    return;
                }

                if (selectedConference.state.toUpperCase() != "CLOSED") {                    
                    $('#chatviewer-queue-message-row').hide();
                    $('#chatviewer-queue-message-label').text('');

                    container.trigger('showUserInfoForm');
                }
                else {
                    $('#chatviewer-queue-message-row').show();
                    $('#chatviewer-queue-message-label').text(localizeString(selectedConference.closedMsg));

                    container.trigger('hideUserInfoForm');
                }                
            });
        }
        
        function init2() {            

            container.append('<div id="chatviewer-queue-container"></div>');            
            
            var queueContainer = container.find('#chatviewer-queue-container');
            
            queueContainer.append('<div id="chatviewer-queue-welcome-message" style="margin-bottom:16px">' + options.welcomeMessage + '</div>');
            
            for (var i = 0; i < options.conferences.length; i++) {

                var item = options.conferences[i];
                
                if(item.type == "dnis") {
                    queueContainer.append(buildQueueCard(options.conferences[i]));
                }
            }
            
            $('.queue-card-chatbutton').click(function(e) {
                e.stopPropagation();
                if ($('.chat-queue-card.disabled').length > 0) { // if there already exists disabled cards, this means a card is already selected, so we will deselect it
                    $('.chat-queue-card').removeClass('disabled');
                    $('#chatviewer-queue-container').find('#chatviewer-queue-message-row').remove();
                    
                    container.trigger('hideUserInfoForm');
                    return;
                }
                
                var thisCard = $(this).closest('.chat-queue-card'); // get this card
                
                $('.chat-queue-card').addClass('disabled'); // disable all cards
                thisCard.removeClass('disabled'); // un-disable this card
                selectedConference = thisCard.data('conferenceRoom');
                
                if (!selectedConference || 
                    selectedConference.state.toUpperCase() == "BADUSERCONFIGURATION") {
                    var text = localizeString('Unable to find matching queueId "') + selectedConference.id + ' / ' + selectedConference.description + localizeString('" in the system configuration. Please contact system administrator.');
                    $('#chatviewer-queue-container').append($('<div id="chatviewer-queue-message-row">').text(text));
                    container.trigger('hideUserInfoForm');
                    return;
                }
                
                container.trigger('queueChange', { conference: selectedConference });

                if (selectedConference.state.toUpperCase() != "CLOSED")
                    container.trigger('showUserInfoForm');
                else 
                    container.trigger('hideUserInfoForm');
            });
            
            $('.chat-queue-card').click(function(e) {
                if ($(this).hasClass('disabled')) {
                    $('.chat-queue-card').removeClass('disabled');
                    $('#chatviewer-queue-container').find('#chatviewer-queue-message-row').remove();
                    container.trigger('hideUserInfoForm');
                }
            });
        }
        
        function buildQueueCard(item) {            
            var chatQueueCard = $('<div class="chat-queue-card" title="' + item.closedMsg + '"+>');
            chatQueueCard.data('conferenceRoom', item);
            
            var chatQueueCardStatusBar = $('<div class="queue-card-statusbar"></div>');            
            chatQueueCardStatusBar.removeClass('open closed error');
            
            if (item.state.toUpperCase() == "BADUSERCONFIGURATION")
                chatQueueCardStatusBar.addClass('error'); 
            else if (item.state.toUpperCase() != "OPEN")
                chatQueueCardStatusBar.addClass('closed');
            else
                chatQueueCardStatusBar.addClass('open');
            
            var chatQueueCardIconDiv = $('<div class="queue-card-icon"></div>');
            checkAndBuildIcon(item.id, chatQueueCardIconDiv);

            var chatQueueCardName = $('<div class="row-with-icon queue-card-name"></div>');
            chatQueueCardName.text(item.description);
            
            var chatQueueCardStatus = $('<div class="row-with-icon queue-card-status"></div>');
            chatQueueCardStatus.text(item.state.toUpperCase() == "CLOSED" ? localizeString("Offline") : localizeString("Online"));
            if (item.state.toUpperCase() == "BADUSERCONFIGURATION") {
                chatQueueCardStatus.text(localizeString("Error"));   
            }
            
            var chatQueueCardMessage = $('<div class="row-with-icon queue-card-message"></div>');
            chatQueueCardMessage.text(item.closedMsg);
            if (item.state.toUpperCase() != "CLOSED" && item.state.toUpperCase() != "BADUSERCONFIGURATION") {
                chatQueueCardMessage.css('visibility', 'hidden');
                chatQueueCard.attr('title', '');
            }
            
			var idTag = 'id=' + item.id;
            var chatQueueCardChatButton = $('<div class="row-with-icon queue-card-actions"></div>');
            chatQueueCardChatButton.append('<div class="queue-card-chatbutton" ' + idTag + '><img class="chatbutton" />Chat Now</div>');
            if (item.state.toUpperCase() == "CLOSED" || item.state.toUpperCase() == "BADUSERCONFIGURATION")
                chatQueueCardChatButton.css('visibility', 'hidden');            
            
            chatQueueCard.append(chatQueueCardStatusBar);
            chatQueueCard.append(chatQueueCardIconDiv);
            chatQueueCard.append(chatQueueCardName);
            chatQueueCard.append(chatQueueCardStatus);
            chatQueueCard.append(chatQueueCardMessage);            
            chatQueueCard.append(chatQueueCardChatButton);
            
            return chatQueueCard;
        }
        
        function checkAndBuildIcon(queueId, container) {
            var queueIconPath = src = window.location.href.substring(0, window.location.href.lastIndexOf('/')) + '/' + options.queueIconBaseUrl;
            var src = queueIconPath + queueId + '.png';
            
            try {
                    var img = document.createElement('img');
                    img.width = "64";
                    img.height = "64";
                    img.onload = function(e) {
                        return container.append('<img src="' + src + '" width="64" height="64" />');
                    };
                    img.onerror = function(e) {
                        if (src.substr(-4) == '.jpg')
                            return;
                        
                        src = queueIconPath + queueId + '.jpg';
                        img.src = src; // try again with .jpg file
                    };
            } catch (e) {
                return;
            }            
            
            img.src = src;
        }
        
        container.methods().getSelectedQueue = function() {
            return selectedConference;
        };
        
        if (options.useCardStyle == true)
            init2();
        else
            init();
    };
    
    $.fn.chatPanelQueue.defaultOptions = {
        conferences: [],
        welcomeMessage: "How can we help you:",
        placeholderItemText: "-- Please select one --",
        useCardStyle: true
    };
    
})(jQuery);