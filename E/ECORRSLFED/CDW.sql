﻿/*
Deployment script for CDW

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "CDW"
:setvar DefaultFilePrefix "CDW"
:setvar DefaultDataPath "D:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "D:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];

GO
PRINT N'Creating [ecorrslfed]...';


GO
CREATE SCHEMA [ecorrslfed]
    AUTHORIZATION [dbo];


GO
PRINT N'Creating [ecorrslfed].[InactivationStoredProcedures]...';


GO
CREATE TABLE [ecorrslfed].[InactivationStoredProcedures] (
    [InactivationStoredProcedureId] INT           IDENTITY (1, 1) NOT NULL,
    [StoredProcedureName]           VARCHAR (100) NULL,
    [AddedAt]                       DATETIME      NULL,
    [AddedBy]                       VARCHAR (50)  NULL,
    [DeletedAt]                     DATETIME      NULL,
    [DeletedBy]                     VARCHAR (50)  NULL
) ON [PRIMARY];


GO
PRINT N'Creating [ecorrslfed].[GetInactivationStoredProcedures]...';


GO
CREATE PROCEDURE [ecorrslfed].[GetInactivationStoredProcedures]
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT DISTINCT
	StoredProcedureName
FROM
	ecorrslfed.InactivationStoredProcedures
WHERE
	DeletedAt IS NULL
GO
