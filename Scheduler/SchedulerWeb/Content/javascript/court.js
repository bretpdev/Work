function CourtHelper(courtData, headerTemplate, rowTemplate) {
    var table = $('#court-table');
    this.init = function () {
        table.append(headerTemplate());
        $.each(courtData, function (index, element) {
            table.append(rowTemplate(element));
        });
        $('.time-col').hide();
        $('.table-header').show();
        $('#request-table').find('.assign-court').show();//.click(function () {
        //    var type = $(this).closest('.type-col').html();
        //    var id = $(this).closest('.id-col').html();
        //    $.ajax({
        //        url: '/Scheduler/',
        //        data: JSON.stringify({ requestId: id, requestType: type }),
        //        type: 'POST',
        //        dataType: 'JSON',
        //        contentType: 'application/json',
        //        success: function (d) {
        //            row.animate({ 'background-color': 'white' })
        //        }
        //    });
        //});
    }
} 