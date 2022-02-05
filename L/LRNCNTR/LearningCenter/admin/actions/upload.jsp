<%@ page import="java.io.*" %>
<%@ page import="java.util.*" %>
<%@ page import="java.sql.*" %>
<%@ page import="com.oreilly.servlet.MultipartRequest" %>
<%@ page import="edu.utahsbr.*" %>

<% 
	String webTempPath = this.getServletContext().getRealPath("/") + "data";
	MultipartRequest mpr = new MultipartRequest(request, webTempPath, 5 * 1024 * 1024);
	String dataDirectory = webTempPath + "/";
	String docTypeDirectory = this.getServletContext().getRealPath("/") + "docs/" + mpr.getParameter("type") + "/";
	Enumeration<String> uploadedFiles = mpr.getFileNames();
	String newFileName = mpr.getFilesystemName(uploadedFiles.nextElement());
	try {
		if (mpr.getParameter("type").equals("staff")) {
			//Upload Staff HTML with no pictures
			//delete current file
			String htmlFileName = mpr.getParameter("docName") + ".html";
			File oldFile = new File(docTypeDirectory + htmlFileName);
			if (oldFile.exists()) { oldFile.delete(); }
			if (mpr.getParameter("action").equals("update")) {
				//copy new file to correct location
				new File(dataDirectory + newFileName).renameTo(new File(docTypeDirectory, htmlFileName));
			}
			out.println("Operation Successful!");
		}
		else {
			//Upload Letters or Training Docs
			if (newFileName != null) {
				new File(dataDirectory + newFileName).renameTo(new File(docTypeDirectory, newFileName));
				File tempFile = new File(dataDirectory + newFileName);
				if (tempFile.exists()) { tempFile.delete(); }
			}
			
			Driver sqlServerDriver = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
			Connection sqlServerConnection = java.sql.DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
			Statement statement = sqlServerConnection.createStatement();
			try {
				if (mpr.getParameter("action").equals("insert")) {
					statement.executeUpdate("insert into LCTR_DAT_Docs (Type, Name, Path, SearchKey) values ('" + mpr.getParameter("type") + "', '" + mpr.getParameter("docName") + "', '" + newFileName + "', '" + mpr.getParameter("docSearch") + "')");            		
				}
				else if (mpr.getParameter("action").equals("update")) {
					if (newFileName == null || newFileName.equals("")) {
						statement.executeUpdate("update LCTR_DAT_Docs set SearchKey = '" + mpr.getParameter("docSearch") + "' where ID = " + mpr.getParameter("docID") + " and Type = '" + mpr.getParameter("type") + "'");
					}
					else {
						statement.executeUpdate("update LCTR_DAT_Docs set SearchKey = '" + mpr.getParameter("docSearch") + "', Path = '" + newFileName + "' where ID = " + mpr.getParameter("docID") + " and Type = '" + mpr.getParameter("type") + "'");
					}
				}
				else if (mpr.getParameter("action").equals("delete")) {
					statement.executeUpdate("delete from LCTR_DAT_Docs where ID = " + mpr.getParameter("docID") + " and Type = '" + mpr.getParameter("type") + "'");
					File oldFile = new File(docTypeDirectory + mpr.getParameter("docPath"));
					if (oldFile.exists()) { oldFile.delete(); }
				}
				out.println("Operation Successful!");
			}
			catch (Exception e) {
				out.println("There was an Error saving the data!<br />");
				out.println(e.getStackTrace().toString().replaceAll("\n", "<br />"));
			}
			finally {
				statement.close();
				sqlServerConnection.close();	
			}
		}
	}
	catch (Exception e) {
		out.println("There was an Error saving the data!<br />");
		out.println(e.getMessage() + "<br />");
		out.println(e.toString().replaceAll("\n", "<br />"));
	}
%>