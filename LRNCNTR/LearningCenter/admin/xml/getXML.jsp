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
	CallableStatement spGetXml = sqlServerConnection.prepareCall("{?= call " + request.getParameter("sp") + "}");
	spGetXml.registerOutParameter(1, Types.LONGVARCHAR);
	spGetXml.execute();
	ResultSet results = spGetXml.getResultSet();
	results.next();
%>

<%=results.getObject(1)%>

<%
	sqlServerConnection.close();
%>