USE NobleCalls
GO

select * from agenttabl.ContactCallDetailUCCX R LEFT JOIN OPENQUERY(UCCX, 'select * from ContactCallDetail') O ON O.sessionid = R.sessionid AND O.sessionseqnum = R.sessionseqnum AND O.nodeid = R.nodeid AND O.profileid = R.profileid
select * from agenttabl.ContactCallDetailUCCX order by sessionid, sessionseqnum, nodeid, profileid
select * from openquery(UCCX, 'select * from ContactCallDetail') order by sessionid, sessionseqnum, nodeid, profileid

select * from agenttabl.AgentStateDetailUCCX R LEFT JOIN OPENQUERY(UCCX, 'select * from AgentStateDetail') O ON O.agentid = R.agentid AND O.eventdatetime = R.eventdatetime AND O.eventType = R.eventtype AND O.reasonCode = R.reasoncode AND O.profileID = R.profileid WHERE O.agentid IS NULL order by R.agentid, R.eventdatetime
select * from agenttabl.AgentStateDetailUCCX order by agentid, eventdatetime
select * from openquery(UCCX, 'select * from AgentStateDetail') order by agentid, eventdatetime

select * from agenttabl.AgentConnectionDetailUCCX R LEFT JOIN OPENQUERY(UCCX, 'select * from AgentConnectionDetail') O on R.sessionid = O.sessionid WHERE O.sessionid IS NULL order by R.sessionid
select * from agenttabl.AgentConnectionDetailUCCX order by sessionid
select * from openquery(UCCX, 'select * from AgentConnectionDetail') order by sessionid

select * from agenttabl.ContactWrapUpDataUCCX R LEFT JOIN OPENQUERY(UCCX, 'select sessionID, sessionSeqNum, resourceID, wrapupData, nodeid, qindex, CAST(startDateTime AS VARCHAR(23)) AS startDateTime, wrapupindex, contactid from ContactWrapUpData') O on R.sessionId = O.sessionId WHERE O.sessionId IS NULL order by R.sessionId
select * from agenttabl.ContactWrapUpDataUCCX order by sessionID
select * from openquery(UCCX, 'SELECT sessionID, sessionSeqNum, resourceID, wrapupData, nodeid, qindex, CAST(startDateTime AS VARCHAR(23)) AS startDateTime, wrapupindex, contactid from ContactWrapUpData') order by sessionID

select * from agenttabl.ResourceUCCX R LEFT JOIN OPENQUERY(UCCX, 'select * from Resource') O on R.resourceId = O.resourceId WHERE O.resourceid IS NULL order by R.resourceid
select * from agenttabl.ResourceUCCX order by resourceid
select * from openquery(UCCX, 'select * from Resource') order by resourceid

select * from agenttabl.ReasonCodeLabelMapUCCX R LEFT JOIN OPENQUERY(UCCX, 'select * from ReasonCodeLabelMap') O on R.code = O.code WHERE O.code IS NULL order by R.code
select * from agenttabl.ReasonCodeLabelMapUCCX order by code
select * from openquery(UCCX, 'select * from ReasonCodeLabelMap') order by code

select * from agenttabl.ContactServiceQueueUCCX R LEFT JOIN OPENQUERY(UCCX, 'select * from ContactServiceQueue') O on R.contactservicequeueid = O.contactservicequeueid WHERE O.contactservicequeueid IS NULL order by R.contactservicequeueid
select * from agenttabl.ContactServiceQueueUCCX order by contactservicequeueid
select * from openquery(UCCX, 'select * from ContactServiceQueue') order by contactservicequeueid

USE NobleCalls
GO
select * from agenttabl.RPT_CallActivityUNEXSYS
select * from agenttabl.OBD_OutcomeUNEXSYS
select * from agenttabl.RPT_CallActivityUNEXSYS_Staging
select * from agenttabl.OBD_OutcomeUNEXSYS_Staging

