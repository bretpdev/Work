<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" errorPage=""%>
<%@ page import="java.io.*"%>
<%@ page import="java.sql.*"%>
<%@ page import="java.util.*"%>
<%@ page import="edu.utahsbr.*"%>

<%
	try {
		if (!request.getParameter("htmlFile").equals("")) {
			String procedureAbsolutePath = getServletContext().getRealPath("/") + "docs/procedures/" + request.getParameter("procedureName") + "/";
			String procedureRelativePath = "docs/procedures/" + request.getParameter("procedureName") + "/";
			File procedureDirectory = new File(procedureAbsolutePath);
			if (procedureDirectory.exists()) {
				for (File procedure : procedureDirectory.listFiles()) { procedure.delete(); }
			}
			else {
				procedureDirectory.mkdir();
			}
			
			//Copy HTML file to procedures directory
			File newHtmlFile = new File(procedureAbsolutePath + request.getParameter("procedureName") + ".html");
			if (newHtmlFile.exists()) { newHtmlFile.delete(); }

			PrintWriter pw = new PrintWriter(new FileWriter(newHtmlFile));
			File tempHtmlFile = new File(getServletContext().getRealPath("/") + "data/" + request.getParameter("htmlFile"));
			BufferedReader in = new BufferedReader(new InputStreamReader(new FileInputStream(tempHtmlFile), "UTF16"));

			ArrayList<String> files = new ArrayList<String>();
			String str = in.readLine();

			String newstr = "";
			String imageStr = "";
			String fullimageStr = "";
			while (str != null) {
				str = str.replace("src = ", "src=");
				str = str.replace("src= ", "src=");
				str = str.replace("src =", "src=");
				str = str.replace("<v:imagedata", "<img");
				str = str.replace("</v:imagedata>", "");
				newstr = str;
				while (str.indexOf(".jpg") > -1 || str.indexOf(".gif") > -1 || str.indexOf(".bmp") > -1 || str.indexOf(".png") > -1) {
					if (str.indexOf(".jpg") > -1) {
						fullimageStr = str.substring(str.lastIndexOf("=", str.indexOf(".jpg")) + 1, str.indexOf(".jpg") + 4);
						fullimageStr = fullimageStr.replace("\"", "");
						imageStr = fullimageStr;
						if (files.indexOf(imageStr) == -1) { files.add(imageStr); }
						str = str.replace(imageStr, "");
						if (fullimageStr != "") {
							newstr = newstr.replace(fullimageStr, "\"" + procedureRelativePath + imageStr + "\"");
							newstr = newstr.replace("\"\"" + procedureRelativePath + imageStr + "\"\"", "\"" + procedureRelativePath + imageStr + "\"");
						}
						fullimageStr = "";
					}
					if (str.indexOf(".gif") > -1) {
						fullimageStr = str.substring(str.lastIndexOf("=", str.indexOf(".gif")) + 1, str.indexOf(".gif") + 4);
						fullimageStr = fullimageStr.replace("\"", "");
						imageStr = fullimageStr;
						if (files.indexOf(imageStr) == -1) { files.add(imageStr); }
						str = str.replace(imageStr, "");
						if (fullimageStr != "") {
							newstr = newstr.replace(fullimageStr, "\"" + procedureRelativePath + imageStr + "\"");
							newstr = newstr.replace("\"\"" + procedureRelativePath + imageStr + "\"\"", "\"" + procedureRelativePath + imageStr + "\"");
						}
						fullimageStr = "";
					}
					if (str.indexOf(".bmp") > -1) {
						fullimageStr = str.substring(str.lastIndexOf("=", str.indexOf(".bmp")) + 1, str.indexOf(".bmp") + 4);
						fullimageStr = fullimageStr.replace("\"", "");
						imageStr = fullimageStr;
						if (files.indexOf(imageStr) == -1) { files.add(imageStr); }
						str = str.replace(imageStr, "");
						if (fullimageStr != "") {
							newstr = newstr.replace(fullimageStr, "\"" + procedureRelativePath + imageStr + "\"");
							newstr = newstr.replace("\"\"" + procedureRelativePath + imageStr + "\"\"", "\"" + procedureRelativePath + imageStr + "\"");
						}
						fullimageStr = "";
					}
					if (str.indexOf(".png") > -1) {
						fullimageStr = str.substring(str.lastIndexOf("=", str.indexOf(".png")) + 1, str.indexOf(".png") + 4);
						fullimageStr = fullimageStr.replace("\"", "");
						imageStr = fullimageStr;
						if (files.indexOf(imageStr) == -1) { files.add(imageStr); }
						str = str.replace(imageStr, "");
						if (fullimageStr != "") {
							newstr = newstr.replace(fullimageStr, "\"" + procedureRelativePath + imageStr + "\"");
							newstr = newstr.replace("\"\"" + procedureRelativePath + imageStr + "\"\"", "\"" + procedureRelativePath + imageStr + "\"");
						}
						fullimageStr = "";
					}
				}
				pw.println(newstr);
				str = in.readLine();
			}
			in.close();
			pw.close();
			tempHtmlFile.delete();

			//Copy image files to procedures directory	
			for (String newFileName : files) {
				File tempImageFile = new File(getServletContext().getRealPath("/") + "data/" + newFileName);
				File newImageFile = new File(procedureAbsolutePath + newFileName);
				if (newImageFile.exists()) { newImageFile.delete(); }
				if (tempImageFile.exists()) { tempImageFile.renameTo(newImageFile); }
			}
		}

		//Add to database
		Driver sqlServerDriver = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
		Connection sqlServerConnection = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
		String procedureName = request.getParameter("procedureName").toString();
		String searchKey = request.getParameter("sKey").toString();
		String query = "Select ID from LCTR_DAT_Procedures where Name = '" + procedureName + "'";
		PreparedStatement statement = sqlServerConnection.prepareStatement(query);
		ResultSet result = statement.executeQuery();
		if (result.next()) {
			query = "update LCTR_DAT_Procedures set SearchKey = '" + searchKey + "', Name = '" + procedureName + "' where Name = '" + procedureName + "'";
			statement = sqlServerConnection.prepareStatement(query);
			statement.executeUpdate();
		}
		else {
			query = "insert into LCTR_DAT_Procedures (Name, SearchKey) values('" + procedureName + "', '" + searchKey + "')";
			statement = sqlServerConnection.prepareStatement(query);
			statement.executeUpdate();
		}
		sqlServerConnection.close();
		out.println("<Dat><Status>Operation Successful!</Status></Dat>");
	}
	catch (Exception e) {
		out.println("<Dat><Status>" + e.getMessage() + "</Status></Dat>");
	}
%>
