CREATE CERTIFICATE [USHE_Financial_Encryption_Certificate]
    AUTHORIZATION [dbo]
    WITH SUBJECT = N'For encrypting financial data', START_DATE = N'2013-08-08T15:45:28', EXPIRY_DATE = N'2014-08-08T15:45:28';


GO
GRANT CONTROL
    ON CERTIFICATE::[USHE_Financial_Encryption_Certificate] TO [db_executor]
    AS [dbo];


GO
GRANT REFERENCES
    ON CERTIFICATE::[USHE_Financial_Encryption_Certificate] TO [db_executor]
    AS [dbo];

