USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	--XXX
	DELETE FROM CLS.cslsltrfed.LoanServicingLetters
	WHERE LoanServicingLettersId IN (X,XX,XXX,X,XX,XX,XX,XX,XX,XX,XX,XX,XX,XX,XX,XX,XX,XX,XX,XX,XX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XX,XX,XX,XX,XX,XX,XX,XXX,XX,XX,XX,XX,XX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XX,XXX,XX,XXX,XXX,XXX,XXX,XX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX,XXX)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		CLS.cslsltrfed.LoanServicingLetters
	SET
		LetterChoices = 'Your form is not signed. Please sign and resubmit your application.'
	WHERE
		LoanServicingLettersId = XX

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE
		CLS.cslsltrfed.LoanServicingLetters
	SET
		LetterChoices = 'Your form was not dated. Please resubmit your application with the date.'
	WHERE
		LoanServicingLettersId = XXX

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
	UPDATE
		CLS.cslsltrfed.LoanServicingLetters
	SET
		LetterChoices = 'Your form was not dated. Please resubmit your application with the date.'
	WHERE
		LoanServicingLettersId = XXX

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
	UPDATE
		CLS.cslsltrfed.LoanServicingLetters
	SET
		LetterChoices = 'Your form was not signed. Please resubmit your application with a signature.'
	WHERE
		LoanServicingLettersId = XXX

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	--XX
	DECLARE @ArcSearchDefense VARCHAR(XXX) = 'DIFRB,WRFXX'
	DECLARE @ArcDefense VARCHAR(XXX) = 'FBDNY'
	INSERT INTO CLS.cslsltrfed.LoanServicingLetters(LetterType, LetterOptions, LetterChoices, CheckForCoBorrower, ArcSearch, Arc, LetterId, Hierarchy, HasDischargeAmount, DischargeAmount, SchoolName, LastDateAttendance, SchoolClosureDate, DefForbType, DefForbEndDate, LoanTermEndDate, SchoolYear, AdditionalReason, DeathLetter)
	VALUES('Forbearance', 'Department of Defense', 'You did not provide a Social Security Number. Please resubmit your completed application, and ensure that your Social Security Number is listed on the top of each page.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'You did not complete Section X. Please resubmit your application with all pages completed.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'You have sent in the incorrect form. Please reach out to one of our Loan Specialists via chat or phone at the contact information above in order to determine which form you need to send in.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'You did not provide complete or valid request dates. Please resubmit your application with all pages completed.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'The start date you have provided is too far in the past.� Please resubmit your completed application with a start date that is no more than XX months in the past�.�', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'The forbearance cannot begin more than XX days in the future. Please resubmit your completed application with a begin date that is less than XX days in the future.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'You did not sign the form. Please resubmit your signed application with all pages completed.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'You did not date the form. Please resubmit your signed and dated application with all pages completed.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'Your loan is currently in a deferment or forbearance.� If you would like to end your existing deferment or forbearance early and replace it with this one, please contact one of our loan specialists via secure live chat�or phone at the contact information above within XX days of your signature date.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'You are not eligible for this forbearance type. In order to qualify, you must be performing service that qualifies you for a partial repayment of your loans under a Department of Defense Student Loan Repayment Program, participating in a medical or dental internship/residency, or engaged in active State National Guard duty.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'You did not select an option in Section X. Please resubmit your application with all pages completed.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'The documentation provided is not sufficient to qualify for the requested forbearance. Please either have an authorized official complete Section X of the request form, or submit separate documentation from an authorized official that includes all of the information requested in Section X.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'The co-borrower did not submit a request. In order for us to apply this forbearance, the co-borrower must also submit a completed forbearance request form.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'Section X has not been completed by an authorized official and/or documentation of Department of Defense loan repayment program was not provided. Please either have an authorized official complete Section X of the request form, or submit separate documentation from an authorized official that includes all of the information requested in Section X.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'Section X was not signed by an authorized official. Please resubmit your completed application, and ensure that it is signed by an authorized official.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'Your loans are not yet in repayment. As soon as your loans enter repayment, you may reapply for this forbearance option. For more information sign on to your online account.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'You have sent in an expired form. Please resubmit your completed application using the request form found on https://mycornerstoneloan.org/forms/all-forms/.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'You requested a cancellation of processing your request.� If you would still like to have your forbearance request processed, please submit a new completed application using the request form found on https://mycornerstoneloan.org/forms/all-forms/.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'Your loan is in a default status. You are not eligible for this forbearance while your loan is in default. Please contact Direct Loans Default Servicing at XXX-XXX-XXXX for more information.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'The request form and/or documentation you submitted was illegible. Please submit a new completed request form and documentation.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'You did not provide a valid signature. Please resubmit your completed application with a valid signature that is no more than XX days old.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Department of Defense', 'Your account is currently in a Total and Permanent Disability (TPD) Collection Suspension. Please contact Nelnet at (XXX) XXX-XXXX for more information on your TPD application.', X, @ArcSearchDefense, @ArcDefense, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	--XX
	DECLARE @ArcSearchGeneral VARCHAR(XXX) = 'DIFRB,WRFXX'
	DECLARE @ArcGeneral VARCHAR(XXX) = 'FBDNY'
	INSERT INTO CLS.cslsltrfed.LoanServicingLetters(LetterType, LetterOptions, LetterChoices, CheckForCoBorrower, ArcSearch, Arc, LetterId, Hierarchy, HasDischargeAmount, DischargeAmount, SchoolName, LastDateAttendance, SchoolClosureDate, DefForbType, DefForbEndDate, LoanTermEndDate, SchoolYear, AdditionalReason, DeathLetter)
	VALUES('Forbearance', 'General (Temporary Hardship)', 'You did not provide a Social Security Number. Please resubmit your completed application, and ensure that your Social Security Number is listed on the top of each page.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'You did not complete Section X. Please resubmit your application with all pages completed.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'You did not provide complete or valid request dates. Please resubmit your application with all pages completed.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'The forbearance cannot begin more than XX days in the future. Please resubmit your completed application with a begin date that is less than XX days in the future.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'You did not sign the form. Please resubmit your signed application with all pages completed.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'You did not date the form. Please resubmit your application with all pages completed.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'Your loan is currently in a deferment or forbearance.� If you would like to end your existing deferment or forbearance early and replace it with this one, please contact one of our loan specialists via secure live chat�� or phone at the contact information above within XX days of your signature date.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'You have used an excessive amount of forbearance time. Please contact us for additional postponement and repayment options.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'You did not select an option in Section X. Please resubmit your application with all pages completed.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'The co-borrower did not submit a request. In order for us to apply this forbearance, the co-borrower must also submit a completed forbearance request form.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'You did not provide a reason for your request. Please select a reason in Section X, and resubmit your application with all pages completed.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'Your loan is in a default status. You are not eligible for this forbearance while your loan is in default. Please contact Direct Loans Default Servicing at XXX-XXX-XXXX for more information.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'The form and/or documentation you submitted was illegible. Please submit a new completed request form and documentation.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'You did not provide a valid signature. Please resubmit your completed application with a valid signature that is no more than XX days old.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'General (Temporary Hardship)', 'Your account is currently in a Total and Permanent Disability (TPD) Collection Suspension. Please contact Nelnet at (XXX) XXX-XXXX for more information on your TPD application.', X, @ArcSearchGeneral, @ArcGeneral, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	

	--XX
	DECLARE @ArcSearchIntern VARCHAR(XXX) = 'DIFRB,WRFXX'
	DECLARE @ArcIntern VARCHAR(XXX) = 'FBDNY'
	INSERT INTO CLS.cslsltrfed.LoanServicingLetters(LetterType, LetterOptions, LetterChoices, CheckForCoBorrower, ArcSearch, Arc, LetterId, Hierarchy, HasDischargeAmount, DischargeAmount, SchoolName, LastDateAttendance, SchoolClosureDate, DefForbType, DefForbEndDate, LoanTermEndDate, SchoolYear, AdditionalReason, DeathLetter)
	VALUES('Forbearance', 'Internship/Residency', 'You did not provide a Social Security NumberSN. Please resubmit your completed application, and ensure that your Social Security NumberSN is listed on the top of each page.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'You did not complete Section X. Please resubmit your application with all pages completed.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'You have sent in the incorrect form. Please reach out to one of our Loan Specialists via chat or phone at the contact information above in order to determine which form you need to send in.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'You did not provide complete or valid request dates. Please resubmit your application with all pages completed.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'The start date you have provided is too far in the past.� Please resubmit your completed application with a start date that is no more than XX months in the past�.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'The forbearance cannot begin more than XX days in the future. Please resubmit your completed application with a begin date that is less than XX days in the future.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'You did not sign the form. Please resubmit your signed application with all pages completed.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'You did not date the form. Please resubmit your application with all pages completed.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'Your loan is currently in a deferment or forbearance.� If you would like to end your existing deferment or forbearance early and replace it with this one, please contact one of our loan specialists via secure live chat or phone at the contact information above within XX days of your signature date.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'You are not eligible for this forbearance type. In order to qualify, you must be participating in a medical or dental internship/residency, engaged in active State National Guard duty, or receiving payments through a Department of Defense Repayment program.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'You did not select an option in Section X. Please resubmit your application with all pages completed.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'The documentation provided is not sufficient to qualify for the requested forbearance. Please carefully review the required documentation outlined in Section X, and resubmit your completed application with the appropriate documentation.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'The co-borrower did not submit a request. In order for us to apply this forbearance, the co-borrower must also submit a completed forbearance request form.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'Documentation of internship/residency was not provided. Please carefully review the required documentation outlined in Section X, and resubmit your completed application with the appropriate documentation.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'Section X was not signed by an authorized official and/or documentation of internship/residency was not provided. Please ensure that Section X is completed by an authorized official, and resubmit your completed application with any applicable documentation.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'Your loan is in a default status. You are not eligible for this forbearance while your loan is in default. Please contact Direct Loans Default Servicing at XXX-XXX-XXXX for more information.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'Your loans are not yet in repayment. As soon as your loans enter repayment, you may reapply for this forbearance option. For more information sign on to your online account.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'The request form and/or documentation you submitted was illegible. Please submit a new completed request form and documentation�.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'You did not provide a valid signature. Please resubmit your completed application with a valid signature that is no more than XX days old.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'Internship/Residency', 'Your account is currently in a Total and Permanent Disability (TPD) Collection Suspension. Please contact Nelnet at (XXX) XXX-XXXX for more information on your TPD application.', X, @ArcSearchIntern, @ArcIntern, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	


	--XX
	DECLARE @ArcSearchNational VARCHAR(XXX) = 'DIFRB,WRFXX'
	DECLARE @ArcNational VARCHAR(XXX) = 'FBDNY'
	INSERT INTO CLS.cslsltrfed.LoanServicingLetters(LetterType, LetterOptions, LetterChoices, CheckForCoBorrower, ArcSearch, Arc, LetterId, Hierarchy, HasDischargeAmount, DischargeAmount, SchoolName, LastDateAttendance, SchoolClosureDate, DefForbType, DefForbEndDate, LoanTermEndDate, SchoolYear, AdditionalReason, DeathLetter)
	VALUES('Forbearance', 'National Guard Duty', 'Section X of the form is incomplete. Please resubmit your application and ensure all fields have been completed.', X, @ArcSearchNational, @ArcNational, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'National Guard Duty', 'Section X Part B of the form is incomplete. Please resubmit your application and ensure all fields have been completed.', X, @ArcSearchNational, @ArcNational, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'National Guard Duty', 'You have not provided documentation that supports eligibility for this forbearance type. If you are unsure what documentation should be provided, please contact one of our loan specialists via secure live chat�or phone at the contact information above.', X, @ArcSearchNational, @ArcNational, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'National Guard Duty', 'A valid request date was not provided. Please provide a valid request date and resubmit your application.', X, @ArcSearchNational, @ArcNational, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'National Guard Duty', 'This forbearance cannot begin more than XX days in the future. Please resubmit your completed application with a begin date that is less than XX days in the future.', X, @ArcSearchNational, @ArcNational, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'National Guard Duty', 'Your form was not signed. Please resubmit your signed application with all pages completed.', X, @ArcSearchNational, @ArcNational, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'National Guard Duty', 'Your loan is currently in a deferment or forbearance. � If you would like to end your existing deferment or forbearance early and replace it with this one, please contact one of our loan specialists via secure live chat or phone at the contact information above within XX days of your signature date.', X, @ArcSearchNational, @ArcNational, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'National Guard Duty', 'Your loan does not qualify for the Mandatory Forbearance. Please contact us at the number above for other options available to you.', X, @ArcSearchNational, @ArcNational, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'National Guard Duty', 'Section X�� was not completed ��by an authorized official and/or documentation of your National Guard orders were not provided. Please have an authorized official complete section X or submit your National Guard orders.', X, @ArcSearchNational, @ArcNational, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'National Guard Duty', 'Section X was not signed by an authorized official. Please resubmit your application with all applicable fields completed.', X, @ArcSearchNational, @ArcNational, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)
	,('Forbearance', 'National Guard Duty', 'Your account is currently in a Total and Permanent Disability (TPD) Collection Suspension. Please contact Nelnet at (XXX) XXX-XXXX for more information on your TPD application.', X, @ArcSearchNational, @ArcNational, 'FORDNFED', X, X, X, X, X, X, X, X, X, X, X, X)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = XXX AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END