CREATE PROCEDURE [print].[GetInternalNames] AS
SELECT 
	repl.[ReplacementSetId],
	repl.[FileHeader],
	repl.[InternalName]
FROM [print].HeaderReplacementCoBorrower repl
RETURN 0
