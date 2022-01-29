CREATE SYMMETRIC KEY [USHE_Financial_Data_Key]
    AUTHORIZATION [dbo]
    WITH ALGORITHM = AES_256
    ENCRYPTION BY CERTIFICATE [USHE_Financial_Encryption_Certificate];


GO
GRANT REFERENCES
    ON SYMMETRIC KEY::[USHE_Financial_Data_Key] TO [db_executor];

