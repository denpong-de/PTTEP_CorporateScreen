mergeInto(LibraryManager.library,
{
    Alert: function(){
        window.alert("Unity to JS Alert!");
    },
    OpenPopup: function(){
        const xhttp = new XMLHttpRequest();
        xhttp.onload = function(){
            var popup = document.getElementById("popup");
            popup.style.display = "block";
        }
    }
});