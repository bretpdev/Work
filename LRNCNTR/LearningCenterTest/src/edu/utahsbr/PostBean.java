package edu.utahsbr;

import java.util.Map;
import java.util.Iterator;
import org.apache.commons.httpclient.HttpClient;
import org.apache.commons.httpclient.HttpStatus;
import org.apache.commons.httpclient.methods.PostMethod;
import org.apache.commons.httpclient.NameValuePair;
import org.apache.commons.httpclient.HttpException;

public class PostBean implements java.io.Serializable
{
	//serialVersionUID is needed when implementing the Serializable interface.
	//It was obtained by running the "serialver" tool and should never be changed.
	private static final long serialVersionUID = -6094967379898973983L;
    private Map<String, String[]> parameters;
    private String url;
    
    //Default constructor takes no action.
    public PostBean(){}
    
    public void setParameters(Map<String, String[]> param)
    {
        if (param != null)
        {
            parameters = param;
        }
    }//setParameters
    
    public Map<String, String[]> getParameters()
    {
        return parameters;
    }
    
    public void setUrl(String url)
    {
        if (url != null && !(url.equals("")))
        {
            this.url = url;
        }
    }//setUrl
    
    public String getUrl()
    {
        return url;
    }
    
    public String getPost() throws java.io.IOException,HttpException
    {
        if (url == null || url.equals("") || parameters == null)
        {
            throw new IllegalStateException("Invalid URL or parameters in PostBean.getPost method.");
        }
        
        String returnData = "";
        HttpClient httpClient = new HttpClient();
        PostMethod postMethod = new PostMethod(url);
        NameValuePair[] postData = getParams(parameters); //Convert the parameters map to a NameValuePair array.
        
        postMethod.setRequestBody(postData);
        httpClient.executeMethod(postMethod);
        
        //Check for "200 OK" HTTP Status Code.
        if (postMethod.getStatusCode() == HttpStatus.SC_OK)
        {
            returnData = postMethod.getResponseBodyAsString();
        }
        else
        {
            returnData = "The POST action raised an error: " + postMethod.getStatusLine();
        }
        
        //Release the connection used by the method.
        postMethod.releaseConnection();
        
        return returnData;
    }//getPost
    
    private NameValuePair[] getParams(Map<String, String[]> map)
    {
        NameValuePair[] pairs = new NameValuePair[map.size()];
        
        //Use an Iterator to put name/value pairs from the Map into the array.
        Iterator<Map.Entry<String, String[]>> iter = map.entrySet().iterator();
        int i = 0;
        
        while (iter.hasNext())
        {
            Map.Entry<String, String[]> me = (Map.Entry<String, String[]>)iter.next();
            //Map.Entry.getValue() return a String array.
            pairs[i] = new NameValuePair((String)me.getKey(), ((String[])me.getValue())[0]);
            i++;
        }
        
        return pairs;
    }//getParams
}
