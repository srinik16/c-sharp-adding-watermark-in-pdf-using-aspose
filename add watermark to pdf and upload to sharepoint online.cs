static void Main(string[] args)
        {
            // The path to the documents directory.
            string dataDir = @"C:\Users\Srini\Documents\";
            byte[] pdfBytes = System.IO.File.ReadAllBytes(dataDir + "Testing.pdf");
            //System.IO.File.WriteAllBytes(dataDir+"File1.pdf", pdfBytes);
            MemoryStream memoryStream = new MemoryStream(pdfBytes);
            TextState ts = new TextState();
            // Set color for stroke
            ts.StrokingColor = Color.Red;
            // Set text rendering mode
            ts.RenderingMode = TextRenderingMode.FillText;
            // Load an input PDF document
            //Aspose.Pdf.Facades.PdfFileStamp fileStamp = new Aspose.Pdf.Facades.PdfFileStamp(new Aspose.Pdf.Document(dataDir + "Testing.pdf"));
            Aspose.Pdf.Facades.PdfFileStamp fileStamp;
            fileStamp = new Aspose.Pdf.Facades.PdfFileStamp(new Document(memoryStream));

            Aspose.Pdf.Facades.Stamp stamp = new Aspose.Pdf.Facades.Stamp();
            stamp.BindLogo(new Aspose.Pdf.Facades.FormattedText("Water mark text", System.Drawing.Color.Red, "Arial", Aspose.Pdf.Facades.EncodingType.Winansi, true, 90));

            // Bind TextState
            stamp.BindTextState(ts);
            // Set X,Y origin
            stamp.SetOrigin(50, 50);
            stamp.Opacity = 40.0E-2f;
            stamp.BlendingSpace = Aspose.Pdf.Facades.BlendingColorSpace.DeviceRGB;
            stamp.Rotation = 58.0F;
            stamp.IsBackground = false;
            // Add Stamp
            fileStamp.AddStamp(stamp);
            MemoryStream stream = new MemoryStream();
            fileStamp.Save(stream);
            byte[] m_Bytes = stream.ToArray();
            fileStamp.Close();

            try
            {
                using (ClientContext client = new ClientContext("https://test.sharepoint.com/sites/contoso"))
                {
                    SecureString password = new SecureString();
                    string username = "username@xxxxxx.onmicrosoft.com";
                    string userpwd = "abcdefgh";
                    foreach (char c in userpwd.ToCharArray()) password.AppendChar(c);
                    client.Credentials = new SharePointOnlineCredentials(username, password);
                    Web web = client.Web;
                    List docLib = web.Lists.GetByTitle("Documents");
                    Folder rootFolder = docLib.RootFolder;
                    client.Load(web);
                    client.Load(docLib);
                    client.Load(rootFolder);
                    client.ExecuteQuery();

                    FileCreationInformation fci = new FileCreationInformation();

                    fci.Content = m_Bytes; 

                    fci.Url = "https://test.sharepoint.com/sites/contoso/Shared%20Documents/File2.pdf";
                    fci.Overwrite = true;
                    Microsoft.SharePoint.Client.File fileToUpload = rootFolder.Files.Add(fci);
                    client.Load(web);
                    client.Load(docLib);
                    client.Load(rootFolder);
                    client.Load(fileToUpload);
                    client.ExecuteQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
