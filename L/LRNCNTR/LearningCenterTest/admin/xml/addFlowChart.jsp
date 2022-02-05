<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" import="java.sql.*" import="java.io.*" import="java.util.*" errorPage="" %>
<%@ page import="edu.utahsbr.*" %>

<%

try {
if (request.getParameter("htmlFile") != "") {
	//Make directory
	File d = new File(getServletContext().getRealPath("/") + "docs/flowcharts/" + request.getParameter("type") + "/" + request.getParameter("topicName") + "/");
	if (!d.exists()){d.mkdir();}
	
	//Copy HTML file to flowcharts directory
	File newhtmlfile = new File(getServletContext().getRealPath("/") + "docs/flowcharts/" + request.getParameter("type") + "/" + request.getParameter("topicName") + "/" + request.getParameter("topicName") + ".html" );
		if (newhtmlfile.exists()){
			newhtmlfile.delete();
		}
		
	PrintWriter pw = new PrintWriter(new FileWriter(newhtmlfile));
	File htmlf = new File(getServletContext().getRealPath("/") + "data/" + request.getParameter("htmlFile"));
	BufferedReader in = new BufferedReader( new FileReader(htmlf));
	
	ArrayList<String> files = new ArrayList<String>();
	String str = in.readLine();
	String newstr ="";
	String imageStr ="";
	String fullimageStr ="";
	while (str != null ){
		newstr = str;
		while (str.contains(".jpg") || str.contains(".gif") || str.contains(".bmp") || str.contains(".png") ){
			if (str.contains(".jpg")){
				fullimageStr = str.substring( str.lastIndexOf("=",str.indexOf(".jpg"))+2,str.indexOf(".jpg") + 4);
				imageStr = fullimageStr.substring(fullimageStr.lastIndexOf("/")+1);
				files.add(imageStr);
				str = str.replace(imageStr,"");
			}
			if (str.contains(".gif")){
				fullimageStr = str.substring( str.lastIndexOf("=",str.indexOf(".gif"))+2,str.indexOf(".gif") + 4);
				imageStr = fullimageStr.substring(fullimageStr.lastIndexOf("/")+1);
				files.add(imageStr);
				str = str.replace(imageStr,"");
			}
			if (str.contains(".gif")){
				fullimageStr = str.substring( str.lastIndexOf("=",str.indexOf(".bmp"))+2,str.indexOf(".bmp") + 4);
				imageStr = fullimageStr.substring(fullimageStr.lastIndexOf("/")+1);
				files.add(imageStr);
				str = str.replace(imageStr,"");
			}
			if (str.contains(".gif")){
				fullimageStr = str.substring( str.lastIndexOf("=",str.indexOf(".png"))+2,str.indexOf(".png") + 4);
				imageStr = fullimageStr.substring(fullimageStr.lastIndexOf("/")+1);
				files.add(imageStr);
				str = str.replace(imageStr,"");
			}
			if (fullimageStr != ""){
				newstr = newstr.replace(fullimageStr,"docs/flowcharts/" + request.getParameter("type") + "/" + request.getParameter("topicName") + "/" + imageStr);
			}
		}
		pw.println(newstr);
		str = in.readLine();
	}
	in.close();
	pw.close();
	
	//Copy image files to procedures directory	
	for (int i = 0;i<files.size();i++){
		File imagef = new File(getServletContext().getRealPath("/") + "data/" + (String)files.get(i));
		File newImagef = new File(getServletContext().getRealPath("/") + "docs/flowcharts/" + request.getParameter("type") + "/" + request.getParameter("topicName") + "/" + (String)files.get(i));
		if (newImagef.exists()){newImagef.delete();}
		if (imagef.exists()){
			imagef.renameTo(newImagef);
		}
		
	}
	htmlf.delete();
}
//Add to database
Driver DriverspGetTopic = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
Connection Conn = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
//Delete
String query = "delete from LCTR_DAT_FlowChart where ID = '" + request.getParameter("id") + "' and Type = '" + request.getParameter("type") + "'";
PreparedStatement Statementq = Conn.prepareStatement(query);
Statementq.executeUpdate();
//Insert
String sql = "insert into LCTR_DAT_FlowChart (ID,Type,Description) values(" + request.getParameter("id") + ",'" + request.getParameter("type") + "','" + request.getParameter("description") + "')";
PreparedStatement Statement = Conn.prepareStatement(sql);
Statement.executeUpdate();
Conn.close();
out.println("<Dat><Status>Operation Successful!</Status></Dat>");
}
catch (Exception e){
	out.println("<Dat><Status>" + e.getMessage() + "</Status></Dat>");
}

%>

