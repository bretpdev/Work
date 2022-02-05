/*
* Noble Systems Web Chat Visitor Portal Chatviewer Form v1.0.0
*
* Copyright (c) 2014 Noble Systems Corporation
*/

(function ($) {
    
    "use strict";
    
    $.fn.chatPanelForm = function(userOptions) {
        
        var options = $.extend(true, {}, $.fn.chatPanelForm.defaultOptions, userOptions);
        
        var container = $(this);

        var htmlStructure =
            '<form id="chatviewer-form-container">' +
            
                '<table id="chatviewer-form-userinfo" class="chatviewer-form">' +
            
                    '<tr>' +
                        '<td><label for="firstname">' + options.labels.firstName + '</label></td>' +
                        '<td>' +
                            '<input id="inputFirstName" name="inputFirstName" type="text" required />' + 
                        '</td>' +
                    '</tr>' +
                    
                    '<tr>' +
                        '<td><label for="lastName">' + options.labels.lastName + '</label></td>' +
                        '<td>' + 
                            '<input id="inputLastName" name="inputLastName" type="text" required />' +
                        '</td>' +
                    '</tr>' +

                    '<tr>' +
                        '<td><label for="chatData1">' + options.labels.chatData1 + '</label></td>' +
                        '<td>' +
                            '<input id="inputChatData1" name="inputChatData1" type="text" maxlength="20" required />' +
                            '<div id="inputChatData1Validation" class="validation"></div>' +
                        '</td>' +
                    '</tr>' +
            
                    '<tr>' +
                        '<td><label for="emailAddress">' + options.labels.email + '</label></td>' +
                        '<td>' +
                            '<input id="inputEmail" name="inputEmail" type="email" />' + 
                        '</td>' +
                    '</tr>' +
            
                  
            
                '</table>' +
            
            (options.showChatDataFields == true ? 
                '<table id="chatviewer-form-chatdatainfo" class="chatviewer-form">' +
            
                    

                    '<tr>' +
                        '<td><label for="phoneNumber">' + options.labels.phone + '</label></td>' +
                        '<td><input id="inputPhone" name="inputPhone" type="text" /></td>' +
                    '</tr>' +
                    
                    '<tr>' +
                        '<td><label for="chatData2">' + options.labels.chatData2 + '</label></td>' +
                        '<td>' +
                            '<input id="inputChatData2" name="inputChatData2" type="text" maxlength="50" />' +
                            '<div id="inputChatData2Validation" class="validation"></div>' +
                        '</td>' +
                    '</tr>' +
            
                    //'<tr>' +
                    //    '<td><label for="chatData3">' + options.labels.chatData3 + '</label></td>' +
                    //    '<td>' +
                    //        '<input id="inputChatData3" name="inputChatData3" type="number" min="-2147483648" max="2147483647" />' +
                    //        '<div id="inputChatData3Validation" class="validation">' + options.invalidText.chatData3 + '</div>' +
                    //    '</td>' +
                    //'</tr>' +
            
                    //'<tr>' +
                    //    '<td><label for="chatData4">' + options.labels.chatData4 + '</label></td>' +
                    //    '<td>' +
                    //        '<input id="inputChatData4" name="inputChatData4" type="number" min="-2147483648" max="2147483647" />' +
                    //        '<div id="inputChatData4Validation" class="validation">' + options.invalidText.chatData4 + '</div>' +
                    //    '</td>' +
                    //'</tr>' +
            
                '</table>'
             :
             '') +
            
                // <div --recaptcha element will be moved here during init()

                '<a id="chatviewer-form-submit" style="width:75px; color: black;border-radius: 5px;   padding: 10px; cursor: pointer"' + (options.recaptchaEnabled ? ' disabled="disabled"' : '') + '"type="submit" value="' + options.labels.submitButton + '" >Chat Now</a>' +
        
            '</form>';
        
        if (!localizeString)
            var localizeString = function(string) { return string; } // if function doesn't exist, just return same text instead of throw error
        
        function setupRecaptcha() {

            //this will move the recapcha elements into the right place
            var recaptchaEl = $('#chatviewer-recaptcha-container');
            container.find('#chatviewer-form-userinfo').append(recaptchaEl);
            recaptchaEl.show();
        }

        function init() {
            
            container.append(htmlStructure);

            if (options.recaptchaEnabled) {
                setupRecaptcha();
            }
            
            setupEvents();

            setupData();
            
            container.find('#chatviewer-form-submit').click(function(e) {
                e.preventDefault();
                $('input').addClass('validation-enabled');
                if ($('.validation').is(':visible') || $('input:invalid').length > 0)
                    return;
                
                container.trigger('formsubmit', getFormData());
            });
        }

        function setupData(){

            var ud = options.userInfoData;
            if(ud)
            {
                if(ud.firstName)
                     container.find('#inputFirstName').val(ud.firstName);  
                if(ud.lastName)
                     container.find('#inputLastName').val(ud.lastName);  
                if(ud.email)
                     container.find('#inputEmail').val(ud.email);  
                if(ud.phone)
                     container.find('#inputPhone').val(ud.phone);  
            }
        }
        
        function setupEvents() {
            container.find('#inputFirstName').blur(function(e) {
                if ($(this).val().trim() == "")
                    $(this).next('.validation').show();
                else
                    $(this).next('.validation').hide();
            });
            
            container.find('#inputLastName').blur(function(e) {
                if ($(this).val().trim() == "")
                    $(this).next('.validation').show();
                else
                    $(this).next('.validation').hide();
            });
            
            container.find('#inputEmail').blur(function(e) {
                if ($(this).val().trim() == "" || 
                   /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/.test($(this).val().trim()) == false)
                    $(this).next('.validation').show();
                else
                    $(this).next('.validation').hide();
            });
            
            if (options.showChatDataFields) {
                container.find('#inputChatData3').keypress(function(e) {
                    if (!(e.which >= 48 && e.which < 58))
                        e.preventDefault();
                });

                container.find('#inputChatData4').keypress(function(e) {
                    if (!(e.which >= 48 && e.which < 58))
                        e.preventDefault();
                });
            }
        }
        
        function getFormData() {
            var userFirstName = container.find('#inputFirstName').val();
            var userLastName = container.find('#inputLastName').val();
            var userEmail = container.find('#inputEmail').val();
            var userPhone = container.find('#inputPhone').val();
            var chatData1 = container.find('#inputChatData1').val();
            var chatData2 = container.find('#inputChatData2').val();
            var chatData3 = container.find('#inputChatData3').val();
            var chatData4 = container.find('#inputChatData4').val();
            var gRecaptchaResponse;
            if (options.recaptchaEnabled) {
                gRecaptchaResponse = container.find('form').serializeArray().find(function (item)
                { return item.name == 'g-recaptcha-response'; });
            }

            return {
                firstName: userFirstName,
                lastName: userLastName,
                email: userEmail,
                phone: userPhone,
                chatData1: options.showChatDataFields ? chatData1 : options.userDefinedData.chatData1,
                chatData2: options.showChatDataFields ? chatData2 : options.userDefinedData.chatData2,
                chatData3: options.showChatDataFields ? chatData3 : options.userDefinedData.chatData3,
                chatData4: options.showChatDataFields ? chatData4 : options.userDefinedData.chatData4,
                recaptchaData: options.recaptchaEnabled && gRecaptchaResponse ? gRecaptchaResponse.value : ''
            };
        }
        
        init();

    };
    
    $.fn.chatPanelForm.defaultOptions = {
        labels: {
            firstName: "First Name:",
            lastName: "Last Name:",
            email: "Email Address:",
            phone: "Phone Number:",
            chatData1: "Call Data 1:",
            chatData2: "Call Data 2:",
            chatData3: "Call Data 3:",
            chatData4: "Call Data 4:",
            submitButton: "Chat Now",
        },
        
        invalidText: {
            firstName: "Please enter a first name.",
            lastName: "Please enter a last name.",
            email: "Please enter a valid email address.",
            chatData3: "Please enter a valid number.",
            chatData4: "Please enter a valid number."
        },
        
        showChatDataFields: false,
    };
    
})(jQuery);