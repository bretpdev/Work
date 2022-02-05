/*
* Noble Systems Web Chat Visitor Portal Configurations v2.0.1
*
* Copyright (c) 2017 Noble Systems Corporation
*/


/*
* Chat Panel Configurations
*   - Configurations for connecting to chat server and the chat viewer client
*/
var chatPanelConfig = {
    
    services: [
        {
            webChatGatewayHost: 'chat.mycornerstoneloan.org',
            webChatGatewayPort: '443', //3010
            useSsl: true
        }
    ],
    
   //queueId: 1234,
   // queueList: [
   //     {
   //         queueId: 1024,
   //         description: "NSC Sales"
   //     },
   //     {
   //         queueId: 1234,
   //         description: "NSC Support"
   //     },
   //     {
   //         queueId: 56781,
   //         description: "NSC Accounting"
   //     }
   // ],

    // Advanced Options
    debug: true,
    useBlockStyle: false,
  	dateFormat: "MM/DD/YYYY HH:mm:ss",
    recaptcha : {
        siteKey: "<Replace Site Key Here>"
    },
    chatTypingState: {
        enabled: false,
        clearInMs: 30000
    }
};

/*
* Chat Panel Queue Configurations
*   - Configurations for the chat queue selection screen
*/
var chatPanelQueueConfig = {
    welcomeMessage: "How can we help you:",
    placeholderItemText: "-- Please select one --",
    
    // Advanced Options
    useCardStyle: true,
    queueIconBaseUrl: "images/queueicons/",
};

/*
* Chat Panel Form Configurations
*   - Configurations for user info form screen
*/
var chatPanelFormConfig = {
    labels: {
        firstName: "First Name:",
        lastName: "Last Name:",
        email: "Email Address:",
        phone: "Phone Number:",
        chatData1: "Date of Birth:",
        chatData2: "Address:",
        submitButton: "Chat Now",
    },

    invalidText: {
        firstName: "Please enter a first name.",
        lastName: "Please enter a last name.",
        email: "Please enter a valid email address.",
        phone: "Please enter a valid phone number.",
    },
    
    // Advanced Options
    userInfoData: {},
    userDefinedData: {},
    showChatDataFields: true,
    recaptchaEnabled: false
};