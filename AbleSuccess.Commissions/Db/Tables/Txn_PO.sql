CREATE TABLE [dbo].[Txn_PO](
	[PoId] [int] IDENTITY(1,1) NOT NULL,
	[PoNumber] [varchar](50) NOT NULL,
	[PoDate] [datetime] NOT NULL,
	[PoFilePath] [varchar](max) NULL,
	[InvoiceFilePath] [varchar](max) NULL,
	[CustomerName] [varchar](255) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]