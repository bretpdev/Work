CREATE TABLE [dbo].[DispositionCodeMapping]
(
	[DispositionCodeMappingId] [int] IDENTITY(1,1) NOT NULL,
	[DispositionCode] [varchar](2) NOT NULL,
	[ArcId] INT NOT NULL,
	[CommentId] INT NOT NULL,
	[ResponseCodeId] INT NOT NULL,
 CONSTRAINT [PK_SCFU_ResultCodes_Arc_Comment] PRIMARY KEY CLUSTERED 
(
	[DispositionCodeMappingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_SCFU_ResultCodes_Arc_Comment_ResultCode] UNIQUE NONCLUSTERED 
(
	[DispositionCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
    CONSTRAINT [FK_DispositionCodeMapping_ToArcs] FOREIGN KEY (ArcId) REFERENCES Arcs(ArcId), 
    CONSTRAINT [FK_DispositionCodeMapping_ToComments] FOREIGN KEY (CommentId) REFERENCES Comments(CommentId), 
    CONSTRAINT [FK_DispositionCodeMapping_ToResponseCodes] FOREIGN KEY (ResponseCodeId) REFERENCES ResponseCodes(ResponseCodeId)
) ON [PRIMARY]
