//open sitecollection open subsite
using (SPSite SyngentaSite = new SPSite("http://vprcsym6mhq2-02:6789/"))
{
	using (SPWeb web = SyngentaSite.OpenWeb())
	{
		//get list
		SPList SoWlist = web.Lists["SoW"];
		//get item according to item ID
        SPListItem currentListItems = SoWlist.GetItemById(SoW_List_ID);
		//check field is exist in item
		if (currentListItems["Title"] != null)
		{
			TxtSowName.Text = currentListItems["Title"].ToString();
		}
	}
}

//collect all look up values from other list
protected void BindLookupList(string siteURL, string listName, string fieldName, DropDownList ddlDropDown)
{
	Dictionary<int, string> fieldList = new Dictionary<int, string>();
	using (SPSite oSite = new SPSite(siteURL))
	{
		using (SPWeb oWeb = oSite.OpenWeb())
		{
			foreach (SPList oList in oWeb.Lists)
			{
				if (oList.Title == listName)
				{
					SPListItemCollection itemCollection = oList.Items;
					int i = 0;
					foreach (SPListItem item in itemCollection)
					{
						string strTitle = item[fieldName].ToString();
						fieldList.Add(i, strTitle);
						i++;
					}
				}
			}
		}
	}
	ddlDropDown.DataSource = fieldList;
	ddlDropDown.DataValueField = "Key";
	ddlDropDown.DataTextField = "Value";
	ddlDropDown.DataBind();
}

//parse Dropdownlist data Dictionary
public static void SelectDDLDefault_Dictionary(DropDownList ddl, string defaultValue)
{
	Dictionary<int, string> sCol = (Dictionary<int, string>)ddl.DataSource;
	foreach (KeyValuePair<int, string> pair in sCol)
	{
		if (pair.Value == defaultValue)
		{
			ddl.SelectedIndex = pair.Key;
		}
	}
}

//Query choices value in Field
public static void BindFieldChoice(string siteColURL, string subSite, string listURL, string fieldName, DropDownList rdl)
{
	using (SPSite site = new SPSite(siteColURL))
	{
		using (SPWeb web = site.OpenWeb(subSite))
		{
			SPList currentList = web.GetList(listURL);
			SPFieldChoice SoWStatus = (SPFieldChoice)currentList.Fields[fieldName];
			rdl.DataSource = SoWStatus.Choices;
			rdl.DataBind();
		}
	}
}

//parse Dropdownlist date Enumerator
public static void SelectDDLDefault(DropDownList ddl, string defaultValue)
{
	StringCollection sCol = (StringCollection)ddl.DataSource;
	int i = 0;
	StringEnumerator myEnumerator = sCol.GetEnumerator();
	while (myEnumerator.MoveNext())
	{
		if (myEnumerator.Current.ToString() == defaultValue)
		{
			ddl.SelectedIndex = i;
		}
		i++;
	}

}

//Query data from xml
public static string GetInternalName(string listName ,string displayName)
{
	XmlDocument doc = new XmlDocument();
	doc.Load(SiteConfiguration.layoutsPath + @"\SyngentaDashboard\ListFieldMapping.xml");
	XmlNode nodeList;
	XmlNode root = doc.DocumentElement;
	nodeList = root.SelectSingleNode("//Lists/List[@name='" + listName + "']/Fields/Field[@DisplayName='" + displayName + "']");
	if (nodeList.Attributes["Name"] != null)
	{
		return nodeList.Attributes["Name"].Value;
	}
	else
	{
		return "";
	}
}

//Query item id use Foreach
public static int get_ID(SPWeb web, string listURL, string fieldName, string itemname)
{
	int id = 0;
	SPList sharedDocumentList = web.GetList(listURL);
	SPListItemCollection listItems = sharedDocumentList.Items;
	foreach (SPListItem item in listItems)
	{
		if (item[fieldName].ToString() == itemname.ToString())
		{
			id = item.ID;
			break;
		}
	}
	return (id);
}

//Query item id use SPQuery
public bool IsContain(string projectCodeName)
{
	using (SPSite site = new SPSite(SiteConfiguration.webCollection))
	{
		using (SPWeb web = site.OpenWeb(SiteConfiguration.webSite))
		{
			// Build a query.
			SPQuery query = new SPQuery();
			query.Query = string.Concat(
						   "<Where><Eq>",
							  "<FieldRef Name='" + "Project_x0020_Code" + "'/>",
							  "<Value Type='Lookup'>" + projectCodeName + "</Value>",
						   "</Eq></Where>");
			query.ViewFieldsOnly = true; // Fetch only the data that we need.

			// Get data from a list.
			SPList list = web.GetList(SiteConfiguration.listConfirmationURL);
			SPListItemCollection items = list.GetItems(query);
			foreach (SPListItem item in items)
			{
				return true;
			}
		}
	}

	return false;
}

