let cCount = 160;

$( document ).ready(function() {
    $("#btnSend").prop("disabled", true);
});

function textChanged(){
    var commentCount = $("#txtComment").val().length
    if (commentCount >= 160){
        ccCount = 0
        $("#txtComment").val($("#txtComment").val().substring(0, 160))
    }
    $("#charcount").html(cCount - commentCount)
 
    if(commentCount > 0){
        $("#btnSend").prop("disabled", false);
    } else {       
        $("#btnSend").prop("disabled", true);
    }
}

function btnSend(){
    $("#btnSend").html("Sending...");
    $("#btnSend").prop("disabled", true);
    var myData = { "msg": $("#txtComment").val() }
    $.ajax({
        type: "post",
        url: 'SendNewSMS',
        contentType: "application/json",
        traditional: true,
        data: JSON.stringify(myData),
        success: function(resp) { 
            alert(resp); 
            $("#btnSend").prop("disabled", false);
            $("#btnSend").html("Send");
        },
        error: errorFunc
    });
}

function errorFunc(jqXHR, exception){
    alert("Sms send error: "+ jqXHR.responseText)
}