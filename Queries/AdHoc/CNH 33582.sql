--uncomment line XXXX to insert into CLS.[dbo].[ArcAddProcessing]
--uncomment line XXXX to insert into CLS.[emailbtcf].[CampaignData]

USE CDW
GO

/************* GET ALL DISASTER DATES AND ZIPS ***************/

--Maria - CNH XXXXX
DECLARE @Maria_BEGIN DATE = 'XX/XX/XXXX';
DECLARE @Maria_END DATE = 'XX/XX/XXXX';
DECLARE @Maria_ZIPS TABLE (ZIPS VARCHAR(X));
INSERT INTO @Maria_ZIPS (ZIPS) VALUES 
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX')
--SELECT * FROM @MARIA_ZIPS

--Irma - CNH XXXXX
DECLARE @Irma_BEGIN DATE = 'XX/XX/XXXX';
DECLARE @Irma_END DATE = 'XX/XX/XXXX';
DECLARE @Irma_ZIPSX TABLE (ZIPS VARCHAR(X));
DECLARE @Irma_ZIPSX TABLE (ZIPS VARCHAR(X));
INSERT INTO @Irma_ZIPSX (ZIPS) VALUES 
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX')
INSERT INTO @Irma_ZIPSX (ZIPS) VALUES 
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX')
--SELECT * FROM @IRMA_ZIPSX UNION SELECT * FROM @IRMA_ZIPSX

--CA Fire- CNH XXXXX
DECLARE @CAFire_BEGIN DATE = 'XXXX-XX-XX';
DECLARE @CAFire_END DATE = 'XXXX-XX-XX';
DECLARE @CAFire_ZIPS TABLE (ZIPS VARCHAR(X));
INSERT INTO @CAFire_ZIPS (ZIPS) VALUES 
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX')
--SELECT * FROM @CAFIRE_ZIPS

--CA Mudslide- CNH XXXXX
DECLARE @CAMudslide_BEGIN DATE = 'XXXX-XX-XX';
DECLARE @CAMudslide_END DATE =  DATEADD(DAY,XX,@CAMudslide_BEGIN);
DECLARE @CAMudslide_ZIPS TABLE (ZIPS VARCHAR(X));
INSERT INTO @CAMudslide_ZIPS (ZIPS) VALUES 
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX')
--SELECT * FROM @CAMUDSLIDE_ZIPS

--Harvey- CNH XXXXX
DECLARE @Harvey_BEGIN DATE = 'XX/XX/XXXX';
DECLARE @Harvey_END DATE = 'XX/XX/XXXX';
DECLARE @Harvey_ZIPS TABLE (ZIPS VARCHAR(X));
INSERT INTO @Harvey_ZIPS (ZIPS) VALUES 
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),('XXXXX'),('XXXXX')
--SELECT * FROM @HARVEY_ZIPS


/************* IDENTIFY POP AND CALCULATE FORB EXTENSION DATES ***************/

