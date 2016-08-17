//use regex check file type
public bool IsImage(string fileName)
{
	string pattern = @"(.jpg)$";
	Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
	MatchCollection matches = rgx.Matches(fileName);
	if (matches.Count > 0)
	{
		return true;
	}
	return false;
}

//generate thumbnail
public static Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
{
	System.Drawing.Bitmap bmpOut = null;
	try
	{
		Bitmap loBMP = new Bitmap(lcFilename);
		ImageFormat loFormat = loBMP.RawFormat;

		decimal lnRatio;
		int lnNewWidth = 0;
		int lnNewHeight = 0;

		//*** If the image is smaller than a thumbnail just return it
		if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
			return loBMP;

		if (loBMP.Width > loBMP.Height)
		{
			lnRatio = (decimal)lnWidth / loBMP.Width;
			lnNewWidth = lnWidth;
			decimal lnTemp = loBMP.Height * lnRatio;
			lnNewHeight = (int)lnTemp;
		}
		else
		{
			lnRatio = (decimal)lnHeight / loBMP.Height;
			lnNewHeight = lnHeight;
			decimal lnTemp = loBMP.Width * lnRatio;
			lnNewWidth = (int)lnTemp;
		}
		bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
		Graphics g = Graphics.FromImage(bmpOut);
		g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
		g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
		g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);

		loBMP.Dispose();
	}
	catch
	{
		return null;
	}

	return bmpOut;
}


//memberary DataTable
private DataTable Query(string path)
{
	DataTable info = new DataTable();
	DataColumn infoColumn;
	DataRow infoRow;

	infoColumn = new DataColumn();
	infoColumn.DataType = System.Type.GetType("System.String");
	infoColumn.ColumnName = "FileName";
	info.Columns.Add(infoColumn);

	infoColumn = new DataColumn();
	infoColumn.DataType = System.Type.GetType("System.String");
	infoColumn.ColumnName = "FilePath";
	info.Columns.Add(infoColumn);

	DirectoryInfo dir = new DirectoryInfo(path);
	foreach (FileInfo file in dir.GetFiles())
	{
		infoRow = info.NewRow();
		infoRow["FileName"] = file.Name;
		infoRow["FilePath"] = file.FullName;
		info.Rows.Add(infoRow);

	}
	return info;
}


//StreamWriter output txt file
 public void OutText(string outPath, DataTable infoTable)
{
	string lines = "VarietyNumber" + "\t" + "Volgnummer" + "\t" + "PhotoDescription" + "\t" + "PhotoFileName" + "\t" + "PhotoThumbnail";
	System.IO.StreamWriter file = new System.IO.StreamWriter(outPath + "\\" + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + ".txt");
	file.WriteLine(lines);
	foreach (DataRow infoRow in infoTable.Rows)
	{
		string fileName = infoRow["FileName"].ToString();
		string varitynumber = "", volgnumber = "", PhotoDescription = "", PhotoFileName = "";
		if (IsImage(fileName))
		{
			try
			{
				//varitynumber = fileName.Substring(0, fileName.IndexOf('_'));
				varitynumber = "VarietyNumber";
				volgnumber = fileName.Substring(0, fileName.IndexOf('.'));
				PhotoDescription = fileName.Substring(0, fileName.IndexOf('.'));
				PhotoFileName = fileName;
			}
			catch
			{
			}

			file.WriteLine(varitynumber + "\t" + volgnumber + "\t" + PhotoDescription + "\t" + PhotoFileName + "\t" + "");
		}

	}

	file.Close();
}

//Donwload file from URL
static public void DownloadFile(string URL,string desFolder)
{
	try
	{
		HttpWebRequest webRequest = WebRequest.Create(URL) as HttpWebRequest;
		webRequest.Proxy = WebRequest.DefaultWebProxy;
		//webRequest.Proxy.Credentials = new NetworkCredential("Joseph_Xu02", "April#14", "itlinfosys");
		webRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
		//webRequest.Proxy.Credentials = new NetworkCredential("t809284", "April#14", "NAFTA");
		System.IO.Stream str = webRequest.GetResponse().GetResponseStream();

		byte[] buffer = new byte[16 * 1024];
		using (FileStream fs = System.IO.File.Create(desFolder + "\\"+ GetFileName (URL)))
		{
			int read;
			while ((read = str.Read(buffer, 0, buffer.Length)) > 0)
			{
				fs.Write(buffer, 0, read);
			}
		}

	}
	catch (Exception ex)
	{ }
}