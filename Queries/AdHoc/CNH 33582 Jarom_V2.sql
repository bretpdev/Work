USE CDW
GO
set transaction isolation level read uncommitted

/************* GET ALL DISASTER DATES AND ZIPS ***************/

--Maria - CNH XXXXX
DECLARE @Maria_BEGIN DATE = 'XX/XX/XXXX';
DECLARE @Maria_END DATE = 'XX/XX/XXXX';
DECLARE @Maria_MAX_END date = DATEADD(DAY, XXX, @Maria_BEGIN)
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
DECLARE  @Irmaa_MAX_END date = DATEADD(DAY, XXX, @Irma_BEGIN)
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
DECLARE  @CAFire_MAX_END date = DATEADD(DAY, XXX, @CAFire_BEGIN)
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

----CA Mudslide- CNH XXXXX
--DECLARE @CAMudslide_BEGIN DATE = 'XXXX-XX-XX';
--DECLARE @CAMudslide_END DATE =  DATEADD(DAY,XX,@CAMudslide_BEGIN);
--DECLARE  @CAMudslide_MAX_END date = DATEADD(DAY, XXX, @CAMudslide_BEGIN)
--DECLARE @CAMudslide_ZIPS TABLE (ZIPS VARCHAR(X));
--INSERT INTO @CAMudslide_ZIPS (ZIPS) VALUES 
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),
--('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX')
----SELECT * FROM @CAMUDSLIDE_ZIPS

--Harvey- CNH XXXXX
DECLARE @Harvey_BEGIN DATE = 'XX/XX/XXXX';
DECLARE @Harvey_END DATE = 'XX/XX/XXXX';
DECLARE  @Harvey_MAX_END date = DATEADD(DAY, XXX, @Harvey_BEGIN)

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

IF OBJECT_ID('tempdb..#FORB_POP') IS NOT NULL 
	DROP TABLE #FORB_POP

IF OBJECT_ID('tempdb..#FINAL_FORB_POP') IS NOT NULL 
	DROP TABLE #FINAL_FORB_POP

/************* IDENTIFY POP AND CALCULATE FORB EXTENSION DATES ***************/

