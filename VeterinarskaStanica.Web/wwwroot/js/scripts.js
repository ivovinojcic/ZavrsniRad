/*(function($) {
    "use strict";

    // Add active state to sidbar nav links
    var path = window.location.href; // because the 'href' property of the DOM element is the absolute path
        $("#layoutSidenav_nav .sb-sidenav a.nav-link").each(function() {
            if (this.href === path) {
                $(this).addClass("active");
            }
        });

    // Toggle the side navigation
    $("#sidebarToggle").on("click", function(e) {
        e.preventDefault();
        $("body").toggleClass("sb-sidenav-toggled");
    });
})(jQuery);*/

// Submit Login form with post data
function SubmitLoginForm() {
    document.getElementById("loginForm").submit();
}

function ChangePageTitle(pageTitle) {
    document.title = pageTitle;
}

ResizeTextArea = function (id) {
    var el = document.getElementById(id);
    if (el) {
        el.style.height = "5px";
        el.style.height = (el.scrollHeight + 5) + "px";
    }
    return true;
}
