﻿--i started to use code migrations. This should be the same version of migrations. Use whatever you would like.


USE [ToracLibraryTest]
GO
/****** Object:  Table [dbo].[Animal]    Script Date: 10/24/2015 5:14:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Animal](
	[AnimalId] [int] IDENTITY(1,1) NOT NULL,
	[Size] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Animal] PRIMARY KEY CLUSTERED 
(
	[AnimalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Cat]    Script Date: 10/24/2015 5:14:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cat](
	[AnimalId] [int] NOT NULL,
	[Meow] [int] NOT NULL,
 CONSTRAINT [PK_Cat] PRIMARY KEY CLUSTERED 
(
	[AnimalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dog]    Script Date: 10/24/2015 5:14:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dog](
	[AnimalId] [int] NOT NULL,
	[Bark] [int] NOT NULL,
 CONSTRAINT [PK_Dog] PRIMARY KEY CLUSTERED 
(
	[AnimalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ref_SqlCachTrigger]    Script Date: 10/24/2015 5:14:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ref_SqlCachTrigger](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Ref_SqlCachTrigger] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ref_Test]    Script Date: 10/24/2015 5:14:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ref_Test](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Ref_Test] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

GO
/****** Object:  Table [dbo].[T4TemplateTest]    Script Date: 12/21/2015 8:51:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T4TemplateTest](
	[Id] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
 CONSTRAINT [PK_T4TemplateTest] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[T4TemplateTest] ([Id], [Description]) VALUES (1, N'Item1')
GO
INSERT [dbo].[T4TemplateTest] ([Id], [Description]) VALUES (2, N'Item2')
GO
INSERT [dbo].[T4TemplateTest] ([Id], [Description]) VALUES (3, N'Item3')
GO
