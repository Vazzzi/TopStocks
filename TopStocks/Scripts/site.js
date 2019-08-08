$(document).ready(function () {
    getRefreshStocksData();
    $.ajax({
        dataType: "json",
        url: "/Stocks/AllStocksJSON",
        success: function (data) {
            window.stocks = data;
            updateStocksList(data);
        }
    });


    $("#stock-filter-form").submit(function (f) {

        var showall = $("#show-all-filter").is(":checked");

        if (showall) {
            var els = $("#stock-filter");
            var len = els.length;

            for (var i = 0; i <= len; i++) {
                els[i].dsabled = true;
            }

            getAllStocksJSON();
        }
        else {
            var minprice = $("#min-price-filter").val();

            if (minprice == "") {
                $("#min-price-filter").val(0);
            }

            var maxprice = $("#max-price-filter").val();

            if (maxprice != null) {
  
                getPriceRangeJSON();
                
            }
        }

        f.preventDefault();
    });
});





function updateStocksList(data) {
    $('#stock-grid div').hide();
    if (!$.trim(data)) {
        $('#stock-grid').append(
            '<div class="alert alert-danger" id="alertTransError" role="alert">No results found</div>'
        );
    } else {
        $.each(data, function (index, stock) {
            $('#stock-grid').append(
                '<div class="col-sm-6 col-md-5 ">'
                + '<div class="thumbnail">'
                + '   <label class="textOverImage" style="background-image:url(' + stock.Photo + ')">'
                + '  <input type="checkbox">'
                + '  <h2>' + stock.Name + '</h2>'
                + '  <div>'
                + '  ' + stock.Description + ''
                + '  </div>'
                + '  </label>'
                + '        <div class="caption" style="padding: 3px;">'

                + '            <div class="row stock-row-margin">'
                + '                <div class="col-md-6">'
                + '                    <ul class="list-group">'
                + '                        <li class="list-group-item">'
                + '                            <span class="badge">' + stock.Price.CurrentPrice + '</span>'
                + '                            Current Price'
                + '                        </li>'
                + '                        <li class="list-group-item">'
                + '                            <span class="badge">' + stock.Price.DayHighPrice + '</span>'
                + '                            Highest Day Price'
                + '                        </li>'
                + '                        <li class="list-group-item">'
                + '                            <span class="badge">' + stock.Price.DayLowPrice + '</span>'
                + '                            Lowest Day Price'
                + '                        </li>'
                + '                        <li class="list-group-item">'
                + '                            <span class="badge">' + stock.Symbol + '</span>'
                + '                            Symbol'
                + '                        </li>'
                + '                        <li class="list-group-item">'
                + '                            <span class="badge">' + new Date(parseInt(stock.NextReportDate.substr(6))).toLocaleDateString() + '</span>'
                + '                            Next Report Date'
                + '                        </li>'
                + '                    </ul>'
                + '                </div>'
                + '                <div class="col-md-6">'
                + '                   <div class="tradingview-widget-container"> '
                + '                     <div class="tradingview-widget-container__widget"></div>'
                + '                                <script type="text/javascript" src="https://s3.tradingview.com/external-embedding/embed-widget-mini-symbol-overview.js" async>'
                + '                                {'
                + '                                "symbol": "' + stock.Symbol + '",'
                + '                                "width": "200",'
                + '                                "height": "280",'
                + '                                "locale": "en",'
                + '                                "dateRange": "1m",'
                + '                                "colorTheme": "dark",'
                + '                                "trendLineColor": "#37a6ef",'
                + '                                "underLineColor": "rgba(55, 166, 239, 0.15)",'
                + '                                "isTransparent": false,'
                + '                                "autosize": false,'
                + '                                "largeChartUrl": ""'
                + '                                }'
                + '                               </script>'
                + '                                </div>'
                + '            </div>'
                + '        </div>'
                + '        <div class="buy-button-div"><button type="submit" data-stockid=' + index + ' class= "btn btn-success buyProperty">Buy Now!</button><div>'
                + '        <br /><br />'
                + '    </div>'
                + '</div>'
            );
        });
    }



    $('.buyProperty').click(function () {
        var stockData = window.stocks[parseInt(this.dataset['stockid'])];
        updateHoldingModalData(stockData);
        $('#holding-modal').modal('toggle');
    });

}


function calculateCharge() {
    var amount = document.getElementById('quantity').value
    var price = document.getElementById('holdingBuy').placeholder
    var charge = (price) * (amount) 
    $('#charge').prop("placeholder", charge );
}

function updateHoldingModalData(stockData) {
    $('#holdingModalTitle').text(stockData.Name);
    $('#holdingBuy').prop("placeholder", Number(stockData.Price.CurrentPrice));
  
  
  

    $('#createHolding').click(function () {
        $.ajax({
            url: "/Holdings/Create",
            method: "POST",
            data: {
                stock: stockData,
                quantity: parseInt(document.getElementById("quantity").value),
                buyingPrice: parseFloat(document.getElementById("holdingBuy").placeholder),
                totalSum: parseFloat(document.getElementById("charge").placeholder)
            }
        })
            .done(function () {
                $('#holdingSuccess').prop('hidden', false);
                $('#holdingError').prop('hidden', true);
                setTimeout(function () {
                    $('#holding-modal').modal('hide');
                    $('#holdingSuccess').prop('hidden', true);
                }, 3000);
            })
            .fail(function (error) {
                if (error.status == 403) {
                    $('#holdingError').text("login/register before buying Stocks!");
                }
                $('#holdingError').prop('hidden', false);
                $('#holdingSuccess').prop('hidden', true);
            })
            .always(function () {
                setTimeout(function () {
                    $('#holdingError').prop('hidden', true);
                    clearData();
                }, 4000);
            })
    });

}


function getRefreshStocksData() {
    $.ajax({
        url: "/Stocks/RefreshStocksData"
    });

}

function getAllStocksJSON() {
    $.ajax({
        dataType: "json",
        url: "/Stocks/AllStocksJSON",

        success: function (data) {
            updateStocksList(data);
        }
    });
}


function clearData() {
    document.getElementById("quantity").value = 1;
    document.getElementById("charge").value = 0;
}



function getPriceRangeJSON() {
    $.ajax({
        dataType: "json",
        url: "/Stocks/PriceRangeJSON",
        data: {
            MinimumPrice: $("#min-price-filter").val(),
            MaximumPrice: $("#max-price-filter").val()
        },
        success: function (data) {
            updateStocksList(data);
        }
    });
}

function showAllStocksCheckbox(checkbox) {
    if (checkbox.checked == true) {
        document.getElementById("min-price-filter").disabled = true;
        document.getElementById("max-price-filter").disabled = true;
        document.getElementById("submit-stock-filter").disabled = true;
        document.getElementById("reset-stock-filter").disabled = true;

        getAllStocksJSON();

    } else {
        document.getElementById("min-price-filter").disabled = false;
        document.getElementById("max-price-filter").disabled = false;


        if (document.getElementById("max-price-filter").value !== "" ) {
            document.getElementById("submit-stock-filter").disabled = false;
        } else {
            document.getElementById("submit-stock-filter").disabled = true;
        }

        document.getElementById("reset-stock-filter").disabled = false;
    }
}

function enableApply() {

    if (document.getElementById("max-price-filter").value !== "" ) {
        document.getElementById("submit-stock-filter").disabled = false;
    } else {
        document.getElementById("submit-stock-filter").disabled = true;
    }
}