using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RecepcionFacturas.Controllers;

namespace ASPNetFileUpDownLoad
{
    public partial class Default : System.Web.UI.Page
    {
    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        if (!IsPostBack)
    //        {
    //            DataTable fileList = FormController.GetFileList();
    //            gvFiles.DataSource = fileList;
    //            gvFiles.DataBind();
    //        }
    //    }

    //    protected void btnUpload_Click(object sender, EventArgs e)
    //    {
    //        // Although I put only one http file control on the aspx page,
    //        // the following code can handle multiple file controls in a single aspx page.
    //        HttpFileCollection files = Request.Files;
    //        foreach (string fileTagName in files)
    //        {
    //            HttpPostedFile file = Request.Files[fileTagName];
    //            if (file.ContentLength > 0)
    //            {
    //                // Due to the limit of the max for a int type, the largest file can be
    //                // uploaded is 2147483647, which is very large anyway.
    //                int size = file.ContentLength;
    //                string name = file.FileName;
    //                int position = name.LastIndexOf("\\");
    //                name = name.Substring(position + 1);
    //                string contentType = file.ContentType;
    //                byte[] fileData = new byte[size];
    //                file.InputStream.Read(fileData, 0, size);

    //                FormController.SaveFile(name, contentType, size, fileData);
    //            }
    //        }

    //        DataTable fileList = FileUtilities.GetFileList();
    //        gvFiles.DataSource = fileList;
    //        gvFiles.DataBind();
        }
    
}