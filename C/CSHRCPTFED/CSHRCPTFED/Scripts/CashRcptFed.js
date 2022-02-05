//$(document).ready(function () {


//    $('#commit').prop('disabled', true);


//    // - - - - - - - - - - - - - - - - - - - - - - -
//    if (document.getElementById('VMAlert')) {
//        alert($('#VMAlert').val());
//        setTimeout(function () { $('#accountText').focus(); }, 1);
//        $('#VMAlert').val('');
//    }
//    else {
//        $('#accountText').focus();
//    }
    

//    $('#amountText').focus(function () {
//        // - - - - - - - - - - - - - - - - - - - - - - -

//        if ($(this).val() == '$') {
//            $('#amountText').val(''); // clear textbox
//        }
//    });


//    $('#amountText').blur(function () {
//        // - - - - - - - - - - - - - - - - - - - - - - -

//        var regx = new RegExp(/^(?=.*[1-9])\d*(?:\.\d{1,2})?$/);
//        var eax = new String($(this).val().trim());
//        var result = regx.test(eax);
        
//        if(eax.indexOf('.') < 0) {
//            eax += '.00';
//        }
//        else if (eax.indexOf('.') == eax.length  -2)
//        {
//            eax += '0';
//        }

//        if (result) {
//            $('#amountErr').text('');
//            $(this).val(eax);
//            Validate();
//        }
//        else {
//            $('#amountErr').text('Please enter a valid amount.');
//            $('#commit').prop('disabled', true);
//        }
//    });

//    $('#checkText').blur(function () {
//        // - - - - - - - - - - - - - - - - - - - - - - -

//        var regx = new RegExp(/^[a-zA-Z0-9]{1,15}$/);
//        var eax = $(this).val().trim();
//        var result = regx.test(eax);

//        if (result) {
//            $('#checkErr').text('');
//            $(this).val(eax);
//            Validate();
//        }
//        else {
//            $('#checkErr').text('Please enter a valid check number.');
//            $('#commit').prop('disabled', true);
//        }
//    });

//    $('#checkText').keydown(function(e) {
//        if(e.which == 13) {
//            Validate();
//        }
//    });

//    $('#accountText').blur(function () {
//        // - - - - - - - - - - - - - - - - - - - - - - -

//        if ($(this).val() == '0000000000') {
//            $('#borrowerName').focus();
//            //$('#borrowerName').val('');
//            $('#borrowerName').prop('placeholder', 'Borrower Not Found');
//        }
//        else {
//            if ($(this).val() == null || $(this).val() == '') {
//                $(this).val('0000000000');
//            }

//            $.ajax({
//                url: '/CashReceiptFed/AjaxGenerateText?id=' + $('#accountText').val(),
//                method: 'POST',
//                success: function (data) {
//                    if (data == 'Borrower Not Found') {
//                        $('#accountText').text = '0000000000';
//                        $('#accountText').val('0000000000');
//                        $('#borrowerName').prop('placeholder', 'Borrower Not Found');
//                        $('#borrowerName').focus();
//                    }
//                    else {
//                        $('#borrowerName').val(data);
//                        var eax = $(this).val().trim();
//                        $(this).val(eax);
//                        $('#amountText').focus(); // proceed to next field
//                    }
//                },
//                error: function (request, status, error) {
//                    alert(error);
//                }
//            });
//        }
//    });


//    $('#begDate').blur(function () {
//        // - - - - - - - - - - - - - - - - - - - - - - -

//        // easy peasy current date
//        var current = new Date();

//        // parse the @type="date" value, minding your zero based month.
//        var parts = $(this).val().split(/[.\/ -]/);
//        var yyyy = parseInt(parts[0], 10);
//        var mn = parseInt(parts[1], 10);
//        mn -= 1;
//        if (mn < 0) {
//            mn = 11;
//        }

//        var dy = parseInt(parts[2], 10);
//        var proposed = new Date(yyyy, mn, dy, 0, 0, 0, 0); // from 12 A.M. this morning, so current date good.

//        if (proposed > current) {
//            $('#dateMessage').text('Date ' + proposed.toDateString() + ' : cannot be into the future.');
//            $('#commit').prop('disabled', true);
//            $('#begDate').focus();
//        }
//        else {
//            $('#dateMessage').text('');
//            //$('#commit').prop('disabled', false);
//        }
//    });

//    $('#borrowerName').blur(function () {
//        // - - - - - - - - - - - - - - - - - - - - - - -

//        if ($(this).val() == '') {
//            $('#nameMessage').text('Must provide valid name.');
//            $('#commit').prop('disabled', true);
//            $('#borrowerName').val('');
//        }
//        else {
//            $(this).prop('placeholder', '');
//            $('#nameMessage').text('');
//            //$('#commit').prop('disabled', false);
//        }
//    });


//    $('#commit').mouseover(function () {
       
//    });

//    // Check only ONE checkbox
//    $('input:checkbox').click(function () {
//        // - - - - - - - - - - - - - - - - - - - - - - -

//        $('input:checkbox').not(this).prop('checked', false);
//        Validate();
//    });

//    $('#forth').keydown(function (e) {
//        setTimeout(function () { $('#first').focus(); }, 1);
//        if (e.which == 9) {
//                $('#first').prop('checked', false);
//                $('#first').focus();
//        }
//        else {
//            $(this).prop('checked', true);
//        }
        
//    });

//    function Validate() {
//        if ($('#accountText').val() == '') {
//            //alert('Provide missing account information.');
//            $('#accountText').focus();
//        }
//        else if ($('#borrowerName').val() == '' || $('#borrowerName').val() == 'Borrower Not Found') {
//            //alert('Provide missing borrower information.');
//            $('#borrowerName').focus();
//        }
//        else if ($('#checkText').val() == '') {
//            //alert('Provide missing check number information.');
//            $('#checkText').focus();
//        }
//        else if ($('#amountText').val() == '') {
//            //alert('Provide missing monetary information.');
//            $('#amountText').focus();
//        }
//        else if ($('#first').prop('checked') != true && $('#second').prop('checked') != true && $('#third').prop('checked') != true && $('#fourth').prop('checked') != true) {
//            //alert('Provide missing Payee information.');
//            setTimeout(function () { $('#first').focus(); }, 1);
//            $('#first').focus();
//        }

//        else {
//            setTimeout(function () { $('#commit').focus(); }, 1);
//                    $('#commit').prop('disabled', false);
//                    $('#commit').focus();
//        }
//    }

//});
