--run on UHEAASQLDB:
ALTER TABLE 
	ULS.docid.DocumentsProcessedResponse_OneLINK
ADD
	InvalidAddressArcAddProcessingId BIGINT NULL
;

ALTER TABLE 
	ULS.docid.DocumentsProcessedResponse_OneLINK
ADD FOREIGN KEY 
	(InvalidAddressArcAddProcessingId) 
REFERENCES 
	ULS.dbo.ArcAddProcessing(ArcAddProcessingId)--bigint data type
;