;WITH ZIPS AS --CTE used in BASE_POP as join and in where clause
(--all affected zip codes combined
		  SELECT 'Maria'[Disaster],@Maria_BEGIN [InitialBegin],@Maria_END [InitialEnd],* FROM @Maria_ZIPS
	UNION SELECT 'Irma'	[Disaster],@Irma_BEGIN [InitialBegin],@Irma_END [InitialEnd],* FROM @Irma_ZIPSX 
	UNION SELECT 'Irma'	[Disaster],@Irma_BEGIN [InitialBegin],@Irma_END [InitialEnd],* FROM @Irma_ZIPSX
	UNION SELECT 'CA Fire'[Disaster],@CAFire_BEGIN [InitialBegin],@CAFire_END [InitialEnd],* FROM @CAFire_ZIPS
	UNION SELECT 'CA Mudslide'[Disaster],@CAMudslide_BEGIN [InitialBegin], @CAMudslide_END [InitialEnd],* FROM @CAMudslide_ZIPS
	UNION SELECT 'Harvey'[Disaster],@Harvey_BEGIN [InitialBegin],@Harvey_END [InitialEnd],* FROM @Harvey_ZIPS
)
,BASE_POP AS
(--core population of affected borrowers
	SELECT
		PDXX.DF_SPE_ACC_ID
		,LNXX.BF_SSN
		,LNXX.LN_SEQ
		,RTRIM(PDXX.DM_PRS_X) AS FirstName
		,RTRIM(PDXX.DM_PRS_LST) AS LastName
		,CAST(LNXX.LD_DLQ_OCC AS DATE) LD_DLQ_OCC
		--,LNXX.LN_DLQ_MAX
		,(LNXX.LN_DLQ_MAX + X) AS [LN_DLQ_MAX+X]
		,LNXX.LC_STA_LONXX
		,FBXX.LC_FOR_TYP
		,FBXX.LF_FOR_CTL_NUM
		,ZIPS.ZIPS
		,ZIPS.Disaster
		,ZIPS.InitialBegin
		,ZIPS.InitialEnd
		,DATEDIFF(DAY,ZIPS.InitialBegin,ZIPS.InitialEnd)+X AS InitialDays -- +X on datediff to count actual begin date
		,DATEADD(DAY,XXX,ZIPS.InitialBegin) AS ExtensionEnd -- -X on dateadd to count actual begin date
		,DATEDIFF(DAY,ZIPS.InitialBegin,DATEADD(DAY,XXX,ZIPS.InitialBegin))+X AS ExtensionDays -- +X on datediff to count actual begin date
		,CAST(LNXX.LD_FOR_BEG AS DATE) AS LD_FOR_BEG
		,CAST(LNXX.LD_FOR_END AS DATE) AS LD_FOR_END
		,DATEDIFF(DAY,LNXX.LD_FOR_BEG,LNXX.LD_FOR_END)+X AS FOR_Days -- +X on datediff to count actual begin date
	FROM
		LNXX_LON LNXX
		INNER JOIN PDXX_PRS_NME PDXX 
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
		INNER JOIN LNXX_LON_DLQ_HST LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		INNER JOIN LNXX_BR_FOR_APV LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		INNER JOIN FBXX_BR_FOR_REQ FBXX
			ON LNXX.BF_SSN = FBXX.BF_SSN
			AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
		INNER JOIN PDXX_PRS_ADR PDXX
			ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		INNER JOIN ZIPS --from ZIPS CTE above
			ON SUBSTRING(PDXX.DF_ZIP_CDE, X, X) = ZIPS.ZIPS
		--LEFT JOIN --TODO: find borrowers with X forbs starting after initialEnd date and exclude from pulling
		--(	
		--	SELECT 
		--		FBXX.BF_SSN
		--		,FBXX.LF_FOR_CTL_NUM
		--		,FBXX.LC_FOR_TYP
		--	FROM
		--		FBXX_BR_FOR_REQ FBXX
		--		INNER JOIN
		--		(--get max control number
		--			SELECT 
		--				 BF_SSN
		--				,MAX(LF_FOR_CTL_NUM) AS LF_FOR_CTL_NUM
		--			FROM
		--				FBXX_BR_FOR_REQ
		--			WHERE
		--				LC_FOR_STA = 'A'
		--				AND LC_STA_FORXX = 'A'
		--			GROUP BY
		--				BF_SSN
		--		) FBXX_MAX
		--			ON FBXX.BF_SSN = FBXX_MAX.BF_SSN
		--			AND FBXX.LF_FOR_CTL_NUM = FBXX_MAX.LF_FOR_CTL_NUM
		--	WHERE
		--		LC_FOR_STA = 'A'
		--		AND LC_STA_FORXX = 'A'
		--) FBXXA
		--	ON LNXX.BF_SSN = FBXXA.BF_SSN
		--	AND LNXX.LF_FOR_CTL_NUM = FBXXA.LF_FOR_CTL_NUM
	WHERE
		LNXX.LN_DLQ_MAX > 'X'
		AND LNXX.LC_STA_LONXX = 'X'
		--AND FBXX.LC_FOR_TYP = XX
		AND FBXX.LC_FOR_STA = 'A'
		AND FBXX.LC_STA_FORXX = 'A'
		AND LNXX.LC_STA_LONXX = 'A'
		AND DATEDIFF(DAY,LNXX.LD_FOR_BEG,LNXX.LD_FOR_END) <= XXX
		AND PDXX.DI_VLD_ADR = 'Y'
		AND 
		(--from ZIPS CTE above
			   (ZIPS.Disaster = 'Maria' AND LNXX.LD_FOR_BEG BETWEEN @Maria_BEGIN AND @Maria_END)
			OR (ZIPS.Disaster = 'Irma' AND LNXX.LD_FOR_BEG BETWEEN @Irma_BEGIN AND @Irma_END)
			OR (ZIPS.Disaster = 'CA Fire' AND LNXX.LD_FOR_BEG BETWEEN @CAFire_BEGIN AND @CAFire_END)
			OR (ZIPS.Disaster = 'CA Mudslide' AND LNXX.LD_FOR_BEG BETWEEN @CAMudslide_BEGIN AND @CAMudslide_END)
			OR (ZIPS.Disaster = 'Harvey' AND LNXX.LD_FOR_BEG BETWEEN @Harvey_BEGIN AND @Harvey_END)
		)
		AND LNXX.LA_CUR_PRI > X.XX
)
,EXTENSION_X AS
(--everyone gets initial XX days extension
	SELECT --TODO: if XX days will wrap XXX window, give less than XX days
		CASE
			WHEN LC_FOR_TYP = XX
			THEN DATEADD(DAY,X,InitialEnd) --TODO: this only works for people that had the initial XX day forb.
			WHEN LC_FOR_TYP <> XX
			THEN DATEADD(DAY,X,LD_DLQ_OCC) --changed for non XX forb accounts
			ELSE NULL
		END AS ForbStartX
		,CASE
			WHEN LC_FOR_TYP = XX
			THEN DATEADD(DAY,XX,InitialEnd) --TODO: this only works for people that had the initial XX day forb.
			WHEN LC_FOR_TYP <> XX
			THEN DATEADD(DAY,XX,LD_DLQ_OCC) --changed for non XX forb accounts
			ELSE NULL
		END AS ForbEndX
		,*
	FROM
		BASE_POP A
--		where df_spe_acc_id = 'XXXXXXXXXX' --TODO: HAVE QUERY CHOOSE THE MAX CONTROL NUMBER TO CALCULATE EXTENSIONS FOR BORROWERS MARKED AS MANUAL REVIEW
)
,EXTENSION_X AS
(--tack on Xnd extension based on initial extension if needed
	SELECT --TODO: if XX days will wrap XXX window, give less than XX days
		CASE
			WHEN [LN_DLQ_MAX+X] >= XX
			THEN DATEADD(DAY,X,ForbEndX)
			ELSE NULL
		END AS ForbStartX
		,CASE
			WHEN [LN_DLQ_MAX+X] >= XX
			THEN DATEADD(DAY,XX,ForbEndX)
			ELSE NULL
		END AS ForbEndX
		,*
	FROM
		EXTENSION_X
)
,EXTENSION_X AS
(--tack on Xrd extension based on Xnd extension if needed
	SELECT --TODO: if XX days will wrap XXX window, give less than XX days
		CASE
			WHEN EX.[LN_DLQ_MAX+X] > XX
			THEN DATEADD(DAY,X,EX.ForbEndX)
			ELSE NULL
		END AS ForbStartX
		,CASE
			WHEN EX.[LN_DLQ_MAX+X] > XX
			THEN DATEADD(DAY,XX,EX.ForbEndX)
			ELSE NULL
		END AS ForbEndX
		,EX.*
		,CASE --flag if have non-XX forb type after initial disaster forb 
			WHEN NONXX.BF_SSN IS NOT NULL
			THEN X
			ELSE X
		END AS ManualReview
	FROM
		EXTENSION_X EX
		LEFT JOIN
		(--flag for manual review if have non-XX forb type after initial disaster forb type XX
			SELECT
				*
			FROM
				BASE_POP
			WHERE
				LC_FOR_TYP <> XX
		)NONXX
			ON  EX.BF_SSN = NONXX.BF_SSN
			AND EX.LN_SEQ = NONXX.LN_SEQ
			AND EX.LC_FOR_TYP <> NONXX.LC_FOR_TYP
			AND EX.LC_FOR_TYP = XX
			AND NONXX.LD_FOR_END > EX.InitialEnd
)
SELECT DISTINCT --final output at loan level
	DF_SPE_ACC_ID
	,BF_SSN
	,FirstName
	,LastName
	,LC_STA_LONXX
	,LC_FOR_TYP
	,LF_FOR_CTL_NUM
	,LD_DLQ_OCC
	,[LN_DLQ_MAX+X]
	,ZIPS
	,Disaster
	,InitialBegin
	,InitialEnd
	,InitialDays
	,ExtensionEnd
	,ExtensionDays
	,LD_FOR_BEG
	,LD_FOR_END
	,FOR_Days
	,ForbStartX
	,ForbEndX
	,ForbStartX
	,ForbEndX
	,ForbStartX
	,ForbEndX
	,ManualReview
