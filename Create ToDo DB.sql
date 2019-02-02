-- Seed the database created by Entity Framework Code First
-- <add name="DefaultConnection" connectionString="server=.\SQLEnterprise;Database=ToDo;Integrated Security=true;MultipleActiveResultSets=true" providerName="System.Data.SqlClient" />
USE ToDo

INSERT [dbo].[ToDoItems] ([Item], [Completed]) VALUES (N'Complete first todo', 0)
INSERT [dbo].[ToDoItems] ([Item], [Completed]) VALUES (N'Get still more stuff done', 0)
INSERT [dbo].[ToDoItems] ([Item], [Completed]) VALUES (N'Finish course', 1)
INSERT [dbo].[ToDoItems] ([Item], [Completed]) VALUES (N'Profit', 0)
INSERT [dbo].[ToDoItems] ([Item], [Completed]) VALUES (N'create more stuff', 0)
INSERT [dbo].[ToDoItems] ([Item], [Completed]) VALUES (N'Check Usage', 0)
INSERT [dbo].[ToDoItems] ([Item], [Completed]) VALUES (N'Finish this piece', 0)


-- Manually create and seed the database.
USE [master]
GO
/****** Object:  Database [ToDo]    Script Date: 1/20/2019 10:32:37 PM ******/
CREATE DATABASE [ToDo]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ToDo', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLENTERPRISE\MSSQL\DATA\ToDo.mdf' , SIZE = 3136KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ToDo_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLENTERPRISE\MSSQL\DATA\ToDo.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ToDo] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ToDo].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ToDo] SET ANSI_NULL_DEFAULT ON 
GO
ALTER DATABASE [ToDo] SET ANSI_NULLS ON 
GO
ALTER DATABASE [ToDo] SET ANSI_PADDING ON 
GO
ALTER DATABASE [ToDo] SET ANSI_WARNINGS ON 
GO
ALTER DATABASE [ToDo] SET ARITHABORT ON 
GO
ALTER DATABASE [ToDo] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ToDo] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ToDo] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ToDo] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ToDo] SET CURSOR_DEFAULT  LOCAL 
GO
ALTER DATABASE [ToDo] SET CONCAT_NULL_YIELDS_NULL ON 
GO
ALTER DATABASE [ToDo] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ToDo] SET QUOTED_IDENTIFIER ON 
GO
ALTER DATABASE [ToDo] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ToDo] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ToDo] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ToDo] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ToDo] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ToDo] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ToDo] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ToDo] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ToDo] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ToDo] SET RECOVERY FULL 
GO
ALTER DATABASE [ToDo] SET  MULTI_USER 
GO
ALTER DATABASE [ToDo] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ToDo] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ToDo] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ToDo] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ToDo', N'ON'
GO
USE [ToDo]
GO

/****** Object:  Table [dbo].[ToDoItems]    Script Date: 1/20/2019 10:32:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToDoItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Item] [nvarchar](25) NULL,
	[Completed] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.ToDoItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[ToDoItems] ON 

GO
INSERT [dbo].[ToDoItems] ([Id], [Item], [Completed]) VALUES (1, N'Complete first todo', 0)
GO
INSERT [dbo].[ToDoItems] ([Id], [Item], [Completed]) VALUES (2, N'Get still more stuff done', 0)
GO
INSERT [dbo].[ToDoItems] ([Id], [Item], [Completed]) VALUES (3, N'Finish course', 1)
GO
INSERT [dbo].[ToDoItems] ([Id], [Item], [Completed]) VALUES (4, N'Profit', 0)
GO
INSERT [dbo].[ToDoItems] ([Id], [Item], [Completed]) VALUES (5, N'create more stuff', 0)
GO
INSERT [dbo].[ToDoItems] ([Id], [Item], [Completed]) VALUES (6, N'Check Usage', 0)
GO
INSERT [dbo].[ToDoItems] ([Id], [Item], [Completed]) VALUES (7, N'Finish this piece', 0)
GO
SET IDENTITY_INSERT [dbo].[ToDoItems] OFF
GO

USE [master]
GO
ALTER DATABASE [ToDo] SET  READ_WRITE 
GO