;WITH ZIPS AS --CTE used in BASE_POP as join and in where clause
(--all affected zip codes combined
		  SELECT 'Maria'[Disaster],@Maria_BEGIN [InitialBegin],@Maria_END [InitialEnd], @Maria_MAX_END [max_end],* FROM @Maria_ZIPS
	UNION SELECT 'Irma'	[Disaster],@Irma_BEGIN [InitialBegin],@Irma_END [InitialEnd],@Irmaa_MAX_END [max_end],* FROM @Irma_ZIPSX 
	UNION SELECT 'Irma'	[Disaster],@Irma_BEGIN [InitialBegin],@Irma_END [InitialEnd],@Irmaa_MAX_END [max_end],* FROM @Irma_ZIPSX
	UNION SELECT 'CA Fire'[Disaster],@CAFire_BEGIN [InitialBegin],@CAFire_END [InitialEnd],@CAFire_MAX_END [max_end],* FROM @CAFire_ZIPS
	--UNION SELECT 'CA Mudslide'[Disaster],@CAMudslide_BEGIN [InitialBegin], @CAMudslide_END [InitialEnd],@CAMudslide_MAX_END [max_end],* FROM @CAMudslide_ZIPS
	UNION SELECT 'Harvey'[Disaster],@Harvey_BEGIN [InitialBegin],@Harvey_END [InitialEnd],@Harvey_MAX_END [max_end],* FROM @Harvey_ZIPS
)
,BASE_POP AS
(--core population of affected borrowers
	SELECT distinct
		PDXX.DF_SPE_ACC_ID
		,LNXX.BF_SSN
		,LNXX.LN_SEQ
		,RTRIM(PDXX.DM_PRS_X) AS FirstName
		,RTRIM(PDXX.DM_PRS_LST) AS LastName
		,CAST(LNXX_mins.LD_DLQ_OCC AS DATE) LD_DLQ_OCC
		,(LNXX_mins.LN_DLQ_MAX + X) AS [LN_DLQ_MAX+X]
		,ZIPS.ZIPS
		,ZIPS.Disaster
		,ZIPS.InitialBegin
		,ZIPS.InitialEnd
		,ZIPS.max_end
		,DATEDIFF(DAY,ZIPS.InitialBegin,ZIPS.InitialEnd)+X AS InitialDays -- +X on datediff to count actual begin date
		,DATEADD(DAY,XXX,ZIPS.InitialBegin) AS ExtensionEnd -- -X on dateadd to count actual begin date
		,DATEDIFF(DAY,ZIPS.InitialBegin,DATEADD(DAY,XXX,ZIPS.InitialBegin))+X AS ExtensionDays -- +X on datediff to count actual begin date
		,WQXX.WF_QUE
		,WQXX.WF_SUB_QUE
		,WQXX.WC_STA_WQUEXX
	FROM
		LNXX_LON LNXX
		INNER JOIN PDXX_PRS_NME PDXX 
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
		INNER JOIN LNXX_LON_DLQ_HST LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		INNER JOIN 
		(
			SELECT
				LNXX.BF_SSN,
				MIN(LD_DLQ_OCC) AS LD_DLQ_OCC,
				MIN(LN_DLQ_MAX) AS LN_DLQ_MAX
			FROM
				LNXX_LON_DLQ_HST LNXX
				INNER JOIN CDW..LNXX_LON LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
			WHERE 
				LNXX.LC_STA_LONXX = 'X'
				AND LNXX.LA_CUR_PRI > X
				AND LNXX.LC_STA_LONXX = 'R'
			GROUP BY	
				LNXX.BF_SSN
		) LNXX_mins
			ON LNXX.BF_SSN = LNXX_mins.BF_SSN
		INNER JOIN PDXX_PRS_ADR PDXX
			ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		INNER JOIN ZIPS --from ZIPS CTE above
			ON SUBSTRING(PDXX.DF_ZIP_CDE, X, X) = ZIPS.ZIPS
		--INNER JOIN--USED TO REMOVE BORROWERS WHO HAD EXTENSION APPLIED THE PREVIOUS DAY
		--(--this portion taken from #FINAL_FORB_POP
		--	SELECT DISTINCT
		--		LNXX.BF_SSN,
		--		LNXX.LN_SEQ,
		--		LNXX.LD_FOR_APL
		--	FROM
		--		FBXX_BR_FOR_REQ FBXX
		--		INNER JOIN LNXX_BR_FOR_APV LNXX
		--			ON LNXX.BF_SSN = FBXX.BF_SSN
		--			AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
		--	WHERE
		--		FBXX.LC_FOR_STA = 'A'
		--		AND FBXX.LC_STA_FORXX = 'A'	
		--		AND LNXX.LC_STA_LONXX = 'A'
		--		AND FBXX.LC_FOR_TYP = 'XX'
		--		AND DATEDIFF(DAY, LNXX.LD_FOR_BEG,LNXX.LD_FOR_END) <= XX
		--		--AND LNXX.LD_FOR_APL <> DATEADD(DAY,-X,CAST(GETDATE() AS DATE)) --exclude lnXX forbearance applied yesterday
		--) LNXXX
		--	ON LNXX.BF_SSN = LNXXX.BF_SSN
		--	AND LNXX.LN_SEQ = LNXXX.LN_SEQ
		LEFT JOIN WQXX_TSK_QUE WQXX --earmarked for exclusion
			ON LNXX.BF_SSN = WQXX.BF_SSN
			AND WQXX.WF_QUE = 'XX'
			AND WQXX.WF_SUB_QUE = 'XX'
			AND WQXX.WC_STA_WQUEXX IN ('A','H','P', 'U', 'W')
	WHERE
		WQXX.BF_SSN IS NULL --excludes WQXX as per criteria above
		--AND LNXXX.LD_FOR_APL <> DATEADD(DAY,-X,CAST(GETDATE() AS DATE)) --exclude lnXX forbearance applied yesterday
		AND LNXX_mins.LN_DLQ_MAX > X
		AND PDXX.DI_VLD_ADR = 'Y'
		AND 
		(--from ZIPS CTE above
			   (ZIPS.Disaster = 'Maria' AND LNXX_mins.LD_DLQ_OCC BETWEEN @Maria_BEGIN AND @Maria_MAX_END)
			OR (ZIPS.Disaster = 'Irma' AND LNXX_mins.LD_DLQ_OCC BETWEEN @Irma_BEGIN AND @Irmaa_MAX_END)
			OR (ZIPS.Disaster = 'CA Fire' AND LNXX_mins.LD_DLQ_OCC BETWEEN @CAFire_BEGIN AND @CAFire_MAX_END)
			--OR (ZIPS.Disaster = 'CA Mudslide' AND LNXX_mins.LD_DLQ_OCC BETWEEN @CAMudslide_BEGIN AND @CAMudslide_MAX_END)
			OR (ZIPS.Disaster = 'Harvey' AND LNXX_mins.LD_DLQ_OCC BETWEEN @Harvey_BEGIN AND @Harvey_MAX_END)
		)
		AND LNXX.LA_CUR_PRI > X.XX
		AND LNXX.LC_STA_LONXX = 'X'
		--and DF_SPE_ACC_ID = 'XXXXXXXXXX'
)
,EXTENSION_X AS
(--everyone gets initial XX days extension
	SELECT
		 DATEADD(DAY,X,LD_DLQ_OCC) AS ForbStartX
		,CASE
			WHEN  DATEADD(DAY,XX,LD_DLQ_OCC) > max_end
			THEN max_end
			ELSE DATEADD(DAY,XX,LD_DLQ_OCC) --changed for non XX forb accounts
		END AS ForbEndX
		,*
	FROM
		BASE_POP A
)
,EXTENSION_X AS
(--tack on Xnd extension based on initial extension if needed
	SELECT 
		CASE
			WHEN [LN_DLQ_MAX+X] >= XX AND DATEADD(DAY,X,ForbEndX) < max_end
			THEN DATEADD(DAY,X,ForbEndX)
			ELSE NULL
		END AS ForbStartX
		,CASE
			WHEN [LN_DLQ_MAX+X] >= XX AND DATEADD(DAY,XX,ForbEndX) > max_end
			THEN max_end
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
			WHEN EX.[LN_DLQ_MAX+X] > XX AND DATEADD(DAY,X,ForbEndX) < EX.max_end
			THEN DATEADD(DAY,X,EX.ForbEndX)
			ELSE NULL
		END AS ForbStartX
		,CASE
			WHEN EX.[LN_DLQ_MAX+X] > XX  AND DATEADD(DAY,XX,ForbEndX) > EX.max_end
			THEN EX.max_end
			WHEN EX.[LN_DLQ_MAX+X] > XX 
			THEN DATEADD(DAY,XX,EX.ForbEndX)
			ELSE NULL
		END AS ForbEndX
		,EX.*
	FROM
		EXTENSION_X EX
)
SELECT DISTINCT --final output at loan level
	DF_SPE_ACC_ID
	,BF_SSN
	,FirstName
	,LastName
	,LD_DLQ_OCC
	,[LN_DLQ_MAX+X]
	,ZIPS
	,Disaster
	,InitialBegin
	,InitialEnd
	,InitialDays
	,ExtensionEnd
	,ExtensionDays
	,ForbStartX
	,ForbEndX
	,ForbStartX
	,ForbEndX
	,ForbStartX
	,ForbEndX
	,WF_QUE
	,WF_SUB_QUE
	,WC_STA_WQUEXX
	--,LD_FOR_APL
