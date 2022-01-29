USE BSYS
GO

INSERT INTO BSYS..SCKR_REF_StatusPIR(Sequence, Request, Class, Status, Begin, End, Court)
VALUES(10077, 4429, 'Scr', 'Post-Implementation Review', GETDATE(), NULL, 'Eric Barnes')
INSERT INTO BSYS..SCKR_DAT_PIR(Request, Class, ReviewNo, Reviewer, Comments, ReturnReason, ReturnDescription)
VALUES(4429,'Scr','1','Michelle Hansen',NULL, NULL, NULL)
