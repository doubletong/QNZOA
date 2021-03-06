USE [master]
GO
/****** Object:  Database [SIGBugsDB]    Script Date: 2018/9/17 0:09:23 ******/
CREATE DATABASE [SIGBugsDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SIGBugsDB', FILENAME = N'G:\Database\SIGBugsDB.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SIGBugsDB_log', FILENAME = N'G:\Database\SIGBugsDB_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [SIGBugsDB] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SIGBugsDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SIGBugsDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SIGBugsDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SIGBugsDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SIGBugsDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SIGBugsDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [SIGBugsDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SIGBugsDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SIGBugsDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SIGBugsDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SIGBugsDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SIGBugsDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SIGBugsDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SIGBugsDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SIGBugsDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SIGBugsDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SIGBugsDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SIGBugsDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SIGBugsDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SIGBugsDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SIGBugsDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SIGBugsDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SIGBugsDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SIGBugsDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SIGBugsDB] SET  MULTI_USER 
GO
ALTER DATABASE [SIGBugsDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SIGBugsDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SIGBugsDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SIGBugsDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [SIGBugsDB] SET DELAYED_DURABILITY = DISABLED 
GO
USE [SIGBugsDB]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 2018/9/17 0:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CustomerType] [tinyint] NOT NULL,
	[Description] [nvarchar](250) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](250) NULL,
	[Email] [nvarchar](150) NULL,
	[Phone] [nvarchar](50) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Menu]    Script Date: 2018/9/17 0:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Action] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
	[Area] [nvarchar](max) NULL,
	[CategoryId] [int] NOT NULL,
	[Controller] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Iconfont] [nvarchar](max) NULL,
	[Importance] [int] NOT NULL,
	[IsExpand] [bit] NOT NULL,
	[LayoutLevel] [int] NULL,
	[MenuType] [smallint] NOT NULL,
	[ParentId] [int] NULL,
	[Title] [nvarchar](50) NOT NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[Url] [nvarchar](max) NULL,
 CONSTRAINT [PK_MenuSet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MenuCategory]    Script Date: 2018/9/17 0:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Active] [bit] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Importance] [int] NOT NULL,
	[IsSys] [bit] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_MenuCategorySet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Paymentlog]    Script Date: 2018/9/17 0:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Paymentlog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Money] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](150) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[ProjectId] [int] NOT NULL,
 CONSTRAINT [PK_Paymentlog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Project]    Script Date: 2018/9/17 0:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CustomerId] [int] NULL,
	[Manager] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ProjectSet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProjectBusiness]    Script Date: 2018/9/17 0:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectBusiness](
	[ProjectId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL CONSTRAINT [DF_ProjectBusiness_Amount]  DEFAULT ((0)),
	[Contract] [nvarchar](250) NULL,
 CONSTRAINT [PK_ProjectBusiness] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 2018/9/17 0:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsSys] [bit] NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_RoleSet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RoleMenu]    Script Date: 2018/9/17 0:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleMenu](
	[RoleId] [int] NOT NULL,
	[MenuId] [int] NOT NULL,
 CONSTRAINT [PK_RoleMenuSet] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Task]    Script Date: 2018/9/17 0:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Body] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[Performer] [nvarchar](50) NULL,
	[Status] [smallint] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[TaskType] [smallint] NOT NULL,
 CONSTRAINT [PK_TaskSet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 2018/9/17 0:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL,
	[Birthday] [datetime2](7) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Gender] [smallint] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[LastActivityDate] [datetime2](7) NULL,
	[Mobile] [nvarchar](50) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[PhotoUrl] [nvarchar](150) NULL,
	[QQ] [nvarchar](50) NULL,
	[RealName] [nvarchar](50) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_UserSet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserProject]    Script Date: 2018/9/17 0:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProject](
	[ProjectId] [int] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserProject] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 2018/9/17 0:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_UserRoleSet] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([Id], [Name], [CustomerType], [Description], [CreatedDate], [CreatedBy], [Address], [Email], [Phone]) VALUES (1, N'逻辑思维', 2, NULL, CAST(N'2018-08-21 00:00:00.000' AS DateTime), N'doubletong', NULL, NULL, NULL)
