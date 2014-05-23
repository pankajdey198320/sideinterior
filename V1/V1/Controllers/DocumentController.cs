using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using SideInfrastructure.Model.Edmx;

namespace V1.Controllers
{
    [OutputCache(NoStore=false, Duration=3600)]
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

        public ActionResult Thumnail(long id)
        {
            using (SIDEContxts context = new SIDEContxts())
            {
                var doc = context.Documents.FirstOrDefault(p => p.DocumentId == id);

                var img = new WebImage(doc.DocumentData).Resize(150, 150, true, false);
                return File(img.GetBytes(), "image/png");
            }

        }

    }
}
