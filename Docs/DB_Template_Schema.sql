-- Create Table Client


CREATE TABLE [dbo].[Client](
	[ClientID] [bigint] NOT NULL,
	[ClientName] [varchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[PhoneNamber] [varchar](15) NOT NULL,

 CONSTRAINT [pk_Client] PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


-- Create Table ClientContacts

CREATE TABLE [dbo].ClientContacts(
	[ClientContactID] [bigint] NOT NULL,
	[ClientID] [bigint] NOT NULL,
	[ContactName] [varchar](50) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[CellPhoneNumber] [varchar](16) NOT NULL,
	CONSTRAINT [pk_ClientContact] PRIMARY KEY CLUSTERED 
	(
	    [ClientContactID] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ClientContacts]  WITH NOCHECK ADD  CONSTRAINT [fk_Client_ClientContact] FOREIGN KEY([ClientID])
REFERENCES [dbo].[Client] ([ClientID])
GO




