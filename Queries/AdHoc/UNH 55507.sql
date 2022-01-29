
DECLARE @temp55507 TABLE(Category VARCHAR(100), Reason VARCHAR(100), UheaaOnly BIT)
INSERT INTO @temp55507(Category, Reason, UheaaOnly)
VALUES
('Alternate Format','Audio CD',0),
('Alternate Format','Braille',0),
('Alternate Format','Data CD',0),
('Alternate Format','Large Text',0),
('ACH','Adjustment',0),
('ACH','Cancellation',0),
('ACH','Suspension',0),
('ACH','New',0),
('Bankruptcy','Declaring',0),
('Bankruptcy','Exiting',0),
('Bankruptcy','Account Transferred to Guarantor',0),
('Billing','Amount',0),
('Billing','Delivery Method',0),
('Billing','Due Date Questions',0),
('Billing','Due Date Change Request',0),
('Billing','Interest Bill',0),
('Billing','Missing Bill',0),
('Borrower Benefits','ACH',0),
('Borrower Benefits','BANA',1),
('Borrower Benefits','Disqualification Inquiry',1),
('Borrower Benefits','Eligibility Inquiry',0),
('Borrower Benefits','Qualification Inquiry',0),
('Borrower Benefits','Timely Payment Benefit',1),
('Consolidation','Applying',0),
('Consolidation','CSF',0),
('Consolidation','Pros and Cons Inquiry',0),
('Consolidation','Status Inquiry',0),
('Consolidation','Transfer',0),
('Conversion','New Conversion',0),
('Conversion','Pre Conversion',0),
('Credit Reporting','Check/Inquiry',0),
('Credit Reporting','Dispute',0),
('Credit Reporting','Response to Credit Reporting Notification',0),
('CURE','Payment',1),
('CURE','Response to CURE Letter',1),
('CURE','Response to Incurable Letter',1),
('CURE','Transferred to Specialist',1),
('Default','Provided DMCS Phone Number',0),
('Default','Provided Guarantor Phone Number',1),
('Default','Transferred to UHEAA Default Group',1),
('Default','Litigation',0),
('Deferment','Adjustment',0),
('Deferment','Cancellation',0),
('Deferment','Economic Hardship',0),
('Deferment','Graduate Fellowship',0),
('Deferment','In- School',0),
('Deferment','In School Status',0),
('Deferment','Parent PLUS',0),
('Deferment','Rehabilitation Training',0),
('Deferment','Responding to Letter/Email',0),
('Deferment','Temporary Total Disability',0),
('Deferment','Unemployment',0),
('Demographics','Address Change',0),
('Demographics','DOB Discrepancy',0),
('Demographics','Email Change',0),
('Demographics','Name Change',0),
('Demographics','Phone Number Change',0),
('Demographics','SSN Discrepancy',0),
('Demographics','Updated References Information',0),
('Disbursements','Amounts',0),
('Disbursements','Cancellation',0),
('Disbursements','Dispute',0),
('Disbursements','Loan Servicer Inquiry',0),
('Disbursements','Origination Fees',0),
('Discharge','Borrower Defense Discharge',0),
('Discharge','Borrower Defense Discharge – Status Inquiry',0),
('Discharge','Death',0),
('Discharge','Death - Status Inquiry',0),
('Discharge','False Certification – Ability to Benefit',0),
('Discharge','False Certification – Ability to Benefit - Status Inquiry',0),
('Discharge','False Certification – Disqualifying Status',0),
('Discharge','False Certification – Disqualifying Status – Status Inquiry',0),
('Discharge','False Certification – Unauthorized Payment/Signature',0),
('Discharge','False Certification – Unauthorized Payment/Signature - Status Inquiry',0),
('Discharge','Identity Theft',0),
('Discharge','Identity Theft - Status Inquiry',0),
('Discharge','School Closure',0),
('Discharge','School Closure - Status Inquiry',0),
('Discharge','TPD',0),
('Discharge','TPD - Status Inquiry',0),
('Discharge','Unpaid Refund',0),
('Discharge','Unpaid Refund - Status Inquiry',0),
('E-Corr','Opt In',0),
('E-Corr','Opt Out',0),
('Escalation','Transfer',0),
('Escalation','Ombudsman',0),
('Escalation','Complaint',0),
('Forbearance','Mandatory',0),
('Forbearance','Responding to Letter/Email',0),
('Forbearance','RPF',0),
('Forbearance','Student Loan Debt Burden',0),
('Forbearance','Teacher Loan Forgiveness',0),
('Forbearance','Verbal/Temporary Hardship',0),
('Forgiveness','PSLF Applying',0),
('Forgiveness','PSLF Status',0),
('Forgiveness','TLF Applying',0),
('Forgiveness','TLF Responding to Letter/Email',0),
('Forgiveness','TLF Status',0),
('Guarantor Call','Default Aversion',1),
('Guarantor Call','Status Inquiry',1),
('Guarantor Call','Update Demographics',1),
('IDR','Applying',0),
('IDR','Expedited Payment/RPF',0),
('IDR','Forgiveness',0),
('IDR','Forgiveness Counter Request',0),
('IDR','Interest Subsidy',0),
('IDR','Payment Amount Inquiry',0),
('IDR','Qualifying Payments',0),
('IDR','Recalculation',0),
('IDR','Recertification',0),
('IDR','Status Inquiry',0),
('ISR','Applying',0),
('ISR','Payment Amount Inquiry',0),
('ISR','Recalculation',0),
('ISR','Recertification',0),
('ISR','Status Inquiry',0),
('Loan Details','Balance Inquiry',0),
('Loan Details','Interest Rate Inquiry',0),
('Loan Details','Loan Type Inquiry',0),
('Loan Details','Term Inquiry',0),
('Military','Deferment Form',0),
('Military','DOD Repayment',0),
('Military','Mandatory Forbearance (DOD)',0),
('Military','No Interest Benefit',0),
('Military','Responding to Letter/Email',0),
('Military','SCRA Eligibility',0),
('Military','SCRA Responding to Letter/Email',0),
('Military','Third Party Authorization/POA',0),
('Military','Verbal Deferment',0),
('Payments','Cancellation Payment',0),
('Payments','Check by Phone',0),
('Payments','Confirmation Number',0),
('Payments','Delete Payment',0),
('Payments','Financial Adjustment',0),
('Payments','Missing Payment',0),
('Payments','Payment History',0),
('Payments','Paid Ahead Status Inquiry',0),
('Payments','Pay off Request',0),
('Payments','Refund',0),
('Payments','Write-off Request',0),
('Reference Call','Demographics Update',0),
('Reference Call','Removal Request',0),
('Reference Call','Responding to Call/Email/Letter',0),
('Repayment Plans','Current Plan Inquiry',0),
('Repayment Plans','Monitor/Re-Disclosure',0),
('Repayment Plans','Payment Estimate',0),
('Repayment Plans','Plan Change',0),
('School Call','Default Aversion',0),
('School Call','School Reports',0),
('School Call','Status Requests',0),
('School Call','Transferred to Specialist',0),
('School Call','Updated Demos',0),
('Taxes','1098-E Request',0),
('Taxes','1099-C Request',0),
('Taxes','Amount Paid Request',0),
('Taxes','Remove E-Corr',0),
('Taxes','Summary Statement Request',0),
('Tech Support','Account Discrepancy',0),
('Tech Support','Creating Account',0),
('Tech Support','Homepage Functionality',0),
('Tech Support','Incorrect Info on Homepage',0),
('Tech Support','Password Reset',0),
('Tech Support','Portal Functionality',0),
('Tech Support','Portal Outage',0),
('Tech Support','Security Reset',0),
('Tech Support','Unlock Account',0),
('Tech Support','Website Issues',0),
('Tech Support','Website Outage',0),
('Third Party','Authorization Form',0),
('Third Party','POA',0),
('Third Party','Removing Authorization',0),
('Third Party','Requesting 24 hour Authorization',0),
('TILP','Application/Eligibility',1),
('TILP','Deferment Inquiry',1),
('TILP','Forgiveness Inquiry',1),
('TILP','Repayment',1)

