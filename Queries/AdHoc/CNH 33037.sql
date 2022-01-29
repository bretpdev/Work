--doc ID's from Excel attachment
DECLARE @XL TABLE (DOCID VARCHAR(X))
INSERT INTO @XL (DOCID)
VALUES
 ('CRBOB')
,('CRBKP')
,('CRBDT')
,('CRBCT')
,('CRBDS')
,('CRBCS')
,('CRBNK')
,('CRBCR')
,('CRLCD')
,('CRCCR')
,('CRCOR')
,('CRREQ')
,('CRCDD')
,('CRDED')
,('CRDEM')
,('CRDBD')
,('CRDFM')
,('CRCDF')
,('CRCDO')
,('CRDDA')
,('CRDDC')
,('CRDDE')
,('CRDIM')
,('CRDIS')
,('CRDCH')
,('CRBRP')
,('CRCSK')
,('CRDTH')
,('CRFCR')
,('CRPSF')
,('CRSXX')
,('CRTLF')
,('CRTPD')
,('CRUPR')
,('CREML')
,('CRCEA')
,('CRCFX')
,('CRXFC')
,('CRFRB')
,('CRCFB')
,('COCOR')
,('CRAAA')
,('CRCIB')
,('CRLIB')
,('CRIBR')
,('CRIBW')
,('CRIBC')
,('CRICR')
,('CRIDR')
,('CRINV')
,('CRKEY')
,('CROEC')
,('CRPOC')
,('CRRPC')
,('CRSSN')
,('CRCTL')
,('CRXRD')
,('CRCHG')
,('CRINC')
,('CRTAX')

--SELECT * FROM @XL

Select 
                     datepart(month,DateTimeStamp) as Month,        
                     Datepart(year,DateTimeStamp) as Year,
					 COUNT(DOCP.DocId) as Count,
					 DOCP.DocId, 
				     DOCD.Arc
      
                     from [CLS]..[DocIdProcessed] DOCP
						INNER JOIN [CLS]..[DocIdDocument] DOCD
						ON DOCP.DocId = DOCD.DocId

						--limits output to only those doc ID's from Excel attachment
						INNER JOIN @XL XL
						ON DOCP.DocId = XL.DOCID					 
                     
                     where DateTimeStamp BETWEEN 'XXXX-XX-XX' and 'XXXX-XX-XX'
                          
              
                     group by datepart(month,DateTimeStamp),  Datepart(year,DateTimeStamp), DOCP.DocId, DOCD.Arc
              

       order by Datepart(year,DateTimeStamp), datepart(month,DateTimeStamp)

--see doc ID's that weren't in the Excel attachment
Select distinct
					'not in excel' [not in excel],
                   	 DOCP.DocId, 
				     DOCD.Arc
      
                     from [CLS]..[DocIdProcessed] DOCP
						INNER JOIN [CLS]..[DocIdDocument] DOCD
						ON DOCP.DocId = DOCD.DocId

						--limits output to only those doc ID's from Excel
						left JOIN @XL XL
						ON DOCP.DocId = XL.DOCID					 
                     
                     where DateTimeStamp BETWEEN 'XXXX-XX-XX' and 'XXXX-XX-XX'
                          and xl.DOCID is null
              
                   