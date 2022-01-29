/****** Script for SelectTopNRows command from SSMS  ******/
SELECT M.[#]
      ,M.[Servicer completing this data file]
      ,M.[X digit SSN]
     ,ISNULL(T.[Has this borrower ever been on your system (as part of your fede],M.[Has this borrower ever been on your system (as part of your fede] ),
ISNULL(T.[Is this borrower currently on your system with a balance >$X?__Y],M.[Is this borrower currently on your system with a balance >$X?__Y] ),
ISNULL(T.[Is the borrower currently in forbearance / stopped collections w],M.[Is the borrower currently in forbearance / stopped collections w] ),
ISNULL(T.[If borrower was not in forbearance / stopped collections, have y],M.[If borrower was not in forbearance / stopped collections, have y] ),
ISNULL(T.[Borrower has been notified of approval or denial__X = Yes (do no],M.[Borrower has been notified of approval or denial__X = Yes (do no] ),
ISNULL(T.[If the borrower is now in forbearance - provide the current end ],M.[If the borrower is now in forbearance - provide the current end ] ),
ISNULL(T.[If borrower is still not in forbearance / stopped collections be],M.[If borrower is still not in forbearance / stopped collections be] ),
ISNULL(T.[Did you send this borrower any billing notices (saying they were],M.[Did you send this borrower any billing notices (saying they were] ),
ISNULL(T.[When was the last date you sent this borrower a billing notice (],M.[When was the last date you sent this borrower a billing notice (] ),
ISNULL(T.[Has this borrower made any payments on your system since X/XX/XX],M.[Has this borrower made any payments on your system since X/XX/XX] ),
ISNULL(T.[If payments were made at ANY TIME - What is the most recent date],M.[If payments were made at ANY TIME - What is the most recent date] ),
ISNULL(T.[(Non-Default only)_Have you submitted any adverse credit for the],M.[(Non-Default only)_Have you submitted any adverse credit for the] ),
ISNULL(T.[(Non-Default Only)_If you have EVER submitted adverse credit - W],M.[(Non-Default Only)_If you have EVER submitted adverse credit - W] ),
ISNULL(T.[(DMCS ONLY) Have we taken any involuntary payments from the borr],M.[(DMCS ONLY) Have we taken any involuntary payments from the borr] ),
ISNULL(T.[(DMCS ONLY)_If an invol# payment was taken, what is the most rec],M.[(DMCS ONLY)_If an invol# payment was taken, what is the most rec] ),
convert(varchar,t.[TRANSFER DATE], XXX) AS [TRANSFER DATE]
  FROM [CDW].[dbo].[CNHXXXXXMAIN] M
  LEFT JOIN CDW..CNHXXXXXTRANSFER T
	ON M.# = T.#
ORDER BY #