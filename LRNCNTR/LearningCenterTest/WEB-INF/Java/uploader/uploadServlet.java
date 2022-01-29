package uploader;

import java.io.*;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import com.oreilly.servlet.MultipartRequest;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.*;

/**
 * Servlet implementation class for Servlet: uploadServlet
 *
 */
 public class uploadServlet extends javax.servlet.http.HttpServlet implements javax.servlet.Servlet {
    static final long serialVersionUID = 1L;
    private String webTempPath;
	private java.sql.Connection  con;		    
	//private final String userName = "procauto";
	//private final String password = "sqlserver";
	private final String userName = "bsweb";
	private final String password = "ProcAuto!";
    
    public void init(){
    	webTempPath = getServletContext().getRealPath("/") + "data";
    }
    
	public uploadServlet() {
		super();
	}   	
	
	/* (non-Java-doc)
	 * @see javax.servlet.http.HttpServlet#doGet(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
		response.setContentType("text/html");
		java.io.PrintWriter out = response.getWriter();
		out.println("Invalid Call!");
	}  	
	
	/* (non-Java-doc)
	 * @see javax.servlet.http.HttpServlet#doPost(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
		response.setContentType("text/html");
		java.io.PrintWriter out = response.getWriter();
		//upload file
		MultipartRequest mpr = new MultipartRequest(request,webTempPath,5 * 1024 * 1024);
		Enumeration enum1 = mpr.getFileNames();
		//get file path on server
		String fname = "";
		fname = (String)mpr.getFilesystemName((String)enum1.nextElement());
		try{
			if (fname != null){
				copyFile("C:/Program Files/Apache Software Foundation/Tomcat 6.0/webapps/LearningCenter" + mpr.getParameter("test") + "/data/" + fname,"C:/Program Files/Apache Software Foundation/Tomcat 6.0/webapps/LearningCenter" + mpr.getParameter("test") + "/docs/" + mpr.getParameter("type") + "/" + fname);
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
		    
			Class.forName("com.microsoft.jdbc.sqlserver.SQLServerDriver");
		    con = java.sql.DriverManager.getConnection("jdbc:microsoft:sqlserver://" + serverName + ":" + portNumber + ";DatabaseName=BSYS",userName,password);
            Statement s = con.createStatement();
            try {
            	if (mpr.getParameter("action").equals("insert")){
            		s.executeUpdate("insert into LCTR_DAT_Docs (Type,Name,Path,SearchKey) values ('" + mpr.getParameter("type") + "','" + mpr.getParameter("docName") + "','" + fname + "','" + mpr.getParameter("docSearch") + "')");            		
            	}
            	else if (mpr.getParameter("action").equals("update")){
            		if (fname == null || fname.equals("")){
            			s.executeUpdate("update LCTR_DAT_Docs set SearchKey = '" + mpr.getParameter("docSearch") + "' where ID = " + mpr.getParameter("docID") + " and Type = '" + mpr.getParameter("type") + "'");
            		}
            		else{
                		s.executeUpdate("update LCTR_DAT_Docs set SearchKey = '" + mpr.getParameter("docSearch") + "', Path = '" + fname + "' where ID = " + mpr.getParameter("docID") + " and Type = '" + mpr.getParameter("type") + "'");            		
                	}
            	}
            	out.println("Operation Successful!");
            }
            catch (Exception e){
            	out.println("<div class='error'>There was an Error saving the data!</div>");
            	out.println(e.getMessage());

            }
            finally{
            	s.close();
                con.close();	
            }
            
		}
		catch (Exception e){
			out.println("There was an Error saving the data!");
			out.println(e.getMessage());
		}
		
		
	}   
	
	public static void copyFile(String origf, String newf) throws Exception {
	    FileInputStream fis  = new FileInputStream(new File(origf));
	    FileOutputStream fos = new FileOutputStream(new File(newf));
	    try {
	        byte[] buf = new byte[1024];
	        int i = 0;
	        while ((i = fis.read(buf)) != -1) {
	            fos.write(buf, 0, i);
	        }
	    } 
	    catch (Exception e) {
	        throw e;
	    }
	    finally {
	        if (fis != null) fis.close();
	        if (fos != null) fos.close();
	    }
	  }
}