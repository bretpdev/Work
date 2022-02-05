<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" errorPage=""%>
<%@ page import="java.io.*"%>
<%@ page import="java.sql.*"%>
<%@ page import="java.util.*"%>
<%@ page import="edu.utahsbr.*"%>

<%
	try {
		File topicDirectory = new File(getServletContext().getRealPath("/") + "docs/flowcharts/" + request.getParameter("type") + "/" + request.getParameter("topicName") + "/");
		if (topicDirectory.exists()) {
			for (File oldFile : topicDirectory.listFiles()) { oldFile.delete(); }
			topicDirectory.delete();
		}
		
		//delete form database
		Driver sqlServerDriver = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
		Connection sqlServerConnection = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
		String command = "delete from LCTR_DAT_FlowChart where ID = '" + request.getParameter("id") + "' and Type = '" + request.getParameter("type") + "'";
		PreparedStatement statement = sqlServerConnection.prepareStatement(command);
		statement.executeUpdate();
		sqlServerConnection.close();
		out.println("<Dat><Status>Operation Successful!</Status></Dat>");
	}
	catch (Exception e) {
		out.println("<Dat><Status>" + e.getMessage() + "</Status></Dat>");
	}
%>