INTO
	#FORB_POP
FROM
	EXTENSION_X
;



/************* OUTPUT PROCESSING:  MANUAL REVIEW ***************/

--manual review:  add to CLS.[dbo].[ArcAddProcessing]
--SELECT * FROM #FORB_POP WHERE ManualReview = X

--INSERT INTO CLS.dbo.ArcAddProcessing(ArcTypeId,ArcResponseCodeId,AccountNumber,RecipientId,ARC,ScriptId,ProcessOn,Comment,IsReference,IsEndorser,ProcessFrom,ProcessTo,NeededBy,RegardsTo,RegardsCode,LN_ATY_SEQ,ProcessingAttempts,CreatedAt,CreatedBy,ProcessedAt)
--SELECT DISTINCT
--	X [ArcTypeId]
--	,NULL [ArcResponseCodeId]
--	,FB.DF_SPE_ACC_ID [AccountNumber]
--	,NULL [RecipientId]
--	,'DASFB' [ARC]
--	,'DASFORBFED' [ScriptId]
--	,GETDATE() [ProcessOn]
--	,FB.Disaster + ' Disaster Forbearance Extension Manual Review' [Comment]
--	,X [IsReference]
--	,X [IsEndorser]
--	,NULL [ProcessFrom]
--	,NULL [ProcessTo]
--	,NULL [NeededBy]
--	,NULL [RegardsTo]
--	,NULL [RegardsCode]
--	,NULL [LN_ATY_SEQ]
--	,X [ProcessingAttempts]
--	,GETDATE() [CreatedAt]
--	,SYSTEM_USER [CreatedBy]
--	,NULL [ProcessedAt]
--FROM
--	#FORB_POP FB
--WHERE
--	ManualReview = X

