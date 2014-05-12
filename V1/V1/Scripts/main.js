

$(function () {

    //var hash = window.location.hash,
    //    hashParts = hash.split("&");
    //if (hash.length > 1) {
    //    $("a[href='" + hashParts[0] + "']").trigger("click");
    //    setTimeout(function () {
    //        $("a[href='#" + hashParts[1] + "']").trigger("click");
    //    }, 100);
    //}



    ///* Hover effects for .MeetTeam */
    //$(".MeetTeam").bind("mouseover", function () {
    //    $(this).find(".InfoWrap").stop().slideDown(400);
    //}).bind("mouseout", function () {
    //    $(this).find(".InfoWrap").stop().slideUp(400);
    //});
    //$(".MeetTeam").trigger("mouseout");

    //$('ul.SocialList li a span').mouseover(function () {
    //    $(this).animate({ opacity: 1 }, 100);
    //}).mouseout(function () {
    //    $(this).animate({ opacity: 0 }, 100);
    //});


    ///* Share popover */
    //$('.share, #share, #shareWhite, #shareBlack, #shareRed').popover({
    //    animation: true,
    //    html: true,
    //    trigger: "click",
    //    content: $(".share-popover, #share-popover").html()
    //});

    /* Sticky menu (sidebar) */
    //$('.BottomMenuWrap').hcSticky({
    //    top: 69,
    //    wrapperClassName: 'sidebar-sticky'

    //});

    ///*********back to top**********/
    //$('#top').click(function (event) {
    //    event.preventDefault();
    //    $('html, body').animate({ scrollTop: '0px' }, 1000);
    //});

    ///*********back to top1**********/
    //$('.nav-tabs a').on("click", function (e) {
    //    e.preventDefault();
    //    if ($(this).parent().parent().attr("data-scroll") !== 'disabled') {
    //        var offset = $($(this).parent().parent().attr('data-parent')).offset().top - 0;
    //        if ($(document).scrollTop() !== offset) {
    //            $('html, body').animate({ scrollTop: offset }, 400);
    //        }
    //    }
    //});


    ///*********remove active class top menu**********/
    //$(".navbar").each(function () {
    //    var self = $(this);
    //    self.find("a[href^='#']").on("click", function () {
    //        if (self.find("button[data-toggle='collapse']").is(":visible")) {
    //            self.find("button[data-toggle='collapse']").trigger("click");
    //        }
    //    });
    //});



    // ISOTOPE SCRIPTS FOR PORTFOLIO FILTER, PORTFOLIO GRID LAYOUT, BLOG MASONRY //
    var $container = $('.grid');
    var $blog_container = $('#masonry-blog');
    var $portfolio_container = $('.portfolio-gallery');
    var $gallery_container = $('.gallery-page');
    var $four_container = $('.four-columns, .three-columns, .two-columns');


    // blog masonry layout
    //$blog_container.imagesLoaded(function () {
    //    $blog_container.isotope({
    //        itemSelector: '.masonry-post',
    //        animationEngine: 'best-available',
    //        gutterWidth: 20
    //    });
    //});

    // gallery masonry layout
    //$gallery_container.imagesLoaded(function () {
    //    $gallery_container.isotope({
    //        itemSelector: 'li',
    //        animationEngine: 'best-available',
    //        gutterWidth: 20
    //    });
    //});

    // portfolio gallery masonry option
    //$portfolio_container.imagesLoaded(function () {
    //    $portfolio_container.isotope({
    //        itemSelector: 'a',
    //        animationEngine: 'best-available',
    //        gutterWidth: 20
    //    });
    //});

    // portfolio grid layout and filtering
    $container.isotope({
        itemSelector: '.isotope-item',
        animationEngine: 'best-available',
        masonry: {
            columnWidth: 5
        }
    });

    // portfolio filtering
    //$four_container.isotope({
    //    itemSelector: 'li',
    //    animationEngine: 'best-available'
    //});

    var $optionSets = $('.nav-tabs').find('.BottomMenu');// $('#options .option-set'),
        $optionLinks = $optionSets.find('a');

    $optionLinks.click(function () {
        var $this = $(this);
        // don't proceed if already selected
        if ($this.hasClass('selected')) {
            return false;
        }
        var $optionSet = $this.parents('.nav-tabs').find('.BottomMenu');
        $optionSet.find('.selected').removeClass('selected');
        $this.addClass('selected');

        // make option object dynamically, i.e. { filter: '.my-filter-class' }
        var options = {},
            key = $optionSet.attr('data-option-key'),
            value = $this.attr('data-option-value');
        // parse 'false' as false boolean
        value = value === 'false' ? false : value;
        options[key] = value;
        if (key === 'layoutMode' && typeof changeLayoutMode === 'function') {
            // changes in layout modes need extra logic
            changeLayoutMode($this, options)
        } else {
            // otherwise, apply new options
            $container.isotope(options);
            $four_container.isotope(options);
        }

        return false;
    });

    /*********ISOTOPE**********/
    //var $container1 = $('#container');

    //$container1.isotope({
    //    itemSelector: '.element',
    //    layoutMode: 'masonry'
    //});

    //var $optionSets = $('.option-set'),
    //    $optionLinks = $optionSets.find('a');

    //$optionLinks.click(function () {
    //    var $this = $(this);
    //    // don't proceed if already selected
    //    if ($this.hasClass('selected')) {
    //        return false;
    //    }
    //    var $optionSet = $this.parents('.option-set');
    //    $optionSet.find('.selected').removeClass('selected');
    //    $this.addClass('selected');

    //    // make option object dynamically, i.e. { filter: '.my-filter-class' }
    //    var options = {},
	//		key = $optionSet.attr('data-option-key'),
	//		value = $this.attr('data-option-value');
    //    // parse 'false' as false boolean
    //    value = value === 'false' ? false : value;
    //    options[key] = value;
    //    if (key === 'layoutMode' && typeof changeLayoutMode === 'function') {
    //        // changes in layout modes need extra logic
    //        changeLayoutMode($this, options)
    //    } else {
    //        // otherwise, apply new options
    //        $container1.isotope(options);
    //    }

    //    return false;
    //});


    /* Accordion */
    //$(".accordion-group").each(function () {
    //    var that = $(this);
    //    $(this).find(".accordion-heading a").on("click", function () {
    //        that.parent().find(".accordion-heading a.active").removeClass("active");
    //        $(this).toggleClass("active");
    //    });
    //});

    ///* iOSSlider */
    //if (document.all && document.querySelector && !document.addEventListener) {
    //    $('.iosSlider').each(function () {
    //        $(this).iosSlider({
    //            snapToChildren: true,
    //            desktopClickDrag: true,
    //            startAtSlide: '2',
    //            snapSlideCenter: true,
    //            onSlideChange: slideChange,
    //            navNextSelector: $(this).find('.next'),
    //            navPrevSelector: $(this).find('.prev')
    //        });
    //    });
    //}
    //else {
    //    $('.iosSlider').each(function () {
    //        $(this).iosSlider({
    //            snapToChildren: true,
    //            desktopClickDrag: true,
    //            infiniteSlider: true,
    //            startAtSlide: '1',
    //            snapSlideCenter: true,
    //            onSlideChange: slideChange,
    //            navNextSelector: $(this).find('.next'),
    //            navPrevSelector: $(this).find('.prev')
    //        });
    //    });
    //}

    //function reinitializeSlider(s) {
    //    $(s.attr("href")).find(".SliderWrapper .iosSlider").iosSlider("update");
    //    $(s.attr("href")).find(".SliderWrapper .iosSlider").iosSlider({
    //        snapToChildren: true,
    //        desktopClickDrag: true,
    //        infiniteSlider: true,
    //        startAtSlide: '1',
    //        snapSlideCenter: true,
    //        onSlideChange: slideChange,
    //        navNextSelector: $(s.attr("href")).find(".SliderWrapper .iosSlider .next"),
    //        navPrevSelector: $(s.attr("href")).find(".SliderWrapper .iosSlider .prev")
    //    });
    //}

    $("#portfolio .nav-tabs a").on("click", function () {
        var self = $(this);
        var id = self.attr('href');
        //reinitializeSlider(self);
        setTimeout(function () {
            reinitializeSlider(self);
        }, 800);

        setTimeout(function () {
            var htmlcurrenttitle = $(id + ' .current .descriptionImg .title').html();
            var htmlcurrentdesc = $(id + ' .current .descriptionImg .TitleText').html();
            $('.descr .container-fluid .container .span12 .title').html(htmlcurrenttitle);
            $('.descr .container-fluid .container .span12 .TitleText').html(htmlcurrentdesc);
        }, 400);
    });
    //function slideChange(args) {

    //    $(args.currentSlideObject).parent().children('.item').removeClass('current');
    //    $(args.currentSlideObject).addClass('current');
    //    var htmlcurrenttitle = $(args.currentSlideObject).children('.descriptionImg').children('.title').html();
    //    var htmlcurrentdesc = $(args.currentSlideObject).children('.descriptionImg').children('.TitleText').html();
    //    $('.descr .container-fluid .container .span12 .title').html(htmlcurrenttitle);
    //    $('.descr .container-fluid .container .span12 .TitleText').html(htmlcurrentdesc);
    //}
    //var htmlcurrenttitle = $('.current .descriptionImg .title').html();
    //var htmlcurrentdesc = $('.current .descriptionImg .TitleText').html();
    //$('.descr .container-fluid .container .span12 .title').html(htmlcurrenttitle);
    //$('.descr .container-fluid .container .span12 .TitleText').html(htmlcurrentdesc);



    //var hash = document.location.hash;
    //if (hash) {
    //    $('.TopMenu li').removeClass('active');
    //    $('a[href=' + hash + ']').parent().addClass('active');
    //}
});

$(window).load(function () {
    //var slider_image_height = $('.m-carousel-inner .m-item img').height();
    //$('.m-carousel-prev').css({ top: (slider_image_height / 2) + 1 });
    //$('.m-carousel-next').css({ top: (slider_image_height / 2) + 1 });
    //$(window).resize(function () {
    //    var slider_image_height = $('.m-carousel-inner .m-item img').height();
    //    $('.m-carousel-prev').css({ top: (slider_image_height / 2) + 1 });
    //    $('.m-carousel-next').css({ top: (slider_image_height / 2) + 1 });
    //});


});