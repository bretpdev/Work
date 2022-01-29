<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" errorPage="" %>
<%@ page import="java.sql.*"%>
<%@ page import="edu.utahsbr.*"%>

<%
	Driver sqlServerDriver = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
	Connection sqlServerConnection = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
	try {
		PreparedStatement statement = sqlServerConnection.prepareStatement(request.getParameter("dsql").toString());
		statement.executeUpdate();
		statement = sqlServerConnection.prepareStatement(request.getParameter("isql").toString());
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
