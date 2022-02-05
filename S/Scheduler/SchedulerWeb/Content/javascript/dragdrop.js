function DragDrop(isAdmin, upDown, headerTemplate) {
    var gray = '#DDD';
    var backColor = function (j, c, now) {
        if (!now)
            $(j).animate({
                'background-color': c
            }, 2000)
        else
            $(j).css('background-color', c);
    };
    var highlight = function (j, now) {
        backColor(j, gray, now);
    }
    var white = function (j, now) {
        backColor(j, 'white', now);
    }
    var transparent = function (j, now) {
        backColor(j, 'transparent', now);
    }
    this.init = function () {
        var workspace = new Workspace(headerTemplate);
        var droppable = function (event, ui, selector) {
            if (ui.draggable.data('drop-table').is($(selector))) {
                if (ui.draggable.data('insert-after'))
                    ui.draggable.insertAfter(ui.draggable.data('insert-after'));
                else
                    $(selector).append(ui.draggable);
                ui.draggable.data('insert-after', null);
                var row = ui.draggable;
                var parentRow = row.prev();
                if (!workspace.contains(ui.draggable)) {
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
                $('.request-table tr').css('border', 'solid 1px black');
            }
        }
        $('.workspace').droppable({
            drop: function (event, ui) {
                droppable(event, ui, '.workspace table');
            }
        });

        $('body').droppable({
            drop: function (event, ui) {
                droppable(event, ui, '#request-table');
            }
        });

        $('#request-table tr:gt(0)').css(
            {'cursor': 'move'}
        )
        .mousedown(function () {
            highlight($(this), true);
        }).mouseup(function () {
            if (workspace.contains(this))
                white(this, true);
            else
                transparent(this, true);
        }).draggable({
            appendTo: "body",
            helper: function () {
                var self = $(this);
                var table = $('<table class="request-table" style="margin:0px;"></table>')
                var clone = self.clone();
                highlight(clone);
                table.append(clone);

                return table;
            },
            start: function (event, ui) {
                $(this).hide();
                workspace.show();
                if (workspace.hasSingleItem() && workspace.contains(this)) //dragging the only existing item
                    workspace.removeHeader();
            },
            stop: function () {
                var self = $(this);
                self.show();
                if (workspace.contains(self))
                    white(self);
                else
                    transparent(self);


                if (!workspace.hasItems())
                    workspace.hide();
                else
                    workspace.ensureHeader(headerTemplate);
                upDown.rowFix();
                $('.request-table tr').css('border', 'solid 1px black');
            },
            drag: function (event, ui) {
                $('.request-table tr').css('border', 'solid 1px black');
                var hoverTable = $('.workspace');
                var bottom = ui.position.top + $(this).height();
                var top = ui.position.top;
                if (bottom <= hoverTable.offset().top || top >= hoverTable.offset().top + hoverTable.height())
                    hoverTable = $('#request-table');
                else
                    hoverTable = $('.workspace table');
                if (hoverTable) {
                    var y = ui.position.top;
                    var topRow, bottomRow;
                    hoverTable.find('tr').css('border', 'solid 1px black');
                    var insertPoint = hoverTable.find('tr:first').eq(0);
                    if (insertPoint.length == 0)
                        insertPoint = null;
                    $.each(hoverTable.find('tr:gt(0)'), function (index, element) {
                        row = $(element);
                        halfPoint = row.offset().top;
                        if (halfPoint < y) {
                            if (topRow == null || topRow.offset().top < row.offset().top) {
                                topRow = row;
                                insertPoint = topRow;
                            }
                        }
                        if (halfPoint >= y) {
                            bottomRow = row;
                            return false;
                        }
                    });
                    if (topRow)
                        topRow.css('border-bottom', 'solid 4px blue');
                    if (bottomRow)
                        bottomRow.css('border-top', 'solid 4px blue');
                    $(this).data('insert-after', insertPoint);
                    $(this).data('drop-table', hoverTable);
                }
            }
        });
    }

}