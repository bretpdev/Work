CREATE TABLE [rccallhist].[InboundCalls]
(
	InboundCallsId INT PRIMARY KEY IDENTITY,
	RowId BIGINT NOT NULL,
	CallType INT,
	ListId VARCHAR(20),
	CampaignId VARCHAR(100),
	AccountNumber VARCHAR(40),
	AreaCode VARCHAR(10),
	Phone VARCHAR(10),
	AdditionalStatus VARCHAR(10),
	[Status] VARCHAR(10),
	AgentId VARCHAR(100), --Additional Primary Key Fields
	StartTime DATETIME,
	VoxFileName VARCHAR(128),
	TimeConnect INT,
	TimeACW INT,
	TimeHold INT,
	AgentHold INT,
	SessionSeqNum INT, --Additional Primary Key Fields
	NodeId INT, --Additional Primary Key Fields
	ProfileId INT, --Additional Primary Key Fields
	DialerField1 VARCHAR(256),
    DialerField2 VARCHAR(256),
    DialerField3 VARCHAR(256),
    DialerField4 VARCHAR(256),
    DialerField5 VARCHAR(256),
    DialerField6 VARCHAR(256),
    DialerField7 VARCHAR(256),
    DialerField8 VARCHAR(256),
    DialerField9 VARCHAR(256),
    DialerField10 VARCHAR(256)
)
