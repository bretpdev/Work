package edu.utahsbr;

public class Globals {
	private static final boolean TEST_MODE = false;
	
	/**
	 * The data context for test mode is the same as that for live mode,
	 * with the exception that it has the word "Test" appended to it.
	 * Therefore, any code that refers to the data context by name
	 * should append it with a call to testString() to add the word
	 * "Test" when necessary (as determined by the TEST_MODE constant).
	 * @return
	 */
	public static String testString() {
		return (TEST_MODE ? "Test" : "");
	}
	
	public static String bsysConnectionString() {
		String testString = "jdbc:sqlserver://BART:1433;DatabaseName=BSYS";
		String liveString = "jdbc:sqlserver://NOCHOUSE:1433;DatabaseName=BSYS";
		return (TEST_MODE ? testString : liveString);
	}
	
	public static String bsysDriver() {
		return "com.microsoft.sqlserver.jdbc.SQLServerDriver";
	}
	
	public static String cookieName() {
		String name = "userLoginCookie";
		return (TEST_MODE ? name + "Test" : name);
	}
	
	public static String databaseUser() {
		return "bsweb";
	}
	
	public static String databasePassword() {
		return "ProcAuto!";
	}
}
