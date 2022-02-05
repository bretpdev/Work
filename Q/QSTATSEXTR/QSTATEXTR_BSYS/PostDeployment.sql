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

IF OBJECT_ID('dbo.[QSTA_DAT_AgentTime]', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.[QSTA_DAT_AgentTime]
END

IF OBJECT_ID('[dbo].[QSTA_DAT_JobTime]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[QSTA_DAT_JobTime]
END

IF OBJECT_ID('[dbo].[QSTA_DAT_PerformanceSummary]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[QSTA_DAT_PerformanceSummary]
END

IF OBJECT_ID('[dbo].[QSTA_DAT_UTDDL1]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[QSTA_DAT_UTDDL1]
END

IF OBJECT_ID('[dbo].[QSTA_DAT_UTDDL2]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[QSTA_DAT_UTDDL2]
END

IF OBJECT_ID('[dbo].[QSTA_DAT_UTDDL3]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[QSTA_DAT_UTDDL3]
END

IF OBJECT_ID('[dbo].[QSTA_DAT_UTDDL4]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[QSTA_DAT_UTDDL4]
END

IF OBJECT_ID('[dbo].[QSTA_DAT_UTDDL93]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[QSTA_DAT_UTDDL93]
END

IF OBJECT_ID('[dbo].[QSTA_LST_ActivityInfo4ResultCode_Camp]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].QSTA_LST_ActivityInfo4ResultCode_Camp
END

IF OBJECT_ID('[dbo].[QSTA_LST_ActivityInfo4ResultCode_CompassAutoDialer]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].QSTA_LST_ActivityInfo4ResultCode_CompassAutoDialer
END

IF OBJECT_ID('[dbo].[QSTA_LST_OneLINKQueueBuilder]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].QSTA_LST_OneLINKQueueBuilder
END

IF OBJECT_ID('[dbo].[QSTA_LST_RefreshQueue]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].QSTA_LST_RefreshQueue
END

IF OBJECT_ID('[dbo].[QSTA_REF_Manager_ADialerCamp1stChar]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].QSTA_REF_Manager_ADialerCamp1stChar
END

IF OBJECT_ID('[dbo].[QSTA_REF_ResCode_Descrip]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].QSTA_REF_ResCode_Descrip
END

IF (OBJECT_ID('dbo.FK_QSTA_DAT_UserData_SYSA_LST_UserIDInfo', 'F') IS NOT NULL)
BEGIN --remove foreign key tying table to SYSA_LST_UserIDInfo.
    ALTER TABLE dbo.QSTA_DAT_UserData DROP CONSTRAINT FK_QSTA_DAT_UserData_SYSA_LST_UserIDInfo
END

GRANT EXECUTE ON SCHEMA::qstatsextr TO [UHEAA\UheaaUsers]