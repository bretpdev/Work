# UTLWS17
Billing Statement not Sent to Borrower and Due Diligence Letters

-Retirement notes:

Previously, ebill and autopay borrowers received AES ebills which didn’t include the due diligence language, so scripts were 
needed to send letters.  Now that these borrowers will receive UHEAA ebills, the due diligence jobs will no longer be needed.

SASR 4179 is integrating ebill and autopay borrowers into the UHEAA Billing SAS job.  This modification will ensure these borrowers 
receive UHEAA-generated billing statements in their ecorr inbox.   Billing SAS will begin pulling all bills, regardless of ebill, 
ecorr, ACH, or HEA Reauth status.  All bills should fall into the normal files so that they include the due diligence language.  

As soon as the billing update (SASR 4179) is promoted, we will retire the following SAS and Scripts:
SAS:  Billing Statement not sent to borrower and due diligence letters (SASR 4185 aka UTLWS17)
Script:  Billing Statement not sent to borrower (SR 4631)
Script:  Collection Notices sent to non billed borrowers (SR 4630)
