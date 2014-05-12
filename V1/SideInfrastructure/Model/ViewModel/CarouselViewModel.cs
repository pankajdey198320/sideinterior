using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace V1.Models.ViewModel
{
    public class CarouselViewModel
    {
        public long Id { get; set; }

        public string  ImageSrc { get; set; }

        public Caption CarouselCaption { get; set; }

        public bool IsActive { get; set; }


        public class Caption
        {
            public string Text { get; set; }
        }
    }
}