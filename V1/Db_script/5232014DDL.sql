
/****** Object:  Table [[LookUpMaster]    Script Date: 5/23/2014 2:31:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [LookUpMaster](
	[LookupId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[DataValue] [nvarchar](50) NULL,
	[Description] [nvarchar](200) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[Status] [int] NOT NULL,
	[ParentId] [bigint] NULL,
 CONSTRAINT [PK_LookUpMaster] PRIMARY KEY CLUSTERED 
(
	[LookupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [[SectionAttributes]    Script Date: 5/23/2014 2:31:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [SectionAttributes](
	[SectionAttributeID] [bigint] IDENTITY(1,1) NOT NULL,
	[SectionId] [bigint] NOT NULL,
	[AttributeId] [bigint] NOT NULL,
 CONSTRAINT [PK_SectionAttributes] PRIMARY KEY CLUSTERED 
(
	[SectionAttributeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [[LookUpMaster] ON 

INSERT [[LookUpMaster] ([LookupId], [Name], [DataValue], [Description], [CreatedDate], [Status], [ParentId]) VALUES (3, N'SectionType', N'SectionType', N'Represents', CAST(0x07F057FE8D8086380B AS DateTime2), 1, NULL)
INSERT [[LookUpMaster] ([LookupId], [Name], [DataValue], [Description], [CreatedDate], [Status], [ParentId]) VALUES (4, N'DocumentType', N'DocumentType', N'DocumentType', CAST(0x07208DC5BB8086380B AS DateTime2), 1, NULL)
INSERT [[LookUpMaster] ([LookupId], [Name], [DataValue], [Description], [CreatedDate], [Status], [ParentId]) VALUES (5, N'IsotopeSigeType', N'IsotopeSigeType', N'IsotopeSigeType', CAST(0x07709510CD8086380B AS DateTime2), 1, NULL)
INSERT [[LookUpMaster] ([LookupId], [Name], [DataValue], [Description], [CreatedDate], [Status], [ParentId]) VALUES (6, N'IsotopDataCatagoryType', N'IsotopDataCatagoryType', N'IsotopDataCatagoryType', CAST(0x0750C741E48086380B AS DateTime2), 1, NULL)
INSERT [[LookUpMaster] ([LookupId], [Name], [DataValue], [Description], [CreatedDate], [Status], [ParentId]) VALUES (7, N'HomePageCaraosel', N'HomePageCaraosel', N'HomePageCaraosel', CAST(0x07006EAE3E8186380B AS DateTime2), 1, 3)
INSERT [[LookUpMaster] ([LookupId], [Name], [DataValue], [Description], [CreatedDate], [Status], [ParentId]) VALUES (8, N'Project', N'Project', N'Project', CAST(0x07D0FEDFF18186380B AS DateTime2), 1, 3)
INSERT [[LookUpMaster] ([LookupId], [Name], [DataValue], [Description], [CreatedDate], [Status], [ParentId]) VALUES (9, N'IsotopSizeTypeBig', N'item-wide', N'item-wide', CAST(0x0750CF08698786380B AS DateTime2), 1, 5)
INSERT [[LookUpMaster] ([LookupId], [Name], [DataValue], [Description], [CreatedDate], [Status], [ParentId]) VALUES (10, N'IsotopSizeTypeSmall', N'item-small', N'item-small', CAST(0x07F0B940798786380B AS DateTime2), 1, 5)
INSERT [[LookUpMaster] ([LookupId], [Name], [DataValue], [Description], [CreatedDate], [Status], [ParentId]) VALUES (11, N'IsotopSizeTypeWide', N'item-long', N'item-long', CAST(0x07502528998786380B AS DateTime2), 1, 5)
INSERT [[LookUpMaster] ([LookupId], [Name], [DataValue], [Description], [CreatedDate], [Status], [ParentId]) VALUES (12, N'IsotopSizeTypeTall', N'item-high', N'item-high', CAST(0x07F08A1FA68786380B AS DateTime2), 1, 5)
INSERT [[LookUpMaster] ([LookupId], [Name], [DataValue], [Description], [CreatedDate], [Status], [ParentId]) VALUES (13, N'IsotopDataCatagoryTypeResidential', N'Residential', NULL, CAST(0x07204F8AE28786380B AS DateTime2), 1, 6)
SET IDENTITY_INSERT [[LookUpMaster] OFF
ALTER TABLE [[LookUpMaster] ADD  CONSTRAINT [DF_LookUpMaster_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [[LookUpMaster] ADD  CONSTRAINT [DF_LookUpMaster_Status]  DEFAULT ((1)) FOR [Status]
GO
GO

ALTER TABLE [Section] DROP CONSTRAINT [FK_Section_SectionType]

update [Section] set sectionTypeid = 7 where sectionTypeid = 1
go

ALTER TABLE [Section]  WITH NOCHECK ADD  CONSTRAINT [FK_Section_SectionType] FOREIGN KEY([SectionTypeId])
REFERENCES [LookUpMaster] ([LookupId])
GO

ALTER TABLE [Section] CHECK CONSTRAINT [FK_Section_SectionType]
GO