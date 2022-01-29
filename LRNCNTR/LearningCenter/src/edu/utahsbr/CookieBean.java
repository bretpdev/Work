package edu.utahsbr;

import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.Cookie;

public class CookieBean
{
    private Cookie cookie = null;
    
    public CookieBean(){}
    
    //Set the cookie name.
    public void setName(String name)
    {
        if (name == null || (name.equals("")))
        {
            throw new IllegalArgumentException("Invalid cookie name set in: " + getClass().getName());
        }
        cookie = new Cookie(name, "" + new java.util.Date().getTime());
    }//setName
    
    //Set the cookie value.
    public void setValue(String value)
    {
        if (value == null || (value.equals("")))
        {
            throw new IllegalArgumentException("Invalid cookie value set in: " + getClass().getName());
        }
        if (cookie != null)
        {
            cookie.setValue(value);
        }
    }//setValue
    
    public void setMaxAge(int maxAge)
    {
        if (cookie != null)
        {
            cookie.setMaxAge(maxAge);
        }
    }//setMaxAge
    
    public void setPath(String path)
    {
        if (path == null || (path.equals("")))
        {
            throw new IllegalArgumentException("Invalid cookie path set in: " + getClass().getName());
        }
        if (cookie != null)
        {
            cookie.setPath(path);
        }
    }//setPath
    
    public void setCookieHeader(HttpServletResponse response)
    {
        if (response == null)
        {
            throw new IllegalArgumentException("Invalid HttpServletResponse set in: " + getClass().getName());
        }
        if (cookie != null)
        {
            response.addCookie(cookie);
        }
    }//setCookieHeader
    
    public String getName()
    {
        if (cookie != null)
        {
            return cookie.getName();
        }
        else
        {
            return "unavailable";
        }
    }//getName
    
    public String getValue()
    {
        if (cookie != null)
        {
            return cookie.getValue();
        }
        else
        {
            return "unavailable";
        }
    }//getValue
    
    public String getPath()
    {
        if (cookie != null)
        {
            return cookie.getPath();
        }
        else
        {
            return "unavailable";
        }
    }//getPath
}
