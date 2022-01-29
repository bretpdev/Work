<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" import="java.sql.*" import="java.io.*" import="java.util.*" errorPage="" %>
<%@ page import="edu.utahsbr.*" %>

<%
	try {
		File procedureDirectory = new File(getServletContext().getRealPath("/") + "docs/procedures/" + request.getParameter("procedureName") + "/");
		if (!procedureDirectory.exists()) { procedureDirectory.mkdir(); }
		
		//Copy HTML file to procedures directory
		File newhtmlfile = new File(getServletContext().getRealPath("/") + "docs/procedures/" + request.getParameter("procedureName") + "/" + request.getParameter("procedureName") + ".html" );
		if (newhtmlfile.exists()){ newhtmlfile.delete(); }
		
		PrintWriter pw = new PrintWriter(new FileWriter(newhtmlfile));
		File htmlf = new File(getServletContext().getRealPath("/") + "data/" + request.getParameter("htmlFile"));
		BufferedReader in = new BufferedReader(new InputStreamReader(new FileInputStream(htmlf), "UTF16"));
		ArrayList<String> files = new ArrayList<String>();
		String str = in.readLine();
		String newstr ="";
		String imageStr ="";
		String fullimageStr ="";
		while (str != null) {
			str = str.replace("src = ","src=");
			str = str.replace("src= ","src=");
			str = str.replace("src =","src=");
			newstr = str;
			while (str.indexOf(".jpg") > -1 || str.indexOf(".gif") > -1 || str.indexOf(".bmp") > -1 || str.indexOf(".png") > -1 ) {		
				if (str.indexOf(".jpg") > -1) {
					fullimageStr = str.substring(str.lastIndexOf("=",str.indexOf(".jpg"))+1, str.indexOf(".jpg") + 4);
					fullimageStr = fullimageStr.replace("\"", "");
					imageStr = fullimageStr;
					if (files.indexOf(imageStr) == -1) { files.add(imageStr); }
					str = str.replace(imageStr, "");
					if (fullimageStr != "") {
						newstr = newstr.replace(fullimageStr, "\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
						newstr = newstr.replace("\"\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"\"", "\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
					}
					fullimageStr = "";
				}
				if (str.indexOf(".gif") > -1) {
					fullimageStr = str.substring(str.lastIndexOf("=",str.indexOf(".gif"))+1,str.indexOf(".gif") + 4);
					fullimageStr = fullimageStr.replace("\"", "");
					imageStr = fullimageStr;
					if (files.indexOf(imageStr) == -1) { files.add(imageStr); }
					str = str.replace(imageStr, "");
					if (fullimageStr != "") {
						newstr = newstr.replace(fullimageStr, "\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
						newstr = newstr.replace("\"\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"\"", "\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
					}
					fullimageStr = "";
				}
				if (str.indexOf(".bmp") > -1) {
					fullimageStr = str.substring(str.lastIndexOf("=", str.indexOf(".bmp"))+1,str.indexOf(".bmp") + 4);
					fullimageStr = fullimageStr.replace("\"", "");
					imageStr = fullimageStr;
					if (files.indexOf(imageStr) == -1) { files.add(imageStr); }
					str = str.replace(imageStr, "");
					if (fullimageStr != "") {
						newstr = newstr.replace(fullimageStr, "\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
						newstr = newstr.replace("\"\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"\"", "\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
					}
					fullimageStr = "";
				}
				if (str.indexOf(".png") > -1) {
					fullimageStr = str.substring(str.lastIndexOf("=",str.indexOf(".png"))+1,str.indexOf(".png") + 4);
					fullimageStr = fullimageStr.replace("\"", "");
					imageStr = fullimageStr;
					if (files.indexOf(imageStr) == -1) { files.add(imageStr); }
					str = str.replace(imageStr, "");
					if (fullimageStr != "") {
						newstr = newstr.replace(fullimageStr, "\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
						newstr = newstr.replace("\"\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"\"", "\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
					}
					fullimageStr = "";
				}
			}//while
			pw.println(newstr);
			str = in.readLine();
		}//while
		in.close();
		pw.close();
		htmlf.delete();
		
		//Copy image files to procedures directory	
		for (String imageFile : files) {
			File imagef = new File(getServletContext().getRealPath("/") + "data/" + imageFile);
			File newImagef = new File(getServletContext().getRealPath("/") + "docs/procedures/" + request.getParameter("procedureName") + "/" + imageFile);
			if (newImagef.exists()){newImagef.delete();}
			if (imagef.exists()) { imagef.renameTo(newImagef); }
		}
		
		//Add to database
		Driver sqlServerDriver = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
		Connection sqlServerConnection = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
		String query = "Select ID from LCTR_DAT_Procedures where Name = '" + request.getParameter("procedureName") + "'";
		PreparedStatement Statementq = sqlServerConnection.prepareStatement(query);
		ResultSet result = Statementq.executeQuery();
		if (!result.next()) {
			query = "insert into LCTR_DAT_Procedures (Name) values('" + request.getParameter("procedureName") + "')";
			PreparedStatement Statement = sqlServerConnection.prepareStatement(query);
			Statement.executeUpdate();
		}
		sqlServerConnection.close();
		out.println("<Dat><Status>Operation Successful!</Status></Dat>");
	}
	catch (Exception e) {
		out.println("<Dat><Status>" + e.getMessage() + "</Status></Dat>");
	}
%>
