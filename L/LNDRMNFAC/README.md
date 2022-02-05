# LNDRMNFAC
Lender Manifest Tasks to Auto-Close
● Identify all open 4X tasks whose ARC (LM749) contain any of the following error codes.  
     0801 -- Invld Dsbmt Dte
     0988 -- More Records
     3601 -- Invalid Date Entered Repay
     2401 -- Invalid Refund Amt
     3001 -- Invalid Date of Last Disb
     3101 -- invalid Disb Amt
     3201 -- Invalid Cancellation Date
     3301 -- Invalid Cancellation Amt
     3302 --- Cancellation Amt Differs From
● Output to table for Queue Completer to auto-close the tasks under reason X/CANCL.
