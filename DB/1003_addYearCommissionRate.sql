DELETE FROM [AbleSuccess_Commission].[dbo].[Mst_CommissionRate]

ALTER TABLE [AbleSuccess_Commission].[dbo].[Mst_CommissionRate]
ADD [Year] int NOT NULL default YEAR(GETDATE());
GO

SET IDENTITY_INSERT [dbo].[Mst_CommissionRate] ON 
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (1, 1, CAST(68.00 AS Decimal(5, 2)), 1,2015)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (2, 2, CAST(12.00 AS Decimal(5, 2)), 1,2015)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (3, 3, CAST(15.00 AS Decimal(5, 2)), 1,2015)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (4, 4, CAST(3.00 AS Decimal(5, 2)), 1,2015)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (5, 5, CAST(2.00 AS Decimal(5, 2)), 1,2015)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (6, 6, CAST(50.00 AS Decimal(5, 2)), 2,2015)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (7, 1, CAST(68.00 AS Decimal(5, 2)), 1,2016)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (8, 2, CAST(12.00 AS Decimal(5, 2)), 1,2016)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (9, 3, CAST(15.00 AS Decimal(5, 2)), 1,2016)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (10, 4, CAST(3.00 AS Decimal(5, 2)), 1,2016)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (11, 5, CAST(2.00 AS Decimal(5, 2)), 1,2016)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (12, 6, CAST(50.00 AS Decimal(5, 2)), 2,2016)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (13, 1, CAST(68.00 AS Decimal(5, 2)), 1,2017)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (14, 2, CAST(12.00 AS Decimal(5, 2)), 1,2017)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (15, 3, CAST(15.00 AS Decimal(5, 2)), 1,2017)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (16, 4, CAST(3.00 AS Decimal(5, 2)), 1,2017)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (17, 5, CAST(2.00 AS Decimal(5, 2)), 1,2017)
INSERT [dbo].[Mst_CommissionRate] ([RateId], [PositionId], [Percentage], [PercentageOf], [Year]) VALUES (18, 6, CAST(50.00 AS Decimal(5, 2)), 2, 2017)
SET IDENTITY_INSERT [dbo].[Mst_CommissionRate] OFF