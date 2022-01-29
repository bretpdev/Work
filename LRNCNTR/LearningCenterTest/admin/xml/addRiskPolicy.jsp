<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" import="java.sql.*" errorPage="" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/functions" prefix="fn" %>
<%@ page import="edu.utahsbr.*" %>

<%
Driver DriverspGetTopic = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
Connection Conn = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
try{
	
	PreparedStatement Statement = Conn.prepareStatement(request.getParameter("dsql").toString());
	Statement.executeUpdate();
	PreparedStatement Statement2 = Conn.prepareStatement(request.getParameter("isql").toString());
	Statement2.executeUpdate();
%>
    <Dat>
	<Status>Operation Successful!</Status>
	</Dat>
<%
}
catch (SQLException e){ 
	if (e.getErrorCode() == 2627){
		out.println("<Dat><Status>Record already exists.</Status></Dat>");
	}
	else {
		out.println("<Dat><Status>" + e.getMessage() + "</Status></Dat>");
	}
%>
	
<%    

}
finally{
	Conn.close();
}
%>