INSERT [dbo].[Customer] ([Id], [Name], [CustomerType], [Description], [CreatedDate], [CreatedBy], [Address], [Email], [Phone]) VALUES (2, N'fdgdf', 2, N'gdfgdf', CAST(N'2018-09-15 17:28:00.690' AS DateTime), N'admin', N'gddffg', N'gdfgdf', N'gdfgdf')
INSERT [dbo].[Customer] ([Id], [Name], [CustomerType], [Description], [CreatedDate], [CreatedBy], [Address], [Email], [Phone]) VALUES (5, N'55465546骤', 2, NULL, CAST(N'2018-09-15 16:23:38.777' AS DateTime), N'admin', NULL, NULL, NULL)
INSERT [dbo].[Customer] ([Id], [Name], [CustomerType], [Description], [CreatedDate], [CreatedBy], [Address], [Email], [Phone]) VALUES (6, N'dsfsd', 2, N'dsfdsfsd', CAST(N'2018-09-16 01:22:07.750' AS DateTime), N'admin', NULL, N'twotong@gmail.com', N'15361828193')
INSERT [dbo].[Customer] ([Id], [Name], [CustomerType], [Description], [CreatedDate], [CreatedBy], [Address], [Email], [Phone]) VALUES (7, N'香蕉设计', 2, NULL, CAST(N'2018-09-16 01:24:57.760' AS DateTime), N'admin', N'深圳市龙岗区坂田街道雪象社区园东工业园A栋5楼(B门上)', N'info@bananadesign.cn', N'111')
INSERT [dbo].[Customer] ([Id], [Name], [CustomerType], [Description], [CreatedDate], [CreatedBy], [Address], [Email], [Phone]) VALUES (1002, N'荷勒', 2, N'dsfsdfsd', CAST(N'2018-09-15 18:23:22.897' AS DateTime), N'admin', N'dsfds', N'two@amdin.com', N'1234434')
INSERT [dbo].[Customer] ([Id], [Name], [CustomerType], [Description], [CreatedDate], [CreatedBy], [Address], [Email], [Phone]) VALUES (1003, N'夺一全', 2, NULL, CAST(N'2018-09-16 03:02:16.880' AS DateTime), N'admin', NULL, NULL, N'15361828193')
SET IDENTITY_INSERT [dbo].[Customer] OFF
SET IDENTITY_INSERT [dbo].[Menu] ON 

INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (3, N'Index', 1, N'Admin', 1, N'Home', N'admin', CAST(N'2017-11-05 00:00:00.000' AS DateTime), N'fa fa-dashboard', 0, 1, 0, 1, NULL, N'控制面版', N'', CAST(N'2018-01-05 02:53:50.9708770' AS DateTime2), N'/admin')
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (4, NULL, 1, NULL, 1, NULL, N'admin', CAST(N'2017-11-06 03:21:10.660' AS DateTime), N'fa fa-cog', 10, 1, 1, 3, 3, N'系统', N'admin', CAST(N'2017-11-15 17:13:22.7692780' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (5, NULL, 1, NULL, 1, NULL, N'admin', CAST(N'2017-11-06 03:32:14.283' AS DateTime), NULL, 3, 0, 1, 3, 3, N'FQA', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (6, NULL, 1, NULL, 1, NULL, N'admin', CAST(N'2017-11-06 03:50:44.857' AS DateTime), NULL, 8, 0, 1, 3, 3, N'下载', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (7, NULL, 1, NULL, 1, NULL, N'admin', CAST(N'2017-11-06 04:06:24.273' AS DateTime), NULL, 9, 0, 1, 3, 3, N'图片', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (8, NULL, 1, NULL, 1, NULL, N'admin', CAST(N'2017-11-06 04:10:20.837' AS DateTime), NULL, 1, 0, 1, 3, 3, N'订单001', N'', CAST(N'2017-11-19 16:35:28.8733780' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (9, NULL, 1, NULL, 1, NULL, N'admin', CAST(N'2017-11-06 04:15:34.983' AS DateTime), NULL, 2, 0, 1, 3, 3, N'视频', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (10, NULL, 1, NULL, 1, NULL, N'admin', CAST(N'2017-11-06 04:16:58.167' AS DateTime), N'fa fa-plug', 4, 0, 1, 3, 3, N'组件', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (11, NULL, 1, NULL, 1, NULL, N'admin', CAST(N'2017-11-06 05:11:42.453' AS DateTime), NULL, 5, 0, 1, 3, 3, N'产品', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (12, NULL, 1, NULL, 1, NULL, N'admin', CAST(N'2017-11-06 05:12:08.447' AS DateTime), N'fa fa-file-text', 6, 0, 1, 3, 3, N'文章', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (13, NULL, 1, NULL, 1, NULL, N'admin', CAST(N'2017-11-06 05:12:35.367' AS DateTime), N'fa fa-file', 7, 0, 1, 3, 3, N'页面', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (14, N'Index', 1, N'Admin', 1, N'Menu', N'admin', CAST(N'2017-11-06 05:37:16.487' AS DateTime), NULL, 23, 0, 2, 1, 4, N'后台菜单', N'admin', CAST(N'2017-11-15 16:09:34.4437191' AS DateTime2), N'/admin/menu/index')
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (15, N'Add', 0, N'Admin', 1, N'Menu', N'admin', CAST(N'2017-11-06 05:37:16.487' AS DateTime), NULL, 24, 0, 3, 2, 14, N'添加', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (16, N'Edit', 0, N'Admin', 1, N'Menu', N'admin', CAST(N'2017-11-06 05:37:16.487' AS DateTime), NULL, 25, 0, 3, 2, 14, N'编辑', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (17, N'IsActive', 0, N'Admin', 1, N'Menu', N'admin', CAST(N'2017-11-06 05:37:16.487' AS DateTime), NULL, 26, 0, 3, 2, 14, N'显示/隐藏', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (18, N'Delete', 0, N'Admin', 1, N'Menu', N'admin', CAST(N'2017-11-06 05:37:16.487' AS DateTime), NULL, 27, 0, 3, 2, 14, N'删除', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (19, N'PageSizeSet', 0, N'Admin', 1, N'Menu', N'admin', CAST(N'2017-11-06 05:37:16.487' AS DateTime), NULL, 28, 0, 3, 2, 14, N'分页设置', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (20, N'Index', 1, N'Admin', 1, N'Log', N'admin', CAST(N'2017-11-06 17:07:19.423' AS DateTime), NULL, 29, 0, 2, 1, 4, N'日志', N'admin', CAST(N'2017-11-15 16:09:35.6369714' AS DateTime2), N'/admin/log')
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (24, N'Delete', 0, N'Admin', 1, N'Log', N'admin', CAST(N'2017-11-06 17:07:19.423' AS DateTime), NULL, 30, 0, 3, 2, 20, N'删除', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (25, N'PageSizeSet', 0, N'Admin', 1, N'Log', N'admin', CAST(N'2017-11-06 17:07:19.423' AS DateTime), NULL, 31, 0, 3, 2, 20, N'分页设置', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (26, N'Index', 1, N'Admin', 1, N'Role', N'admin', CAST(N'2017-11-06 17:36:26.300' AS DateTime), NULL, 17, 1, 2, 1, 4, N'角色', N'admin', CAST(N'2017-11-15 17:13:30.6188669' AS DateTime2), N'/Admin/Role')
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (27, N'Add', 0, N'Admin', 1, N'Role', N'admin', CAST(N'2017-11-06 17:36:26.300' AS DateTime), NULL, 18, 0, 3, 2, 26, N'添加', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (28, N'Edit', 0, N'Admin', 1, N'Role', N'admin', CAST(N'2017-11-06 17:36:26.300' AS DateTime), NULL, 19, 0, 3, 2, 26, N'编辑', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (29, N'IsActive', 0, N'Admin', 1, N'Role', N'admin', CAST(N'2017-11-06 17:36:26.300' AS DateTime), NULL, 20, 0, 3, 2, 26, N'显示/隐藏', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (30, N'Delete', 0, N'Admin', 1, N'Role', N'admin', CAST(N'2017-11-06 17:36:26.300' AS DateTime), NULL, 21, 0, 3, 2, 26, N'删除', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (31, N'PageSizeSet', 0, N'Admin', 1, N'Role', N'admin', CAST(N'2017-11-06 17:36:26.300' AS DateTime), NULL, 22, 0, 3, 2, 26, N'分页设置', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (32, N'Index', 1, N'Admin', 1, N'User', N'admin', CAST(N'2017-11-06 17:40:21.377' AS DateTime), NULL, 11, 1, 2, 1, 4, N'用户', N'', CAST(N'2018-01-05 02:54:10.0791713' AS DateTime2), N'/Admin/User')
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (33, N'Add', 0, N'Admin', 1, N'User', N'admin', CAST(N'2017-11-06 17:40:21.377' AS DateTime), NULL, 12, 0, 3, 2, 32, N'添加', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (34, N'Edit', 0, N'Admin', 1, N'User', N'admin', CAST(N'2017-11-06 17:40:21.377' AS DateTime), NULL, 13, 0, 3, 2, 32, N'编辑', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (35, N'IsActive', 0, N'Admin', 1, N'User', N'admin', CAST(N'2017-11-06 17:40:21.377' AS DateTime), NULL, 14, 0, 3, 2, 32, N'显示/隐藏', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (36, N'Delete', 0, N'Admin', 1, N'User', N'admin', CAST(N'2017-11-06 17:40:21.377' AS DateTime), NULL, 15, 0, 3, 2, 32, N'删除', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
INSERT [dbo].[Menu] ([Id], [Action], [Active], [Area], [CategoryId], [Controller], [CreatedBy], [CreatedDate], [Iconfont], [Importance], [IsExpand], [LayoutLevel], [MenuType], [ParentId], [Title], [UpdatedBy], [UpdatedDate], [Url]) VALUES (37, N'PageSizeSet', 0, N'Admin', 1, N'User', N'admin', CAST(N'2017-11-06 17:40:21.377' AS DateTime), NULL, 16, 0, 3, 2, 32, N'分页设置', N'admin', CAST(N'2017-11-15 16:08:48.5799956' AS DateTime2), NULL)
SET IDENTITY_INSERT [dbo].[Menu] OFF
SET IDENTITY_INSERT [dbo].[MenuCategory] ON 

INSERT [dbo].[MenuCategory] ([Id], [Active], [CreatedBy], [CreatedDate], [Importance], [IsSys], [Title], [UpdatedBy], [UpdatedDate]) VALUES (1, 1, N'admin', CAST(N'2017-11-05 00:00:00.000' AS DateTime), 100, 1, N'后台菜单', NULL, NULL)
SET IDENTITY_INSERT [dbo].[MenuCategory] OFF
SET IDENTITY_INSERT [dbo].[Paymentlog] ON 

INSERT [dbo].[Paymentlog] ([Id], [Money], [Description], [CreatedDate], [CreatedBy], [ProjectId]) VALUES (1, CAST(-3400.00 AS Decimal(18, 2)), N'付给刘生', CAST(N'2018-08-08 00:00:00.000' AS DateTime), N'doubletong', 1)
SET IDENTITY_INSERT [dbo].[Paymentlog] OFF
SET IDENTITY_INSERT [dbo].[Project] ON 

INSERT [dbo].[Project] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [CustomerId], [Manager]) VALUES (1, N'SIG Bugs Manager', N'项目BUG管理', CAST(N'2018-05-21 00:00:00.000' AS DateTime), N'doubletong', NULL, NULL)
INSERT [dbo].[Project] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [CustomerId], [Manager]) VALUES (2, N'SIGCMS .net core 2', N'内容管理系统', CAST(N'2018-05-21 00:00:00.000' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Project] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [CustomerId], [Manager]) VALUES (3, N'SIGShop', N'商城系统开发', CAST(N'2018-05-22 16:36:37.953' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Project] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [CustomerId], [Manager]) VALUES (4, N'fdsfsd', N'五十二一', CAST(N'2018-05-24 14:29:59.877' AS DateTime), N'admin', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Project] OFF
INSERT [dbo].[ProjectBusiness] ([ProjectId], [Amount], [Contract]) VALUES (1, CAST(5000.00 AS Decimal(18, 2)), NULL)
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([Id], [Description], [IsSys], [RoleName]) VALUES (1, N'', 1, N'系统管理员')
INSERT [dbo].[Role] ([Id], [Description], [IsSys], [RoleName]) VALUES (2, N'', 1, N'用户')
INSERT [dbo].[Role] ([Id], [Description], [IsSys], [RoleName]) VALUES (3, NULL, 0, N'商家')
INSERT [dbo].[Role] ([Id], [Description], [IsSys], [RoleName]) VALUES (5, NULL, 0, N'考核员')
INSERT [dbo].[Role] ([Id], [Description], [IsSys], [RoleName]) VALUES (6, N'商家会员', 0, N'商家')
SET IDENTITY_INSERT [dbo].[Role] OFF
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 3)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 4)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 5)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 6)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 7)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 8)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 9)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 10)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 11)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 12)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 13)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 14)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 15)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 16)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 17)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 18)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 19)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 20)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 24)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 25)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 26)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 27)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 28)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 29)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 30)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 31)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 32)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 33)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 34)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 35)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 36)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (1, 37)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 3)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 4)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 5)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 6)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 7)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 8)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 9)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 10)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 11)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 12)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 13)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 14)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 15)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 16)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 17)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 18)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 19)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 20)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 24)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 25)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 26)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 27)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 28)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 29)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 30)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 31)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 32)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 33)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 34)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 35)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 36)
INSERT [dbo].[RoleMenu] ([RoleId], [MenuId]) VALUES (5, 37)
SET IDENTITY_INSERT [dbo].[Task] ON 

