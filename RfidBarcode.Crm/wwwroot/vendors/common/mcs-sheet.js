
let sheetFunction = (() => {
    var sheet;
    var rowId;
    var consumeKeyDown = true;
    var selectedCell;
    var columnTypes;
    var prevValue;
    var gridModal;
    var gridModalLocation;
    var gridModalDepartment;
    var gridModalJob;
    var gridSummary;
    var token;
    var prevInput;
    var projectEndPeriod;
    var projectLastPeriod;
    var projectSessionOid;
    var deletedRowOids = [];
    var columnRoles = [];
    var masterSessionStatusId;

    function IsReadOnly() {
        var readonly = sheet.row(selectedCell.index().row).node().className.includes('final');
        if (!readonly) {
            readonly = sheet.cell(selectedCell.index().row, selectedCell.index().column).node().className.includes('readonly');
        }
        return readonly;
    }
    return {
        GetSelectedCell: (() => {
            return selectedCell;
        }),
        Init: ((_token, _sheet, _dataRoles, _projectEndPeriod, _projectSessionOid, _projectLastPeriod, _masterSessionStatusId) => {
            token = _token;
            sheet = _sheet;
            dataRoles = _dataRoles;
            projectEndPeriod = _projectEndPeriod;
            projectSessionOid = _projectSessionOid;
            projectLastPeriod = _projectLastPeriod;
            masterSessionStatusId = _masterSessionStatusId;

            for (let i = 0; i < dataRoles.length; i++) {
                columnRoles[dataRoles[i]] = i;
            }
            
            _sheet
                .on('key', function (e, datatable, key, cell, originalEvent) {
                    var idx = cell.index().column;
                    if (IsReadOnly()) { return; }

                    if (key == 13 && consumeKeyDown) {
                        sheetFunction.CellRefocus(cell);
                    } else if (dataRoles[idx] == 'input') {
                        if ((key >= 48 && key <= 57) || key == 190) {
                            selectedCell = cell;
                            if (key >= 48 && key <= 57) {
                                var decimalIx = prevInput.indexOf('.');
                                var decimalDigit = 0;
                                if (decimalIx > 0) {
                                    decimalDigit = prevInput.length - decimalIx;
                                    if (decimalDigit > 2) {
                                        return; //max 2 digit decimal
                                    }
                                }
                                prevInput += String.fromCharCode(key);
                            } else if (key == 190) {
                                if (prevInput.indexOf('.') > 0) {
                                    return;
                                }
                                if (prevInput.length == 0) {
                                    prevInput = '0';
                                }
                                prevInput += '.';
                            }
                            selectedCell.data(prevInput);
                            sheetFunction.RowUpdated(cell);
                            var idx = selectedCell.index().row;
                            sheetFunction.Recalculate(idx);
                        }
                        else if (key == 8 || key == 46) {
                            //backspace
                            var len = prevInput.length;
                            if (len > 0) {
                                prevInput = prevInput.substring(0, len - 1);
                                selectedCell.data(prevInput);
                                sheetFunction.RowUpdated(cell);
                                var idx = selectedCell.index().row;
                                sheetFunction.Recalculate(idx);
                            }
                        }
                    } else if (dataRoles[idx] == 'grade') {
                        var rowIdx = selectedCell.index().row;
                        if (key >= 48 && key <= 57) {
                            prevInput += String.fromCharCode(key);

                            selectedCell.data(prevInput);
                            sheetFunction.RowUpdated(cell);

                            sheet.cell(rowIdx, columnRoles["employee"]).data('');
                            sheet.cell(rowIdx, columnRoles["employeeOid"]).data('');
                        }
                        else if (key == 8 || key == 46) {
                            //backspace
                            var len = prevInput.length;
                            if (len > 0) {
                                prevInput = prevInput.substring(0, len - 1);
                                selectedCell.data(prevInput);
                                sheetFunction.RowUpdated(cell);
                            }

                            selectedCell.data(prevInput);
                            sheetFunction.RowUpdated(cell);

                            sheet.cell(rowIdx, columnRoles["employee"]).data('');
                            sheet.cell(rowIdx, columnRoles["employeeOid"]).data('');
                        }
                    } else if (dataRoles[idx] == 'employee' || dataRoles[idx] == 'location' || dataRoles[idx] == 'job') {
                        if (key == 8 || key == 46) {
                            var rowIndex = selectedCell.index().row;
                            sheet.cell(rowIndex, columnRoles['employee']).data('');
                        }
                    }

                    consumeKeyDown = true;
                })
                .on('key-focus', function (e, datatable, cell) {
                    console.log('key-focus 1');
                    selectedCell = cell;
                    prevInput = '';
                    var idx = cell.index().row;
                    $('.selected1').removeClass('selected1');
                    var rowId = sheet.row(idx).node().id;
                    $('#' + rowId).addClass('selected1');
                    if ($('#' + rowId).hasClass('new') == true && $('#' + rowId).hasClass('final') == false) {
                        $('#btn-sheet-delete').prop('disabled', false);
                    } else {
                        $('#btn-sheet-delete').prop('disabled', true);
                    }
                })
                .on('key-refocus', function (e, datatable, cell, originalEvent) {
                    console.log('key-refocus');
                    if (IsReadOnly()) { return; }
                    if (consumeKeyDown) {
                        consumeKeyDown = false;
                        return;
                    }
                    sheetFunction.CellRefocus(cell);
                })
                .on('key-blur', function (e, datatable, cell) {
                    var el = $('#sheet-input');
                    if (el.length) {
                        cell.data(el.val());
                        sheetFunction.EnableKeys();
                        if (prevValue != el.val()) {
                            sheetFunction.RowUpdated(cell);
                            var idx = selectedCell.index().row;
                            sheetFunction.Recalculate(idx);
                        }
                    }
                })
                .on('order.dt search.dt', function () {
                    let i = 1;

                    sheet
                        .cells(null, 0, { search: 'applied', order: 'applied' })
                        .every(function (cell) {
                            this.data(i++);
                        });
                })
                .draw();


            gridSummary = $('#gridSummary').DataTable({
                dom: 'tp',
                scrollX: true,
                columns: [
                    {
                        data: "grade",
                        name: "Grade",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('ps-2');
                        }
                    },
                    {
                        data: "bac",
                        name: "Bac",
                        autoWidth: true,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-end');
                        }
                    },
                    {
                        data: "prevEac",
                        name: "PrevEac",
                        autoWidth: true,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-end');
                        }
                    },
                    {
                        data: "acwp",
                        name: "Acwp",
                        autoWidth: true,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-end');
                        }
                    },
                    {
                        data: "eac",
                        name: "Eac",
                        autoWidth: true,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-end');
                        }
                    },
                    {
                        data: "etc",
                        name: "Etc",
                        autoWidth: true,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-end');
                        }
                    },
                    {
                        data: "v1",
                        name: "V1",
                        autoWidth: true,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-end');
                        }
                    },
                    {
                        data: "v2",
                        name: "V2",
                        autoWidth: true,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('text-end pe-2');
                        }
                    }
                ],

            });

            sheetFunction.RecalculateAllSummary();
            
        }),
        MergeOpen: (() => {
            var idx = selectedCell.index().row;
            var sourceOid = sheet.row(idx).node().id;

            //console.log('RowID >> ', oid);
            var employeeIndex = dataRoles.indexOf("employeeOid");
            var employeeNameIndex = dataRoles.indexOf("employee");
            var departmentIndex = dataRoles.indexOf("departmentOid");
            var deptNameIndex = dataRoles.indexOf("department");
            var gradeIndex = dataRoles.indexOf("grade");
            var jobNameIndex = dataRoles.indexOf("job");
            var jobIndex = dataRoles.indexOf("jobOid");
            var jobCodeIndex = dataRoles.indexOf("jobCode");
            var locationIndex = dataRoles.indexOf("location");
            var bacIndex = dataRoles.indexOf('bac')

            var deptOid = sheet.cell(idx, departmentIndex).data();
            var dept = sheet.cell(idx, deptNameIndex).data();
            var grade = sheet.cell(idx, gradeIndex).data();
            var jobOid = sheet.cell(idx, jobIndex).data();
            var location = sheet.cell(idx, locationIndex).data();


            var html = '';
            console.log('Source >> ' + deptOid + ' >> ' + jobOid + ' >> ' + grade + ' >> ' + ' >> ' + location);
            for (let ix = 0; ix < sheet.rows().count(); ix++) {
                var targetDeptOid = sheet.cell(ix, departmentIndex).data();
                var targetEmpOid = sheet.cell(ix, employeeIndex).data();
                var targetGrade = sheet.cell(ix, gradeIndex).data();
                var targetJobOid = sheet.cell(ix, jobIndex).data();
                var targetLocation = sheet.cell(ix, locationIndex).data();

                if (targetEmpOid != '' && targetDeptOid == deptOid && jobOid == targetJobOid && grade == targetGrade && 
                    location == targetLocation ) {
                    var targetOid = sheet.row(ix).node().id;
                    var targetEmployee = sheet.cell(ix, employeeNameIndex).data();
                    html += '<tr>';
                    html += `<td><a href="javascript:sheetFunction.MergeConfirm('` + sourceOid + `','` + targetOid + `')" class="btn btn-link ps-1 pe-1">Merge</a></td>`;
                    html += '<td class="align-middle">' + targetEmployee + '</td>';
                    html += '</tr>';
                }
            }
            $('#merge-target').html(html);

            $('#merge-bac').html(sheet.cell(idx, bacIndex).data());
            $('#merge-dept').html(dept);
            $('#merge-grade').html(grade);
            $('#merge-job').html(sheet.cell(idx, jobNameIndex).data());
            $('#merge-location').html(sheet.cell(idx, locationIndex).data());
            $('#modalMerge').modal('show');
        }),
        MergeConfirm: ((sourceOid, targetOid) => {
            LoadingScreenFunction.Show();
            $.ajax({
                type: "POST",
                url: PATHBASE + '/api/McsSheet/Merge',
                headers: { 'RequestVerificationToken': token },
                data: {
                    sourceOid: sourceOid,
                    targetOid: targetOid
                },
                success: function (objRes) {
                    LoadingScreenFunction.Hide();
                    if (objRes.result != 0) {
                        Swal.fire(objRes.message, '', 'error')
                    } else {
                        Swal.fire('Data has been saved!', '', 'success')
                            .then(okay => {
                                if (okay) {
                                    location.reload();
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
        ToggleColumn: ((dataRole, enable) => {
            var index = dataRoles.indexOf(dataRole);
            if (enable) {
                sheet.column(index).visible(true);
            } else {
                sheet.column(index).visible(false);
        }
        }),
        CellRefocus: ((cell) => {
            var idx = cell.index().column;

            console.log('cell refocus >> ', dataRoles[idx]);
            if (dataRoles[idx] == 'input') {
                var el = $('#sheet-input');
                if (el.length) {
                    cell.data(el.val());
                    sheet.keys.enable();
                } else {
                    sheet.keys.disable();
                    prevValue = cell.data();
                    var input = `<input onblur="sheetFunction.EnableKeys()" type="number" min="0" step="0.01" onkeydown="return sheetFunction.CellInputOnKeyDown(event);" id="sheet-input" name="sheet-input" value="` + cell.data() + `" class="form-control w-100 p-0 m-0" />`;
                    cell.data(input);
                    $('#sheet-input').focus().select();
                }
            }
            else if (dataRoles[idx] == 'employee') {
                $('#modalEmployee').on('shown.bs.modal', function () {
                    // Do something when the modal is shown
                    sheet.keys.disable();
                    $('#grid-modal_length select').focus();
                });
                $('#modalEmployee').on('hidden.bs.modal', function () {
                    // Do something when the modal is dismissed
                    sheet.keys.enable();
                });
                if (gridModal == null) {
                    gridModal = $('#grid-modal').DataTable({
                        processing: true,
                        serverSide: true,
                        scrollX: true,
                        ajax: {
                            "url": PATHBASE + '/Projects/Session?Handler=RefreshDataEmployee',
                            "type": "POST",
                            "datatype": "json",
                            "headers": { 'RequestVerificationToken': token },
                            "data": function (d) {
                                var rowIndex = selectedCell.index().row;
                                var departmentOid = sheet.cell(rowIndex, columnRoles["departmentOid"]).data();
                                var jobOid = sheet.cell(rowIndex, columnRoles["jobOid"]).data();
                                var grade = sheet.cell(rowIndex, columnRoles["grade"]).data();
                                d.employee = {
                                    DepartmentOid: departmentOid,
                                    Grade: grade
                                }

                                console.log('dataEmployee >> ', d.employee);
                            }
                        },
                        columnDefs: [{
                            "targets": [0],
                            "visible": true,
                            "searchable": false,
                            "orderable": false
                        }
                        ],
                        order: [1, 'asc'],
                        columns: [
                            {
                                data: null,
                                render: function (data, type, row, node) {
                                    var button = `<span class="text-nowrap d-inline-block"><button rel="tooltip" type="button" data-bs-placement="top" title="Select" class="btn btn-sm btn-soft-success me-1 mb-1 add" onclick="sheetFunction.SelectEmployee('` + node.row + `')">`;
                                    button += '<i class="fas fa-plus"> </i>';
                                    button += '</button>';
                                    return button;
                                },
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            },
                            {
                                data: "nip",
                                name: "Nip",
                                autoWidth: true,
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            },
                            {
                                data: "name",
                                name: "Name",
                                autoWidth: true,
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            },
                            {
                                data: "grade",
                                name: "Grade",
                                autoWidth: true,
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            },
                            {
                                data: "jobName",
                                name: "JobName",
                                autoWidth: true,
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            },
                            {
                                data: "departmentName",
                                name: "DepartmentName",
                                autoWidth: true,
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            }
                        ],
                        initComplete: function (settings, json) {
                            $('#modalEmployee').modal('show');
                        }
                    })
                } else {
                    gridModal.ajax.reload(null, false);
                    $('#modalEmployee').modal('show');
                }
                    
                
            }
            else if (dataRoles[idx] == 'location') {
                $('#modalLocation').on('shown.bs.modal', function () {
                    // Do something when the modal is shown
                    sheet.keys.disable();
                    $('#grid-modal-location_length select').focus();
                });
                $('#modalLocation').on('hidden.bs.modal', function () {
                    // Do something when the modal is dismissed
                    sheet.keys.enable();
                });
                if (gridModalLocation == null) {
                    gridModalLocation = $('#grid-modal-location').DataTable({
                        processing: true,
                        serverSide: true,
                        scrollX: true,
                        ajax: {
                            "url": PATHBASE + '/Projects/Session?Handler=RefreshDataLocation',
                            "type": "POST",
                            "datatype": "json",
                            "headers": { 'RequestVerificationToken': token },
                        },
                        columnDefs: [{
                            "targets": [0],
                            "visible": true,
                            "searchable": false,
                            "orderable": false
                        }
                        ],
                        order: [1, 'asc'],
                        columns: [
                            {
                                data: null,
                                render: function (data, type, row, node) {
                                    var button = `<span class="text-nowrap d-inline-block"><button rel="tooltip" type="button" data-bs-placement="top" title="Select" class="btn btn-sm btn-soft-success me-1 mb-1 add" onclick="sheetFunction.SelectLocation('` + node.row + `')">`;
                                    button += '<i class="fas fa-plus"> </i>';
                                    button += '</button>';
                                    return button;
                                },
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            },
                            {
                                data: "name",
                                name: "Name",
                                autoWidth: true,
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            }
                        ],
                        initComplete: function (settings, json) {
                            $('#modalLocation').modal('show');
                        }
                    })
                } else {
                    gridModalLocation.ajax.reload();
                    $('#modalLocation').modal('show');
                }
            }
            else if (dataRoles[idx] == 'department') {
                $('#modalDepartment').on('shown.bs.modal', function () {
                    // Do something when the modal is shown
                    sheet.keys.disable();
                    $('#grid-modal-department_length select').focus();
                });
                $('#modalDepartment').on('hidden.bs.modal', function () {
                    // Do something when the modal is dismissed
                    sheet.keys.enable();
                });
                if (gridModalDepartment == null) {
                    gridModalDepartment = $('#grid-modal-department').DataTable({
                        processing: true,
                        serverSide: true,
                        scrollX: true,
                        ajax: {
                            "url": PATHBASE + '/Projects/Session?Handler=RefreshDataDepartment',
                            "type": "POST",
                            "datatype": "json",
                            "headers": { 'RequestVerificationToken': token },
                        },
                        columnDefs: [{
                            "targets": [0],
                            "visible": true,
                            "searchable": false,
                            "orderable": false
                        }
                        ],
                        order: [1, 'asc'],
                        columns: [
                            {
                                data: null,
                                render: function (data, type, row, node) {
                                    var button = `<span class="text-nowrap d-inline-block"><button rel="tooltip" type="button" data-bs-placement="top" title="Select" class="btn btn-sm btn-soft-success me-1 mb-1 add" onclick="sheetFunction.SelectDepartment('` + node.row + `')">`;
                                    button += '<i class="fas fa-plus"> </i>';
                                    button += '</button>';
                                    return button;
                                },
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            },
                            {
                                data: "description",
                                name: "Description",
                                autoWidth: true,
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            }
                        ],
                        initComplete: function (settings, json) {
                            $('#modalDepartment').modal('show');
                        }
                    })
                } else {
                    gridModalDepartment.ajax.reload();
                    $('#modalDepartment').modal('show');
                }
            }
            else if (dataRoles[idx] == 'job') {
                $('#modalJob').on('shown.bs.modal', function () {
                    // Do something when the modal is shown
                    sheet.keys.disable();
                    $('#grid-job-position_length select').focus();
                    gridModalJob.columns.adjust();
                });
                $('#modalJob').on('hidden.bs.modal', function () {
                    // Do something when the modal is dismissed
                    sheet.keys.enable();
                });
                if (gridModalJob == null) {
                    gridModalJob = $('#grid-modal-job').DataTable({
                        processing: true,
                        serverSide: true,
                        scrollX: true,
                        ajax: {
                            "url": PATHBASE + '/Projects/Session?Handler=RefreshDataJob',
                            "type": "POST",
                            "datatype": "json",
                            "headers": { 'RequestVerificationToken': token },
                        },
                        columnDefs: [{
                            "targets": [0],
                            "visible": true,
                            "searchable": false,
                            "orderable": false
                        }
                        ],
                        order: [1, 'asc'],
                        columns: [
                            {
                                data: null,
                                render: function (data, type, row, node) {
                                    var button = `<span class="text-nowrap d-inline-block"><button rel="tooltip" type="button" data-bs-placement="top" title="Select" class="btn btn-sm btn-soft-success me-1 mb-1 add" onclick="sheetFunction.SelectJob('` + node.row + `')">`;
                                    button += '<i class="fas fa-plus"> </i>';
                                    button += '</button>';
                                    return button;
                                },
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            },
                            {
                                data: "jobCode",
                                name: "JobCode",
                                autoWidth: true,
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            },
                            {
                                data: "jobPosition",
                                name: "JobPosition",
                                autoWidth: true,
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            },
                            {
                                data: "type",
                                name: "Type",
                                autoWidth: true,
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            },
                            {
                                data: "location",
                                name: "Location",
                                autoWidth: true,
                                createdCell: function (td, cellData, rowData, row, col) {
                                    $(td).addClass('align-middle text-start Edit');
                                }
                            }
                        ],
                        initComplete: function (settings, json) {
                            $('#modalJob').modal('show');
                        }
                    })
                } else {
                    gridModalJob.ajax.reload();
                    $('#modalJob').modal('show');
                }
            }
        }),
        Recalculate: ((row) => {
            var colIx = selectedCell.index().column;
            if (dataRoles[colIx] == 'input') {
                var sum = 0;
                //probably it is a month
                for (var i = 0; i < sheet.rows().count(); i++) {
                    console.log('data : ' + sheet.cell(i, colIx).data())
                    sum += Number(sheet.cell(i, colIx).data());
                }
                $('#footer-' + colIx).html(sum.toFixed(2));
            }

            var selectedRow = sheet.row(row);
            var prevData = selectedRow.data();
            var deltaData = {
                bac:0, prevEac:0, acwp:0, eac:0, etc:0
            }
            deltaData.bac = prevData[dataRoles.indexOf('bac')];
            deltaData.prevEac = prevData[dataRoles.indexOf('prevEac')];
            deltaData.acwp = prevData[dataRoles.indexOf('acwp')];
            deltaData.eac = prevData[dataRoles.indexOf('eac')];
            deltaData.etc = prevData[dataRoles.indexOf('etc')];

            var etc = 0;
            var bac = 0;
            var acwp = 0;
            var prevEac = 0;
            var acwpIndex = -1;
            for (let i = 0; i < dataRoles.length; i++) {
                if (dataRoles[i] == 'input') {
                    var cell = sheet.cell(row, i).node();
                    if (cell.classList.contains('readonly')) {
                        acwpIndex = i;
                    } else {
                        etc += Number(sheet.cell(row, i).data());
                    }
                }
            }

            if (acwpIndex < 0) {
                bac = etc;
                var index = dataRoles.indexOf('bac');
                if (index > -1) {
                    sheet.cell(row, index).data(bac.toFixed(2));
                }

                index = dataRoles.indexOf('prevEac');
                if (index > -1) {
                    sheet.cell(row, index).data(bac.toFixed(2));
                }

                index = dataRoles.indexOf('acwp');
                acwp = Number(sheet.cell(row, index).data());

                index = dataRoles.indexOf('etc');
                if (index > -1) {
                    sheet.cell(row, index).data(etc.toFixed(2));
                }
                var eac = acwp + etc;
                
                index = dataRoles.indexOf('eac');
                if (index > -1) {
                    sheet.cell(row, index).data(eac.toFixed(2));
                }
            } else {
                var acwp = Number(sheet.cell(row, columnRoles['acwp']).data());
                sheet.cell(row, columnRoles['etc']).data(etc.toFixed(2));
                var eac = acwp + etc;
                var index = dataRoles.indexOf('eac');
                if (index > -1) {
                    sheet.cell(row, index).data(eac.toFixed(2));
                }
            }

            if (masterSessionStatusId == 0) { //this is session 0, prevEac = bac
                prevEac = bac;
            } else {
                bac = deltaData.bac;
                prevEac = deltaData.prevEac;
            }
            var grade = prevData[dataRoles.indexOf('grade')];
            deltaData.bac = bac - deltaData.bac;
            deltaData.prevEac = prevEac - deltaData.prevEac;
            deltaData.acwp = acwp - deltaData.acwp;
            deltaData.eac = eac - deltaData.eac;
            deltaData.etc = etc - deltaData.etc;
            sheetFunction.RecalculateSummary(grade, deltaData);
        }),
        RecalculateAllSummary: (() => {
            gridSummary.clear();

            var grades = [];

            var indexGrade = dataRoles.indexOf('grade');
            var indexBac = dataRoles.indexOf('bac');
            var indexPrevEac = dataRoles.indexOf('prevEac');
            var indexAcwp = dataRoles.indexOf('acwp');
            var indexEac = dataRoles.indexOf('eac');
            var indexEtc = dataRoles.indexOf('etc');
            var rowsData = sheet.rows().data();
            for (var i = 0; i < rowsData.length; i++) {
                var dt = rowsData[i];
                var grade = dt[indexGrade];
                if (grades[grade] == null) {
                    grades[grade] = { bac: 0, prevEac: 0, acwp: 0, eac: 0, etc: 0 };
                }
                grades[grade].bac += Number(dt[indexBac]);
                grades[grade].prevEac += Number(dt[indexPrevEac]);
                grades[grade].acwp += Number(dt[indexAcwp]);
                grades[grade].eac += Number(dt[indexEac]);
                grades[grade].etc += Number(dt[indexEtc]);
            }

            for (let [index, val] of grades.entries()) {
                if (val != null) {
                    gridSummary.row.add({
                        grade: index,
                        bac: val.bac.toFixed(2),
                        prevEac: val.prevEac.toFixed(2),
                        acwp: val.acwp.toFixed(2),
                        eac: val.eac.toFixed(2),
                        etc: val.etc.toFixed(2),
                        v1: (val.eac - val.bac).toFixed(2),
                        v2: (val.eac - val.prevEac).toFixed(2),
                    }).draw();
                }
                /*else {
                    gridSummary.row.add({
                        grade: index,
                        bac: 0,
                        prevEac: 0,
                        acwp: 0,
                        eac: 0,
                        etc: 0
                    }).draw();
                }*/
            }
        }),
        RecalculateSummary: ((deltaGrade, delta) => {
            var newRow = true;
            for (var i = 0; i < gridSummary.rows().count(); i++) {
                var data = gridSummary.row(i).data();
                if (data.grade == deltaGrade) {
                    newRow = false;
                    var bac = Number(data.bac) + Number(delta.bac);
                    data.bac = bac.toFixed(2);
                    var acwp = Number(data.acwp) + Number(delta.acwp);
                    data.acwp = acwp.toFixed(2);
                    var prevEac = Number(data.prevEac) + Number(delta.prevEac);
                    data.prevEac = prevEac.toFixed(2);
                    var etc = Number(data.etc) + Number(delta.etc);
                    data.etc = etc.toFixed(2);
                    var eac = Number(data.eac) + Number(delta.eac);
                    data.eac = eac.toFixed(2);
                    var v1 = Number(data.eac) - Number(data.bac);
                    data.v1 = v1.toFixed(2);
                    var v2 = Number(data.eac) - Number(data.prevEac);
                    data.v2 = v2.toFixed(2);

                    gridSummary.row(i).data(data).draw();
                    var el = gridSummary.row(i).node();
                    el.classList.add('updated');
                    break;
                }
            }

            if (newRow) {
                sheetFunction.RecalculateAllSummary();
            }
        }),
        RowUpdated: ((cell) => {
            var idx = selectedCell.index().row;
            var selectedRow = sheet.row(idx);
            $('#' + selectedRow.id()).addClass('updated');
        }),
        RowReset: ((cell) => {
            var idx = selectedCell.index().row;
            var selectedRow = sheet.row(idx);
            $('#' + selectedRow.id()).removeClass('updated');
        }),
        CellBlur: (() => {
            console.log('CellBlur >> ', selectedCell.data());
            var el = $('#sheet-input');
            if (el.length) {
                var text = el.val();
                var decimalIx = text.indexOf('.');
                if (decimalIx > 0) {
                    var delta = decimalIx + 3;
                    if (delta < text.length) {
                        text = text.substring(0, delta);
                    }
                } else if (decimalIx == 0) {
                    text = '0' + text;
                }

                sheetFunction.EnableKeys();
                selectedCell.data(text);
                if (prevValue != text) {
                    sheetFunction.RowUpdated(selectedCell);
                    var idx = selectedCell.index().row;
                    sheetFunction.Recalculate(idx);
                }

                console.log('CellBlur 1 >> ', selectedCell.data());
            }
        }),
        EnableKeys: (() => {
            sheet.keys.enable();
        }),
        CellInputOnKeyDown: ((e) => {
            if (e.keyCode == 13) {
                sheetFunction.CellBlur();
                consumeKeyDown = false;
            }
            return true;
        }),
        SelectEmployee: ((row) => {
            rowId = row || -1;
            var idx = selectedCell.index().row;
            var employeeIndex = dataRoles.indexOf("employeeOid");
            var departmentIndex = dataRoles.indexOf("departmentOid");
            var deptNameIndex = dataRoles.indexOf("department");
            var gradeIndex = dataRoles.indexOf("grade");
            var jobNameIndex = dataRoles.indexOf("job");
            var jobIndex = dataRoles.indexOf("jobOid");
            var jobCodeIndex = dataRoles.indexOf("jobCode");
            if (rowId == -1) {
                selectedCell.data('TBN');
                sheet.cell(idx, employeeIndex).data('');
                //sheet.cell(idx, departmentIndex).data('');
                //sheet.cell(idx, deptNameIndex).data('');
                //sheet.cell(idx, gradeIndex).data('');
                //sheet.cell(idx, jobNameIndex).data('');
                //sheet.cell(idx, jobCodeIndex).data('');
                sheet.cell(idx, jobIndex).data('');
            } else {
                var rowData = gridModal.row(rowId).data();

                selectedCell.data(rowData.nip + ' - ' + rowData.name);

                if (sheetFunction.IsDataInvalid(rowData.oid)) {
                    Swal.fire('Fail', 'CSC dan Employee already exist!', 'info');
                    return;
                } else {
                    sheet.cell(idx, employeeIndex).data(rowData.oid);
                    sheet.cell(idx, departmentIndex).data(rowData.departmentOid);
                    sheet.cell(idx, deptNameIndex).data(rowData.departmentName);
                    sheet.cell(idx, gradeIndex).data(rowData.grade);
                    //sheet.cell(idx, jobNameIndex).data(rowData.jobCode + ' - ' + rowData.jobName);
                    //sheet.cell(idx, jobIndex).data(rowData.jobOid);
                    //sheet.cell(idx, jobCodeIndex).data(rowData.jobCode);
                }
            }

            sheetFunction.RowUpdated(selectedCell);
            $('#modalEmployee').modal('hide');

            var keyTable = new $.fn.dataTable.KeyTable(sheet);
            keyTable.focus(idx, selectedCell.index().column);
        }),
        SelectLocation: ((row) => {
            rowId = row || -1;
            var rowData = gridModalLocation.row(rowId).data();
            var idx = selectedCell.index().row;
            var colIndex = dataRoles.indexOf("location");
            var colOidIndex = dataRoles.indexOf("locationOid");

            if (rowId == -1) {
                selectedCell.data('');
                sheet.cell(idx, colOidIndex).data('');
                sheet.cell(idx, colIndex).data('');
            } else {
                selectedCell.data(rowData.name);
                sheet.cell(idx, colOidIndex).data(rowData.oid);
                sheet.cell(idx, colIndex).data(rowData.name);
            }
            sheetFunction.RowUpdated(selectedCell);
            $('#modalLocation').modal('hide');

            var keyTable = new $.fn.dataTable.KeyTable(sheet);
            keyTable.focus(idx, selectedCell.index().column);
            
        }),
        SelectJob: ((row) => {
            rowId = row || -1;
            var rowData = gridModalJob.row(rowId).data();
            var idx = selectedCell.index().row;

            if (rowId == -1) {
                selectedCell.data('');
                sheet.cell(idx, columnRoles["jobOid"]).data('');
                sheet.cell(idx, columnRoles["jobCode"]).data('');
                sheet.cell(idx, columnRoles["job"]).data('');
            } else {
                selectedCell.data(rowData.jobPosition);
                sheet.cell(idx, columnRoles["jobOid"]).data(rowData.oid);
                sheet.cell(idx, columnRoles["jobCode"]).data(rowData.jobCode);
                sheet.cell(idx, columnRoles["job"]).data(rowData.jobCode + " - " + rowData.jobPosition);
            }
            sheetFunction.RowUpdated(selectedCell);
            $('#modalJob').modal('hide');

            var keyTable = new $.fn.dataTable.KeyTable(sheet);
            keyTable.focus(idx, selectedCell.index().column);

        }),
        SelectDepartment: ((row) => {
            rowId = row || -1;
            var rowData = gridModalDepartment.row(rowId).data();
            var idx = selectedCell.index().row;

            if (rowId == -1) {
                selectedCell.data('');
                sheet.cell(idx, columnRoles["departmentOid"]).data('');
                sheet.cell(idx, columnRoles["department"]).data('');
                sheet.cell(idx, columnRoles["employee"]).data('TBN');
                sheet.cell(idx, columnRoles["employeeOid"]).data('');
            } else {
                selectedCell.data(rowData.jobPosition);
                sheet.cell(idx, columnRoles["departmentOid"]).data(rowData.oid);
                sheet.cell(idx, columnRoles["department"]).data(rowData.description);
                sheet.cell(idx, columnRoles["employee"]).data('TBN');
                sheet.cell(idx, columnRoles["employeeOid"]).data('');
            }
            sheetFunction.RowUpdated(selectedCell);
            $('#modalDepartment').modal('hide');

            var keyTable = new $.fn.dataTable.KeyTable(sheet);
            keyTable.focus(idx, selectedCell.index().column);

        }),
        GetSheet: (() => {
            return sheet;
        }),
        AddRow: (() => {
            consumeKeyDown = true;
            var guid = "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
                (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
            );
            var row = `<tr id="` + guid + `" class="new">`;
            row += `<td>`;
            row += sheet.rows().count() + 1;
            row += `</td>`;
            for (let i = 1; i < sheet.columns().count(); i++) {
                
                row += "<td>";
                if (dataRoles[i] == 'input' || dataRoles[i] == 'acwp') {
                    row += "0.00";
                } else if (dataRoles[i] == 'employee') {
                    row += "TBN";
                }
                row += "</td>";
            }
            row += '</tr>';
            var newIndex = sheet.rows().count();
            sheet.row.add($(row)).draw();
            sheetFunction.Recalculate(newIndex);
            sheet.cell(newIndex, columnRoles["employee"]).data('TBN');
            var keyTable = new $.fn.dataTable.KeyTable(sheet);
            keyTable.focus(newIndex, 1);
        }),
        GetUpdatedData: ((projectSessionOid) => {
            var data = [];
            var index = 0;
            var id = sheet.tables().nodes().to$().attr('id');

            var colEmployeeOid = dataRoles.indexOf("employeeOid");
            var colDepartmentOid = dataRoles.indexOf("departmentOid");
            var colDepartment = dataRoles.indexOf("department");
            var colLocation = dataRoles.indexOf("location");
            var colLocationOid = dataRoles.indexOf("locationOid");
            var colJobOid = dataRoles.indexOf("jobOid");
            var colJob = dataRoles.indexOf("job");
            var colGrade = dataRoles.indexOf("grade");
            var colInput = dataRoles.indexOf("input");
            var colJobCode = dataRoles.indexOf("jobCode");
            $('#' + id + ' tbody tr.updated').each(function () {
                var that = $(this);
                var oid = $(this)[0].id;

                var rowIndex = sheet.row('#' + oid).index();
                var periods = [];
                for (let i = 0; i < projectLastPeriod; i++) {
                    var value = parseFloat(sheet.cell(rowIndex, colInput + i).data()).toFixed(2);
                    periods[i] = {
                        Period: i,
                        Value: value
                    };
                }

                var employeeOid = sheet.cell(rowIndex, colEmployeeOid).data();
                var departmentOid = sheet.cell(rowIndex, colDepartmentOid).data();
                var departmentName = sheet.cell(rowIndex, colDepartment).data();
                var jobOid = sheet.cell(rowIndex, colJobOid).data();
                var jobName = sheet.cell(rowIndex, colJob).data();
                var jobCode = sheet.cell(rowIndex, colJobCode).data();
                var locationOid = sheet.cell(rowIndex, colLocationOid).data();
                var locationName = sheet.cell(rowIndex, colLocation).data();
                var grade = sheet.cell(rowIndex, colGrade).data();

                data[index] = {
                    Oid: oid,
                    ProjectSessionOid: projectSessionOid,
                    EmployeeOid: employeeOid,
                    DepartmentOid: departmentOid,
                    DepartmentName: departmentName,
                    LocationOid: locationOid,
                    LocationName: locationName,
                    JobOid: jobOid,
                    JobName: jobName,
                    JobCode: jobCode,
                    Periods: periods,
                    Grade: grade
                }
                index++;
            });
            return data;
        }),
        GetRejectedData: ((projectSessionOid) => {
            var data = [];
            var index = 0;
            var id = sheet.tables().nodes().to$().attr('id');
            $('#' + id + ' tbody tr.updated').each(function () {
                var oid = $(this)[0].id;

                data[index] = oid
                
                index++;
            });
            return data;
        }),
        DeleteRow: (() => {
            var idx = selectedCell.index().row;
            var oid = sheet.row(idx).node().id;
            deletedRowOids.push(oid);
            sheet.row(idx).remove().draw();

        }),
        GetDeletedOids: (() => {
            return deletedRowOids;
        }),
        ResetDeletedOids: (() => {
            deletedRowOids = [];
        }),
        IsDataInvalid: ((newEmployee) => {
            var cscIndex = sheet.columns().nodes().length - 1;
            var employeeIndex = sheet.columns().nodes().length - 2;
            var selectedRow = selectedCell.index().row;
            var newCsc = sheet.cell(selectedRow, cscIndex).data();
            var numRows = sheet.rows().nodes().length;
            for (let i = 0; i < numRows; i++) {
                if (i != selectedRow) {
                    var csc = sheet.cell(i, cscIndex).data();
                    var employee = sheet.cell(i, employeeIndex).data();
                    //alert(i + ' : ' + selectedRow + ' : ' + employee + ' : ' + newEmployee);
                    if (employee == newEmployee && csc == newCsc) {
                        return true;
                    }
                }
            }
            return false;
        }),
        ImportOpen: (() => {
            $('#modalImport').modal('show');
        }),
        ImportTemplate: (() => {
            LoadingScreenFunction.Show();
            $.ajax({
                type: "GET",
                url: PATHBASE + '/api/McsSheet/Template?oid=' + projectSessionOid + "&lastPeriod=" + projectLastPeriod,
                headers: { 'RequestVerificationToken': token },
                traditional: true,
                xhrFields: {
                    responseType: 'blob'
                },
                contentType: false,
                processData: false,
                success: function (objRes) {
                    LoadingScreenFunction.Hide();
                    var a = document.createElement('a');
                    var url = window.URL.createObjectURL(objRes);
                    a.href = url;
                    a.download = 'template.xlsx';
                    document.body.append(a);
                    a.click();
                    a.remove();
                    window.URL.revokeObjectURL(url);
                },
                error: function (response) {
                    LoadingScreenFunction.Hide();
                    alert('Error function please contact developer');
                }
            })
        }),
        ImportSave: (() => {
            LoadingScreenFunction.Show();
            var fileExtension = ['xls', 'xlsx', 'xlsm'];
            var filename = $('#fileupload').val();
            if (filename.length == 0) {
                LoadingScreenFunction.Hide();
                $('#fileupload').addClass('is-invalid');
                alert("Please select a file.");
                return false;
            }
            else {
                var extension = filename.replace(/^.*\./, '');
                if ($.inArray(extension, fileExtension) == -1) {
                    LoadingScreenFunction.Hide();
                    $('#fileupload').addClass('is-invalid');
                    alert("Please select only excel files.");
                    return false;
                }
            }
            var fdata = new FormData();
            var fileUpload = $("#fileupload").get(0);
            var files = fileUpload.files;
            fdata.append(files[0].name, files[0]);
            fdata.append('projectSessionOid', projectSessionOid);
            $.ajax({
                type: "POST",
                url: PATHBASE + "/api/McsSheet/",
                headers: { 'RequestVerificationToken': token },
                data: fdata,
                contentType: false,
                processData: false,
                success: function (objRes) {
                    LoadingScreenFunction.Hide();
                    if (objRes.result != 0) {
                        Swal.fire(objRes.message, '', 'error')
                            .then(_ => {
                                $('#textImportError').val(objRes.data.join("\n"));
                                $('#modalImportError').modal('show');
                            });
                    } else {
                        $('#modalImport').modal('toggle');
                        Swal.fire('Data has been saved!', '', 'success')
                            .then(okay => {
                                if (okay) {
                                    window.location.reload();
                                }
                            });
                    }
                },
                error: function (e) {
                    LoadingScreenFunction.Hide();
                    alert('Error function please contact developer');
                }
            });
        }),
        Export: (() => {
            LoadingScreenFunction.Show();
            $.ajax({
                type: "GET",
                url: PATHBASE + '/api/McsSheet/Export?oid=' + projectSessionOid + "&lastPeriod=" + projectLastPeriod + "&isPM=" + options['isPM'],
                headers: { 'RequestVerificationToken': token },
                traditional: true,
                xhrFields: {
                    responseType: 'blob'
                },
                contentType: false,
                processData: false,
                success: function (objRes) {
                    LoadingScreenFunction.Hide();
                    var a = document.createElement('a');
                    var url = window.URL.createObjectURL(objRes);
                    a.href = url;
                    a.download = 'export.xlsx';
                    document.body.append(a);
                    a.click();
                    a.remove();
                    window.URL.revokeObjectURL(url);
                },
                error: function (response) {
                    LoadingScreenFunction.Hide();
                    alert('Error function please contact developer');
                }
            })
        }),
        ResetStatus: (() => {
            Swal.fire({
                title: 'Are you sure to reset the status?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, reset it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    LoadingScreenFunction.Show();
                    $.ajax({
                        type: "POST",
                        url: PATHBASE + "/api/McsSheet/ResetStatus/",
                        headers: { 'RequestVerificationToken': token },
                        data: {
                            projectSessionOid: projectSessionOid
                        },
                        success: function (objRes) {
                            LoadingScreenFunction.Hide();
                            if (objRes.result != 0) {
                                Swal.fire('Failed', objRes.message, 'error')
                            } else {
                                Swal.fire('Reset!', 'Project session has been reset.', 'success');
                                window.location.reload();
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
        PMReject: ((el) => {
            if (el.value == 1) {
                sheetFunction.RowUpdated();
            } else {
                sheetFunction.RowReset();
            }
        })
    }
})();

var McsSheet = function (elementId, options) {
    this.elementId = elementId;
    this.options = options || {};
    console.log('mcsSheet options >> ', this.options);
    this.token = this.options['token'];
    if (this.token === undefined) {
        this.token = '';
    }
    this.editable = this.options['editable'];
    if (this.editable === undefined) {
        this.editable = true;
    }

    this.columns = [];
    this.columns[this.columns.length] = 'no readonly numeric';
    if (this.options['isPM']) {
        this.columns[this.columns.length] = 'action readonly';
    } else {
        this.columns[this.columns.length] = 'action readonly hide';
    }
    if (this.options['isAdmin'] || this.options['isPM']) {
        this.columns[this.columns.length] = 'dept';
    } else {
        this.columns[this.columns.length] = 'dept';
    }

    this.projectLastPeriod = this.options['projectLastPeriod'];
    if (this.projectLastPeriod === undefined) {
        this.projectLastPeriod = 0;
    }

    this.projectEndPeriod = this.options['projectEndPeriod'];
    if (this.projectEndPeriod === undefined) {
        this.projectEndPeriod = 0;
    }
    this.projectSessionOid = this.options['projectSessionOid'];

    this.projectSessionStatus = this.options['projectSessionStatus'];

    this.columns[this.columns.length] = 'employee';
    this.columns[this.columns.length] = 'grade numeric';
    this.columns[this.columns.length] = 'job';
    this.columns[this.columns.length] = 'location';
    this.columns[this.columns.length] = 'bac readonly numeric';
    this.columns[this.columns.length] = 'prevEac readonly numeric';
    this.columns[this.columns.length] = 'acwp readonly numeric';
    this.columns[this.columns.length] = 'etc readonly numeric';
    this.columns[this.columns.length] = 'eac readonly numeric';

    for (let i = 1; i <= this.projectLastPeriod; i++) {
        if (!this.editable) {
            this.columns[this.columns.length] = 'm' + i + ' readonly numeric';
        }
        else if (this.projectEndPeriod >= i) {
            this.columns[this.columns.length] = 'm' + i + ' readonly numeric';
        }
        else {
            this.columns[this.columns.length] = 'm' + i + ' input numeric';

        }
    }
    console.log('columns >> ', this.columns);
    var ix = 1;
    var jx = 0;
    var disableCols = [0];
    var hiddenCols = [];
    var numericCols = [];
    for (let i = 0; i < columns.length; i++) {
        if (columns[i].includes('readonly')) {
            disableCols[ix++] = i;
        }

        if (columns[i].includes('hide')) {
            hiddenCols[jx++] = i;
        }

        if (columns[i].includes('numeric')) {
            numericCols[numericCols.length] = i;
        }

    }
    //$('#' + this.elementId + '_wrapper.dataTables_scrollBody').css('max-height', '500px');
    $('#' + this.elementId).show();
    $('#div-filter').show();
    $('#loading-grid').hide();
    //$('#' + this.elementId).on('draw.dt', function () {
    //    $('#' + this.elementId).show();
    //    //alert('draw');
    //});
    this.grid = $('#' + this.elementId).DataTable({
        fixedHeader: {
            header: true,
            footer: true
        },
        dom: 'rftip',
        dom: "'<'d-flex justify-content-between'<'toolbar2'><''f>>rtip",
        scrollX: true,
        scrollY: true,
        keys: true,
        info: false,
        pageLength: 1000,
        aoColumnDefs: [
            {
                bSortable: false,
                aTargets: [0]
            }
        ],
        columnDefs: [
            {
                className: "align-middle",
                targets: "_all"
            },
            {
                className: "text-start",
                targets: [1, 2, 3, 4]
            },
            {
                target: [-1,-2,-3,-4,-5],
                visible: false,
                searchable: false
            },
            {
                className: "numeric",
                targets: numericCols
            },
            {
                targets: hiddenCols,
                visible: false,
                searchable: false
            },
            {
                className: "readonly",
                targets: disableCols
            },
        ],
        order: [[0, 'asc']],
        pagingType: $(window).width() < 768 ? "numbers" : "simple_numbers",
        initComplete: function (settings, json) {
            //alert('class : ' + $('#' + elementId + " tbody").attr('style'));
            $('#' + elementId + " tbody").show();
            var html = '';
            //console.log('status : ' + projectSessionStatus + ', options >> ', options);
            if (editable == true && projectSessionStatus == 0) {
                html = '';
                html += `<button rel="tooltip" data-bs-placement="top" data-container="body" data-bs-title="Import" class="btn btn-sm btn-primary me-1 mb-1" onclick="sheetFunction.ImportOpen()">` +
                    '<i class="fas fa-upload"> </i></button>';
                html += `<button rel="tooltip" data-bs-placement="top" data-container="body" data-bs-title="Add Row" class="btn btn-sm btn-success me-1 mb-1" onclick="sheetFunction.AddRow()">` +
                    '<i class="fas fa-add"> </i></button>';
                html += `<button id="btn-sheet-delete" rel="tooltip" data-bs-placement="top" data-container="body" data-bs-title="Delete Row" class="btn btn-sm btn-danger me-1 mb-1"onclick="sheetFunction.DeleteRow()" disabled>` +
                    '<i class="fas fa-minus"> </i></button>';

            } else {
                if (projectSessionStatus == 10 && !options['isPM']) { //ready to submit
                    html = '';
                    html += `<button rel="tooltip" data-bs-placement="top" data-container="body" data-bs-title="Reset to In Progress" class="btn btn-sm btn-danger me-1 mb-1" onclick="sheetFunction.ResetStatus()">` +
                        '<i class="fas fa-undo"> </i></button>';
                    html += '<button rel="tooltip" data-bs-placement="top" data-container="body" data-bs-title="Download Import Template" ' +
                        'class="btn btn-sm btn-primary me-1 mb-1" onclick = "sheetFunction.ImportTemplate()" >' +
                        '<i class="fas fa-download"> </i></button>';
                    //document.querySelector('div.toolbar2').innerHTML = html;
                } else if (projectSessionStatus == 100 ) {
                    html = '<button rel="tooltip" data-bs-placement="top" data-container="body" data-bs-title="Download Import Template" ' + 
                        'class="btn btn-sm btn-primary me-1 mb-1" onclick = "sheetFunction.ImportTemplate()" >' +
                        '<i class="fas fa-download"> </i></button>';
                    if (options['masterSessionStatusId'] == 1 && (options['isPM'] || options['isAdmin'])) {
                        html += `<button rel="tooltip" data-bs-placement="top" data-container="body" data-bs-title="Reset to PM Review" class="btn btn-sm btn-danger me-1 mb-1" onclick="sheetFunction.ResetStatus()">` +
                            '<i class="fas fa-undo"> </i></button>';
                    }
                }
            }

            html = `<button rel="tooltip" data-bs-placement="top" data-container="body" data-bs-title="Download XLSX" class="btn btn-sm btn-secondary me-1 mb-1" onclick="sheetFunction.Export()">` +
                '<i class="fas fa-download"> </i></button>' + html;
            document.querySelector('div.toolbar2').innerHTML = html;

            const tooltipTriggerList = document.querySelectorAll('[rel="tooltip"]');
            const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
            //$('body').tooltip({
            //    selector: '[rel=tooltip]',
            //});
            $('[rel="tooltip"]').click(function () {
                $('[rel="tooltip"]').tooltip("hide");
            });

        }
    });
    this.dataRoles = [];
    for (let i = 0; i < this.grid.columns().count(); i++) {
        var col = this.grid.column(i).header().getAttribute('data-role');
        this.dataRoles[this.dataRoles.length] = col;
    }

    sheetFunction.Init(this.token, this.grid, this.dataRoles,
        this.projectEndPeriod, this.projectSessionOid, this.projectLastPeriod, this.options['masterSessionStatusId']);
    
}