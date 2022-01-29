CREATE PROCEDURE [dbo].[spGetQueueInfoForBin] 

@Bin		VARCHAR(100),
@BU 		VARCHAR(50)

AS

SELECT A.Queue, 'N' as DataPresent, Priority
FROM dbo.BinAndQueueXRef A
JOIN dbo.BUAndBinXRef B
	ON A.BinID = B.BinID
WHERE B.Bin = @Bin and B.BU = @BU
ORDER BY Priority