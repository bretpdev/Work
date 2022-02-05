<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" errorPage="" %>
<%@ page import="java.io.*"%>
<%@ page import="java.sql.*"%>
<%@ page import="java.util.*"%>
<%@ page import="edu.utahsbr.*" %>

<%
	try {
		String topicDirectoryName = getServletContext().getRealPath("/") + "docs/flowcharts/" + request.getParameter("type") + "/" + request.getParameter("topicName") + "/";
		if (request.getParameter("htmlFile") != "") {
			File topicDirectory = new File(topicDirectoryName);
			if (!topicDirectory.exists()) { topicDirectory.mkdir(); }
			
			//Copy HTML file to flowcharts directory
			File newhtmlfile = new File(topicDirectoryName + request.getParameter("topicName") + ".html" );
			if (newhtmlfile.exists()) { newhtmlfile.delete(); }
			
			PrintWriter pw = new PrintWriter(new FileWriter(newhtmlfile));
			File tempHtmlFile = new File(getServletContext().getRealPath("/") + "data/" + request.getParameter("tempHtmlFile"));
			BufferedReader in = new BufferedReader(new FileReader(tempHtmlFile));
			
			ArrayList<String> files = new ArrayList<String>();
			String str = in.readLine();
			String newstr ="";
			String imageStr ="";
			String fullimageStr ="";
			while (str != null) {
				newstr = str;
				while (str.contains(".jpg") || str.contains(".gif") || str.contains(".bmp") || str.contains(".png")) {
					if (str.contains(".jpg")) {
						fullimageStr = str.substring(str.lastIndexOf("=", str.indexOf(".jpg")) + 2, str.indexOf(".jpg") + 4);
						imageStr = fullimageStr.substring(fullimageStr.lastIndexOf("/") + 1);
						files.add(imageStr);
						str = str.replace(imageStr, "");
					}
					if (str.contains(".gif")) {
						fullimageStr = str.substring(str.lastIndexOf("=", str.indexOf(".gif")) + 2, str.indexOf(".gif") + 4);
						imageStr = fullimageStr.substring(fullimageStr.lastIndexOf("/") + 1);
						files.add(imageStr);
						str = str.replace(imageStr, "");
					}
					if (str.contains(".bmp")) {
						fullimageStr = str.substring(str.lastIndexOf("=", str.indexOf(".bmp")) + 2, str.indexOf(".bmp") + 4);
						imageStr = fullimageStr.substring(fullimageStr.lastIndexOf("/") + 1);
						files.add(imageStr);
						str = str.replace(imageStr, "");
					}
					if (str.contains(".png")) {
						fullimageStr = str.substring(str.lastIndexOf("=", str.indexOf(".png")) + 2, str.indexOf(".png") + 4);
						imageStr = fullimageStr.substring(fullimageStr.lastIndexOf("/") + 1);
						files.add(imageStr);
						str = str.replace(imageStr, "");
					}
					if (fullimageStr != "") {
						newstr = newstr.replace(fullimageStr, "docs/flowcharts/" + request.getParameter("type") + "/" + request.getParameter("topicName") + "/" + imageStr);
					}
				}
				pw.println(newstr);
				str = in.readLine();
			}
			in.close();
			pw.close();
			
			//Copy image files to procedures directory	
			for (String imageFileName : files) {
				File tempImageFile = new File(getServletContext().getRealPath("/") + "data/" + imageFileName);
				File newImageFile = new File(topicDirectoryName + imageFileName);
				if (newImageFile.exists()) { newImageFile.delete(); }
				if (tempImageFile.exists()){ tempImageFile.renameTo(newImageFile); }
			}
			tempHtmlFile.delete();
		}
		//Add to database
		Driver sqlServerDriver = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
		Connection sqlServerConnection = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
		//Delete
		String deleteCommand = "delete from LCTR_DAT_FlowChart where ID = '" + request.getParameter("id") + "' and Type = '" + request.getParameter("type") + "'";
		PreparedStatement deleteStatement = sqlServerConnection.prepareStatement(deleteCommand);
		deleteStatement.executeUpdate();
		//Insert
		String insertCommand = "insert into LCTR_DAT_FlowChart (ID,Type,Description) values(" + request.getParameter("id") + ",'" + request.getParameter("type") + "','" + request.getParameter("description") + "')";
		PreparedStatement insertStatement = sqlServerConnection.prepareStatement(insertCommand);
		insertStatement.executeUpdate();
		sqlServerConnection.close();
		out.println("<Dat><Status>Operation Successful!</Status></Dat>");
	}
	catch (Exception e) {
		out.println("<Dat><Status>" + e.getMessage() + "</Status></Dat>");
	}
%>
