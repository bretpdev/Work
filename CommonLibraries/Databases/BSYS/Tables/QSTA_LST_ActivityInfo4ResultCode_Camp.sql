CREATE TABLE [dbo].[QSTA_LST_ActivityInfo4ResultCode_Camp] (
    [ResultCode]      NVARCHAR (5)    NOT NULL,
    [Campaign]        NVARCHAR (10)   NOT NULL,
    [Type]            NVARCHAR (2)    NULL,
    [ToFro]           NVARCHAR (2)    NULL,
    [ActionCode]      NVARCHAR (5)    NULL,
    [ActivityComment] NVARCHAR (1000) NULL,
    [AddLP50]         NVARCHAR (10)   NULL,
    CONSTRAINT [PK_QS_ActivityInfo4ResultCode_Camp] PRIMARY KEY CLUSTERED ([ResultCode] ASC, [Campaign] ASC)
);

