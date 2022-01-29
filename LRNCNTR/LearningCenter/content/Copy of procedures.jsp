<%--
	The following script code parses a procedure's HTML code to find terms that it gets
	from a database, and makes links out of them, storing the final result in a JSP page.
	We're not using it for the Procedures page any more, but it may come in handy for
	something else, so it's being preserved here.
--%>
<%--
<%
	//Query the database for all known titles.
	Driver DriverspGetTopic = (Driver)Class.forName(BSYS_DRIVER).newInstance();
	Connection Conn = DriverManager.getConnection(BSYS_STRING,Conn_USERNAME,Conn_PASSWORD);
	String query = "SELECT DISTINCT title FROM SYSA_LST_Users GROUP BY title";
	Statement Statementq = Conn.createStatement(ResultSet.TYPE_SCROLL_INSENSITIVE, ResultSet.CONCUR_UPDATABLE);
	ResultSet result = Statementq.executeQuery(query);
	//The query will return NULL as the first result if any NULLs are found in the database (which there probably always will be), so we need to set the result pointer to first() instead of beforeFirst(). 
	result.first();

	//Make a copy of the existing procedures HTML file.
	File newhtmlfile = new File(application.getRealPath("/docs/procedures/" + request.getParameter("procedure") + "/" + request.getParameter("procedure") + "WithLinks.jsp"));
	//Start with a fresh copy if there's already a copy there.
	if (newhtmlfile.exists())
	{
	    newhtmlfile.delete();
	}
		
	PrintWriter printWriter = new PrintWriter(new FileWriter(newhtmlfile));
	//We need to have a taglib directive at the top of the page for the links to work.
	printWriter.println("<%@ taglib uri=\"http://java.sun.com/jsp/jstl/core\" prefix=\"c\" " + '%' + '>');
	
	File procedure = new File(application.getRealPath("/docs/procedures/" + request.getParameter("procedure") + "/" + request.getParameter("procedure") + ".html"));
	BufferedReader in = new BufferedReader( new FileReader(procedure));
	
	String str = in.readLine();
	
	//Go through each line of the document and see if it contains any of the titles.
	while (str != null )
	{
		while (result.next())
		{
			if (str.contains(result.getString("title")))
			{
				str = str.substring(0, str.indexOf(result.getString("title")))
						+ "<a href=\"<c:url value=\"/ElSea.jsp\"><c:param name=\"page\" value=\"staff\" /><c:param name=\"topic\" value=\"${param.topic}\" /><c:param name=\"procedure\" value=\"${param.procedure}\" /><c:param name=\"title\" value=\""
						+ result.getString("title")
						+ "\" /></c:url>\">"
						+ result.getString("title")
						+ "</a>"
						+ str.substring(str.indexOf(result.getString("title")) + result.getString("title").length());
			}
		}//while (result.next())
    result.first();
		printWriter.println(str);
		str = in.readLine();
	}//while (str != null )
	printWriter.close();
	Conn.close();
%>

<jsp:include page="/docs/procedures/${param.procedure}/${param.procedure}WithLinks.jsp" />
--%>

<jsp:include page="/docs/procedures/${param.procedure}/${param.procedure}.html" />
