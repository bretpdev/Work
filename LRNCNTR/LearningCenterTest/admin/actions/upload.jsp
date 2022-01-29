<%@ page import="java.io.*" %>
<%@ page import="java.sql.Driver" %>
<%@ page import="java.sql.Statement" %>
<%@ page import="java.util.*" %>
<%@ page import="com.oreilly.servlet.MultipartRequest" %>
<%@ page import="edu.utahsbr.*" %>

<% 
	String webTempPath;
	long serialVersionUID = 1L;
	java.sql.Connection  con;
	webTempPath = getServletContext().getRealPath("/") + "data";
	MultipartRequest mpr = new MultipartRequest(request,webTempPath,5 * 1024 * 1024);
	Enumeration<?> enum1 = mpr.getFileNames();
	String fname = (String)mpr.getFilesystemName((String)enum1.nextElement());
		try {
			if (mpr.getParameter("type").equals("staff")) {
			//Upload Staff HTML with no pictures
				if (mpr.getParameter("action").equals("update")) {
					//delete old file
					File f = new File(getServletContext().getRealPath("/") + "docs/" + mpr.getParameter("type") + "/" + mpr.getParameter("docName") + ".html");
					if (f.exists()) {
						f.delete();
					}
					//copy new file to correct location
					File origf = new File(getServletContext().getRealPath("/") + "data/" + fname);
					origf.renameTo(new File(getServletContext().getRealPath("/") + "docs/" + mpr.getParameter("type") + "/",mpr.getParameter("docName") + ".html"));
				}
				else if (mpr.getParameter("action").equals("delete")) {
					//delete current file
					File f = new File(getServletContext().getRealPath("/") + "docs/" + mpr.getParameter("type") + "/" + mpr.getParameter("docName") + ".html");
					if (f.exists()) {
						f.delete();
					}
				}
			}
			else {
			//Upload Letters or Training Docs
				if (fname != null) {
					File origf = new File(getServletContext().getRealPath("/") + "data/" + fname);
					origf.renameTo(new File(getServletContext().getRealPath("/") + "docs/" + mpr.getParameter("type") + "/",fname));
					File del = new File(getServletContext().getRealPath("/") + "data/" + fname);
					if (del.exists()) {
						del.delete();
					}
				}
				String serverName;
				String portNumber;
				String url = "jdbc:microsoft:sqlserver://";
				if (mpr.getParameter("test").equals("Test")){
					serverName= "BART";
					portNumber = "1143";
				}
				else {
					serverName= "NOCHOUSE";
					portNumber = "1433";
				}
				
				Driver driverUser = (Driver)Class.forName(Globals.bsysDriver()).newInstance();
				con = java.sql.DriverManager.getConnection(Globals.bsysConnectionString(), Globals.databaseUser(), Globals.databasePassword());
				Statement s = con.createStatement();
				try {
					if (mpr.getParameter("action").equals("insert")) {
						s.executeUpdate("INSERT INTO LCTR_DAT_Docs (Type, Name, Path, SearchKey) VALUES ('" + mpr.getParameter("type") + "','" + mpr.getParameter("docName") + "','" + fname + "','" + mpr.getParameter("docSearch") + "')");            		
					}
					else if (mpr.getParameter("action").equals("update")) {
						if (fname == null || fname.equals("")){
							s.executeUpdate("UPDATE LCTR_DAT_Docs SET SearchKey = '" + mpr.getParameter("docSearch") + "' WHERE ID = " + mpr.getParameter("docID") + " AND Type = '" + mpr.getParameter("type") + "'");
						}
						else {
							s.executeUpdate("UPDATE LCTR_DAT_Docs SET SearchKey = '" + mpr.getParameter("docSearch") + "', Path = '" + fname + "' WHERE ID = " + mpr.getParameter("docID") + " AND Type = '" + mpr.getParameter("type") + "'");
						}
					}
					else if (mpr.getParameter("action").equals("delete")) {
						s.executeUpdate("DELETE FROM LCTR_DAT_Docs WHERE ID = " + mpr.getParameter("docID") + " AND Type = '" + mpr.getParameter("type") + "'");
						File oldF = new File(getServletContext().getRealPath("/") + "docs/" + mpr.getParameter("type") + "/" + mpr.getParameter("docPath"));
						if (oldF.exists()) {
							oldF.delete();
						}
					}
				}
				catch (Exception e) {
					out.println("There was an Error saving the data!");
					out.println(e.getMessage());
				}
				finally {
					s.close();
					con.close();	
				}
			}
		}
		catch (Exception e) {
			out.println("There was an Error saving the data!");
			out.println(e.getMessage());
		}
		out.println("Operation Successful!");
	%>