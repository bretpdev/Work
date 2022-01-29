CREATE CERTIFICATE [USHE_Financial_Encryption_Certificate]
    AUTHORIZATION [dbo]
    WITH SUBJECT = N'For encrypting financial data', START_DATE = N'2017-09-05T20:45:32', EXPIRY_DATE = N'2018-09-05T20:45:32';


GO
GRANT CONTROL
    ON CERTIFICATE::[USHE_Financial_Encryption_Certificate] TO [db_executor];

