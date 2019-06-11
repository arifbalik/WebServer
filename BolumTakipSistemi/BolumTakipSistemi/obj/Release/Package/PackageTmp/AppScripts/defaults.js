
function notifyS(msg) {
    showMessage(msg, 1, 0);
}

function notifyE(msg) {
    showMessage(msg, 2, 0);

}

function notifyD(msg) {
    //msg ekranda kalır gitmez
    //sap den dönen hatalarda kullanabilirsin
    showMessage(msg, 2, 1);

}



function showMessage(msg, type, clickeble) {
    var x = document.getElementById("snackbar")
    x.className = "show";

    $('#txt').html(msg);

    if (clickeble == 0) {
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);

    }

    if (type == 1) {
        $('#snackbar').css('background-color', '#57ae35e6');

    }
    else if (type == 2) {
        $('#snackbar').css('background-color', '#ae3535e6');

    }

}


function getFormattedDate(date) {
    var year = date.getFullYear();

    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;

    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;

    return day + '.' + month + '.' + year;
}

