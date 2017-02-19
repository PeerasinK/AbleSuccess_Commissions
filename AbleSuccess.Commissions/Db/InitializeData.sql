SET IDENTITY_INSERT [dbo].[Mst_CommissionRate] ON 
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf]) VALUES (1, 1, CAST(68.00 AS Decimal(5, 2)), 1)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf]) VALUES (2, 2, CAST(12.00 AS Decimal(5, 2)), 1)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf]) VALUES (3, 3, CAST(15.00 AS Decimal(5, 2)), 1)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf]) VALUES (4, 4, CAST(3.00 AS Decimal(5, 2)), 1)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf]) VALUES (5, 5, CAST(2.00 AS Decimal(5, 2)), 1)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf]) VALUES (6, 6, CAST(50.00 AS Decimal(5, 2)), 2)
SET IDENTITY_INSERT [dbo].[Mst_CommissionRate] OFF
GO

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
GO

SET IDENTITY_INSERT [dbo].[Mst_Profile] ON 
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo]) VALUES (1, 3, N'Chanawat', N'???', NULL, 2, 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo]) VALUES (2, 4, N'Suwan', N'???', NULL, 2, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo]) VALUES (3, 5, N'Niti', N'???', NULL, 3, 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo]) VALUES (4, 6, N'Taradon', N'???', NULL, 3, 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo]) VALUES (5, 7, N'Pornsiri', N'???', NULL, 3, 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo]) VALUES (6, 8, N'Arnon', N'???', NULL, 4, 1, N'Biw@Able.com', N'0812345678', N'Bangkok', N'..\Content\UserImages\Arnon.jpg')
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo]) VALUES (7, 9, N'Jintana', N'???', NULL, 5, 2, NULL, NULL, NULL, NULL)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo]) VALUES (8, 10, N'Suwat', N'???', NULL, 6, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Mst_Profile] ([ProfileId], [UserId], [FirstName], [LastName], [NickName], [Position], [Devision], [Email], [Telephone], [Address], [Photo]) VALUES (9, 11, N'Nipon', N'???', NULL, 1, 9, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Mst_Profile] OFF
GO
