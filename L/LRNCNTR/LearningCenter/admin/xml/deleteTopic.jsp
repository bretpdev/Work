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
	try {
		//Delete linked topics
		String sql = "delete from LCTR_DAT_LinkedTopics where ID = " + request.getParameter("id");
		PreparedStatement statement = sqlServerConnection.prepareStatement(sql);
		statement.executeUpdate();
		
		//Delete linked topics
		sql = "delete from LCTR_DAT_LinkedTopics where ToID = " + request.getParameter("id");
		statement = sqlServerConnection.prepareStatement(sql);
		statement.executeUpdate();
		
		//Delete linked procedures
		sql = "delete from LCTR_DAT_TopicProcedure where TopicID = " + request.getParameter("id");
		statement = sqlServerConnection.prepareStatement(sql);
		statement.executeUpdate();
		
		//Delete FlowChart
		sql = "delete from LCTR_DAT_FlowChart where ID = " + request.getParameter("id") + " and Type = 'Topic'";
		statement = sqlServerConnection.prepareStatement(sql);
		statement.executeUpdate();
		
		//Delete Risk
		sql = "delete from LCTR_DAT_Risk where ID = " + request.getParameter("id") + " and Type = 'Topic'";
		statement = sqlServerConnection.prepareStatement(sql);
		statement.executeUpdate();
		
		//Delete FlowChart
		sql = "delete from LCTR_DAT_Policy where ID = " + request.getParameter("id") + " and Type = 'Topic'";
		statement = sqlServerConnection.prepareStatement(sql);
		statement.executeUpdate();
		
		sql = "delete from LCTR_DAT_Topic where ID = " + request.getParameter("id");
		statement = sqlServerConnection.prepareStatement(sql);
		statement.executeUpdate();
		out.println("<Dat><Status>Operation Successful!</Status></Dat>");
	}
	catch (SQLException e) {
		if (e.getErrorCode() == 2627) {
			out.println("<Dat><Status>Record already exists.</Status></Dat>");
		}
		else {
			out.println("<Dat><Status>" + e.getMessage() + "</Status></Dat>");
		}
	}
	finally {
		sqlServerConnection.close();
	}
%>

