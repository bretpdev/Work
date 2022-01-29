--run on UHEAASQLDB:
ALTER TABLE 
	CLS.docid.DocumentsProcessedResponse
ADD
	InvalidAddressArcAddProcessingId BIGINT NULL
;

ALTER TABLE 
	CLS.docid.DocumentsProcessedResponse
ADD FOREIGN KEY 
	(InvalidAddressArcAddProcessingId) 
REFERENCES 
	CLS.dbo.ArcAddProcessing(ArcAddProcessingId)--bigint data type
;