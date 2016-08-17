using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

List<String> fileNames = new List<string>();
fileNames.Add("E:\\poc doc combin\\SOW Test\\doc\\AA.DOCX");
fileNames.Add("E:\\poc doc combin\\SOW Test\\doc\\CC.DOCX");

//get the first document
MemoryStream mainStream = new MemoryStream();
byte[] buffer = File.ReadAllBytes(fileNames[0]);
mainStream.Write(buffer, 0, buffer.Length);

using (WordprocessingDocument mainDocument = WordprocessingDocument.Open(mainStream, true))
{
	//xml for the new document
	XElement newBody = XElement.Parse(mainDocument.MainDocumentPart.Document.Body.OuterXml);
	//iterate through eacah file
	for (int i = 1; i < fileNames.Count; i++)
	{
		//read in the document
		byte[] tempBuffer = File.ReadAllBytes(fileNames[i]);
		WordprocessingDocument tempDocument = WordprocessingDocument.Open(new MemoryStream(tempBuffer), true);
		//new documents XML
		XElement tempBody = XElement.Parse(tempDocument.MainDocumentPart.Document.Body.OuterXml);
		//add the new xml
		newBody.Add(tempBody);
		string str = newBody.ToString();
		//write to the main document and save
		mainDocument.MainDocumentPart.Document.Body = new Body(newBody.ToString());
		mainDocument.MainDocumentPart.Document.Save();
		mainDocument.Package.Flush();
		tempBuffer = null;
	}
	mainDocument.Close(); 
}

FileStream fileStream = new FileStream("E:\\poc doc combin\\SOW Test\\doc\\TEST.docx", FileMode.Create);
mainStream.WriteTo(fileStream);
fileStream.Close();
mainStream.Close();
