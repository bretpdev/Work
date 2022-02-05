CREATE PROCEDURE [peps].[GetIdentiferData]

AS

	SELECT
		[SCHIDS_ID] AS RecordId,
		[RecordType],
		[OpeId],
		[ChangeIndicator],
		[TinCurrent],
		[TinPrevious],
		[CmoGdunsNbrCurrent],
		[CmoGdunsNbrPrevious],
		[PellGdunsNbrCurrent],
		[PellGdunsNbrPrevious],
		[Filler1],
		[FdslpGdunsNbrCurrent],
		[FdslpGdunsNbrPrevious],
		[Filler2],
		[CampusBasedGdunsNbrCurrent],
		[CampusBasedGdunsNbrPrevious],
		[PellIdCurrent],
		[PellIdPrevious],
		[FfelIdCurrent],
		[FfelIdPrevious],
		[FdslpIdCurrent],
		[FdslpIdPrevious],
		[CampusBasedIdCurrent],
		[CampusBasedIdPrevious],
		[FedSchlCdCurrent],
		[FedSchlCdPrevious],
		[FdslpSeqNbrCurrent],
		[FdslpSeqNbrPrevious],
		[Filler]
	FROM 
		[CLS].[peps].[SCHIDS]
	WHERE
		ProcessedAt IS NULL 
		AND
		DeletedAt IS NULL
RETURN 0
