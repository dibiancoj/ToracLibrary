﻿USE [master]
GO

/****** Object:  Database [ToracLibraryTest]    Script Date: 8/3/2015 9:16:34 AM ******/
CREATE DATABASE [ToracLibraryTest]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ToracLibraryTest', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\ToracLibraryTest.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ToracLibraryTest_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\ToracLibraryTest_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [ToracLibraryTest] SET COMPATIBILITY_LEVEL = 110
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ToracLibraryTest].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [ToracLibraryTest] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET ARITHABORT OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [ToracLibraryTest] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [ToracLibraryTest] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET  DISABLE_BROKER 
GO

ALTER DATABASE [ToracLibraryTest] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [ToracLibraryTest] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET RECOVERY FULL 
GO

ALTER DATABASE [ToracLibraryTest] SET  MULTI_USER 
GO

ALTER DATABASE [ToracLibraryTest] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [ToracLibraryTest] SET DB_CHAINING OFF 
GO

ALTER DATABASE [ToracLibraryTest] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [ToracLibraryTest] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [ToracLibraryTest] SET  READ_WRITE 
GO




USE [ToracLibraryTest]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Ref_SubObject](
	[SubObjectId] [int] NOT NULL,
	[SubObjectText] [varchar](50) NULL,
 CONSTRAINT [PK_Ref_SubObject] PRIMARY KEY CLUSTERED 
(
	[SubObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Ref_Test](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[Description2] [varchar](50) NULL,
	[CreateDate] [datetime] NULL CONSTRAINT [DF_Ref_Test_CreateDate]  DEFAULT (getdate()),
	[BooleanTest] [bit] NULL CONSTRAINT [DF_Ref_Test_BooleanTest]  DEFAULT ((1)),
	[NullId] [int] NULL,
	[SubObjectId] [int] NULL,
 CONSTRAINT [PK_Ref_Test] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Ref_Test]  WITH CHECK ADD  CONSTRAINT [FK_Ref_Test_Ref_SubObject] FOREIGN KEY([SubObjectId])
REFERENCES [dbo].[Ref_SubObject] ([SubObjectId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Ref_Test] CHECK CONSTRAINT [FK_Ref_Test_Ref_SubObject]
GO

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


--enable the broker for sql cache dependency
ALTER DATABASE ToracLibraryTest SET NEW_BROKER WITH ROLLBACK IMMEDIATE;
ALTER DATABASE ToracLibraryTest SET ENABLE_BROKER;