;

/************* OUTPUT PROCESSING:  SEND EMAIL ***************/

--not manual review:  email batch campaign add to CLS.[emailbtcf].[CampaignData]
--SELECT * FROM #FORB_POP WHERE ManualReview = X

--INSERT INTO CLS.emailbtcf.CampaignData(EmailCampaignId,Recipient,AccountNumber,FirstName,LastName,AddedAt,AddedBy,EmailSentAt,ArcProcessedAt,ArcAddProcessingId,InactivatedAt)
SELECT DISTINCT
	XX [EmailCampaignId]
	,COALESCE(PHXX.DX_CNC_EML_ADR, PDXX.ALT_EM) [Recipient]
	,FP.DF_SPE_ACC_ID [AccountNumber]
	,FP.FirstName
	,FP.LastName
	,GETDATE() [AddedAt]
	,SUSER_SNAME() [AddedBy]
	,NULL [EmailSentAt]
	,NULL [ArcProcessedAt]
	,NULL [ArcAddProcessingId]
	,NULL [InactivatedAt]
FROM
	#FORB_POP FP
	LEFT JOIN PHXX_CNC_EML PHXX 
		ON PHXX.DF_SPE_ID = FP.DF_SPE_ACC_ID
		AND	PHXX.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
		AND PHXX.DI_CNC_ELT_OPI = 'Y' --on ecorr
	LEFT JOIN
	( -- email address
		SELECT 
			DF_PRS_ID, 
			Email.EM [ALT_EM],
 			ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) [EmailPriority] -- number in order of Email.PriorityNumber 
 		FROM 
 		( 
 			SELECT 
 				PDXX.DF_PRS_ID, 
 				PDXX.DX_ADR_EML [EM], 
 				CASE	  
 					WHEN DC_ADR_EML = 'H' THEN X -- home 
 					WHEN DC_ADR_EML = 'A' THEN X -- alternate 
 					WHEN DC_ADR_EML = 'W' THEN X -- work 
 				END AS PriorityNumber
 			FROM 
 				PDXX_PRS_ADR_EML PDXX 
 			WHERE 
 				PDXX.DI_VLD_ADR_EML = 'Y' -- valid email address 
 				AND PDXX.DC_STA_PDXX = 'A' -- active email address record 

 		) Email 
	) PDXX 
		ON PDXX.DF_PRS_ID = FP.BF_SSN
		AND PDXX.EmailPriority = X
	--LEFT JOIN #FORB_POP FPMRX --forbearance pop manual review = X
	--	ON FP.DF_SPE_ACC_ID = FPMRX.DF_SPE_ACC_ID
	--	AND FPMRX.ManualReview = X
