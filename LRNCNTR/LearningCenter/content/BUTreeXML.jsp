<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" import="java.sql.*" errorPage="" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/functions" prefix="fn" %>
<%@ page import="edu.utahsbr.*" %>

<%
	Driver sqlServerDriver = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
	Connection sqlServerConnection = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
	CallableStatement topicCallableStatement = sqlServerConnection.prepareCall("{?= call dbo.spLCTRUsersBusinessUnit(?)}");
	topicCallableStatement.registerOutParameter(1, Types.INTEGER);
	topicCallableStatement.setString(2, request.getParameter("BU").toString());
	topicCallableStatement.execute();
	ResultSet topicResults = topicCallableStatement.getResultSet();
	String result = "<A><eName>None</eName></A>";
	if (topicResults.next()) {
		result = topicResults.getObject(1).toString();
	}
	sqlServerConnection.close();
%>
<DAT>
	<%=result%>
</DAT>
