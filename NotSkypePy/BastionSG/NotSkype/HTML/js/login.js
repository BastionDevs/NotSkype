function redirLoad() {
    window.location.href = "load.html";
}

function getUname() {
    document.getElementById('ermwhatthesigma').innerHTML = 'Enter the password for ' + urlParam('uname');
}

//https://stackoverflow.com/questions/45758837/script5009-urlsearchparams-is-undefined-in-ie-11
urlParam = function(name){
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null){
       return null;
    }
    else {
       return decodeURI(results[1]) || 0;
    }
}