CREATE PROCEDURE [dbo].[spCallCategorizationInsert]

@CAT 	VARCHAR(50),
@REA  VARCHAR(50),
@LID 	VARCHAR(10),
@CMT	VARCHAR(30),
@UID	VARCHAR(100),
@Region varchar(11) = 'Uheaa'

AS

INSERT INTO dbo.CallCat_Data (Catergory, Reason, LetterID, Comments, UserID, [Region]) VALUES (@CAT, @REA, @LID, @CMT, @UID, @Region)