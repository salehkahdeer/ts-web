/****** Object:  Table [dbo].[Logs]    Script Date: 08/06/2014 16:54:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Logs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CorrelationId] [uniqueidentifier] NULL,
	[Date] [datetime] NULL,
	[Thread] [varchar](255) NULL,
	[Level] [varchar](50) NULL,
	[Logger] [varchar](255) NULL,
	[Method] [varchar](255) NULL,
	[Line] [varchar](50) NULL,
	[Message] [varchar](4000) NULL,
	[Exception] [varchar](2000) NULL,
	[StackTrace] [varchar](4000) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


