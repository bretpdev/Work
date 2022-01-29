USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @Expected INT = XX


INSERT INTO CLS.cslsltrfed.LoanServicingLetters(LetterType,LetterOptions,LetterChoices,CheckForCoBorrower,ArcSearch,Arc,LetterId,Hierarchy,TxXxSearch,Note,StoredProceduresId,HasDischargeAmount,DischargeAmount,SchoolName,LastDateAttendance,SchoolClosureDate,DefForbType,DefForbEndDate,LoanTermEndDate,SchoolYear,AdditionalReason,DeathLetter)
VALUES
('Deferment','Graduate Fellowship','You did not provide a Social Security Number. Please resubmit your completed application, and ensure that your Social Security Number is listed on the top of each page.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','You did not complete Section X. Please resubmit your application with all pages completed.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','You indicated in Section X that you do not have a bachelor''s degree. In order to qualify, you must have completed your bachelor''s degree.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','You indicated in Section X that you have not been accepted or recommended by an institution of higher education for acceptance into a graduate fellowship program on a full-time basis. In order to qualify, you need to have been accepted or recommended for acceptance into a graduate fellowship program on a full time basis.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','You indicated in Section X that your graduate fellowship program does not provide sufficient financial aid support to allow for full-time study for a period of at least X months. In order to qualify, your program must provide sufficient financial support to allow for full-time study for a period of at least X months.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','You indicated in Section X that your program does not require a written statement from you that explains your objective. In order to qualify, your program must require a written statement from you that explains your objectives.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','In Section X you indicated that your program does not require you to submit periodic reports, projects, or other evidence of your progress. In order to qualify, you program must require you to submit periodic reports, projects, or other evidence of your progress.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','In Section X you indicated that you are studying in a foreign university and your program will not accept the course of study from the foreign university towards completion of the fellowship program. In order to qualify, your program must accept the course of study from the foreign university.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','You did not provide a valid signature. Please resubmit your signed and completed application using an ink signature, visibly signed digital signature, or a certificate-based electronic signature issued by the US Government.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','The co-maker of the spousal consolidation loan did not submit a request to suspend payments. In order to qualify, both co-makers must qualify and submit a separate request form.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','The authorized official did not provide complete or valid dates of the graduate fellowship program. Please have an authorized official complete Section X, and resubmit your application.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','The authorizing official did not provide their title. Please have an authorized official complete Section X, and resubmit your application.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','Section X was not completed by an authorized official and/or documentation of graduate fellowship program was not provided. Please have an authorized official complete Section X, and resubmit your application.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','The program you are enrolled in is not eligible for the graduate fellowship deferment; however, you may be eligible for the Internship/Residency forbearance, which you can apply for by completing the Mandatory Forbearance (SERV) request form.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','Your loans are not yet in repayment. As soon as your loans enter repayment, you may reapply for this deferment option. For more information sign on to your online account.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','The loans are currently in a deferment. If you are still in an eligible graduate fellowship program when your deferment ends, you may reapply.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','The account is in a Total and Permanent Disability (TPD) collection suspension status. Please contact Nelnet at (XXX) XXX-XXXX for more information on your TPD application.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','The account is in a Bankruptcy Status. You are not eligible for this deferment while your loan is in bankruptcy; however, you may reapply after your bankruptcy proceedings are complete.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Graduate Fellowship','The application you submitted was completed using the student�s information. In order for us to process this deferment, the primary account holder must be participating in an eligible graduate fellowship program.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','You did not provide a Social Security Number. Please resubmit your completed application, and ensure that your Social Security Number is listed on the top of each page.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','You did not complete Section X. Please resubmit your application with all pages completed.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','You indicated in Section X that your program is not recognized by the Department of Veterans Affairs (VA) or a state agency. In order to qualify, your program must be recognized by the VA or a state agency.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','You indicated in Section X that your program does not provide services under a written, individualized plan that specifies the date the services are expected to end. In order to qualify, your program must provide services under a written, individualized plan that specifies the date the services are expected to end.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','You indicated in section X that your program does not require substantial commitment by you to your rehabilitation. In order to qualify, your program must require a commitment of time and effort that would normally prevent a person from being employed XX or more hours per week in a position expected to last at least X months.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','You indicated in section two that you were not receiving or scheduled to receive vocational, drug abuse, mental health, or alcohol abuse rehabilitation services in your program. In order to qualify, you must be scheduled to receive at least one of these services in your program.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','You did not provide a valid signature. Please resubmit your signed and completed application using an ink signature, visibly signed digital signature, or a certificate-based electronic signature issued by the US Government.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','The co-maker of the spousal consolidation loan did not submit a request to suspend payments. In order to qualify, both co-makers must qualify and submit a separate request form.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','The authorized official did not provide complete or valid dates of the rehabilitation training. Please have an authorized official complete Section X, and resubmit your application.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','The authorizing official did not provide their title. Please have an authorized official complete Section X, and resubmit your application.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','Section X was not completed by an authorized official and/or documentation of rehabilitation training services was not provided. Please have an authorized official complete Section X, and resubmit your application.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','The supporting documentation provided does not meet the requirements for this deferment type. Please contact us for additional postponement and repayment options.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','Your loans are not yet in repayment. As soon as your loans enter repayment, you may reapply for this deferment option. For more information sign on to your online account.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','The loans are currently in a deferment. If you are still in an eligible rehabilitation training program when your deferment ends, you may reapply.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','The account is in a Total and Permanent Disability (TPD) collection suspension status. Please contact Nelnet at (XXX) XXX-XXXX for more information on your TPD application.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X),
('Deferment','Rehabilitation Training','The account is in a Bankruptcy Status. You are not eligible for this deferment while your loan is in bankruptcy; however, you may reapply after your bankruptcy proceedings are complete.',X,'DIDFR,GXXXX,GXXXC','DFDNY','DEFDNFED',X,NULL,NULL,NULL,X,X,X,X,X,X,X,X,X,X,X)

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @Expected AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END