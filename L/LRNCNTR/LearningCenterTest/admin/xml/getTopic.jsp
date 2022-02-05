<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" import="java.sql.*" errorPage="" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/functions" prefix="fn" %>
<%@ page import="edu.utahsbr.*" %>

<%

Driver DriverspGetTopic = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
Connection ConnspGetTopic = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
CallableStatement spGetTopic = ConnspGetTopic.prepareCall("{?= call dbo.spLCTR_getTopic(?)}");
Object spGetTopic_data;
spGetTopic.registerOutParameter(1,Types.LONGVARCHAR);
spGetTopic.setString(2,request.getParameter("topicID").toString());
spGetTopic.execute();
ResultSet rsResults = spGetTopic.getResultSet();
boolean rsResults_isEmpty = !rsResults.next();
boolean rsResults_hasData = !rsResults_isEmpty;
Object rsResults_data;
int rsResults_numRows = 0;
%>

<%= rsResults.getObject(1) %>

<%
ConnspGetTopic.close();
%>
