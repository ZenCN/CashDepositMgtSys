USE [CashDepositMgtSys]
GO
/****** Object:  Table [dbo].[agency]    Script Date: 2017-2-17 下午 05:28:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[agency](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NULL,
	[p_code] [varchar](50) NULL,
	[name] [varchar](50) NULL,
	[type] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[deducted]    Script Date: 2017-2-17 下午 05:28:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[deducted](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[generation_gives_id] [int] NULL,
	[item] [varchar](100) NULL,
	[amount] [decimal](10, 2) NULL,
	[remark] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[generation_buckle]    Script Date: 2017-2-17 下午 05:28:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[generation_buckle](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[record_date] [datetime] NULL,
	[recorder_code] [varchar](50) NULL,
	[reviewer_code] [varchar](50) NULL,
	[review_state] [int] NULL,
	[review_date] [datetime] NULL,
	[agency_code] [varchar](50) NULL,
	[salesman_name] [varchar](20) NULL,
	[salesman_sex] [varchar](10) NULL,
	[salesman_card_type] [varchar](20) NULL,
	[salesman_card_id] [varchar](50) NULL,
	[salesman_phone] [varchar](50) NULL,
	[salesman_bank_account_name] [varchar](20) NULL,
	[salesman_bank_account_number] [varchar](50) NULL,
	[salesman_bank_name] [varchar](50) NULL,
	[salesman_bank_province] [varchar](20) NULL,
	[salesman_bank_city] [varchar](20) NULL,
	[salesman_subbank_name] [varchar](50) NULL,
	[cash_deposit] [decimal](10, 2) NULL,
	[remark] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[generation_gives]    Script Date: 2017-2-17 下午 05:28:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[generation_gives](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[record_date] [datetime] NULL,
	[recorder_code] [varchar](50) NULL,
	[reviewer_code] [varchar](50) NULL,
	[review_state] [int] NULL,
	[review_date] [datetime] NULL,
	[agency_code] [varchar](50) NULL,
	[salesman_name] [varchar](20) NULL,
	[salesman_sex] [varchar](10) NULL,
	[salesman_card_type] [varchar](20) NULL,
	[salesman_card_id] [varchar](50) NULL,
	[salesman_phone] [varchar](50) NULL,
	[salesman_bank_account_name] [varchar](20) NULL,
	[salesman_bank_account_number] [varchar](50) NULL,
	[salesman_bank_name] [varchar](50) NULL,
	[salesman_bank_province] [varchar](20) NULL,
	[salesman_bank_city] [varchar](20) NULL,
	[salesman_subbank_name] [varchar](50) NULL,
	[salesman_cash_deposit] [decimal](10, 2) NULL,
	[salesman_refunds] [decimal](10, 2) NULL,
	[salesman_refunds_state] [int] NULL,
	[remark] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[staff]    Script Date: 2017-2-17 下午 05:28:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[staff](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](50) NULL,
	[agency_code] [varchar](50) NULL,
	[name] [varchar](50) NULL,
	[pwd] [varchar](50) NULL,
	[enable] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理机构代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'agency', @level2type=N'COLUMN',@level2name=N'code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属管理机构代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'agency', @level2type=N'COLUMN',@level2name=N'p_code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理机构名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'agency', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理机构类别：业务/财务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'agency', @level2type=N'COLUMN',@level2name=N'type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'代付表id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'deducted', @level2type=N'COLUMN',@level2name=N'generation_gives_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扣款项目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'deducted', @level2type=N'COLUMN',@level2name=N'item'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扣款金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'deducted', @level2type=N'COLUMN',@level2name=N'amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'deducted', @level2type=N'COLUMN',@level2name=N'remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'录入日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'record_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'录入人代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'recorder_code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核人代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'reviewer_code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态：0 未审核、1 审核通过、2 拒绝' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'review_state'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'review_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理机构代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'agency_code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售人员姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'salesman_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售人员性别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'salesman_sex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售人员证件类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'salesman_card_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售人员证件号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'salesman_card_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售人员手机号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'salesman_phone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'银行户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'salesman_bank_account_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'银行账号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'salesman_bank_account_number'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户银行名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'salesman_bank_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'银行所在省份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'salesman_bank_province'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'银行所在市' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'salesman_bank_city'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支行名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'salesman_subbank_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'保证金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'cash_deposit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_buckle', @level2type=N'COLUMN',@level2name=N'remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'录入日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'record_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'录入人代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'recorder_code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核人代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'reviewer_code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态：0 未审核、1 审核通过、2 拒绝' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'review_state'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'review_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理机构代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'agency_code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售人员姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售人员性别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_sex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售人员证件类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_card_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售人员证件号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_card_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售人员手机号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_phone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'银行户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_bank_account_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'银行账号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_bank_account_number'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户银行名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_bank_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'银行所在省份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_bank_province'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'银行所在市' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_bank_city'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支行名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_subbank_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'保证金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_cash_deposit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退款金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_refunds'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退款状态：0 未处理、1 已打印付款声明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'salesman_refunds_state'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generation_gives', @level2type=N'COLUMN',@level2name=N'remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作人员代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'staff', @level2type=N'COLUMN',@level2name=N'code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属管理机构代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'staff', @level2type=N'COLUMN',@level2name=N'agency_code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'staff', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'staff', @level2type=N'COLUMN',@level2name=N'pwd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'启用状态：0 未启用、1 已启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'staff', @level2type=N'COLUMN',@level2name=N'enable'
GO
