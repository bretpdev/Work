<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" import="java.sql.*" import="java.io.*" import="java.util.*" errorPage="" %>

<%
try{
//Upload Staff HTML with no pictures
	if (request.getParameter("action").equals("update")){
		//delete old file
		File f = new File(getServletContext().getRealPath("/") + "docs/staff/" + request.getParameter("docName") + ".html");
		if (f.exists()){
			f.delete();
		}
		//copy new file to correct location
		File data = new File(getServletContext().getRealPath("/") + "data/" + request.getParameter("htmlFile"));
		data.renameTo(new File(getServletContext().getRealPath("/") + "docs/staff/",request.getParameter("docName") + ".html"));
	}
	else if (request.getParameter("action").equals("delete")){
		//delete current file
		File f = new File(getServletContext().getRealPath("/") + "docs/staff/" + request.getParameter("docName") + ".html");
		if (f.exists()){f.delete();}
	}
	
	out.println("<Dat><Status>Operation Successful!</Status></Dat>");
}
catch (Exception e){
	out.println("<Dat><Status>" + e.getMessage() + "</Status></Dat>");
}

%>

