<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java"
	errorPage=""%>
<%@ page import="java.io.*"%>
<%@ page import="java.sql.*"%>
<%@ page import="java.util.*"%>
<%@ page import="edu.utahsbr.*"%>

<%
	Driver sqlServerDriver = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
	Connection sqlServerConnection = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
	CallableStatement spGetProcedure = sqlServerConnection.prepareCall("{?= call dbo.spLCTR_getProcedure(?)}");
	spGetProcedure.registerOutParameter(1, Types.LONGVARCHAR);
	spGetProcedure.setString(2, request.getParameter("pID").toString());
	spGetProcedure.execute();
	ResultSet rsResults = spGetProcedure.getResultSet();
	rsResults.next();
%>

<%=rsResults.getObject(1)%>

<%
	sqlServerConnection.close();
%>
