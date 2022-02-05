package Tree;
import java.sql.ResultSet;
import java.sql.Statement;
import java.lang.*;
import java.sql.ResultSetMetaData;
import java.util.*;

public class DBQuery {
	private final boolean testMode;
	private java.sql.Connection  con;		    
	private final String portNumber = "1433";
	private final String databaseName;
	//private final String userName = "procauto";
	//private final String password = "sqlserver";
	private final String userName = "bsweb";
	private final String password = "ProcAuto!";
	
	public DBQuery(String db, boolean test){
		testMode = test;
		databaseName = db;
	}
	public ArrayList select(String queryStr){
		ArrayList rows = new ArrayList();
		try {
		    //String url = "jdbc:microsoft:sqlserver://";
			String serverName = "";
		    if (testMode){
		    	serverName= "BART:1143";
		    }
		    else {
		    	serverName= "NOCHOUSE:1433";		    	
		    }
		    
			Class.forName("com.microsoft.jdbc.sqlserver.SQLServerDriver");
		    con = java.sql.DriverManager.getConnection("jdbc:microsoft:sqlserver://" + serverName + ";DatabaseName=BSYS",userName,password);
            Statement s = con.createStatement();
            ResultSet rs = s.executeQuery(queryStr);
            ResultSetMetaData rsmd = rs.getMetaData();
            int numberOfColumns = rsmd.getColumnCount();
            //System.out.println(numberOfColumns);
            String temp = new String();
            if (rs != null){
            	while ( rs.next() )
            	{
            		ArrayList sl =  new ArrayList();
            		
            		for(int i = 1;i<=numberOfColumns;i++){
            			temp = rs.getString(i);
            			if (temp != null){
            				sl.add(temp);
            			}
            			else{
            				sl.add("");
            			}
            				
            			//System.out.println(rs.getString(i));
            		}
            		rows.add(sl);
                }
            	s.close();
            	con.close();
            }
            //System.out.println(rows.size());
            return rows;   
        }
        catch (Exception e) {
            System.out.println("Error: " + e);
            return rows;
        }
	}
	public boolean insert(String queryStr){
		return true;
	}
	public boolean update(String queryStr){
		return true;
	}
	public boolean delete(String queryStr){
		return true;
	}
}
