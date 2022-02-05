<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ page contentType="text/html; charset=ISO-8859-1" language="java" errorPage=""%>
<%@ page import="java.io.*"%>
<%@ page import="java.sql.*"%>
<%@ page import="java.util.*"%>

<%
	if (request.getParameter("htmlFile") != "") {
		String topicDirectoryName = getServletContext().getRealPath("/") + "docs/staff/" + request.getParameter("topicName") + "/";
		//Make directory
		File topicDirectory = new File(topicDirectoryName);
		if (!topicDirectory.exists()) { topicDirectory.mkdir(); }
		
		//Copy HTML file to procedures directory
		File newHtmlFile = new File(topicDirectoryName + request.getParameter("topicName") + ".html");
		if (newHtmlFile.exists()) { newHtmlFile.delete(); }
		
		PrintWriter pw = new PrintWriter(new FileWriter(newHtmlFile));
		File tempHtmlFile = new File(getServletContext().getRealPath("/") + "data/" + request.getParameter("htmlFile"));
		BufferedReader in = new BufferedReader(new FileReader(tempHtmlFile));
		
		String str = in.readLine();
		String newstr = "";
		String imageStr = "";
		String fullimageStr = "";
		while (str != null) {
			newstr = str;
			while (str.contains(".jpg") || str.contains(".gif") || str.contains(".bmp") || str.contains(".png")) {
				if (str.contains(".jpg")) {
					fullimageStr = str.substring(str.lastIndexOf("=", str.indexOf(".jpg")) + 2, str.indexOf(".jpg") + 4);
					imageStr = fullimageStr.substring(fullimageStr.lastIndexOf("/") + 1);
					str = str.replace(imageStr, "");
				}
				if (str.contains(".gif")) {
					fullimageStr = str.substring(str.lastIndexOf("=", str.indexOf(".gif")) + 2, str.indexOf(".gif") + 4);
					imageStr = fullimageStr.substring(fullimageStr.lastIndexOf("/") + 1);
					str = str.replace(imageStr, "");
				}
				if (str.contains(".bmp")) {
					fullimageStr = str.substring(str.lastIndexOf("=", str.indexOf(".bmp")) + 2, str.indexOf(".bmp") + 4);
					imageStr = fullimageStr.substring(fullimageStr.lastIndexOf("/") + 1);
					str = str.replace(imageStr, "");
				}
				if (str.contains(".png")) {
					fullimageStr = str.substring(str.lastIndexOf("=", str.indexOf(".png")) + 2, str.indexOf(".png") + 4);
					imageStr = fullimageStr.substring(fullimageStr.lastIndexOf("/") + 1);
					str = str.replace(imageStr, "");
				}
				if (fullimageStr != "") {
					newstr = newstr.replace(fullimageStr, "docs/staff/" + imageStr);
				}
			}
			pw.println(newstr);
			str = in.readLine();
		}
		
		in.close();
		pw.close();
		tempHtmlFile.delete();
	}
%>
