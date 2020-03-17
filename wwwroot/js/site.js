// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Check does object is in the viewport
jQuery.fn.extend({
    isInViewport: (offsetTop) => {
        let elementTop = offsetTop + 150;
        var elementBottom = elementTop + $(this).outerHeight();

        var viewportTop = $(window).scrollTop();
        var viewportBottom = viewportTop + $(window).height();

        return elementBottom > viewportTop && elementTop < viewportBottom; 
    },
    // If element is in the view, show him
    showInView: () => {
        $(".Post").each((i, obj) => {
            if ($(this).isInViewport(obj.offsetTop) &&
                !$(this).hasClass("Show")) {
                obj.classList.add("Show");
            }
        });

    }
});

// Scroll
$(window).on('resize scroll', function () {
    $(this).showInView();
    // If showed element is bottom of the user scroll hide him!
    $(".Show").each((i, obj) => {
        if (!$(this).isInViewport(obj.offsetTop))
            obj.classList.remove("Show");
    }); 
});

// Document ready
$(() => {
    // Check does element exist
    if ($("Header").length) {
        // Animate it, by css.
        if ($("Header").hasClass("Invisible")) { // Check does header exist
            $("Header").toggleClass("Invisible"); // if exist, remove class for animation object
        }
    }
    // Show every elements which are available to see in first view
    $(this).showInView();

    // Add expand button into
    if ($("#sidebar").length) { // If sidebar exist
        let extendButton = $("<div class='nav-expand' id='sidebar-expand'>>></div >"); // Create it, it is defined in css
        $("#sidebar").append(extendButton); // add button
    }
});

// Dashboard, show sidebar when user hover at navbar-expand

$("#sidebar-expand, #sidebar").on("mouseenter", () => {
    if (!$("#sidebar").hasClass("expand")) { // If sidebar is not expanded
        $("#sidebar").addClass("expand");
    }
});

// Hide sidebar when user not hover any more sidebar
$("#sidebar-expand, #sidebar").on("mouseleave", () => {
    if ($("#sidebar").hasClass("expand")) {
        $("sidebar").removeClass("expand");
    }
});

// Confirm user delete
$(document).one('click', '.ConfirmButton', () => {
    var isConfirmed = confirm("Na pewno chcesz usunąć użytkownika?");

    return isConfirmed == true ? true : false;
});