INSERT [dbo].[Task] ([Id], [Title], [Body], [CreatedDate], [CreatedBy], [Performer], [Status], [ProjectId], [TaskType]) VALUES (1, N'会员系统', N'test', CAST(N'2018-05-21 00:00:00.000' AS DateTime), N'doubletong', N'admin', 1, 1, 1)
SET IDENTITY_INSERT [dbo].[Task] OFF
INSERT [dbo].[User] ([Id], [Birthday], [CreateDate], [Email], [Gender], [IsActive], [LastActivityDate], [Mobile], [PasswordHash], [PhotoUrl], [QQ], [RealName], [SecurityStamp], [Username], [Balance]) VALUES (N'f7c80667-73a3-c857-94a6-08d5221b963e', NULL, CAST(N'2017-11-03 02:00:22.2946489' AS DateTime2), N'twotong@gmail.com', 1, 1, NULL, N'dfs', N'lNh3Mr/RO3LvoLMECqHLE1ZqIE/taIgMtMjrbpDVcq4=', NULL, NULL, N'童柱港', N'/gT4bi9WsTq2ufS9VGZ4sFqPLAv7q9duwKQqntLUOBE=', N'doubetong', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[User] ([Id], [Birthday], [CreateDate], [Email], [Gender], [IsActive], [LastActivityDate], [Mobile], [PasswordHash], [PhotoUrl], [QQ], [RealName], [SecurityStamp], [Username], [Balance]) VALUES (N'13d818fd-687c-c44c-8413-08d522565bcc', CAST(N'2010-02-20 00:00:00.0000000' AS DateTime2), CAST(N'2017-11-03 09:01:04.5463077' AS DateTime2), N'13212847@qq.com', 1, 1, NULL, N'15361828193', N'2lWznlGD9qfbd+nBZRafKs6niK3vtq34RL7p9GfEOVU=', NULL, N'13212847', N'童柱港', N'Egp0K+eDLCNACnyHRDq+0otOeJLdofrCeAIGPk/yOC4=', N'admin', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[User] ([Id], [Birthday], [CreateDate], [Email], [Gender], [IsActive], [LastActivityDate], [Mobile], [PasswordHash], [PhotoUrl], [QQ], [RealName], [SecurityStamp], [Username], [Balance]) VALUES (N'1c25fd90-6042-c3b4-b049-08d529b2f0a1', NULL, CAST(N'2017-11-12 17:51:26.0840774' AS DateTime2), N'11@qq.com', 1, 1, NULL, N'15361828193', N's+00PJwx6Q5cRr79sw6XSMucsfopc8/FBAXY+elgxo4=', NULL, NULL, N'twot', N'qqxo/B7H8F4jX2CJREtaffzgIYcuSVbT1ZDxt3sw8VU=', N'xiao', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[User] ([Id], [Birthday], [CreateDate], [Email], [Gender], [IsActive], [LastActivityDate], [Mobile], [PasswordHash], [PhotoUrl], [QQ], [RealName], [SecurityStamp], [Username], [Balance]) VALUES (N'469ffcd7-60ba-c401-cdbd-08d529b53c23', NULL, CAST(N'2017-11-12 18:07:51.7594668' AS DateTime2), N'ytjang@qq.com', 0, 1, NULL, NULL, N'lEbJUIUGv6CG3fnPeTd3XNpP9pinKT0mQBZk12/f0Ak=', NULL, NULL, N'dfs', N'bYbgYzMcswXr0u2gnfKpY2QdyEYQgsd/wTFPGi+o/x8=', N'ytjang', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (N'f7c80667-73a3-c857-94a6-08d5221b963e', 1)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (N'f7c80667-73a3-c857-94a6-08d5221b963e', 2)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (N'f7c80667-73a3-c857-94a6-08d5221b963e', 5)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (N'f7c80667-73a3-c857-94a6-08d5221b963e', 6)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (N'13d818fd-687c-c44c-8413-08d522565bcc', 1)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (N'13d818fd-687c-c44c-8413-08d522565bcc', 2)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (N'13d818fd-687c-c44c-8413-08d522565bcc', 5)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (N'13d818fd-687c-c44c-8413-08d522565bcc', 6)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (N'1c25fd90-6042-c3b4-b049-08d529b2f0a1', 2)
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (N'469ffcd7-60ba-c401-cdbd-08d529b53c23', 1)
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD  CONSTRAINT [FK_MenuSet_MenuCategorySet_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[MenuCategory] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Menu] CHECK CONSTRAINT [FK_MenuSet_MenuCategorySet_CategoryId]
GO
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD  CONSTRAINT [FK_MenuSet_MenuSet_ParentId] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Menu] ([Id])
GO
ALTER TABLE [dbo].[Menu] CHECK CONSTRAINT [FK_MenuSet_MenuSet_ParentId]
GO
ALTER TABLE [dbo].[Paymentlog]  WITH CHECK ADD  CONSTRAINT [FK_Paymentlog_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[Paymentlog] CHECK CONSTRAINT [FK_Paymentlog_Project]
GO
ALTER TABLE [dbo].[Paymentlog]  WITH CHECK ADD  CONSTRAINT [FK_Paymentlog_ProjectBusiness] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[ProjectBusiness] ([ProjectId])
GO
ALTER TABLE [dbo].[Paymentlog] CHECK CONSTRAINT [FK_Paymentlog_ProjectBusiness]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_User] FOREIGN KEY([Manager])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_User]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_ProjectSet_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_ProjectSet_Customer]
GO
ALTER TABLE [dbo].[ProjectBusiness]  WITH CHECK ADD  CONSTRAINT [FK_ProjectBusiness_ProjectSet] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[ProjectBusiness] CHECK CONSTRAINT [FK_ProjectBusiness_ProjectSet]
GO
ALTER TABLE [dbo].[RoleMenu]  WITH CHECK ADD  CONSTRAINT [FK_RoleMenuSet_MenuSet_MenuId] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menu] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleMenu] CHECK CONSTRAINT [FK_RoleMenuSet_MenuSet_MenuId]
GO
ALTER TABLE [dbo].[RoleMenu]  WITH CHECK ADD  CONSTRAINT [FK_RoleMenuSet_RoleSet_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleMenu] CHECK CONSTRAINT [FK_RoleMenuSet_RoleSet_RoleId]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_TaskSet_TaskSet] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_TaskSet_TaskSet]
GO
ALTER TABLE [dbo].[UserProject]  WITH CHECK ADD  CONSTRAINT [FK_UserProject_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserProject] CHECK CONSTRAINT [FK_UserProject_User]
GO
ALTER TABLE [dbo].[UserProject]  WITH CHECK ADD  CONSTRAINT [FK_UserProject_UserProject] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[UserProject] CHECK CONSTRAINT [FK_UserProject_UserProject]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleSet_RoleSet_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRoleSet_RoleSet_RoleId]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleSet_UserSet_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRoleSet_UserSet_UserId]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1：人个；2：公司' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CustomerType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目负责人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Project', @level2type=N'COLUMN',@level2name=N'Manager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'合同' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProjectBusiness', @level2type=N'COLUMN',@level2name=N'Contract'
GO
USE [master]
GO
ALTER DATABASE [SIGBugsDB] SET  READ_WRITE 
GO
