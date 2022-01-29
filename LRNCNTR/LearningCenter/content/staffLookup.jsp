<%--
	This page is a go-between that takes a search parameter from the staff page
	and passes them on to UHEAA's intranet staff search page using the POST method.
	
	Note that this page is not yet used. Posting to the given address returns an UNAUTHORIZED response.
--%>

<jsp:useBean id="postBean" class="edu.utahsbr.PostBean" />
<jsp:setProperty name="postBean" property="parameters" value="<%= request.getParameterMap() %>" />
<jsp:setProperty name="postBean" property="url" value="http://uconnect.utahsbr.edu/search_submit.cfm?MenuID=4" />
<jsp:getProperty name="postBean" property="post" />
