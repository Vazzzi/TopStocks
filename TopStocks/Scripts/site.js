$(document).ready(function () {
    $.ajax({
        dataType: "json",
        url: "/Stocks/AllStocksJSON",
        success: function (data) {
            window.stocks = data;
            updateStocksList(data);
        }
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
                + '                            <span class="badge">' + stock.Price.WeekHighPrice + '</span>'
                + '                            Highest Week Price'
                + '                        </li>'
                + '                        <li class="list-group-item">'
                + '                            <span class="badge">' + stock.Price.WeekLowPrice + '</span>'
                + '                            Lowest Week Price'
                + '                        </li>'
                + '                        <li class="list-group-item">'
                + '                            <span class="badge">' + stock.Symbol + '</span>'
                + '                            Symbol'
                + '                        </li>'
                + '                        <li class="list-group-item">'
                + '                            <span class="badge">' + new Date(parseInt(stock.NextReportDate.substr(6))).toLocaleDateString() + '</span>'
                + '                            Report Date'
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
                buyingPrice: parseInt(document.getElementById("holdingBuy").placeholder),
                totalSum: parseInt(document.getElementById("charge").placeholder)
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
                    $('#holdingError').text("just login/register and continue the purchase right away!");
                }
                if (error.status == 409) {
                    $('#holdingError').text("you own this house! purchase another one if you can...");
                }
                $('#holdingError').prop('hidden', false);
                $('#holdingSuccess').prop('hidden', true);
            })
            .always(function () {
                setTimeout(function () {
                    $('#holdingError').prop('hidden', true);
                    //clearTesxtData();
                }, 4000);
            })
    });

}



function getAllStocksJSON() {
    $.ajax({
        dataType: "json",
        url: "/Stocks/AllstocksJSON",

        success: function (data) {
            updateStocksList(data);
        }
    });
}


function showAllStocks() {


    getAllStocksJSON();

}


