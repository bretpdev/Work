CREATE PROCEDURE [pretnfrnot].[GetPreTransferServicerQA]
	@RegionDeconversion char(3),
	@TransferDate varchar(20)
AS

SELECT
	'The U.S. Department of Education (ED) will soon transfer the customer service of your federal student loan account to ' + ServicerName + ', another member of ED’s federal loan servicer team that services other federal student loans for you.
	 ED is transferring your loan account to ' + ServicerName + ' so that one servicer manages all of your federal student loans and assists you on ED’s behalf. This transfer will not impact the existing terms, conditions, interest rate, or available repayment plans of your federal student loans.
	 Your loan account will be transferred to ' + ServicerName + ' on or about ' + @TransferDate + '. If ED doesn’t transfer your account as planned, we’ll let you know.' AS ImportantNotice,
	'To help ensure a smooth transition to ' + ServicerName + ', you’ll need to do the following:' AS YourActions,
	'What is the ' + ServicerName + ' contact information?' AS Q1,
	'The contact information for ' + ServicerName + ' is as follows:' AS A1,
	'When will ' + ServicerName + ' contact me?' AS Q2,
	 ServicerName + ' will contact you after your federal student loan account has been transfered and loaded to its system.  If you have questions any time after the transfer date, give ' + ServicerName + ' a call.  The contact information you''ll need is in Q&A #1.' AS A2,
	'Will ' + ServicerName + ' assign a new account number to me?' AS Q3,
	'No, you will continue to have the same federal student loan account number that you already have with CornerStone.  The loans we transfer to ' + ServicerName + ' will be added to your existing account.' AS A3,
	'Will ' + ServicerName + ' continue to offer online account access?' AS Q4,
	'Yes.  ' + ServicerName + ' will continue to offer online account access.  If you already have online account access with ' + ServicerName + ', you''ll use your established username and password to access your federal student loan account.' AS A4,
	'To whom will I make federal student loan payments after the transfer to ' + ServicerName + '?' AS Q5,
	'After ED transfers your federal student loan account to ' + ServicerName + ', you''ll make payments to that servicer.  ' + ServicerName + ' will let you know when your payments will be due.' AS A5
FROM
	PreTransferServicer
WHERE
	RegionDeconversion = @RegionDeconversion

RETURN 0


