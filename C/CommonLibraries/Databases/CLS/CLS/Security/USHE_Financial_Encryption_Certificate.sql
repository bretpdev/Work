CREATE CERTIFICATE [USHE_Financial_Encryption_Certificate]
    AUTHORIZATION [dbo]
    WITH SUBJECT = N'For encrypting financial data', START_DATE = N'2014-09-30T18:17:19', EXPIRY_DATE = N'2015-09-30T18:17:19';




GO
GRANT CONTROL
    ON CERTIFICATE::[USHE_Financial_Encryption_Certificate] TO [db_executor];



