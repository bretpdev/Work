function UpDown() {
    var self = this;
    var table = $(".request-table");
    this.rowFix = function () {
        $('.up, .down, .updown').show();
        $.each(table, function (index, element) {
            element = $(element);
            if (element.find('tr').length > 1) {
                element.find('tr').eq(1).find('.up').hide();
                element.find('tr:last').find('.down').hide();
            }
        });
    }
    this.init = function () {
        self.rowFix();
        $(".updown").show();
        swapAjax = function (row, parentRow, bothRows, reallySwap) {
            row.css('background-color', '#DDD');
            if (reallySwap) {
                $.ajax({
                    url: '/Home/Reorder',
                    data: JSON.stringify({ requestId: row.data('priority-id'), newParent: parentRow.data('priority-id') }),
                    type: 'POST',
                    dataType: 'JSON',
                    contentType: 'application/json',
                    success: function (d) {
                        row.animate({ 'background-color': 'white' })
                    }
                });
            }
            else
                row.animate({ 'background-color': 'white' })
            self.rowFix();
        }
        $(".up").click(function () {
            var reallySwap = $(this).parents('#request-table').length > 0;
            var row = $(this).closest('tr');
            var rowAbove = row.prev();
            var newParent = rowAbove.prev();
            var bothRows = row.add(rowAbove);
            rowAbove.before(row); //swap both rows
            swapAjax(row, newParent, bothRows, reallySwap);
        });
        $(".down").click(function () {
            var reallySwap = $(this).parents('#request-table').length > 0;
            var row = $(this).closest('tr');
            var rowBelow = row.next();
            var newParent = rowBelow
            var bothRows = row.add(rowBelow);
            rowBelow.after(row); //swap both rows
            swapAjax(row, newParent, bothRows, reallySwap);
        });
    }
}