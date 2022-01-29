<?xml version="1.0" encoding="utf-8"?>
<%@ page contentType="text/xml;charset=ISO-8859-1" language="java" import="java.sql.*" import="java.io.*" import="java.util.*" errorPage="" %>
<%@ page import="edu.utahsbr.*" %>

<%
try{
	if (!request.getParameter("htmlFile").equals("") ){
	
	File d = new File(getServletContext().getRealPath("/") + "docs/procedures/" + request.getParameter("procedureName") + "/");
	
	if( d.exists() ) {
	  File[] delfiles = d.listFiles();
	  for(int i=0; i<delfiles.length; i++) {
		   delfiles[i].delete();
	  }
	  d.delete();
	  while (d.exists())
	  {}
	}
	
	d.mkdir();
	//Copy HTML file to procedures directory
	File newhtmlfile = new File(getServletContext().getRealPath("/") + "docs/procedures/" + request.getParameter("procedureName") + "/" + request.getParameter("procedureName") + ".html" );
		if (newhtmlfile.exists()){
			newhtmlfile.delete();
		}
		
	PrintWriter pw = new PrintWriter(new FileWriter(newhtmlfile));
	File htmlf = new File(getServletContext().getRealPath("/") + "data/" + request.getParameter("htmlFile"));
	//BufferedReader in = new BufferedReader( new FileReader(htmlf));
	BufferedReader in = new BufferedReader( new InputStreamReader( new FileInputStream(htmlf),"UTF16"));
	
	ArrayList<String> files = new ArrayList<String>();
	String str = in.readLine();
	
	String newstr ="";
	String imageStr ="";
	String fullimageStr ="";
	while (str != null ){
		str = str.replace("src = ","src=");
		str = str.replace("src= ","src=");
		str = str.replace("src =","src=");
		str = str.replace("<v:imagedata","<img");
		str = str.replace("</v:imagedata>","");
		newstr = str;
		while (str.indexOf(".jpg") > -1 || str.indexOf(".gif") > -1 || str.indexOf(".bmp") > -1 || str.indexOf(".png") > -1 ){		
			if (str.indexOf(".jpg") > -1){
				fullimageStr = str.substring( str.lastIndexOf("=",str.indexOf(".jpg"))+1,str.indexOf(".jpg") + 4);
				fullimageStr = fullimageStr.replace("\"","");
				imageStr = fullimageStr;
				if (files.indexOf(imageStr) == -1 ){ 
					files.add(imageStr);
				}
				str = str.replace(imageStr,"");
				if (fullimageStr != ""){
					newstr = newstr.replace(fullimageStr,"\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
					newstr = newstr.replace("\"\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"\"","\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
				}
				fullimageStr = "";
			}
			if (str.indexOf(".gif") > -1){
				fullimageStr = str.substring( str.lastIndexOf("=",str.indexOf(".gif"))+1,str.indexOf(".gif") + 4);
				fullimageStr = fullimageStr.replace("\"","");
				imageStr = fullimageStr;
				if (files.indexOf(imageStr) == -1 ){ 
					files.add(imageStr);
				}
				str = str.replace(imageStr,"");
				if (fullimageStr != ""){
					newstr = newstr.replace(fullimageStr,"\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
					newstr = newstr.replace("\"\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"\"","\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
				}
				fullimageStr = "";
			}
			if (str.indexOf(".bmp") > -1){
				fullimageStr = str.substring( str.lastIndexOf("=",str.indexOf(".bmp"))+1,str.indexOf(".bmp") + 4);
				fullimageStr = fullimageStr.replace("\"","");
				imageStr = fullimageStr;
				if (files.indexOf(imageStr) == -1 ){ 
					files.add(imageStr);
				}
				str = str.replace(imageStr,"");
				if (fullimageStr != ""){
					newstr = newstr.replace(fullimageStr,"\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
					newstr = newstr.replace("\"\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"\"","\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
				}
				fullimageStr = "";
			}
			if (str.indexOf(".png") > -1){
				fullimageStr = str.substring( str.lastIndexOf("=",str.indexOf(".png"))+1,str.indexOf(".png") + 4);
				fullimageStr = fullimageStr.replace("\"","");
				imageStr = fullimageStr;
				if (files.indexOf(imageStr) == -1 ){ 
					files.add(imageStr);
				}
				str = str.replace(imageStr,"");
				if (fullimageStr != ""){
					newstr = newstr.replace(fullimageStr,"\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
					newstr = newstr.replace("\"\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"\"","\"docs/procedures/" + request.getParameter("procedureName") + "/" + imageStr + "\"");
				}
				fullimageStr = "";
			}
		}
		
		//newstr = newstr.replace("<![if !vml]>", "<![if true]>");
		//newstr = newstr.replace("<!--[if gte vml 1]>", "<!--[if false]>");
		
		
		pw.println(newstr);
		str = in.readLine();
	}
	in.close();
	pw.close();
	
	htmlf.delete();
	
	//File testfile = new File(getServletContext().getRealPath("/") + "data/test.txt" );
	//PrintWriter pwt = new PrintWriter(new FileWriter(testfile));
	//Copy image files to procedures directory	
	//String myDirStr ="";
	for (int i = 0;i<files.size();i++){
		//myDirStr = ((String)files.get(i)).substring(0,((String)files.get(i)).indexOf("/"));
		//myDirStr = "blah";
		//pwt.println("test= " + myDirStr);
		File imagef = new File(getServletContext().getRealPath("/") + "data/" + (String)files.get(i));
		File newImagef = new File(getServletContext().getRealPath("/") + "docs/procedures/" + request.getParameter("procedureName") + "/" + (String)files.get(i));
		
		//newImagef.mkdirs();
		if (newImagef.exists()){newImagef.delete();}
		if (imagef.exists()){
			imagef.renameTo(newImagef);
		}
	}
	//pwt.println("final= " + myDirStr);
	
	/*
	File path = new File( getServletContext().getRealPath("/") + "data/" + myDirStr );
	if( path.exists() ) {
	  File[] delfiles = path.listFiles();
	  for(int i=0; i<delfiles.length; i++) {
		   delfiles[i].delete();
	  }
	  path.delete();
	}
	*/
	//pwt.close();
}


//Add to database
Driver DriverspGetTopic = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
Connection Conn = DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
String query = "Select ID from LCTR_DAT_Procedures where Name = '" + request.getParameter("procedureName") + "'";
PreparedStatement Statementq = Conn.prepareStatement(query);
ResultSet result = Statementq.executeQuery();
boolean resultNotEmpty = result.next();
if (!resultNotEmpty){
	String sql = "insert into LCTR_DAT_Procedures (Name,SearchKey) values('" + request.getParameter("procedureName") + "','" + request.getParameter("sKey") + "')";
	PreparedStatement Statement = Conn.prepareStatement(sql);
	Statement.executeUpdate();
	out.println("<Dat><Status>Operation Successful!</Status></Dat>");
}
else{
	String sql = "update LCTR_DAT_Procedures set SearchKey = '" + request.getParameter("sKey") + "', Name = '" + request.getParameter("procedureName") + "' where Name = '" + request.getParameter("procedureName") + "'";
	PreparedStatement Statement = Conn.prepareStatement(sql);
	Statement.executeUpdate();
	out.println("<Dat><Status>Operation Successful!</Status></Dat>");
}
Conn.close();
}
catch (Exception e){
	out.println("<Dat><Status>" + e.getMessage() + "</Status></Dat>");
}
//out.println("<Dat><Status>Operation Successful!</Status></Dat>");
%>

