$(document).ready(function () {
    $.ajax({
        dataType: "json",
        url: "/stocks/AllstocksJSON",
        success: function (data) {
            window.stocks = data;
            updatestockList(data);
        }
    });

    $(document).ready(function () {

        $('.stock-view-image-delete-button').click(function () {
            if ($(".stock-view-image-container").length <= 1) {
                alert("There needs to be at least one photo!")
            } else {
                $('#Photos')[0].value = $('#Photos')[0].value.replace(this.dataset['img'] + ',', "");
                $('#Photos')[0].value = $('#Photos')[0].value.replace(',' + this.dataset['img'], "");
                $('#Photos')[0].value = $('#Photos')[0].value.replace(this.dataset['img'], "");
                this.parentElement.parentElement.remove();
            }
        });
    });
    
    $("#stock-filter-form").submit(function (e) {

        var showall = $("#show-all-filter").is(":checked");

        if (showall) {
            var els = $("#stock-filter");
            var len = els.length;

            for (var i = 0; i <= len; i++) {
                els[i].dsabled = true;
            }

            getAllstocksJSON();
        }
        else {
            var minprice = $("#min-price-filter").val();

            if (minprice == "") {
                $("#min-price-filter").val(0);
            }

            var maxprice = $("#max-price-filter").val();

            if (maxprice != null) {
                if (maxprice == "") {
                    getSizeBalconyMinOrMaxPriceJSON();
                }
                else {
                    getSizeBalconyPriceRangeJSON();
                }
            }
        }

        e.preventDefault();
    });
});

function updateModalData(stock) {

    $.get("/stocks/PredictstockSale",
        {
            "size": stock.Size,
            "value": stock.PropertyValue,
            "floor": stock.FloorNumber
        },
        function(data) {
            if (data === "True") {

                $("#predict-label")[0].className = "label label-success";
                $("#predict-label")[0].innerText = "Predicted to be sold!";

            } else {

                $("#predict-label")[0].className = "label label-danger";
                $("#predict-label")[0].innerText = "Not predicted to be sold";
            }
        }
    );

    $('.carousel-indicators').empty();
    $('.carousel-inner').empty();
    $('.modal-caption').empty();

    $.each(stock.Photo, function (index, photo) {

        $('.carousel-indicators').append('<li data-target="#stock-photos-carousel" class="' + (index == 0 ? "active" : "") + '" data-slide-to="' + index + '"></li>');
        $('.carousel-inner').append('<div style="background-image:url(' + "\'" + photo + "\'" + ')" class="item ' + (index == 0 ? "active" : "") + '"><img style="display: none" src="' + photo + '"></div>');
    });

    $('.modal-caption').append(
        '            <h4 class="thumbnail-caption-header">' + stock.Location.City + ', <small>' + stock.Location.Neighborhood + '</small><span id="predict-label" class="label label-default">Checking...</span></h4>'
        + '            <div class="row">'
        + '                <div class="col-md-6">'
        + '                    <ul class="list-group">'
        + '                        <li class="list-group-item">'
        + '                            <span class="badge">' + stock.Size + '</span>'
        + '                            Sqr Meters'
        + '                        </li>'
        + '                        <li class="list-group-item">'
        + '                            <span class="badge">' + stock.NumberOfRooms + '</span>'
        + '                            # Rooms'
        + '                        </li>'
        + '                        <li class="list-group-item">'
        + '                            <span class="badge">' + (stock.Balcony ? "Yes" : "No") + '</span>'
        + '                            Balcony'
        + '                        </li>'
        + '                        <li class="list-group-item">'
        + '                            <span class="badge">' + stock.FloorNumber + '</span>'
        + '                            Floor #'
        + '                        </li>'
        + '                    </ul>'
        + '                </div>'
        + '                <div class="col-md-6">'
        + '                    <div style="min-height: 100%"><iframe width="100%" height="100%" src="https://maps.google.com/maps?width=100%&amp;height=100&amp;hl=en&amp;q=' + encodeURIComponent(stock.Location.Address) + '+(My%20Business%20Name)&amp;ie=UTF8&amp;t=&amp;z=14&amp;iwloc=B&amp;output=embed" frameborder="0" scrolling="no" marginheight="0" marginwidth="0"></iframe></div><br />'
        + '                </div>'
        + '            </div>'

    );
};

