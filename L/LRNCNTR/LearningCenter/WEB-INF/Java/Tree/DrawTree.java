package Tree;
import java.awt.*;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.image.BufferedImage;
import java.io.*;
import java.io.IOException;
import java.util.ArrayList;
import javax.imageio.ImageIO;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.swing.BorderFactory;
import javax.swing.JButton;
import javax.swing.JPanel;

/**
 * Servlet implementation class for Servlet: DrawTree
 *
 */
 public class DrawTree extends javax.servlet.http.HttpServlet implements javax.servlet.Servlet {
   static final long serialVersionUID = 1L;
   private static int boxWidth = 150;
	private static int boxHeight = 20;
	private static int baseX = 0;
	private static int baseY = 0;
	private static int PWidth = 5000;
	private static int PHeight = 400;
	private static ArrayList BUList;
	private static String fontFamily = "Comic Sans MS";
	private static Font myFont = new Font(fontFamily,Font.BOLD,9);
	private static int columSpace = 10;
	private static int rowSpace = 30;
	private static String imageMap = "" ;
   
    /* (non-Java-doc)
	 * @see javax.servlet.http.HttpServlet#HttpServlet()
	 */
	public DrawTree() {
		super();
	}   	
	
	/* (non-Java-doc)
	 * @see javax.servlet.http.HttpServlet#doGet(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
		response.setContentType("text/html");
		PrintWriter out = response.getWriter();
		String docType = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 " + "Transitional//EN\">\n";
		
		PWidth = Integer.parseInt(request.getParameter("width"));
		PHeight = Integer.parseInt(request.getParameter("height"));
		boxWidth = Integer.parseInt(request.getParameter("bwidth"));
		boxHeight = Integer.parseInt(request.getParameter("bheight"));
		
		String showBU = "function showBU(BU,Manager){" + 
		"document.getElementById('BU').innerHTML = '<h2>' + BU + '</h2>';" +
		"document.getElementById('BU2').innerHTML = '<h2>' + BU + ' Members</h2>';" + 
		"document.getElementById('Manager').innerHTML = Manager;}";
		
		String loadBU = "function loadBU(BU){" +
		"try" +
		"{ajaxReq = new ActiveXObject('Msxml2.XMLHTTP');}" +
		"catch (e)" +
		"{ajaxReq = new ActiveXObject('Microsoft.XMLHTTP'); }" +
		"ajaxReq.onreadystatechange = HandlesOnReady4selectBU;" +
		"myIdentifier=Math.round(Math.random()*10000);" +
		"ajaxReq.open('GET','content/BUTreeXML.jsp?BU=' + BU,true);" +
		"ajaxReq.send(null);}";
		
		String HandlesOnReady4selectBU = "function HandlesOnReady4selectBU(){var xmlDoc; if (ajaxReq.readyState == 4){if (ajaxReq.status == 200){	xmlDoc = ajaxReq.responseXML.documentElement; if (xmlDoc.getElementsByTagName('eName')[0] == null){i = 0;}else{	document.getElementById('BUpop').innerHTML = ''; for(i=0; i< xmlDoc.getElementsByTagName('eName').length; i++){ document.getElementById('BUpop').innerHTML += xmlDoc.getElementsByTagName('eName')[i].childNodes[0].nodeValue + '<br>';	}}}else{alert('The following error occured on the server ' + ajaxReq.status + '.');}}}";
		
		//String header = docType + "<html><head><title>Business Unit Tree</title>" + 
		//"<script language='javascript'>var ajaxReq; " + 
		String header = "<script language='javascript'>var ajaxReq; " +
		showBU +
		loadBU +
		HandlesOnReady4selectBU +
		"</script>" +
		"</head>";
		
		String div = "<div style='width:" + PWidth + "' id='BU' align='center'><h2>Business Unit</h2></div>" +
			"<div style='width:" + PWidth + "' id='Manager' align='center'>Manager</div>";
		
		String BUpop = "<div style='width:" + PWidth + "' id='BU2' align='center'><h2>Business Unit</h2></div>" +
				"<div style='width:" + PWidth + "' id='BUpop' align='center'>Members</div>";
		
		imageMap = "<map name='Map' id='Map'>";
		
		// Create a buffered image in which to draw
        BufferedImage image = new BufferedImage(PWidth, PHeight, BufferedImage.BITMASK);
        // Create a graphics contents on the buffered image
        Graphics2D g2 = image.createGraphics();
        g2.setComposite(AlphaComposite.Src);
        g2.setColor(new Color(150,150,150,0));
        g2.fillRect(0, 0, image.getWidth(), image.getHeight());
        createTree(g2);
        
        imageMap = imageMap + "</map>";
        g2.dispose();
        try {
          ImageIO.write(image, "png", new File(getServletContext().getRealPath("/") + "images/tree.png"));
          out.println(header + "<div align='left'>" + div + "<img src='images/tree.png' border='0' usemap='#Map'/> " + imageMap + BUpop + "</div>");
        }
          catch(IOException ioe) {
          out.println(docType + " Error: " + ioe.getMessage() + " ");
        }
        
	}  	
	
	/* (non-Java-doc)
	 * @see javax.servlet.http.HttpServlet#doPost(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
	} 
	private void createTree(Graphics2D g2){
		ArrayList rows;
		rows = new ArrayList();
		DBQuery dbq = new DBQuery("BSYS",true);
		rows = dbq.select("spGENRBusinessUnitOrder");
		BUList = new ArrayList();
		convertListToBUs(rows);
		getBU("Executive Director of UHEAA").paint(g2,0);
		
	}
	
	private static void convertListToBUs(ArrayList rows){
		for (int x = 0;x<rows.size();x++){
			//System.out.println(((ArrayList) rows.get(x)).get(0).toString() + " " );
			//Create each BU and add to ListArray
			if (((ArrayList) rows.get(x)).get(3).toString() != ""){ 
				BusinessUnit bu = new BusinessUnit(((ArrayList) rows.get(x)).get(0).toString(),((ArrayList) rows.get(x)).get(3).toString(),((ArrayList) rows.get(x)).get(1).toString(),((ArrayList) rows.get(x)).get(4).toString());
				BUList.add(bu);
			}
			else{
				BusinessUnit bu = new BusinessUnit(((ArrayList) rows.get(x)).get(0).toString(),"None",((ArrayList) rows.get(x)).get(1).toString(),((ArrayList) rows.get(x)).get(4).toString());
				BUList.add(bu);
			}
			
		}
		for (int x2 = 0;x2<BUList.size();x2++){
			//set the parent for each BU
			BusinessUnit tBU = ((BusinessUnit) BUList.get(x2));
			BusinessUnit pBU = getBU(getParentName(rows,tBU.getBU()));
			tBU.setParent(pBU);
			if (pBU != null){
				pBU.addChildCnt();
				pBU.addChild(tBU);
			}
		}
	}
	
	private static String getParentName(ArrayList rows,String Name){
		String parent = "";
		for (int x = 0;x<rows.size();x++){
			if (((ArrayList) rows.get(x)).get(0).toString().equals(Name)){
				parent = ((ArrayList) rows.get(x)).get(2).toString();
			}
		}
		return parent;
	}
	private static BusinessUnit getBU(String name){
		for (int x2 = 0;x2<BUList.size();x2++){
			if (((BusinessUnit) BUList.get(x2)).getBU().equals(name)){
				return ((BusinessUnit) BUList.get(x2));
			}
		}
		return null;
	}
	
	public static class BusinessUnit {
		private String BU;
		private String Manager;
		private BusinessUnit Parent;
		private String Type;
		private String Abbreviation;
		private int children;
		private ArrayList ChildList;
		private int x;
		private int y;
		
 
		public BusinessUnit(String aBU, String aManager,String aType, String aAbbreviation) {
			BU = aBU ;
			Manager = aManager;
			Type = aType;
			children = 0;
			Abbreviation = aAbbreviation;
			ChildList = new ArrayList();
		}
		public void setParent(BusinessUnit parent){
			Parent = parent;
		}
		public String getBU() {
			return BU;
		}
 
		public String getManager() {
			return Manager;
		}
 
		public BusinessUnit getParent() {
			return Parent;
		}
		
		public String getType(){
			return Type;
		}
		
		public int getChildrenWidth(){
			return children;
		}
		
		public void addChildCnt(){
			children += 1;
			if (children > 1 && Parent != null){
				Parent.addChildCnt();
			}
		}
		
		public void addChild(BusinessUnit c){
			ChildList.add(c);
		}
		
		public void paint(Graphics g2,int cNum){

			g2.setFont(myFont);
			if (Parent == null){
				x = baseX + (children * (boxWidth + columSpace) / 2 + (boxWidth + columSpace) / 2);
				y = baseY + rowSpace;
				
				g2.setColor(Color.cyan);
				g2.fillRect(x, y, boxWidth, boxHeight);
				g2.setColor(Color.black);
				g2.drawRect(x, y, boxWidth, boxHeight);
				g2.setColor(Color.black);
				g2.drawString(Abbreviation, x+2, y+14);
				
				imageMap = imageMap + "<area shape='rect' coords='" + x + "," + y + "," + (x + boxWidth) + "," + (y + boxHeight) + "' href='#' onmouseover='showBU(\"" + BU + "\",\"" + Manager + "\"); loadBU(\"" + BU + "\")' />";
				
				for (int i=0;i<ChildList.size();i++){
					((BusinessUnit) ChildList.get(i)).paint(g2,i);
				}
			}
			else{
				if (cNum == 0){//child number 
					if (Parent.getChildren().size() > 1){
						x = Parent.getX() - (Parent.getChildrenWidth() * (boxWidth + columSpace) / 2) + (children * (boxWidth + columSpace) / 2 );
					}
					else{
						x = Parent.getX();
					}
				}
				else{
					x = lastChildsRightMost((BusinessUnit)Parent.getChildren().get(cNum-1)) + boxWidth + columSpace + (children * (boxWidth + columSpace) / 2);
				}
				y = Parent.getY() + boxHeight + rowSpace;
				
				g2.setColor(Color.cyan);
				g2.fillRect(x, y, boxWidth, boxHeight);
				g2.setColor(Color.black);
				g2.drawRect(x, y, boxWidth, boxHeight);
				g2.setColor(Color.black);
				g2.drawString(Abbreviation, x+2, y+14);
				
				//imageMap = imageMap + "<area shape='rect' coords='" + x + "," + y + "," + (x + boxWidth) + "," + (y + boxHeight) + "' href='#' onmouseover='showBU(\"" + BU + "\",\"" + Manager + "\")' />";
				//imageMap = imageMap + "<area shape='rect' coords='" + x + "," + y + "," + (x + boxWidth) + "," + (y + boxHeight) + "' href='#' onmouseover='showBU(\"" + BU + "\",\"" + Manager + "\")' onclick='loadBU(\"" + BU + "\")' />";
				imageMap = imageMap + "<area shape='rect' coords='" + x + "," + y + "," + (x + boxWidth) + "," + (y + boxHeight) + "' href='#' onmouseover='showBU(\"" + BU + "\",\"" + Manager + "\"); loadBU(\"" + BU + "\")' />";
				
				g2.drawLine(Parent.getX() + (boxWidth / 2), Parent.getY() + boxHeight, x + (boxWidth / 2), y);
				for (int i=0;i<ChildList.size();i++){
					((BusinessUnit) ChildList.get(i)).paint(g2,i);
				}
			}
		}
		
		public int lastChildsRightMost(BusinessUnit s){
			int ll;
			if (s.getChildren().size() == 0){
				ll = s.getX();
			}
			else{
				ll = lastChildsRightMost((BusinessUnit)s.getChildren().get(s.getChildren().size()-1));
			}
			return ll;
		}
		
		public void setX(int ix){
			x = ix;
		}
		public void setY(int iy){
			x = iy;
		}
		public int getX(){
			return x;
		}
		public int getY(){
			return y;
		}
		
		public ArrayList getChildren(){
			return ChildList;
		}
	}
}