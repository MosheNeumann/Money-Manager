$(function () {
    console.log('in js')
    $("#My-Modal").click(function () {
        console.log('in modal function')
        $("#new-simcha-modal").modal();
    });

    $(".Deposit-Model").click(function () {
        console.log('in modal function')
        console.log($(this).data('id'));
        $("#Contributor-id").val($(this).data('id'));
        $("#Deposit-Modal").modal();

    });


    $("#new-contributor").click(function () {
        console.log('in new-contributor modal function')
      
       
        $("#Contributor-Modal").modal();

    });
});