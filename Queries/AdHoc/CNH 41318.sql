USE CDW
GO

GRANT EXECUTE ON GetLoanLabel TO db_executor
GRANT EXECUTE ON PYOF_GetLoanInformation TO db_executor
GRANT EXECUTE ON spGetAccountNumberFromSsn TO db_executor
GRANT EXECUTE ON spGetSSNFromAcctNumber TO db_executor


USE CLS
GO

GRANT EXECUTE ON CheckAddressEcorr TO db_executor
GRANT EXECUTE ON PyOffFedGetCoBorrower TO db_executor
GRANT EXECUTE ON PyOffFedGetBorrower TO db_executor