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
		//delete directory
		File procedureDirectory = new File(getServletContext().getRealPath("/") + "docs/procedures/" + request.getParameter("procedureName") + "/");
		if (procedureDirectory.exists()) {
			for (File procedure : procedureDirectory.listFiles()) { procedure.delete(); }
			procedureDirectory.delete();
		}
		
		//Delete linked topics
		String sql = "delete from LCTR_DAT_TopicProcedure where ProcedureID = '" + request.getParameter("id") + "'";
		PreparedStatement Statement = sqlServerConnection.prepareStatement(sql);
		Statement.executeUpdate();
		
		//Delete FlowChart
		sql = "delete from LCTR_DAT_FlowChart where ID = '" + request.getParameter("id") + "' and Type = 'Procedure'";
		Statement = sqlServerConnection.prepareStatement(sql);
		Statement.executeUpdate();
		
		//Delete Risk
		sql = "delete from LCTR_DAT_Risk where ID = '" + request.getParameter("id") + "' and Type = 'Procedure'";
		Statement = sqlServerConnection.prepareStatement(sql);
		Statement.executeUpdate();
		
		//Delete FlowChart
		sql = "delete from LCTR_DAT_Policy where ID = '" + request.getParameter("id") + "' and Type = 'Procedure'";
		Statement = sqlServerConnection.prepareStatement(sql);
		Statement.executeUpdate();
		
		sql = "delete from LCTR_DAT_Procedures where ID = '" + request.getParameter("id") + "'";
		Statement = sqlServerConnection.prepareStatement(sql);
		Statement.executeUpdate();
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
