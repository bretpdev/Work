function SchedulerSync(reInitFunction) {
    var self = this;
    var label = '.run-info label';
    var button = '.run-info span';
    this.init = function () {
        $('.run-info').parent().width($('#request-table').width());
        $('.run-info').show();
        var updateLoop = function () {
            self.update(function () {
                setTimeout(updateLoop, 5000);
            });
        }
        updateLoop();
        $(button).click(function () {
            $(button).hide();
            $.ajax({
                url: '/Scheduler/Run',
                type: 'POST',
                dataType: 'JSON',
                contentType: 'application/json',
                success: function (d) {
                    reInitFunction(d);
                }
            });
        });
    }

    this.update = function (onSuccess) {
        $.ajax({
            url: '/Scheduler/GetRunInfo',
            type: 'POST',
            dataType: 'JSON',
            contentType: 'application/json',
            success: function (d) {
                if (d.IsRunning) {
                    $(label).html("Scheduler running Now");
                    $(button).hide();
                } else if (d.LastRunTime) {
                    $(label).html("Scheduler last run on " + d.LastRunTime);
                    $(button).show();
                } else {
                    $(label).html("Scheduler has never run.");
                    $(button).show();
                }
                if (onSuccess)
                    onSuccess();
            }
        });
    }
}