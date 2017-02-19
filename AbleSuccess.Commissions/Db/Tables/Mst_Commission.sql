CREATE TABLE [dbo].[Txn_Commission](
	[CommissionId] [int] IDENTITY(1,1) NOT NULL,
	[PdDetailId] [int] NOT NULL,
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
