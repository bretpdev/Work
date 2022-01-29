USE [CLS]
GO

INSERT INTO [pridrcrp].[RepaymentPlanTypes]
           ([RepaymentPlanType])
     VALUES
           ('ALT FIXED PAYMENT'),
		   ('ALT FIXED TERM'),
		   ('CONSOL GRADUATED'),
		   ('CONSOL STANDARD'),
		   ('EXTENDED - GRANDFATHERED'),
		   ('EXTENDED FIXED'),
		   ('EXTENDED GRAD'),
		   ('EXTENDED GRADUATED'),
		   ('EXTENDED-GRANDFATHERED'),
		   ('FORCED ICR'),
		   ('GRADUATED 10 YEAR'),
		   ('GRADUATED-GRANDFATHERED'),
		   ('INCOME BASED'),
		   ('INCOME CONTINGENT'),
		   ('STANDARD'),
		   ('STANDARD - GRANDFATHERED'),
		   ('STANDARD-GRANDFATHERED')

GO