SELECT * FROM @temp55507

UPDATE R 
	SET R.[Enabled] = 0 
FROM 
	MauiDUDE.calls.Categories C 
	INNER JOIN MauiDUDE.calls.Reasons R 
		ON R.CategoryId = C.CategoryId 
	LEFT JOIN @temp55507 T 
		ON T.Category = C.Title 
		AND T.Reason = R.ReasonText 
WHERE 
	T.Category IS NULL

INSERT INTO MauiDUDE.calls.Categories(Title)
SELECT DISTINCT
	T.Category 
FROM 
	@temp55507 T 
	LEFT JOIN MauiDUDE.calls.Categories C 
		ON T.Category = C.Title 
	LEFT JOIN MauiDUDE.calls.Reasons R 
		ON R.CategoryId = C.CategoryId 
		AND T.Reason = R.ReasonText 
WHERE 
	C.Title IS NULL

INSERT INTO MauiDUDE.calls.Reasons(CategoryId, ReasonText, Uheaa, Cornerstone, Inbound, Outbound, Enabled)
SELECT DISTINCT
	C.CategoryId,
	T.Reason,
	1,
	CASE WHEN T.UheaaOnly = 1 THEN 0 ELSE 1 END,
	1,
	1,
	1
FROM
	@temp55507 T 
	LEFT JOIN MauiDUDE.calls.Categories C 
		ON T.Category = C.Title 
	LEFT JOIN MauiDUDE.calls.Reasons R 
		ON R.CategoryId = C.CategoryId 
		AND T.Reason = R.ReasonText 
WHERE 
	C.Title IS NOT NULL 
	AND R.ReasonId IS NULL

UPDATE MauiDUDE.calls.Reasons SET Outbound = 1, Inbound = 1, Uheaa = 1