/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DECLARE @Desc VARCHAR(50) = 'Parent'
DECLARE @CompassCode VARCHAR(2) = '02'
DECLARE @OnelinkCode VARCHAR(2) = 'PA'
IF NOT EXISTS(SELECT * FROM pwratrny.Relationships WHERE [Description] = @Desc)
    BEGIN
        INSERT INTO pwratrny.Relationships([Description], CompassCode, OnelinkCode)
        VALUES(@Desc, @CompassCode, @OnelinkCode)
    END

SET @Desc = 'Relative'
SET @CompassCode = '03'
SET @OnelinkCode = 'RE'
IF NOT EXISTS(SELECT * FROM pwratrny.Relationships WHERE [Description] = @Desc)
    BEGIN
        INSERT INTO pwratrny.Relationships([Description], CompassCode, OnelinkCode)
        VALUES(@Desc, @CompassCode, @OnelinkCode)
    END

SET @Desc = 'Non-Relative/Freind'
SET @CompassCode = '04'
SET @OnelinkCode = 'FR'
IF NOT EXISTS(SELECT * FROM pwratrny.Relationships WHERE [Description] = @Desc)
    BEGIN
        INSERT INTO pwratrny.Relationships([Description], CompassCode, OnelinkCode)
        VALUES(@Desc, @CompassCode, @OnelinkCode)
    END

SET @Desc = 'Employer'
SET @CompassCode = '05'
SET @OnelinkCode = 'EM'
IF NOT EXISTS(SELECT * FROM pwratrny.Relationships WHERE [Description] = @Desc)
    BEGIN
        INSERT INTO pwratrny.Relationships([Description], CompassCode, OnelinkCode)
        VALUES(@Desc, @CompassCode, @OnelinkCode)
    END

SET @Desc = 'Spouse'
SET @CompassCode = '06'
SET @OnelinkCode = 'SP'
IF NOT EXISTS(SELECT * FROM pwratrny.Relationships WHERE [Description] = @Desc)
    BEGIN
        INSERT INTO pwratrny.Relationships([Description], CompassCode, OnelinkCode)
        VALUES(@Desc, @CompassCode, @OnelinkCode)
    END

SET @Desc = 'Sibling'
SET @CompassCode = '07'
SET @OnelinkCode = 'SI'
IF NOT EXISTS(SELECT * FROM pwratrny.Relationships WHERE [Description] = @Desc)
    BEGIN
        INSERT INTO pwratrny.Relationships([Description], CompassCode, OnelinkCode)
        VALUES(@Desc, @CompassCode, @OnelinkCode)
    END

SET @Desc = 'Room Mate'
SET @CompassCode = '08'
SET @OnelinkCode = 'RM'
IF NOT EXISTS(SELECT * FROM pwratrny.Relationships WHERE [Description] = @Desc)
    BEGIN
        INSERT INTO pwratrny.Relationships([Description], CompassCode, OnelinkCode)
        VALUES(@Desc, @CompassCode, @OnelinkCode)
    END

SET @Desc = 'Neighbor'
SET @CompassCode = '09'
SET @OnelinkCode = 'NE'
IF NOT EXISTS(SELECT * FROM pwratrny.Relationships WHERE [Description] = @Desc)
    BEGIN
        INSERT INTO pwratrny.Relationships([Description], CompassCode, OnelinkCode)
        VALUES(@Desc, @CompassCode, @OnelinkCode)
    END

SET @Desc = 'Guardian'
SET @CompassCode = '12'
SET @OnelinkCode = 'GU'
IF NOT EXISTS(SELECT * FROM pwratrny.Relationships WHERE [Description] = @Desc)
    BEGIN
        INSERT INTO pwratrny.Relationships([Description], CompassCode, OnelinkCode)
        VALUES(@Desc, @CompassCode, @OnelinkCode)
    END

SET @Desc = 'Physician'
SET @CompassCode = '01'
SET @OnelinkCode = 'OT'
IF NOT EXISTS(SELECT * FROM pwratrny.Relationships WHERE [Description] = @Desc)
    BEGIN
        INSERT INTO pwratrny.Relationships([Description], CompassCode, OnelinkCode)
        VALUES(@Desc, @CompassCode, @OnelinkCode)
    END

SET @Desc = 'Attorney'
SET @CompassCode = '01'
SET @OnelinkCode = 'OT'
IF NOT EXISTS(SELECT * FROM pwratrny.Relationships WHERE [Description] = @Desc)
    BEGIN
        INSERT INTO pwratrny.Relationships([Description], CompassCode, OnelinkCode)
        VALUES(@Desc, @CompassCode, @OnelinkCode)
    END

SET @Desc = 'Other'
SET @CompassCode = '01'
SET @OnelinkCode = 'OT'
IF NOT EXISTS(SELECT * FROM pwratrny.Relationships WHERE [Description] = @Desc)
    BEGIN
        INSERT INTO pwratrny.Relationships([Description], CompassCode, OnelinkCode)
        VALUES(@Desc, @CompassCode, @OnelinkCode)
    END

GRANT EXECUTE ON SCHEMA::pwratrny TO db_executor