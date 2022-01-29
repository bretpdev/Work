/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
declare @corr int
insert into dbo.DocTypes (DocTypeValue)
values ('corr')
select @corr = SCOPE_IDENTITY()

declare @bcor int
insert into dbo.DocTypes (DocTypeValue)
values ('bcor')
select @bcor = SCOPE_IDENTITY()

declare @ddbd int
insert into dbo.DocTypes (DocTypeValue)
values ('ddbd')
select @ddbd = SCOPE_IDENTITY()

declare @defr int
insert into dbo.DocTypes (DocTypeValue)
values ('defr')
select @defr = SCOPE_IDENTITY()

declare @disc int
insert into dbo.DocTypes (DocTypeValue)
values ('disc')
select @disc = SCOPE_IDENTITY()

declare @dodf int
insert into dbo.DocTypes (DocTypeValue)
values ('dodf')
select @dodf = SCOPE_IDENTITY()

declare @enda int
insert into dbo.DocTypes (DocTypeValue)
values ('enda')
select @enda = SCOPE_IDENTITY()

declare @forb int
insert into dbo.DocTypes (DocTypeValue)
values ('forb')
select @forb = SCOPE_IDENTITY()

declare @ibrf int
insert into dbo.DocTypes (DocTypeValue)
values ('ibrf')
select @ibrf = SCOPE_IDENTITY()

declare @note int
insert into dbo.DocTypes (DocTypeValue)
values ('note')
select @note = SCOPE_IDENTITY()

declare @payh int
insert into dbo.DocTypes (DocTypeValue)
values ('payh')
select @payh = SCOPE_IDENTITY()

declare @phst int
insert into dbo.DocTypes (DocTypeValue)
values ('phst')
select @phst = SCOPE_IDENTITY()

declare @scor int
insert into dbo.DocTypes (DocTypeValue)
values ('scor')
select @scor = SCOPE_IDENTITY()

declare @srvh int
insert into dbo.DocTypes (DocTypeValue)
values ('srvh')
select @srvh = SCOPE_IDENTITY()

declare @tlfo int
insert into dbo.DocTypes (DocTypeValue)
values ('tlfo')
select @tlfo = SCOPE_IDENTITY()

declare @tpdd int
insert into dbo.DocTypes (DocTypeValue)
values ('tpdd')
select @tpdd = SCOPE_IDENTITY()

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCOR', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRDDC', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRAAA', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRUPR', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCCR', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRSSN', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRDCF', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCBF', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRS11', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRFCR', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCFX', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRENR', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRPSF', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRARC', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRRSH', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRKEY', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRDDA', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCSK', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CR3RD', @corr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRLCD', @corr)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRBCR', @bcor)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRDTH', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRBCS', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRBCT', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRDIM', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRDLT', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRDRJ', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRDIS', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRBRP', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRDED', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRDEM', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRDBD', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCDD', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRBDT', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRDCH', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRBKP', @ddbd)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRBDS', @ddbd)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCDF', @defr)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRDFM', @defr)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCDS', @disc)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCDO', @dodf)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCEA', @enda)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCFB', @forb)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRFRB', @forb)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRIDR', @ibrf)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRADI', @ibrf)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCIB', @ibrf)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRLIB', @ibrf)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRIBW', @ibrf)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRIBR', @ibrf)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRIBC', @ibrf)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRICR', @ibrf)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCPN', @note)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCPO', @note)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCPH', @payh)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRPHS', @payh)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRPMT', @payh)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCHS', @phst)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRHIS', @phst)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCSC', @scor)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCSH', @srvh)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRCTL', @tlfo)
insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRTLF', @tlfo)

insert into dbo.DocIds (DocIdValue, DocTypeId)
values ('CRTPD', @tpdd)

