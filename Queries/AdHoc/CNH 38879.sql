SELECT
	*
FROM
	NobleCalls..NobleCallHistory
WHERE
	CreatedAt between 'XX/XX/XXXX' and 'XX/XX/XXXX'
	AND RegionId = X
	AND CallLength > X
	AND isnull(VoxFileLocation,'') = ''
	and VoxFileId != ''