function updatestockList(data) {
    $('#stock-grid div').hide();
    if (!$.trim(data)) {
        $('#stock-grid').append(
            '<div class="alert alert-danger" id="alertTransError" role="alert">No results found</div>'
        );
    } else {
        $.each(data, function (index, stock) {
            $('#stock-grid').append(
                '<div class="col-sm-6 col-md-4 ">'
                + '<div class="thumbnail">'
                + '    <a href="javascript:;"><img data-stockid=' + index + ' src = "' + stock.PhotoList[0] + '" class= "stock-image" style = "height: 200px; width: 100%; display: block;"></a> '
                + '        <div class="caption" style="padding: 3px;">'
                + '            <h4 class="thumbnail-caption-header">' + stock.Location.City + ', <small>' + stock.Location.Neighborhood + '</small></h4>'
                + '            <div class="row stock-row-margin">'
                + '                <div class="col-md-6">'
                + '                    <ul class="list-group">'
                + '                        <li class="list-group-item">'
                + '                            <span class="badge">' + stock.Size + '</span>'
                + '                            Sqr Meters'
                + '                        </li>'
                + '                        <li class="list-group-item">'
                + '                            <span class="badge">' + stock.NumberOfRooms + '</span>'
                + '                            # Rooms'
                + '                        </li>'
                + '                        <li class="list-group-item">'
                + '                            <span class="badge">' + (stock.Balcony ? "Yes" : "No") + '</span>'
                + '                            Balcony'
                + '                        </li>'
                + '                        <li class="list-group-item">'
                + '                            <span class="badge">' + stock.FloorNumber + '</span>'
                + '                            Floor #'
                + '                        </li>'
                + '                    </ul>'
                + '                </div>'
                + '                <div class="col-md-6">'
                + '                    <div style="min-height: 100%"><iframe width="100%" height="100%" src="https://maps.google.com/maps?width=100%&amp;height=100&amp;hl=en&amp;q=' + encodeURIComponent(stock.Location.Address) + '+(My%20Business%20Name)&amp;ie=UTF8&amp;t=&amp;z=14&amp;iwloc=B&amp;output=embed" frameborder="0" scrolling="no" marginheight="0" marginwidth="0"></iframe></div><br />'
                + '                </div>'
                + '            </div>'
                + '        </div>'
                + '        <div class="buy-button-div"><button type="submit" data-stockid=' + index + ' class= "btn btn-success buyProperty">Buy Now!</button><div>'
                + '        <br /><br />'
                + '    </div>'
                + '</div>'
            );
        });
    }
    
    $('.stock-image').click(function () {
        var stockData = window.stocks[parseInt(this.dataset['stockid'])];
        updateModalData(stockData)

        $('#stock-modal').modal('toggle')
    });

    $('.buyProperty').click(function () {
        var stockData = window.stocks[parseInt(this.dataset['stockid'])];
        updateTransactionModalData(stockData);
        $('#transaction-modal').modal('toggle');
    });

}

function updateTransactionModalData(stockData) {
    $('#transactionModalTitle').text(stockData.Description);
    $('#transactionPayment').prop("min", stockData.PropertyValue);
    $('#transactionPayment').prop("placeholder", stockData.PropertyValue);

    $('.form-control').keypress(function () {
        isCheckAllowed();
    });

    $('#createTransaction').click(function () {
        $.ajax({
            url: "/Transactions/CreateTransaction",
            method: "POST",
            data: {
                stock: stockData,
                buyingPrice: parseInt($('#transactionPayment').val())
            }
        })
            .done(function () {
                $('#alertTransSuccess').prop('hidden', false);
                $('#alertTransError').prop('hidden', true);
                setTimeout(function () {
                    $('#transaction-modal').modal('hide');
                    $('#alertTransSuccess').prop('hidden', true);
                }, 3000);
            })
            .fail(function (error) {
                if (error.status == 403) {
                    $('#alertTransError').text("just login/register and continue the purchase right away!");
                }
                if (error.status == 409) {
                    $('#alertTransError').text("you own this house! purchase another one if you can...");
                }
                $('#alertTransError').prop('hidden', false);
                $('#alertTransSuccess').prop('hidden', true);
            })
            .always(function () {
                setTimeout(function () {
                    $('#alertTransError').prop('hidden', true);
                    clearTesxtData();
                }, 4000);
            })
    });

}

function isCheckAllowed() {
    if ($('#transactionPayment').val() !== '' &&
        $('#cardNumber').val() !== '' &&
        $('#cardExpiry').val() !== '' &&
        $('#cardCVC').val() !== '') {
        $('#createTransaction').prop('disabled', false);
    }
}

function clearTesxtData() {
    $('#transactionPayment').val("");
    $('#cardNumber').val("");
    $('#cardExpiry').val("");
    $('#cardCVC').val("");
}
function getAllstocksJSON() {
    $.ajax({
        dataType: "json",
        url: "/stocks/AllstocksJSON",
        success: function (data) {
            updatestockList(data);
        }
    });
}

function getSizeBalconyMinOrMaxPriceJSON() {
    $.ajax({
        dataType: "json",
        url: "/stocks/SizeBalconyMinOrMaxPriceJSON",
        data: {
            Balcony: $("#balcony-filter").is(":checked"),
            Size: $("#size-filter").val(),
            MinimumPrice: $("#min-price-filter").val()
        },
        success: function (data) {
            updatestockList(data);
        }
    });
}

function getSizeBalconyPriceRangeJSON() {
    $.ajax({
        dataType: "json",
        url: "/stocks/SizeBalconyPriceRangeJSON",
        data: {
            Balcony: $("#balcony-filter").is(":checked"),
            Size: $("#size-filter").val(),
            MinimumPrice: $("#min-price-filter").val(),
            MaximumPrice: $("#max-price-filter").val()
        },
        success: function (data) {
            updatestockList(data);
        }
    });
}

function showAllAptCheckbox(checkbox) {
    if (checkbox.checked == true) {
        document.getElementById("min-price-filter").disabled = true;
        document.getElementById("max-price-filter").disabled = true;
        document.getElementById("size-filter").disabled = true;
        document.getElementById("balcony-filter").disabled = true;
        document.getElementById("submit-stock-filter").disabled = true;
        document.getElementById("reset-stock-filter").disabled = true;

        getAllstocksJSON();

    } else {
        document.getElementById("min-price-filter").disabled = false;
        document.getElementById("max-price-filter").disabled = false;
        document.getElementById("size-filter").disabled = false;
        document.getElementById("balcony-filter").disabled = false;

        if (document.getElementById("max-price-filter").value !== "" && document.getElementById("size-filter").value !== "") {
            document.getElementById("submit-stock-filter").disabled = false;
        } else {
            document.getElementById("submit-stock-filter").disabled = true;
        }

        document.getElementById("reset-stock-filter").disabled = false;
    }
}

function enableApply() {

    if (document.getElementById("max-price-filter").value !== "" && document.getElementById("size-filter").value !== "") {
        document.getElementById("submit-stock-filter").disabled = false;
    } else {
        document.getElementById("submit-stock-filter").disabled = true;
    }
}