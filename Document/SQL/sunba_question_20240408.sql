USE [sunba_question]
GO
/****** Object:  UserDefinedFunction [dbo].[clearTag]    Script Date: 2024/4/8 下午 05:53:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[clearTag]
(
 -- Add the parameters for the function here
 @HTMLText VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
BEGIN
 DECLARE @Start INT
 DECLARE @End INT
 DECLARE @Length INT
 SET @Start = CHARINDEX('<',@HTMLText) SET @End = 
 CHARINDEX('>',@HTMLText,CHARINDEX('<',@HTMLText)) 
 SET @Length = (@End - @Start) + 1 WHILE @Start > 0
 AND @End > 0
 AND @Length > 0
 BEGIN
 SET @HTMLText = STUFF(@HTMLText,@Start,@Length,'')
 SET @Start = CHARINDEX('<',@HTMLText) SET @End = CHARINDEX('>',@HTMLText,CHARINDEX('<',@HTMLText))
 SET @Length = (@End - @Start) + 1
 END
 RETURN LTRIM(RTRIM(     replace(replace(replace(replace(replace( replace( replace(  replace( replace(replace( replace(   replace(@HTMLText,'&nbsp;',' '),'</strong>',''),'<strong>','')  ,'</p>','') ,'<p>','')    ,'&amp;','&') ,'&gt;','>') ,'&lt;','<') ,'&rdquo;','"') ,'&ldquo;','"') ,'&rsquo;','''') ,'&hellip;','...')    ) )

END
GO
/****** Object:  Table [dbo].[代碼檔]    Script Date: 2024/4/8 下午 05:53:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[代碼檔](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[群組名稱] [nvarchar](50) NULL,
	[群組代碼] [nvarchar](5) NOT NULL,
	[項目名稱] [nvarchar](50) NULL,
	[項目代碼] [nvarchar](5) NOT NULL,
	[排序] [int] NULL,
	[是否顯示] [nvarchar](2) NULL,
 CONSTRAINT [PK_代碼檔] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[群組代碼] ASC,
	[項目代碼] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[附件檔]    Script Date: 2024/4/8 下午 05:53:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[附件檔](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[guid] [nvarchar](50) NULL,
	[業者guid] [nvarchar](50) NULL,
	[年度] [nvarchar](4) NULL,
	[檔案類型] [nvarchar](20) NULL,
	[原檔名] [nvarchar](500) NULL,
	[新檔名] [nvarchar](100) NULL,
	[附檔名] [nvarchar](7) NULL,
	[排序] [nvarchar](10) NULL,
	[檔案大小] [nvarchar](10) NULL,
	[資料狀態] [nvarchar](2) NULL,
	[建立日期] [datetime] NULL,
	[修改日期] [datetime] NULL,
 CONSTRAINT [PK_附件檔] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[提問表單]    Script Date: 2024/4/8 下午 05:53:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[提問表單](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[guid] [nvarchar](50) NULL,
	[項次] [nvarchar](10) NULL,
	[編號] [nvarchar](20) NULL,
	[舊編號] [nvarchar](10) NULL,
	[年度] [nvarchar](10) NULL,
	[月份] [nvarchar](10) NULL,
	[序號] [nvarchar](20) NULL,
	[填表人] [nvarchar](50) NULL,
	[公司別] [nvarchar](3) NULL,
	[單位] [nvarchar](3) NULL,
	[提出日期] [nvarchar](10) NULL,
	[程度] [nvarchar](3) NULL,
	[目前狀態] [nvarchar](3) NULL,
	[內容] [nvarchar](max) NULL,
	[回覆日期] [nvarchar](10) NULL,
	[預計完成日] [nvarchar](10) NULL,
	[資料狀態] [nvarchar](2) NULL,
	[回覆內容] [nvarchar](max) NULL,
	[建立日期] [datetime] NULL,
	[修改日期] [datetime] NULL,
 CONSTRAINT [PK_提問表單_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[提問表單儲存log]    Script Date: 2024/4/8 下午 05:53:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[提問表單儲存log](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[類別] [nvarchar](10) NULL,
	[儲存類別] [nvarchar](10) NULL,
	[填表人] [nvarchar](50) NULL,
	[儲存內容] [nvarchar](max) NULL,
	[建立日期] [datetime] NULL,
 CONSTRAINT [PK_提問表單儲存log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[附件檔] ADD  CONSTRAINT [DF_附件檔_guid]  DEFAULT (newid()) FOR [guid]
GO
ALTER TABLE [dbo].[附件檔] ADD  CONSTRAINT [DF_附件檔_建立日期]  DEFAULT (getdate()) FOR [建立日期]
GO
ALTER TABLE [dbo].[附件檔] ADD  CONSTRAINT [DF_附件檔_修改日期]  DEFAULT (getdate()) FOR [修改日期]
GO
ALTER TABLE [dbo].[提問表單] ADD  CONSTRAINT [DF_提問表單_guid]  DEFAULT (newid()) FOR [guid]
GO
ALTER TABLE [dbo].[提問表單] ADD  CONSTRAINT [DF_提問表單_資料狀態]  DEFAULT ('A') FOR [資料狀態]
GO
ALTER TABLE [dbo].[提問表單] ADD  CONSTRAINT [DF_提問表單_建立日期]  DEFAULT (getdate()) FOR [建立日期]
GO
ALTER TABLE [dbo].[提問表單] ADD  CONSTRAINT [DF_提問表單_修改日期]  DEFAULT (getdate()) FOR [修改日期]
GO
ALTER TABLE [dbo].[提問表單儲存log] ADD  CONSTRAINT [DF_提問表單儲存log_建立日期]  DEFAULT (getdate()) FOR [建立日期]
GO
