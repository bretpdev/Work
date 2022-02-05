<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" import="java.sql.*" import="java.io.*" import="java.util.*" errorPage="" %>
<%@ page import="edu.utahsbr.*" %>

<%
//out.println("<html><body bgcolor='red'>");

Driver DriverspGetTopic = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
Connection Conn = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
try{
	//delete form database
	
	//Delete linked topics
	String sql = "delete from LCTR_DAT_LinkedTopics where ID = " + request.getParameter("id") ;
	PreparedStatement Statement = Conn.prepareStatement(sql);
	Statement.executeUpdate();
	
	//Delete linked topics
	sql = "delete from LCTR_DAT_LinkedTopics where ToID = " + request.getParameter("id") ;
	Statement = Conn.prepareStatement(sql);
	Statement.executeUpdate();
	
	//Delete linked procedures
	sql = "delete from LCTR_DAT_TopicProcedure where TopicID = " + request.getParameter("id") ;
	Statement = Conn.prepareStatement(sql);
	Statement.executeUpdate();
	
	//Delete FlowChart
	sql = "delete from LCTR_DAT_FlowChart where ID = " + request.getParameter("id") + " and Type = 'Topic'";
	Statement = Conn.prepareStatement(sql);
	Statement.executeUpdate();
	
	//Delete Risk
	sql = "delete from LCTR_DAT_Risk where ID = " + request.getParameter("id") + " and Type = 'Topic'";
	Statement = Conn.prepareStatement(sql);
	Statement.executeUpdate();
	
	//Delete FlowChart
	sql = "delete from LCTR_DAT_Policy where ID = " + request.getParameter("id") + " and Type = 'Topic'";
	Statement = Conn.prepareStatement(sql);
	Statement.executeUpdate();
	
	sql = "delete from LCTR_DAT_Topic where ID = " + request.getParameter("id") ;
	Statement = Conn.prepareStatement(sql);
	Statement.executeUpdate();
	out.println("<Dat><Status>Operation Successful!</Status></Dat>");
}
catch (SQLException e){ 
	if (e.getErrorCode() == 2627){
		out.println("<Dat><Status>Record already exists.</Status></Dat>");
	}
	else {
		out.println("<Dat><Status>" + e.getMessage() + "</Status></Dat>");
	}
}
finally{
	Conn.close();
}
%>

