// ///////////////////////////////////////////////
// version: 0.1.0                               //
// ///////////////////////////////////////////////

var SimplePageForm = (() => {
    var _token;
    var _url;
    return {
        Init: ((token, url) => {
            _token = token;
            _url = url;
        }),
        Select2: ((id, placeholder, handler, dropdownParent) => {
            var select2 = $('#' + id).select2({
                width: '100%',
                allowClear: true,
                placeholder: placeholder,
                theme: 'bootstrap',
                dropdownParent: dropdownParent,
                ajax: {
                    url: _url + "?Handler=" + handler,
                    type: "POST",
                    headers: { 'RequestVerificationToken': _token },
                    dataType: 'json',
                    delay: 100,
                    processResults: function (data) {
                        return {
                            results: $.map(data, function (item) {
                                return {
                                    text: item.text,
                                    id: item.value
                                }
                            })
                        };
                    },
                    cache: true
                }
            });
        }),
        LoadData: ((id, func) => {
            console.log('LoadData >> ', id);
            LoadingScreenFunction.Show();
            $.ajax({
                type: "POST",
                url: _url + "?Handler=LoadData",
                data: {
                    id: id
                },
                headers: { 'RequestVerificationToken': _token },
                success: function (objRes) {
                    LoadingScreenFunction.Hide();
                    if (objRes.result == 0) {
                        func(objRes);
                    } else {
                        Swal.fire(objRes.message, '', 'error');
                    }
                },
                error: function (response) {
                    LoadingScreenFunction.Hide();
                    alert('Error function please contact developer');
                }
            })
        })
    }
})();
