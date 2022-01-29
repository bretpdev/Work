--run on UHEAASQLDB:
ALTER TABLE 
	ULS.docid.DocumentsProcessedResponse
ADD
	InvalidAddressArcAddProcessingId BIGINT NULL
;

ALTER TABLE 
	ULS.docid.DocumentsProcessedResponse
ADD FOREIGN KEY 
	(InvalidAddressArcAddProcessingId) 
REFERENCES 
	ULS.dbo.ArcAddProcessing(ArcAddProcessingId)--bigint data type
;