WHERE
	--FPMRX.DF_SPE_ACC_ID IS NULL --remove manual review pop
	--AND FP.ManualReview = X
	(
			PHXX.DX_CNC_EML_ADR IS NOT NULL
			OR PDXX.ALT_EM IS NOT NULL
	)
;

/************* OUTPUT PROCESSING:  SCRIPT FILE ***************/

--not manual review:  script processing
SELECT
	SCRIPTFILE.DF_SPE_ACC_ID
	,SCRIPTFILE.BeginDate
	,SCRIPTFILE.EndDate
FROM
	(
		SELECT
			DF_SPE_ACC_ID
			,ForbStartX [BeginDate]
			,ForbEndX [EndDate]
			,ManualReview
		FROM
			#FORB_POP

		UNION

		SELECT
			DF_SPE_ACC_ID
			,ForbStartX [BeginDate]
			,ForbEndX [EndDate]
			,ManualReview
		FROM
			#FORB_POP

		UNION

		SELECT
			DF_SPE_ACC_ID
			,ForbStartX [BeginDate]
			,ForbEndX [EndDate]
			,ManualReview
		FROM
			#FORB_POP
	)SCRIPTFILE
	--LEFT JOIN #FORB_POP FPMRX --forbearance pop manual review = X
	--	ON SCRIPTFILE.DF_SPE_ACC_ID = FPMRX.DF_SPE_ACC_ID
	--	AND FPMRX.ManualReview = X
WHERE
	--FPMRX.DF_SPE_ACC_ID IS NULL --remove manual review pop
	--AND	SCRIPTFILE.ManualReview = X
	SCRIPTFILE.BeginDate IS NOT NULL
	AND SCRIPTFILE.EndDate IS NOT NULL
ORDER BY 
	 SCRIPTFILE.DF_SPE_ACC_ID
	,SCRIPTFILE.BeginDate
	,SCRIPTFILE.EndDate
;
