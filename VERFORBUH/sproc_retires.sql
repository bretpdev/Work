--The following stored procedures have all been migrated to the verforbfed schema.

USE CDW
GO
DROP PROCEDURE dbo.GetNextBillDueDate
DROP PROCEDURE dbo.GetAllValidLoansForForbearance
DROP PROCEDURE dbo.GetForbearanceEndDate
DROP PROCEDURE dbo.GetDefermentEndDate
DROP PROCEDURE dbo.CheckPaidAhead
DROP PROCEDURE dbo.CheckForSplitSchedules
DROP PROCEDURE dbo.GetPaymentAmount
DROP PROCEDURE dbo.GetDateDelinquencyOccured
DROP PROCEDURE dbo.CheckForFutureDateForbearance
DROP PROCEDURE dbo.GetCollectionSuspenseInfo
DROP PROCEDURE dbo.GetMaxNumberOfForbUsed
DROP PROCEDURE dbo.Verify120Forb
DROP PROCEDURE dbo.GetBorrowersLI_FOR_VRB_DFL_RUL
DROP PROCEDURE dbo.CheckSpousalLoans
DROP PROCEDURE dbo.CheckForFutureDateDeferment

USE UDW
GO
DROP PROCEDURE dbo.GetNextBillDueDate
DROP PROCEDURE dbo.GetAllValidLoansForForbearance
DROP PROCEDURE dbo.GetForbearanceEndDate
DROP PROCEDURE dbo.GetDefermentEndDate
DROP PROCEDURE dbo.CheckPaidAhead
DROP PROCEDURE dbo.CheckForSplitSchedules
DROP PROCEDURE dbo.GetPaymentAmount
DROP PROCEDURE dbo.GetDateDelinquencyOccured
DROP PROCEDURE dbo.CheckForFutureDateForbearance
DROP PROCEDURE dbo.Verify120Forb
DROP PROCEDURE dbo.GetBorrowersLI_FOR_VRB_DFL_RUL
DROP PROCEDURE dbo.CheckSpousalLoans
DROP PROCEDURE dbo.CheckForFutureDateDeferment