$('.btnShare').click(function () {
    elem = $(this);
    postToFeed(elem.data('title'), elem.data('desc'), "http://facebook.com", elem.data('image'));

    return false;
});
$(document).on("click", ".open-AddBookDialog", function () {
    var myBookId = $(this).data('id');
    $(".modal-body #bookId").val(myBookId);
    // As pointed out in comments,
    // it is superfluous to have to manually call the modal.
    // $('#addBookDialog').modal('show');
});

function createOrder() {
    $.ajax({
        type: "POST",
        url: "/api/order/create",
        data: "{wishId: '"+$('#wishId').val()+"'}",
        success: function (data) {
            debugger;
        },
        headers: {
            Accept: "application/json; charset=utf-8",
            "Content-Type": "application/json; charset=utf-8"
        }
    });
}

function sentCost() {

}

function checkStatus() {

}