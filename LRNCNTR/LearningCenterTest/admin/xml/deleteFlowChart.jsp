<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" import="java.sql.*" import="java.io.*" import="java.util.*" errorPage="" %>
<%@ page import="edu.utahsbr.*" %>

<%
try{
File d = new File(getServletContext().getRealPath("/") + "docs/flowcharts/" + request.getParameter("type")+ "/" + request.getParameter("topicName") + "/");
out.println(getServletContext().getRealPath("/") + "docs/flowcharts/" + request.getParameter("type")+ "/" + request.getParameter("topicName") + "/");
if (d.exists()){
	String arr[] = d.list();
	for (int x = 0;x<arr.length;x++){
		out.println(getServletContext().getRealPath("/") + "docs/flowcharts/" + request.getParameter("type")+ "/" + request.getParameter("topicName") + "/" + arr[x]);
		File del = new File(getServletContext().getRealPath("/") + "docs/flowcharts/" + request.getParameter("type")+ "/" + request.getParameter("topicName") + "/" + arr[x]);
		del.delete();
	}
	d.delete();
}

//delete form database
Driver DriverspGetTopic = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
Connection Conn = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
String sql = "delete from LCTR_DAT_FlowChart where ID = '" + request.getParameter("id") + "' and Type = '" + request.getParameter("type") + "'";
PreparedStatement Statement = Conn.prepareStatement(sql);
Statement.executeUpdate();
Conn.close();
out.println("<Dat><Status>Operation Successful!</Status></Dat>");
}
catch (Exception e){
out.println("<Dat><Status>" + e.getMessage() + "</Status></Dat>");
}
%>

