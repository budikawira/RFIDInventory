const searchInit = [];

$(document).ajaxComplete(function () {
    // Required for Bootstrap tooltips in DataTables
    $('body').tooltip({
        selector: '[rel=tooltip]',
    });
    $('[rel="tooltip"]').click(function () {
        $('[rel="tooltip"]').tooltip("hide");

    });
});

let setGridTables = function ({
    id,
    useFilter = false,
}) {
    if (useFilter) {
        $('#' + id + ' thead tr')
            .clone(true)
            .addClass('filters')
            .addClass('d-none')
            .attr('id', 'filters_rows')
            .appendTo('#' + id + ' thead');

        if (useFilter) {
            let nodeFilter = document.createElement("div");
            nodeFilter.className = 'float-end';
            let spanFilter = document.createElement("span");
            spanFilter.className = 'fas fa-filter';
            nodeFilter.append(spanFilter);

            $('#' + id).before(nodeFilter);

            $(nodeFilter).on("click", () => {
                let filters_rows = $('#filters_rows');
                filters_rows.toggleClass('d-none', 'd-block');
            });

            //#region create filters
            let headerFilter = $('.filters th');

            headerFilter.each(function (colIdx, col) {
                let cell = $(col);
                let dataCell = $(cell).data() || {};
                if (dataCell != undefined && dataCell.sort != undefined) {
                    searchInit.push({
                        name: dataCell.sort,
                        value: ''
                    });
                }
            });

            headerFilter.each(function (colIdx, col) {

                let cell = $(col);
                cell.removeClass();
                cell.addClass('align-middle ps-3');

                let title = $(cell).text();
                if (title != null)
                    title = title.trim();

                $(cell).css('padding', '0');

                let dataCell = $(cell).data() || {};

                let nodeHtml = '';
                switch (dataCell.role) {
                    case 'action':
                        break;
                    case 'combobox':
                        nodeHtml = document.createElement("select");
                        nodeHtml.className = "form-select form-select-sm";
                        nodeHtml.ariaLabel = title;

                        var option = document.createElement("option");
                        option.value = title;
                        option.text = title;
                        option.disabled = false;
                        nodeHtml.appendChild(option);

                        for (var i = 0; i < dataCell.listsearch.length; i++) {
                            option = document.createElement("option");
                            option.value = dataCell.listsearch[i];
                            option.text = dataCell.listsearch[i];
                            nodeHtml.appendChild(option);
                        }
                        break;
                    default:
                        nodeHtml = document.createElement("input");
                        nodeHtml.className = "form-control form-control-sm";
                        nodeHtml.setAttribute("type", "text");
                        nodeHtml.setAttribute("placeholder", title);
                        break;
                }
                $(cell).html(nodeHtml);

                if (dataCell.property != undefined) {
                    $(nodeHtml).on('keyup', function () {
                        var value = $(this).val().toLowerCase();
                        let idxSearch = searchInit.findIndex(x => x.name == dataCell.property);
                        searchInit[idxSearch].value = value;

                        $('#' + id + ' tbody tr').filter(function () {
                            var trNode = $(this);
                            let isFilter = true;
                            trNode.find("td").each(function (tdIdx, tdNode) {
                                searchInit.forEach(function (a, i) {
                                    console.log(searchInit[i].name.trim(), $(tdNode).text().trim());
                                    if (isFilter && $(tdNode).hasClass(searchInit[i].name.trim()))
                                        isFilter = $(tdNode).text().trim().toLowerCase().indexOf(searchInit[i].value) > -1
                                    // if (isFilter && $(tdNode).hasClass(searchInit[i].name))
                                    //     isFilter = ($(tdNode).text().toLowerCase().indexOf(searchInit[i].value) > -1)
                                })
                            });
                            trNode.toggle(isFilter)
                            // $(this).toggle($(this).find("td").text().toLowerCase().indexOf(value) > -1)
                            // $(this).toggle($(this).find("td").text().toLowerCase().indexOf(value) > -1)
                        });
                    });
                }
            });
            //#endregion


        }
    }
}

let validateBlank = function (id) {
    if ($('#' + id).val() == null || $('#' + id).val().length == 0) {
        if ($('#' + id).hasClass('select2-hidden-accessible')) {
            //select2
            $('#' + id + ' + span').addClass('is-invalid');
            $('#' + id + 'span').focus(function () {
                $(this).addClass("is-invalid");
            })
        } else {
            $('#' + id).addClass('is-invalid');
        }
        return 1;
    }
    return 0;
};


let LoadingScreenFunction = (() => {
    var token = '';
    var loading = null;
    return {
        Show: (() => {
            if (loading == null) {
                loading = $('body').loadingModal({
                    position: 'auto',
                    text: '',
                    color: '#fff',
                    opacity: '0.7',
                    backgroundColor: 'rgb(0,0,0)',
                    animation: 'doubleBounce'
                });
            } else {
                loading.loadingModal('show');
            }

        }),
        Hide: (() => {
            $('body').loadingModal('hide');
        })
    }
})();

$(document).on("select2:open", () => {
    document.querySelector(".select2-container--open .select2-search__field").focus()
})