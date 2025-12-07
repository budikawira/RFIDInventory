// ///////////////////////////////////////////////
// version: 2025.07.30                          //
// ///////////////////////////////////////////////

var SimplePageTable = (() => {
    var _token;
    var _grid;
    var _row;
    var _rowId;
    var _url;

    function isChoicesInstance(id) {
        // Check if the element has the 'choices__input' class,
        // which is a key indicator of a Choices.js-enhanced element.
        var element = document.getElementById(id);
        return element && element.classList && element.classList.contains('choices__input');
    }

    function getChoicesValue(id) {
        var selectedValues = $('#id').val();
        // Handle single or multiple selections
        if (selectedValues && selectedValues.length > 0) {
            // If multiple selections are allowed
            if (Array.isArray(selectedValues)) {
                // Process multiple selected values
                console.log("Selected values:", selectedValues);
            } else {
                // Process single selected value
                console.log("Selected value:", selectedValues);
            }
        } else {
            // Handle no selection
            console.log("No value selected.");
        }
    }

    return {
        Init: ((token, url) => {
            _token = token;
            _url = url;

            document.querySelectorAll('.input-decimal').forEach(input => {
                input.addEventListener('input', function () {
                    let value = this.value;

                    // Remove any non-numeric characters except for the decimal point
                    value = value.replace(/[^0-9.]/g, '');

                    // Ensure only one decimal point is allowed
                    const decimalIndex = value.indexOf('.');
                    if (decimalIndex !== -1) {
                        value = value.substring(0, decimalIndex + 1) +
                            value.substring(decimalIndex + 1).replace('.', '');
                    }

                    // Limit to 2 decimal places
                    const parts = value.split('.');
                    console.log('parts >> ', parts);
                    if (parts[1]) {
                        parts[1] = parts[1].substring(0, 2);
                    }
                    this.value = parts.join('.');
                });
            });
        }),
        GetRowData: function () {
            if (_row >= 0) {
                var rowData = _grid.row(_row).data();
                return rowData;
            }
            return null;
        },
        GetTable: (() => {
            return _grid;
        }),
        InitTable: ((options) => {
            console.log('options >> ', options);
            var dataParam = {};

            var pageLength = 10;
            if (options.hasOwnProperty("pageLength")) {
                pageLength = options["pageLength"];
            }
            var columnDefs = [{ className: "align-middle", targets: "_all" }];

            if (options.hasOwnProperty("columnDefs")) {
                console.log("Property columnDefs >> ", options["columnDefs"].length);
                for (let i = 0; i < options["columnDefs"].length; i++) {
                    columnDefs.push(options["columnDefs"][i]);
                }
            }

            let dataSrc = function (d) {
                return d.data;
            }
            if (options.hasOwnProperty("dataSrc")) {
                dataSrc = options['dataSrc'];
            }

            if (options['hasAction'] === true) {
                var actionType = 'popup';
                if (typeof options['action'] !== "undefined") {
                    actionType = options['action'];
                }

                if (options['hasEdit'] && options['hasDelete']) {
                    columnDefs.push({ targets: [0], sortable: false });
                } else {
                    columnDefs.push({ targets: [0], sortable: false });
                }

                var action = {};
                action['data'] = null;
                action['render'] = function (data, type, row, node) {
                    var button = `<span class="text-nowrap d-inline-block">`;
                    if (options['hasEdit']) {
                        if (actionType == 'popup') {
                            button += `<button rel="tooltip" type="button" data-bs-placement="top" title="Edit" class="btn btn-sm btn-soft-primary me-1 mb-1" `;
                            button += `onclick="SimplePageTable.Open('` + node.row + `')" `;
                            button += `>`;
                            button += '<i class="far fa-edit"> </i>';
                            button += '</button>';
                        } else if (actionType == 'page') {
                            //console.log('data >> ', data);
                            button += `<a rel="tooltip" data-bs-placement="top" title="Edit" class="btn btn-sm btn-soft-primary me-1 mb-1" `;
                            button += `href="` + _url + `/Update?id=` + data['id'] + `"`;
                            button += `>`;
                            button += '<i class="far fa-edit"> </i>';
                            button += '</a>';
                        }
                    }
                    if (options['hasView']) {
                        button += `<a rel="tooltip" data-bs-placement="top" title="View" class="btn btn-sm btn-soft-primary me-1 mb-1" `;
                        button += `href="` + _url + `/Update?id=` + data['id'] + `"`;
                        button += `>`;
                        button += '<i class="far fa-eye"> </i>';
                        button += '</a>';
                    }
                    if (options['hasDelete']) {

                        button += `<button rel="tooltip" type="button" data-bs-placement="top" data-container="body" title="Delete" class="btn btn-sm btn-soft-danger me-1 mb-1" onclick="SimplePageTable.Delete('` + node.row + `') ">`;
                        button += '<i class="far fa-trash-alt"> </i></button>';
                    }
                    button += '</span>';
                    return button;
                }
                options['columns'].unshift(action);
            }

            //check the selection
            if (options['hasSelection'] === true) {
                $('#grid thead tr').prepend('<th><input type="checkbox" class="form-check-input dt-checkbox-input-all" /></th>');
                var col = {};
                col['data'] = null;
                col['render'] = function (data, type, row, node) {
                    var html = `<input class="form-check-input dt-checkbox-input ms-1" type="checkbox" value="${data['id']}" />`;
                    return html;
                }
                options['columns'].unshift(col);

                // Handle "select all" checkbox
                $(document).on('change', '.dt-checkbox-input-all', function () {
                    const checked = $(this).is(':checked');
                    $('#grid tbody input.dt-checkbox-input').prop('checked', checked);
                });

            }

            console.log("columnDefs >> ", columnDefs);

            //default order
            var gridOrder = [[1, 'asc']];
            if (options['order'] !== undefined) {
                gridOrder = options['order'];
            }
            var dom = '<"top d-flex flex-sm-row flex-column justify-content-between"<"ms-0 m-1"l>f>tr<"bottom"ip>';
            if (options['dom'] !== undefined) {
                dom = options['dom'];
            }
            _grid = $('#grid').DataTable({
                processing: true,
                serverSide: true,
                orderCellsTop: true,
                autoWidth: true,
                scrollX: true,
                dom: dom,
                ajax: {
                    url: _url + '?Handler=RefreshData',
                    type: 'POST',
                    headers: { 'RequestVerificationToken': _token },
                    data: function (d) {
                        $('.grid-param').each(function (index, element) {
                            var value = element.value;
                            if (element.classList.contains("select2-hidden-accessible")) {
                                value = $('#' + element.id).select2('val');
                            }
                            d[element.id] = value;
                        });
                    },
                    dataSrc: dataSrc
                },
                initComplete: function (settings, json) {
                    //setTimeout(function () {
                    //    _grid.columns.adjust();
                    //}, 100);
                },
                columnDefs: columnDefs,
                order: gridOrder,
                pagingType: $(window).width() < 768 ? "numbers" : "simple_numbers",
                columns: options['columns'],
                pageLength: pageLength,
                lengthMenu: [[10, 25, 50, 100, 500, 1000], [10, 25, 50, 100, 500, 1000]]

            });

            $('.grid-param').on('change', function () {
                _grid.ajax.reload();
            });

            $('#grid').on('draw.dt', function () {
                // Your code to execute after the draw event goes here
                console.log('Table has been redrawn!');
                setTimeout(function () {
                    _grid.columns.adjust();
                }, 100);
            });
        }),
        Open: ((row) => {
            $('.is-invalid').removeClass('is-invalid');
            _row = row || -1;

            if (_row == -1) {
                $('.modal-input').each(function (index, element) {
                    var attr = element.getAttribute('type');
                    console.log('attr > ', attr);
                    if (element.classList.contains("select2-hidden-accessible")) {
                        // For multiple Select2, set empty array
                        if ($("#" + element.id).attr('multiple') !== undefined) {
                            $("#" + element.id).val([]).trigger('change');
                        } else {
                            $("#" + element.id).val(null).trigger('change');
                        }
                    } else if (attr != 'hidden') {
                        element.value = '';
                        if (attr == 'checkbox') {
                            $("#" + element.id).prop("checked", false);
                        }
                    }
                });
                $('#id').val('');
                $('#modalTitle').html('Tambah');
            } else {
                var rowData = _grid.row(_row).data();
                console.log('rowData >> ', rowData);
                _rowId = rowData.id;

                $('.modal-input').each(function (index, element) {
                    var inputType = element.getAttribute("type");
                    console.log(element.id + ':' + inputType);

                    if (element.classList.contains("select2-hidden-accessible")) {
                        var isMultiple = $("#" + element.id).attr('multiple') !== undefined;

                        if (rowData[element.id] == null) {
                            if (isMultiple) {
                                $("#" + element.id).val([]).trigger('change');
                            } else {
                                $("#" + element.id).val(null).trigger('change');
                            }
                        } else {
                            $('#' + element.id).empty();
                            if (isMultiple) {
                                var label = element.getAttribute("data-select2-label");
                                var values = rowData[element.id];
                                console.log('is Multiple label ' + label + 'values >> ', values);
                                values.forEach(function (value) {
                                    $('#' + element.id)
                                        .append(new Option(value[label], value['id'], true, true))
                                        .trigger('change');
                                });

                            } else {
                                // Single selection
                                var label = element.getAttribute("data-select2-label");
                                var option = new Option(rowData[label], rowData[element.id], true, true);
                                $("#" + element.id).append(option).trigger('change');
                            }
                        }
                    } else {
                        if (inputType == 'checkbox') {
                            $("#" + element.id).prop("checked", rowData[element.id]);
                        } else {
                            element.value = rowData[element.id];
                        }
                    }
                });
                $('#modalTitle').html('Ubah');
            }
            $('#modal').modal('show');
            SimplePageTable.PostOpen(row);
        }),
        PostOpen: ((row) => {
            //dummy function for customizing Open function
        }),
        Save: (() => {
            LoadingScreenFunction.Show();

            var countBlankError = 0;

            var dataParam = {};
            $('.modal-input').each(function (index, element) {
                if (element.hasAttribute('required')) {
                    countBlankError += validateBlank(element.id);
                }

                var minLength = -1;
                var maxLength = -1;
                if (element.hasAttribute('minlength')) {
                    minLength = element.minLength;
                }

                if (element.hasAttribute('maxlength')) {
                    maxLength = element.maxLength;
                }

                if (minLength > 0 || maxLength > 0) {
                    countBlankError += validateMinMax(element.id, minLength, maxLength);
                }

                if (element.hasAttribute('select2')) {
                    dataParam[element.id] = $('#' + element.id).select2('val');
                }
                var attr = element.getAttribute('type');

                if (attr == 'checkbox') {
                    dataParam[element.id] = $('#' + element.id).is(":checked")
                } else if (isChoicesInstance(element.id)) {
                    dataParam[element.id] = JSON.stringify($('#' + element.id).val());
                    console.log('isChoiceInstance-' + element.id, dataParam[element.id]);
                } else if ($('#' + element.id).hasClass('select2-hidden-accessible') && $('#' + element.id).attr('multiple')) {
                    // Handle Select2 multiple select
                    var values = $('#' + element.id).val();
                    var resultArray = [];

                    if (values && values.length > 0) {
                        values.forEach(function (value) {
                            resultArray.push({ id: value });
                        });
                    }

                    dataParam[element.id] = resultArray;
                    console.log('Select2 Multiple-' + element.id, dataParam[element.id]);
                } else if (element.classList.contains("input-decimal")) {
                    dataParam[element.id] = parseFloat(element.value);
                    console.log('Float param >> ' + element.id + ' >> ', dataParam[element.id])
                } else {
                    dataParam[element.id] = element.value;
                }
            });
            console.log('dataParam >> ', dataParam);
            if (countBlankError > 0) {
                LoadingScreenFunction.Hide();
                Swal.fire('Harap cek kembali data yang diisi!', '', 'error');
                return;
            }

            $.ajax({
                type: "POST",
                url: _url + "?Handler=Save",
                data: dataParam,
                headers: { 'RequestVerificationToken': _token },
                success: function (objRes) {
                    LoadingScreenFunction.Hide();
                    if (objRes.result != 0) {
                        Swal.fire(objRes.message, '', 'error');
                    } else {
                        $('#modal').modal('hide');
                        Swal.fire(objRes.message, '', 'Sukses')
                            .then(okay => {
                                if (okay) {
                                    if (_row == -1) {
                                        _grid.ajax.reload();
                                    } else {
                                        _grid.row(_row).data(objRes.data).draw();
                                    }
                                }
                            });
                    }
                },
                error: function (response) {
                    LoadingScreenFunction.Hide();
                    alert('Error function please contact developer');
                }
            })
        }),
        Delete: ((row) => {
            _row = row || -1;
            var rowData = _grid.row(_row).data();
            console.log('rowData >> ', rowData);
            Swal.fire({
                title: 'Are you sure to delete this data?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    LoadingScreenFunction.Show();
                    $.ajax({
                        type: "POST",
                        url: _url + "?Handler=Delete",
                        headers: { 'RequestVerificationToken': _token },
                        data: {
                            id: rowData.id
                        },
                        success: function (objRes) {
                            LoadingScreenFunction.Hide();
                            console.log('Delete objRes >> ', objRes);
                            if (objRes.result != 0) {
                                Swal.fire(objRes.message, '', 'error')
                            } else {
                                Swal.fire('Deleted!', 'Your data has been deleted.', 'success');
                                _grid.ajax.reload(null, false);
                            }
                        },
                        error: function (response) {
                            LoadingScreenFunction.Hide();
                            alert('Error function please contact developer');
                        }
                    })
                }
            })
        }),
        Export: ((fileName) => {
            LoadingScreenFunction.Show();

            var countBlankError = 0;

            var dataParam = {};
            $('.export-param').each(function (index, element) {
                dataParam[element.id] = element.value;
                if (element.hasAttribute('required')) {
                    countBlankError = validateBlank(element.id);
                }

                if (element.hasAttribute('select2')) {
                    dataParam[element.id] = $('#' + element.id).select2('val');
                }
            });
            console.log('dataParam >> ', dataParam);
            if (countBlankError > 0) {
                LoadingScreenFunction.Hide();
                Swal.fire('Please input mandatory fields!', '', 'error');
                return;
            }

            $.ajax({
                type: "POST",
                url: _url + "?Handler=Export",
                data: dataParam,
                headers: { 'RequestVerificationToken': _token },
                traditional: true,
                xhrFields: {
                    responseType: 'blob'
                },
                success: function (objRes) {
                    LoadingScreenFunction.Hide();
                    var a = document.createElement('a');
                    var url = window.URL.createObjectURL(objRes);
                    a.href = url;
                    a.download = fileName;
                    document.body.append(a);
                    a.click();
                    a.remove();
                    window.URL.revokeObjectURL(url);

                },
                error: function (response) {
                    console.log('response err >> ', response);
                    LoadingScreenFunction.Hide();
                    Swal.fire('Error : ' + response.status, response.responseText, 'error');
                }
            })
        }),
        Import: (() => {
            LoadingScreenFunction.Show();
            var fileExtension = ['xls', 'xlsx', 'xlsm'];
            var filename = $('#importFile').val();
            if (filename.length == 0) {
                LoadingScreenFunction.Hide();
                $('#importFile').addClass('is-invalid');
                alert("Please select a file.");
                return false;
            }
            else {
                var extension = filename.replace(/^.*\./, '');
                if ($.inArray(extension, fileExtension) == -1) {
                    LoadingScreenFunction.Hide();
                    $('#importFile').addClass('is-invalid');
                    alert("Please select only excel files.");
                    return false;
                }
            }
            var fdata = new FormData();
            var fileUpload = $("#importFile").get(0);
            var files = fileUpload.files;
            fdata.append(files[0].name, files[0]);
            $.ajax({
                type: "POST",
                url: _url + "?Handler=Import",
                headers: { 'RequestVerificationToken': _token },
                data: fdata,
                contentType: false,
                processData: false,
                success: function (objRes) {
                    LoadingScreenFunction.Hide();
                    if (objRes.result != 0) {
                        Swal.fire('Import failed!', '', 'error')
                            .then(_ => {
                                $('#textImportError').val(objRes.message);
                                $('#modalImportError').modal('show');
                            });
                    } else {
                        $('#modalImport').modal('toggle');
                        Swal.fire('Data has been saved!', '', 'success')
                            .then(okay => {
                                if (okay) {
                                    _grid.ajax.reload();
                                }
                            });
                    }
                },
                error: function (e) {
                    LoadingScreenFunction.Hide();
                    alert('Error function please contact developer');
                }
            });
        })
    }
})();

let validateMinMax = function (id, min, max) {
    var error = '';
    var value = $('#' + id).val();
    if (min > 0 && value.length < min) {
        $('#' + id).addClass('is-invalid');
        return 1;
    } else if (max > 0 && value.length > max) {
        $('#' + id).addClass('is-invalid');
        return 1;
    }

    return 0;
}


