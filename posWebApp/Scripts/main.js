$(document).ready(function(){
    'use strict';

    // sync Navbar Links With Sections

    $(window).scroll(function(){
        $(".block").each(function(){
            if($(window).scrollTop() > $(this).offset().top){
                var blockId = $(this).attr("id");
                $(".navbar li").removeClass("active");
                $(".navbar li a[data-scroll = "+ blockId +"]").parent().addClass("active");
            }
        });
        var scrollToTop = $(".scroll-to-top");
            if($(window).scrollTop() >= 1000){
                if(scrollToTop.is(":hidden")){
                    $(".scroll-to-top").fadeIn(500);
                }
            }else{
                $(".scroll-to-top").fadeOut(500);
            }


            });

        $(".scroll-to-top, .home-btn").click(function(e){
            e.preventDefault();

            $('html, body').animate({
                scrollTop: 0
            },1000);

        });

        // $(".main-menu-button").click(function(){
        //   $(".main-menu").slideToggleRight(300,function() {
        //     $(this).hasClass('close'); }
        //   );
        // });

        // $('.main-menu-button').click(function () {
        //     $('.main-menu').animate({
        //       right: 0
        //     });
        // });

          $('.main-menu-button').click(function(){
            if ($('.main-menu').hasClass('open')) {
              $('nav .main-menu-button i').toggle(200);
              
              $('nav .main-menu-button .m').slideDown(200);
              $('.main-menu').removeClass('open');
              $('.main-menu').animate({
               left:0
              },500);
            } else {
            $('nav .main-menu-button i').toggle(200);

            $('.main-menu').addClass('open');
            $('.main-menu').animate({
             left:"-=200"
            },500);
            }
          });

        $('.main-menu ul li').hover(function(){
          var dmenu = $(this).data('menu');
          if(dmenu == "products") {
            $('.main-menu .products .dropdown-mobile').css('display','block');
          }else if (dmenu == "contacts") {
            $('.main-menu .contacts .dropdown-mobile').css('display','block');
          }
        },function(){
          $('.main-menu .dropdown-mobile').css('display','none');
        });

        function toggleDropdown (e) {
          const _d = $(e.target).closest('.dropdown'),
            _m = $('.dropdown-menu', _d);
          setTimeout(function(){
            const shouldOpen = e.type !== 'click' && _d.is(':hover');
            _m.toggleClass('show', shouldOpen);
            _d.toggleClass('show', shouldOpen);
            $('[data-toggle="dropdown"]', _d).attr('aria-expanded', shouldOpen);
          }, e.type === 'mouseleave' ? 300 : 0);
        }

        $('body')
          .on('mouseenter mouseleave','.dropdown',toggleDropdown)
          .on('click', '.dropdown-menu a', toggleDropdown);

});
