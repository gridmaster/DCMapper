USE [DCMap]
GO

/****** Object:  Table [dbo].[DCFile]    Script Date: 06/27/2015 16:21:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MapFiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Attributes] [nvarchar](20) NULL,
	[CreationTime] [datetime] NULL,
	[CreationTimeUtc] [datetime] NULL,
	[Exists] [bit] NULL,
	[Extension] [nvarchar](20) NULL,
	[FullName] [nvarchar](1024) NULL,
	[LastAccessTime] [datetime] NULL,
	[LastAccessTimeUtc] [datetime] NULL,
	[LastWriteTime] [datetime] NULL,
	[LastWriteTimeUtc] [datetime] NULL,
	[Name] [nvarchar](256) NULL,
	[DirectoryName] [nvarchar](256) NULL,
	[IsReadOnly] [bit] NULL,
	[Length] [decimal] NULL,
	[Timestamp] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


