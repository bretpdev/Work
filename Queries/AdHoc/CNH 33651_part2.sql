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
	SELECT
		PDXX.DF_SPE_ACC_ID
		,LNXX.BF_SSN
		,LNXX.LN_SEQ
		,RTRIM(PDXX.DM_PRS_X) AS FirstName
		,RTRIM(PDXX.DM_PRS_LST) AS LastName
		,CAST(LNXX.LD_DLQ_OCC AS DATE) LD_DLQ_OCC
		,(LNXX.LN_DLQ_MAX + X) AS [LN_DLQ_MAX+X]
		,ZIPS.ZIPS
		,ZIPS.Disaster
		,ZIPS.InitialBegin
		,ZIPS.InitialEnd
		,ZIPS.max_end
		,DATEDIFF(DAY,ZIPS.InitialBegin,ZIPS.InitialEnd)+X AS InitialDays -- +X on datediff to count actual begin date
		,DATEADD(DAY,XXX,ZIPS.InitialBegin) AS ExtensionEnd -- -X on dateadd to count actual begin date
		,DATEDIFF(DAY,ZIPS.InitialBegin,DATEADD(DAY,XXX,ZIPS.InitialBegin))+X AS ExtensionDays -- +X on datediff to count actual begin date
	FROM
		LNXX_LON LNXX
		INNER JOIN PDXX_PRS_NME PDXX 
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
		INNER JOIN 
		(
			SELECT
				BF_SSN,
				MIN(LD_DLQ_OCC) AS LD_DLQ_OCC,
				MIN(LN_DLQ_MAX) AS LN_DLQ_MAX
			FROM
				LNXX_LON_DLQ_HST LNXX
			WHERE 
				LNXX.LC_STA_LONXX = 'X'
			GROUP BY	
				BF_SSN		
		) LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
		INNER JOIN PDXX_PRS_ADR PDXX
			ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		INNER JOIN ZIPS --from ZIPS CTE above
			ON SUBSTRING(PDXX.DF_ZIP_CDE, X, X) = ZIPS.ZIPS
		INNER JOIN
		(
			SELECT DISTINCT
				WN_CTL_TSK
			FROM
				[CDW].[dbo].[WQXX_TSK_QUE] WQXX
			WHERE
				WQXX.WF_QUE = 'XX'
				AND WQXX.WX_MSG_X_TSK LIKE '%MUST ENTER%'
				AND WQXX.WC_STA_WQUEXX = 'U'
		) WQXXA
			ON LNXX.BF_SSN = WQXXA.WN_CTL_TSK
	WHERE
		LNXX.LN_DLQ_MAX > X
		AND PDXX.DI_VLD_ADR = 'Y'
		AND 
		(--from ZIPS CTE above
			   (ZIPS.Disaster = 'Maria' AND LNXX.LD_DLQ_OCC BETWEEN @Maria_BEGIN AND @Maria_MAX_END)
			OR (ZIPS.Disaster = 'Irma' AND LNXX.LD_DLQ_OCC BETWEEN @Irma_BEGIN AND @Irmaa_MAX_END)
			OR (ZIPS.Disaster = 'CA Fire' AND LNXX.LD_DLQ_OCC BETWEEN @CAFire_BEGIN AND @CAFire_MAX_END)
			--OR (ZIPS.Disaster = 'CA Mudslide' AND LNXX.LD_DLQ_OCC BETWEEN @CAMudslide_BEGIN AND @CAMudslide_MAX_END)
			OR (ZIPS.Disaster = 'Harvey' AND LNXX.LD_DLQ_OCC BETWEEN @Harvey_BEGIN AND @Harvey_MAX_END)
		)
		AND LNXX.LA_CUR_PRI > X.XX
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
INTO
	#FORB_POP
FROM
	EXTENSION_X
;

SELECT distinct
	POP.*, 
	ISNULL(
			SUM(
				CASE 
					WHEN POP.InitialBegin between FORB.LD_FOR_BEG AND FORB.LD_FOR_END THEN X
					WHEN DATEADD(DAY, XXX, POP.InitialBegin) between FORB.LD_FOR_BEG AND FORB.LD_FOR_END THEN X
					ELSE X
				END
				)
			OVER(PARTITION BY POP.BF_SSN, POP.DISASTER)
		,X)
	AS FORB_COUNT
INTO
	#FINAL_FORB_POP
FROM 
	#FORB_POP POP
	LEFT JOIN 
	(
		SELECT DISTINCT
			FBXX.BF_SSN,
			LNXX.LD_FOR_BEG,
			LNXX.LD_FOR_END,
			DATEDIFF(DAY, LNXX.LD_FOR_BEG,LNXX.LD_FOR_END) AS DIFF
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
	) FORB
		ON POP.BF_SSN = FORB.BF_SSN
;

DELETE FROM #FINAL_FORB_POP WHERE FORB_COUNT > X;

--display for testing
SELECT
	*
FROM
	#FINAL_FORB_POP 
ORDER BY 
	FORB_COUNT DESC
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