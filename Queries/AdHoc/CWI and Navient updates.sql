UPDATE 
	voyager..schools 
SET
	rc_school_logo = 'http://cdn.mcauto-images-production.sendgrid.net/9ad14f3e449db626/87b7958e-b461-482d-93a6-6f7c7bb2e79a/526x149.png'
WHERE
	school_code = '042118' -- CWI

UPDATE
	voyager..servicers
SET
	email = 'https://about.navient.com/contact-us',
	contact = 'https://about.navient.com/contact-us',
	[delay] = 'https://www.navient.com/in-repayment/federal-student-loans',
	repay = 'https://www.navient.com/in-repayment/federal-student-loans',
	missed = 'https://www.navient.com/in-repayment/federal-student-loans'
WHERE 
	servicer_code = '578' --Navient links