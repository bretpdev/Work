<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ page contentType="text/html; charset=ISO-8859-1" language="java" import="java.sql.*" import="java.io.*" import="java.util.*" errorPage="" %>

<%
if (request.getParameter("htmlFile") != "") {
	//out.println("<html><body bgcolor='red'>");
	//Make directory
	File d = new File(getServletContext().getRealPath("/") + "docs/staff/" + request.getParameter("topicName") + "/");
	if (!d.exists()){d.mkdir();}
	
	//Copy HTML file to procedures directory
	File newhtmlfile = new File(getServletContext().getRealPath("/") + "docs/staff/" + request.getParameter("topicName") + "/" + request.getParameter("topicName") + ".html" );
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
				newstr = newstr.replace(fullimageStr,"docs/staff/" + imageStr);
			}
		}
		pw.println(newstr);
		str = in.readLine();
	}
	
	in.close();
	pw.close();
	htmlf.delete();
	
	
}

%>

