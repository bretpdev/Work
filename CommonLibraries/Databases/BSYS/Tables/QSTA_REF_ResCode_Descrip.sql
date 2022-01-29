CREATE TABLE [dbo].[QSTA_REF_ResCode_Descrip] (
    [ResultCode] NVARCHAR (5)   NOT NULL,
    [Descrip]    NVARCHAR (200) NULL,
    CONSTRAINT [PK_QS_XRefResCode_Descrip] PRIMARY KEY CLUSTERED ([ResultCode] ASC)
);

