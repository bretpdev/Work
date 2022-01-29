CREATE PROCEDURE [pretnfrnot].[GetPreTransferServicerQA_TS06BPRTCH]
	@RegionDeconversion char(3),
	@TransferDate varchar(20)
AS

SELECT
	'The U.S. Department of Education (ED) will soon transfer the customer service of your federal student loan account to ' + ServicerName + ', another member of ED''s federal loan servicer team. Your loans are not being sold. ED will continue to own your loans; however, a different servicer will manage your loans and assist you on ED''s behalf.
	 ED is transferring your loan account to ' + ServicerName + ' as part of an initiative to keep customer service and repayment support balanced across all federal loan servicers. This change will not impact the existing terms, conditions, interest rate, or available repayment plans of your federal student loans.
	 Your loan account will be transferred to ' + ServicerName + ' on or about ' + @TransferDate + '. If ED doesn''t transfer your account as planned, we''ll let you know.' AS ImportantNotice,
	'To help ensure a smooth transition to ' + ServicerName + ', you''ll need to do the following:' AS YourActions,
	'What is the  contact information for my new servicer?' AS Q1,
	'The contact information for ' + ServicerName + ' is as follows:' AS A1,
	'When will ' + ServicerName + ' contact me?' AS Q2,
	 ServicerName + ' will contact you after your federal student loan account has been fully transfered and loaded to its system.  If you have questions any time after the transfer date, give ' + ServicerName + ' a call.  The contact information you''ll need is in Q&A #1.' AS A2,
	'Will I have a new account number with ' + ServicerName + '?' AS Q3,
	'Yes. ' + ServicerName + ' will assign a new federal student loan account number to you once it loads your loans to its system. ' + ServicerName + '  will inform you of your new account number in the first communication it sends to you. ' AS A3,
	'Will ' + ServicerName + ' have online account access?' AS Q4,
	'Yes.  ' + ServicerName + ' will have online federal student loan account access.  You''ll just need to follow the instructions for signing up once ' + ServicerName + ' provides those instructions to you. As part of that process, you''ll establish a new username and password.' AS A4,
	'To whom will I make federal student loan payments after ' + ServicerName + ' is my servicer?' AS Q5,
	'After ED transfers your federal student loan account to ' + ServicerName + ', you''ll make payments to that servicer.  ' + ServicerName + ' will let you know when your payments will be due.' AS A5
FROM
	PreTransferServicer
WHERE
	RegionDeconversion = @RegionDeconversion

RETURN 0