INTO
	#FORB_POP
FROM
	EXTENSION_X
;

--Thursday X/XX exclusion logic:
--Borrower has used X type XX forbearances between the XX Initial end date and XXX day End date 
--Or The borrower has used greater than or equal to XXX days of type XX forbearance between: XX Initial Start and XXX day end 

--SELECT * FROM #FORB_POP where DF_SPE_ACC_ID = 'XXXXXXXXXX'

;WITH FORB AS
(--get forbearance dates and days used to flag if -Borrower has used X type XX forbearances between the XX Initial end date and XXX day End date 
		SELECT DISTINCT
			FBXX.BF_SSN,
			LNXX.LD_FOR_BEG,
			LNXX.LD_FOR_END,
			DATEDIFF(DAY, LNXX.LD_FOR_BEG,LNXX.LD_FOR_END)+X AS DIFF  -- +X on datediff to count actual begin date
		FROM
			FBXX_BR_FOR_REQ FBXX
			INNER JOIN LNXX_BR_FOR_APV LNXX
				ON LNXX.BF_SSN = FBXX.BF_SSN
				AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
		WHERE
			FBXX.LC_FOR_STA = 'A'
			AND FBXX.LC_STA_FORXX = 'A'	
			AND LNXX.LC_STA_LONXX = 'A'
			AND FBXX.LC_FOR_TYP = 'XX'
			AND DATEDIFF(DAY, LNXX.LD_FOR_BEG,LNXX.LD_FOR_END) <= XX
			--and lnXX.BF_SSN = 'XXXXXXXXX'
)
,CASEXPOP AS
(--Borrower has used X type XX forbearances between the XX Initial end date and XXX day End date 
	SELECT distinct
		POP.*, 
		ISNULL(
				SUM(--flag if -Borrower has used X type XX forbearances between the XX Initial end date and XXX day End date 
						CASE 
							WHEN FORB.LD_FOR_BEG between POP.InitialEnd and POP.ExtensionEnd 
							THEN X
							ELSE X
						END
					)
				OVER(PARTITION BY POP.BF_SSN, POP.DISASTER)
			,X)
		AS XFORB_COUNT
	FROM 
		#FORB_POP POP
		LEFT JOIN FORB
			ON POP.BF_SSN = FORB.BF_SSN
)
SELECT DISTINCT
	CASEXPOP.*,
	ISNULL(--flag if -The borrower has used greater than or equal to XXX days of type XX forbearance between: XX Initial Start and XXX day end 
			SUM(
					CASE 
						WHEN FORB.LD_FOR_BEG BETWEEN CASEXPOP.InitialBegin AND CASEXPOP.ExtensionEnd
						THEN FORB.DIFF
						ELSE X
					END
				)
			OVER(PARTITION BY CASEXPOP.BF_SSN, CASEXPOP.DISASTER)
		,X) 
	AS XFORBDAYS_USED
