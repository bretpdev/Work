CREATE FUNCTION [dbo].[GET_LOAN_PROGRAM]
(
    @NelNetProgram VARCHAR(4),
    @DisbDate DATE,
    @SpousalSSN VARCHAR(9)
)
RETURNS VARCHAR(6)
AS
BEGIN

    RETURN CASE
        WHEN @NelNetProgram IN ('CONS','CONU','CONV') AND @DisbDate < '11/13/1997' THEN  'CNSLDN'
        WHEN @NelNetProgram  = 'CONS' AND @DisbDate >= '11/13/1997' and @SpousalSSN = '' THEN  'SUBCNS'
        WHEN @NelNetProgram IN ('CONU','CONV') AND @DisbDate >= '11/13/1997' and @SpousalSSN = '' THEN  'UNCNS'
        WHEN @NelNetProgram  = 'CONS' AND @DisbDate >= '11/13/1997' and @SpousalSSN != '' THEN  'SUBSPC'
        WHEN @NelNetProgram IN ('CONU','CONV') AND @DisbDate >= '11/13/1997' and @SpousalSSN != '' THEN  'UNSPC'
        WHEN @NelNetProgram  = 'STAF' THEN 'STFFRD'
        WHEN @NelNetProgram  = 'STAU' THEN 'UNSTFD'
        WHEN @NelNetProgram  = 'PLUS' THEN 'PLUS'
        WHEN @NelNetProgram  IN ('GRAD','GPSL') THEN 'PLUSGB'
        WHEN @NelNetProgram = 'SLS' THEN 'SLS'
        ELSE 'ERROR'

    END
END
