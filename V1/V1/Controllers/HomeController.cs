using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoundInTheory.DynamicImage.Fluent;
using V1.Models.ViewModel;

namespace V1.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //var imageUrl = new CompositionBuilder()
            //    .WithLayer(LayerBuilder.Image.SourceFile(Server.MapPath("~/App_Data/img/pic47.jpg"))
            //    .WithFilter(FilterBuilder.Resize.ToWidth(800)))
            //    .WithLayer(LayerBuilder.Text.Text("Hello World")
            //    .WithFilter(FilterBuilder.OuterGlow));
            List<CarouselViewModel> model = new List<CarouselViewModel>();
            model.Add(new CarouselViewModel()
            {
                ImageSrc = "https://fbcdn-sphotos-g-a.akamaihd.net/hphotos-ak-ash2/t1.0-9/401023_293919477331147_1141379364_n.jpg",
                IsActive = true
            });
            model.Add(new CarouselViewModel()
            {
                ImageSrc = "https://fbcdn-sphotos-g-a.akamaihd.net/hphotos-ak-ash2/t1.0-9/401023_293919477331147_1141379364_n.jpg"
            });
            model.Add(new CarouselViewModel()
            {
                ImageSrc = "https://fbcdn-sphotos-a-a.akamaihd.net/hphotos-ak-ash2/t1.0-9/416790_293919577331137_1141897026_n.jpg"
            });
            return View(model);
        }


    }
}
