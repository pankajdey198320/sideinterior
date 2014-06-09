using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using SideInfrastructure.Model.Edmx;

namespace V1.Controllers
{
    [OutputCache(NoStore=false, Duration=600)]
    public class DocumentController : Controller
    {
        //
        // GET: /Document/

        public ActionResult Index(long id)
        {
            using (SIDEContxts context = new SIDEContxts())
            {
                var doc = context.Documents.FirstOrDefault(p => p.DocumentId == id);
                return File(doc != null ? doc.DocumentData : null, "image/png");
            }
        }

        public ActionResult Thumnail(long id, int height = 150, int width = 150)
        {
            using (SIDEContxts context = new SIDEContxts())
            {
                var doc = context.Documents.FirstOrDefault(p => p.DocumentId == id);

                var img = new WebImage(doc.DocumentData).Resize(width, height, true, false);
                return File(img.GetBytes(), "image/png");
            }

        }



    }
}
