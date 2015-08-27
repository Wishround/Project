var confirmCostHTML = ""
var isFirstShow = true;
$('.btnShare').click(function () {
    elem = $(this);
    postToFeed(elem.data('title'), elem.data('desc'), "http://facebook.com", elem.data('image'));

    return false;
});
$(document).on("click", ".open-AddBookDialog", function () {
    if (isFirstShow) {
        createOrder();
        isFirstShow = false;
    }
});

function createOrder() {
    $.ajax({
        type: "POST",
        url: "/api/order/create",
        data: "{wishId: '"+$('#wishId').val()+"'}",
        success: onSuccessCreatedOrder,
        error: onModalError,
        headers: {
            Accept: "application/json; charset=utf-8",
            "Content-Type": "application/json; charset=utf-8"
        }
    });
}

$(document).on("click", '#sendCostBtn', function () {
    $("#failAmount").remove();
    var amount = $("#amount").val();
    if (amount == "") {
        $('#amount').parent().append('<span id="failAmount" class="input-group-addon danger"><span class="glyphicon glyphicon-remove"></span></span>');
        return;
    }
    var sendData = {
        version: LiqPay.version,
        public_key: LiqPay.public_key,
        amount: amount,
        currency: LiqPay.currency,
        description: LiqPay.description,
        order_id: $("#orderId").val(),
        server_url: LiqPay.server_url,
        sandbox: LiqPay.sandbox,
        result_url: document.location.href + "?orderId=" + $("#orderId").val(),
    }
    var encodeData = btoa(JSON.stringify(sendData));
    $('input[name="data"]').val(encodeData);
    createSignature(encodeData);
    $("#sendCost").hide();
    $(".modal-body .please-wait").show();
    $(this).hide();
})

function onSuccessCreatedOrder(data) {
    $(".modal-body .please-wait").hide();
    $("#orderId").val(data.orderId);
}

function createSignature(encodeData) {
    $.ajax({
        type: "POST",
        url: "/api/order/getsignature",
        data: "{data: '" + encodeData + "'}",
        success: onSuccessCreatedSignature,
        error: onModalError,
        headers: {
            Accept: "application/json; charset=utf-8",
            "Content-Type": "application/json; charset=utf-8"
        }
    });
}

function onSuccessCreatedSignature(data) {
    $('input[name="signature"]').val(data.signature);
    $("#sendCost").submit();
}

function onModalError(){

}

function sentCost() {

}

function checkStatus(orderId) {
    $.ajax({
        type: "GET",
        url: "/api/order/getstatus/" + orderId,
        success: onSuccessLoadStatus,
        error: onModalError,
        headers: {
            Accept: "application/json; charset=utf-8",
            "Content-Type": "application/json; charset=utf-8"
        }
    });
}

function onSuccessLoadStatus(data) {
    var status = data.status;
    if (status !== "sandbox") {
        setTimeout(function () { checkStatus(data.orderId) }, 5000);
    }
    else {
        $("#payStatusModal .please-wait").hide();
        $("#payStatus").text("Платеж проведен успешно");
    }
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

$(document).ready(function () {
    var orderId = getParameterByName("orderId");
    if (orderId != "") {
        $('#payStatusModal').modal('show');
        checkStatus(orderId);
    }
});