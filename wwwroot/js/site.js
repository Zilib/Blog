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
});