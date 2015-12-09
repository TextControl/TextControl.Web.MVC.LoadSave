using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tx_savecontroller.Models;

namespace tx_savecontroller.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // receives a base64 encoded string and saves the document
        // using a ServerTextControl instance
        [HttpPost]
        public bool SaveTemplate(DocumentViewModel model)
        {
            string name = model.DocumentName;
            byte[] document = Convert.FromBase64String(model.BinaryDocument);

            using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl())
            {
                tx.Create();
                tx.Load(document, TXTextControl.BinaryStreamType.InternalUnicodeFormat);

                tx.Save(Server.MapPath("/App_Data/documents/" + model.DocumentName), TXTextControl.StreamType.WordprocessingML);
            }

            return true;
        }

        // loads a document into the ServerTextControl and returns the
        // document as a base64 encoded string
        [HttpPost]
        public string LoadTemplate(LoadDocumentViewModel model)
        {
            byte[] data;

            using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl())
            {
                tx.Create();
                tx.Load(Server.MapPath("/App_Data/documents/" + model.DocumentName), TXTextControl.StreamType.WordprocessingML);

                tx.Save(out data, TXTextControl.BinaryStreamType.InternalUnicodeFormat);
            }

            return Convert.ToBase64String(data);
        }
    }
}