//update lookup field
newItem["SoW"] = new SPFieldLookupValue(idItem, sow);
newItem.update();

//Looup Reference
class LookupReference
{
	public int id;
	public string value;
	public LookupReference(string lookupValue)
		: base()
	{
		if (lookupValue != "")
		{
			this.id = Convert.ToInt16(lookupValue.Substring(0, lookupValue.IndexOf('#') - 1));
			this.value = lookupValue.Substring(lookupValue.IndexOf('#') + 1);
		}
	}
}

//Connection Excel

//Jet OLE DB or ACE(Access Engine) run this X86
public static void ConnectExcel()
{
	DataTable tblEXCEL = new DataTable();
	using (OleDbConnection conn = new OleDbConnection())
	{
		string path = SiteConfiguration.filePath;
		string excelPATH = path;
		string fileExtension = ".xlsx";
		if (fileExtension == ".xls")
		conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelPATH + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
		if (fileExtension == ".xlsx")
		conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelPATH + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
		using (OleDbCommand comm = new OleDbCommand())
		{
			comm.CommandText = "Select * from [" + "Sheet1" + "$]";
			comm.Connection = conn;
			using (OleDbDataAdapter oda = new OleDbDataAdapter())
			{
				oda.SelectCommand = comm;
				oda.Fill(tblEXCEL);
			}
		}
		
	}
}

//create parent folder and sub folder
SPListItem folder = list.AddItem(list.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder, "Sales Documents");
folder.Update();
SPListItem SubFolder = list.AddItem(folder.Folder.ServerRelativeUrl , SPFileSystemObjectType.Folder, "APAC Sales Docs");
SubFolder.Update();  


//ClientObjectModel Query and delete perticular item
ClientContext context = new ClientContext(site);
List list = context.Web.Lists.GetByTitle(documentName);
CamlQuery camlQuery = new CamlQuery();
camlQuery = new CamlQuery();
camlQuery.ViewXml = @"<View Scope='Recursive' />
						<Query>
							<Where>
								<Eq>
									<FieldRef Name='FileRef'/>
									<Value Type='Text'>" + fileName + @"</Value>
								</Eq>
							</Where>
						</Query>
					</View>";
camlQuery.FolderServerRelativeUrl = "/" + subFolder + "/";
ListItemCollection listItems = list.GetItems(camlQuery);
context.Load(listItems);
context.ExecuteQuery();
foreach (ListItem item in listItems)
{
	string relateFile = item["FileRef"].ToString();
	if (relateFile == fileName)
	{
		item.DeleteObject();
		break;
	}
}
context.ExecuteQuery();


//sharepoint web service 
Lists.Lists list = new Lists.Lists();
list.Credentials = System.Net.CredentialCache.DefaultCredentials;
list.Credentials = new System.Net.NetworkCredential(QueryXMLValue("userName"), QueryXMLValue("passWord"), QueryXMLValue("domain"));
list.Url = QueryXMLValue("siteService");
XmlNode ndListView = list.GetListAndView(QueryXMLValue("listName"), "");
string strListID = ndListView.ChildNodes[0].Attributes["Name"].Value;
XmlNode ListItems = list.GetListItems(QueryXMLValue("listName"), "", null, null, "100000", null, null);
//foreach (XmlNode xn in ListItems.ChildNodes.Item(1))
//{
//    if (xn.LocalName == "row")
//    {
//        string strFileName = xn.Attributes["ows_LinkFilename"].Value;
//        if (strFileName.ToString() == "Note.txt")
//        {
//            id = xn.Attributes["ows_ID"].Value;
//        }
//    }
//}
XmlDocument doc = new XmlDocument();
XmlElement batch = doc.CreateElement("Batch");
batch.SetAttribute("OnError", "Continue");
batch.SetAttribute("ListVersion", "1?");
//batch.SetAttribute("ViewName", "");
List<string> listDeleteURLS = new List<string>();
listDeleteURLS = GetFileURL();
for (int i = 0; i < listDeleteURLS.Count; i++)
{ 
batch.InnerXml +=
	"<Method ID='" + (i).ToString() + "' Cmd='Delete'><Field Name='ID'>" + "" + "</Field><Field Name=\"FileRef\">" + listDeleteURLS[i].ToString () + "</Field></Method>";
}
list.UpdateListItems(strListID, batch);