INTO
	#FINAL_FORB_POP
FROM 
	CASEXPOP
	LEFT JOIN FORB
		ON CASEXPOP.BF_SSN = FORB.BF_SSN
--where DF_SPE_ACC_ID = 'XXXXXXXXXX'
;
--SELECT * FROM #FINAL_FORB_POP
DELETE FROM #FINAL_FORB_POP WHERE XFORB_COUNT > X OR XFORBDAYS_USED >= XXX;

--display for testing
SELECT
	*
FROM
	#FINAL_FORB_POP 
ORDER BY 
	XFORB_COUNT DESC
;

--borrower counts per disaster
SELECT
	Disaster
	,COUNT(BF_SSN) AS BorrowerCount
FROM
	#FINAL_FORB_POP
GROUP BY
	Disaster
ORDER BY
	Disaster
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
--;

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
	#FINAL_FORB_POP FP
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
WHERE
	PHXX.DX_CNC_EML_ADR IS NOT NULL
	OR PDXX.ALT_EM IS NOT NULL
;

/************* OUTPUT PROCESSING:  SCRIPT FILE ***************/

--not manual review:  script processing
SELECT
	 DF_SPE_ACC_ID
	,CONVERT(VARCHAR(XX),BeginDate,XXX) AS BeginDate
	,CONVERT(VARCHAR(XX),EndDate  ,XXX)	AS EndDate
FROM
	(
		SELECT
			DF_SPE_ACC_ID
			,ForbStartX [BeginDate]
			,ForbEndX [EndDate]
		FROM
			#FINAL_FORB_POP

		UNION

		SELECT
			DF_SPE_ACC_ID
			,ForbStartX [BeginDate]
			,ForbEndX [EndDate]
		FROM
			#FINAL_FORB_POP

		UNION

		SELECT
			DF_SPE_ACC_ID
			,ForbStartX [BeginDate]
			,ForbEndX [EndDate]
		FROM
			#FINAL_FORB_POP
	)SCRIPTFILE
WHERE
	BeginDate IS NOT NULL
	AND EndDate IS NOT NULL
ORDER BY 
	 DF_SPE_ACC_ID
	,CAST(BeginDate AS DATE) ASC
	,EndDate
;