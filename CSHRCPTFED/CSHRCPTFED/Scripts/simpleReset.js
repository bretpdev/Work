
$('#reset').click(function () {
                var nd = new Date();
                var mon = nd.getMonth() + 1;
                var day = nd.getDate();
                var phrase = mon + '/' + day + '/' + nd.getFullYear();
                $('#accountText').val('');
                $('#amountText').val('');
                $('#checkText').val('');
                $('#borrowerName').val('');
                $('#borrowerName').fadeOut(320);
                $('#begDate').val(phrase);
                $('input:radio[id = "Payee"]:checked').prop('checked', false);
            });
