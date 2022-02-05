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
	CallableStatement spGetGlossaryTerm = sqlServerConnection.prepareCall("{?= call dbo.spLCTR_getGlossaryTerm(?)}");
	spGetGlossaryTerm.registerOutParameter(1, Types.INTEGER);
	spGetGlossaryTerm.setString(2, request.getParameter("TermID").toString());
	spGetGlossaryTerm.execute();
	ResultSet results = spGetGlossaryTerm.getResultSet();
	results.next();
%>

<%=results.getObject(1)%>

<%
	sqlServerConnection.close();
%>
