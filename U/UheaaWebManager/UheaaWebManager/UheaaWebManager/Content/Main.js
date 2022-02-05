$(function () {
    $("input[type='checkbox']").click(function () {
        $(this).attr('value', this.checked ? "true" : "false");
    })
    $.each($("tr[data-href]"), function (index, item) {
        var cells = $(item).find("td");
        var href = $(item).data('href');
        $.each(cells, function (cellIndex, cellItem) {
            $(cellItem).html("<a href=\"" + href + "\">" + $(cellItem).html() + "</a>");
        });

        $(item)
            .css('cursor', 'pointer')
            .click(function () {
                document.location = href;
            })
    });
})