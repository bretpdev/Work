<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" errorPage=""%>
<%@ page import="java.io.*"%>
<%@ page import="java.sql.*"%>
<%@ page import="java.util.*"%>

<%
	try {
		//Upload Staff HTML with no pictures
		String newDocName = getServletContext().getRealPath("/") + "docs/staff/" + request.getParameter("docName") + ".html";
		//delete old file
		File oldDoc = new File(newDocName);
		if (oldDoc.exists()) { oldDoc.delete(); }
		if (request.getParameter("action").equals("update")) {
			//copy new file to correct location
			File newDoc = new File(getServletContext().getRealPath("/") + "data/" + request.getParameter("htmlFile"));
			newDoc.renameTo(new File(newDocName));
		}
		out.println("<Dat><Status>Operation Successful!</Status></Dat>");
	}
	catch (Exception e) {
		out.println("<Dat><Status>" + e.getMessage() + "</Status></Dat>");
	}
%>
