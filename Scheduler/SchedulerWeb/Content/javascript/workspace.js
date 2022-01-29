function Workspace()
{
    var ui = $('.workspace, .workspace-background');
    ui.height(0);
    var height = '35%';
    this.show = function () {
        ui.show();
        ui.animate({
            height: height
        });
    }
    this.hide = function () {
        ui.animate({
            height: 0,
            display: 'none'
        });
    }

    this.hasItems = function () {
        return $('.workspace table tr:not(.header)').length > 0;
    }

    this.hasSingleItem = function () {
        return $('.workspace table tr:not(.header)').length == 1;
    }

    this.ensureHeader = function (headerTemplate) {
        if ($('.workspace th').length == 0)
            $('.workspace table').prepend(headerTemplate());
    }

    this.removeHeader = function () {
        $('.workspace tr.header').remove();
    }

    this.contains = function (j) {
        j = $(j);
        return j.parents('.workspace').length > 0;
    }
}