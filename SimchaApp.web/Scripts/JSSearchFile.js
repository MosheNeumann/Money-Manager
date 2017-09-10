$(function () {
    console.log('in the jquery')
    $('#search').on('keyup', function () {

        var text = $(this).val();
        $('table tr:gt(0)').each(function () {
            var tr = $(this);
            var name = tr.find('td:eq(1)').text();
            if (name.toLowerCase().includes(text)) {
                tr.show();
            } else {
                tr.hide();
            }

        });

    });

    $('#clear').on('click', function () {
        $('#search').val("");
        $('tr').show();
    });
});