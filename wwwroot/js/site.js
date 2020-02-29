// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(() => {
    // Check does element exist
    if ($("Header").length) {
        // Animate it, by css.
        if ($("Header").hasClass("Invisible")) { // Check does header exist
            $("Header").toggleClass("Invisible"); // if exist, remove class for animation object
        }
    }
});