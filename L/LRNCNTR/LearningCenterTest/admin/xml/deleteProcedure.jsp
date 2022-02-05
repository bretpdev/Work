<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" import="java.sql.*" import="java.io.*" import="java.util.*" errorPage="" %>
<%@ page import="edu.utahsbr.*" %>

<%
//out.println("<html><body bgcolor='red'>");

Driver DriverspGetTopic = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
Connection Conn = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
try{
	//delete directory
	File d = new File(getServletContext().getRealPath("/") + "docs/procedures/" + request.getParameter("procedureName") + "/");
	if (d.exists()){
		String arr[] = d.list();
		for (int x = 0;x<arr.length;x++){
			File del = new File(getServletContext().getRealPath("/") + "docs/procedures/" + request.getParameter("procedureName") + "/" + arr[x]);
			del.delete();
		}
		d.delete();
	}
	
	//delete form database
	
	//Delete linked topics
	String sql = "delete from LCTR_DAT_TopicProcedure where ProcedureID = '" + request.getParameter("id") + "'";
	PreparedStatement Statement = Conn.prepareStatement(sql);
	Statement.executeUpdate();
	
	//Delete FlowChart
	sql = "delete from LCTR_DAT_FlowChart where ID = '" + request.getParameter("id") + "' and Type = 'Procedure'";
	Statement = Conn.prepareStatement(sql);
	Statement.executeUpdate();
	
	//Delete Risk
	sql = "delete from LCTR_DAT_Risk where ID = '" + request.getParameter("id") + "' and Type = 'Procedure'";
	Statement = Conn.prepareStatement(sql);
	Statement.executeUpdate();
	
	//Delete FlowChart
	sql = "delete from LCTR_DAT_Policy where ID = '" + request.getParameter("id") + "' and Type = 'Procedure'";
	Statement = Conn.prepareStatement(sql);
	Statement.executeUpdate();
	
	sql = "delete from LCTR_DAT_Procedures where ID = '" + request.getParameter("id") + "'";
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

