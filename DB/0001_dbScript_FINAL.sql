USE [AbleSuccess_Commission]
GO
/****** Object:  Table [dbo].[Mst_CommissionRate]    Script Date: 2/25/2016 12:20:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mst_CommissionRate](
	[RateId] [int] IDENTITY(1,1) NOT NULL,
	[PositionId] [int] NOT NULL,
	[Percentage] [decimal](5, 2) NOT NULL,
	[PercentageOf] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Mst_Credential]    Script Date: 2/25/2016 12:20:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Mst_Credential](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserCode] [varchar](20) NULL,
	[Username] [varchar](20) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[Role] [int] NOT NULL,
	[Status] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Mst_Profile]    Script Date: 2/25/2016 12:20:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Mst_Profile](
	[ProfileId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[NickName] [varchar](10) NULL,
	[Position] [int] NOT NULL,
	[Devision] [int] NOT NULL,
	[Email] [varchar](50) NULL,
	[Telephone] [varchar](50) NULL,
	[Address] [varchar](max) NULL,
	[Photo] [varchar](max) NULL,
	[Status] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Txn_Commission]    Script Date: 2/25/2016 12:20:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Txn_Commission](
	[CommissionId] [int] IDENTITY(1,1) NOT NULL,
	[PoDetailId] [int] NOT NULL,
	[TotalCommission] [decimal](18, 2) NOT NULL,
	[CommissionSales] [decimal](18, 2) NULL,
	[CommissionPM] [decimal](18, 2) NULL,
	[CommissionApp] [decimal](18, 2) NULL,
	[CommissionInstall] [decimal](18, 2) NULL,
	[CommissionAdmin] [decimal](18, 2) NULL,
	[CommissionOutside] [decimal](18, 2) NULL,
	[ConclusionCommission] [decimal](18, 2) NULL,
	[AuditCommission] [decimal](18, 2) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Txn_CommissionDetail]    Script Date: 2/25/2016 12:20:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Txn_CommissionDetail](
	[CommissionDetailId] [int] IDENTITY(1,1) NOT NULL,
	[PoDetailId] [int] NOT NULL,
	[ProfileId] [int] NOT NULL,
	[Position] [int] NOT NULL,
	[Rate] [decimal](18, 2) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Txn_PO]    Script Date: 2/25/2016 12:20:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Txn_PO](
	[PoId] [int] IDENTITY(1,1) NOT NULL,
	[PoNumber] [varchar](50) NOT NULL,
	[PoDate] [datetime] NOT NULL,
	[PoFilePath] [varchar](max) NULL,
	[InvoiceFilePath] [varchar](max) NULL,
	[CustomerName] [varchar](255) NOT NULL,
	[Status] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Txn_PODetail]    Script Date: 2/25/2016 12:20:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Txn_PODetail](
	[PoDetailId] [int] IDENTITY(1,1) NOT NULL,
	[PoId] [int] NOT NULL,
	[ProductName] [varchar](255) NOT NULL,
	[ProductBrand] [int] NULL,
	[ProductType] [int] NULL,
	[PricePerUnit] [decimal](18, 2) NOT NULL,
	[ActualPricePerUnit] [decimal](18, 2) NOT NULL,
	[Amount] [int] NOT NULL,
	[TotalPrice] [decimal](18, 2) NOT NULL,
	[ActualTotalPrice] [decimal](18, 2) NOT NULL,
	[Tax] [decimal](18, 2) NOT NULL,
	[TransportFee] [decimal](18, 2) NOT NULL,
	[TransportLocation] [varchar](20) NULL,
	[ParcelFee] [decimal](18, 2) NOT NULL,
	[ServiceFee] [decimal](18, 2) NOT NULL,
	[OtherFee] [decimal](18, 2) NOT NULL,
	[CustomerLead] [decimal](18, 2) NOT NULL,
	[TotalCost] [decimal](18, 2) NOT NULL,
	[Profit] [decimal](18, 2) NOT NULL,
	[ProfitPercentage] [decimal](5, 2) NOT NULL,
	[Remark] [varchar](max) NULL,
	[Status] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Mst_CommissionRate] ON 

INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf]) VALUES (1, 1, CAST(68.00 AS Decimal(5, 2)), 1)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf]) VALUES (2, 2, CAST(12.00 AS Decimal(5, 2)), 1)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf]) VALUES (3, 3, CAST(15.00 AS Decimal(5, 2)), 1)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf]) VALUES (4, 4, CAST(3.00 AS Decimal(5, 2)), 1)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf]) VALUES (5, 5, CAST(2.00 AS Decimal(5, 2)), 1)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf]) VALUES (6, 6, CAST(50.00 AS Decimal(5, 2)), 2)
SET IDENTITY_INSERT [dbo].[Mst_CommissionRate] OFF
SET IDENTITY_INSERT [dbo].[Mst_Credential] ON 

INSERT [dbo].[Mst_Credential] ([UserId], [UserCode], [Username], [Password], [Role], [Status]) VALUES (1, NULL, N'system', N'7110EDA4D09E062AA5E4A390B0A572AC0D2C0220', 0, 1)
INSERT [dbo].[Mst_Credential] ([UserId], [UserCode], [Username], [Password], [Role], [Status]) VALUES (2, NULL, N'able', N'7110EDA4D09E062AA5E4A390B0A572AC0D2C0220', 1, 1)
INSERT [dbo].[Mst_Credential] ([UserId], [UserCode], [Username], [Password], [Role], [Status]) VALUES (3, NULL, N'chanawat', N'7110EDA4D09E062AA5E4A390B0A572AC0D2C0220', 2, 1)
INSERT [dbo].[Mst_Credential] ([UserId], [UserCode], [Username], [Password], [Role], [Status]) VALUES (4, NULL, N'suwan', N'7110EDA4D09E062AA5E4A390B0A572AC0D2C0220', 2, 1)
INSERT [dbo].[Mst_Credential] ([UserId], [UserCode], [Username], [Password], [Role], [Status]) VALUES (5, NULL, N'niti', N'7110EDA4D09E062AA5E4A390B0A572AC0D2C0220', 2, 1)
INSERT [dbo].[Mst_Credential] ([UserId], [UserCode], [Username], [Password], [Role], [Status]) VALUES (6, NULL, N'taradon', N'7110EDA4D09E062AA5E4A390B0A572AC0D2C0220', 2, 1)
INSERT [dbo].[Mst_Credential] ([UserId], [UserCode], [Username], [Password], [Role], [Status]) VALUES (7, NULL, N'pornsiri', N'7110EDA4D09E062AA5E4A390B0A572AC0D2C0220', 2, 1)
INSERT [dbo].[Mst_Credential] ([UserId], [UserCode], [Username], [Password], [Role], [Status]) VALUES (8, NULL, N'arnon', N'7110EDA4D09E062AA5E4A390B0A572AC0D2C0220', 2, 1)
INSERT [dbo].[Mst_Credential] ([UserId], [UserCode], [Username], [Password], [Role], [Status]) VALUES (9, NULL, N'jintana', N'7110EDA4D09E062AA5E4A390B0A572AC0D2C0220', 2, 1)
INSERT [dbo].[Mst_Credential] ([UserId], [UserCode], [Username], [Password], [Role], [Status]) VALUES (10, NULL, N'suwat', N'7110EDA4D09E062AA5E4A390B0A572AC0D2C0220', 2, 1)
INSERT [dbo].[Mst_Credential] ([UserId], [UserCode], [Username], [Password], [Role], [Status]) VALUES (11, NULL, N'nipon', N'7110EDA4D09E062AA5E4A390B0A572AC0D2C0220', 2, 1)
SET IDENTITY_INSERT [dbo].[Mst_Credential] OFF
SET IDENTITY_INSERT [dbo].[Mst_Profile] ON 

INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo], [Status]) VALUES (1, 3, N'Chanawat', N'LastName', NULL, 2, 1, NULL, NULL, NULL, N'..\Contents\UserImages\Chanawat.jpg', 1)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo], [Status]) VALUES (2, 4, N'Suwan', N'LastName', NULL, 2, 0, NULL, NULL, NULL, N'..\Contents\UserImages\Suwan.jpg', 1)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo], [Status]) VALUES (3, 5, N'Niti', N'LastName', NULL, 3, 1, NULL, NULL, NULL, N'..\Contents\UserImages\Niti.jpg', 1)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo], [Status]) VALUES (4, 6, N'Taradon', N'LastName', NULL, 3, 1, NULL, NULL, NULL, N'..\Contents\UserImages\Taradon.jpg', 1)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo], [Status]) VALUES (5, 7, N'Pornsiri', N'LastName', NULL, 3, 1, NULL, NULL, NULL, N'..\Contents\UserImages\Pornsiri.jpg', 1)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo], [Status]) VALUES (6, 8, N'Arnon', N'LastName', 'Biw', 4, 1, N'Biw@Able.com', N'0812345678', N'Bangkok', N'..\Contents\UserImages\Arnon.jpg', 1)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo], [Status]) VALUES (7, 9, N'Jintana', N'LastName', NULL, 5, 2, NULL, NULL, NULL, N'..\Contents\UserImages\Jintana.jpg', 1)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo], [Status]) VALUES (8, 10, N'Suwat', N'LastName', NULL, 6, 0, NULL, NULL, NULL, N'..\Contents\UserImages\Suwat.jpg', 1)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo], [Status]) VALUES (9, 11, N'Nipon', N'LastName', NULL, 1, 9, NULL, NULL, NULL, N'..\Contents\UserImages\Nipon.jpg', 1)
SET IDENTITY_INSERT [dbo].[Mst_Profile] OFF
USE [master]
GO
ALTER DATABASE [AbleSuccess_Commission] SET  READ_WRITE 
GO
