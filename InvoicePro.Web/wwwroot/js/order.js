AssetPro.Order = {
    GetAllOrder: ''
};

var grandTotalAmount = 0;

var productList = [];
$.ajax({
    type: "GET",
    url: '/product/getall',
    data: "{}",
    success: function (data) {
        productList = data.data;
    }
});

$("#productType").change(function () {
    var value = document.getElementById("productType").value;
    $('#product').val('');
    $('#unit').val(0);
    $('#discount').val(0);
    $('#price').val(0);
    $('#total').val(0);
    makeProductDropdown(value);
});

function makeProductDropdown(type) {
    var filteredProducts = productList.filter(object => object.productType == type);

    var productDropdown = "";

    productDropdown = '<option value="-1">Please Select Product</option>';
    for (var i = 0; i < filteredProducts.length; i++) {
        productDropdown += '<option value="' + filteredProducts[i].id + '">' + filteredProducts[i].productName + '</option>';
    }
    $("#product").html(productDropdown);
}

var orderData = [];

var productIdWithName = {};
var productPrice = {};

$.ajax({
    type: "GET",
    url: '/product/getall',
    data: "{}",
    success: function (data) {
        data = data.data;
        for (var i = 0; i < data.length; i++) {
            productIdWithName[data[i].id] = data[i].productName;
            productPrice[data[i].id] = data[i].salesPrice;
        }
    }
});

$("#product").change(function () {
    var prod = document.getElementById("product").value;
    $('#price').val(productPrice[prod]);
});

$("#discount").change(function () {
    var discount = parseFloat(document.getElementById("discount").value);
    var unit = parseFloat(document.getElementById("unit").value);
    var price = parseFloat(document.getElementById("price").value);

    var total = unit * price;

    if (discount >= 0 && discount <= 100) {
        total = total - total * (discount / 100);
    }

    $('#total').val(total);
});

$("#unit").change(function () {
    var unit = document.getElementById("unit").value;
    var price = document.getElementById("price").value;
    var discount = parseFloat(document.getElementById("discount").value);

    if (unit === 0 || price === 0) {
        $('#total').val(0);
    }
    else {
        var total = parseFloat(unit) * parseFloat(price);

        if (discount != 0) {
            total = total - (total * (discount / 100));
        }

        $('#total').val(total);
    }
});

AssetPro.Order.GetAllOrder = function (id, dimmerId) {
    AssetPro.Datables.ShowDimmer(dimmerId);
    var component = '#' + id;
    $(component).DataTable();

    appClient.get('/order/getall', null,
        function (response) {
            AssetPro.Order.ShowAll(response.data, component, dimmerId);
        })
}

AssetPro.Order.ShowAll = function (data, component, dimmerId) {
    $(component).dataTable().fnDestroy();
    AssetPro.Order.table = $(component).DataTable({
        "aLengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        "processing": true,
        "serverSide": false,
        "filter": true,
        "orderMulti": false,
        "bAutoWidth": false,
        "data": data,
        "dom": 'Bfrtip',
        "buttons": [
            'csv', 'excel', 'pdf', 'print'
        ],
        "columnDefs":
            [],
        "columns": [

            { "data": "id", "name": "Order Id", "autoWidth": true },
            { "data": "customerId", "name": "Customer", "autoWidth": true },
            { "data": "orderStatus", "name": "Order Status", "autoWidth": true },
            { "data": "paymentStatus", "name": "Payment Status", "autoWidth": true },
            { "data": "amount", "name": "Amount", "autoWidth": true },
            { "data": "paidAmount", "name": "Paid Amount", "autoWidth": true },
            { "data": "dueAmount", "name": "Due Amount", "autoWidth": true },
            {
                "render": function (data, type, full, meta) {
                    var btn = "<a title='Edit' class='label label-info icon-left update' onclick=AssetPro.Order.Edit('" + encodeURIComponent(full.id) + "') ><i class='entypo-pencil'></i></a>";
                    btn = btn + "<a title='Download' class='label label-info icon-left update' onclick=AssetPro.Order.DownloadInvoice('" + encodeURIComponent(full.id) + "') ><i class='entypo-download'></i></a>";
                    btn = btn + "<a title='Delete' class='label label-danger icon-left delete'  onclick=DeleteEntity('" + encodeURIComponent(full.id) + "','Order','" + component + "')> <i class='entypo-trash'></i></a>";
                    return btn;
                },
                "width": 300
            }
        ]
    });
    AssetPro.Datables.HideDimmer(dimmerId);
    AssetPro.Datables.SetDdl(component);
}

AssetPro.Order.Edit = function (id) {
    window.location.href = "/order/editview/" + id;
}

$("#Order_add_modal").submit(function (e) {
    e.preventDefault();
    var id = $("#id").val();
    var product = $("#product").val();
    var productType = $("#productType").val();
    var price = $("#price").val();
    var unit = $("#unit").val();
    var total = $("#total").val();
    var discount = parseFloat($("#discount").val());

    if (discount < 0 || discount > 100) {
        AssetPro.Settings.Toast('Error', 'Input discount properly', 'error');
        return;
    }

    if (id === '') {
        var currentdate = new Date();
        id = currentdate.getDate() + "_"
            + (currentdate.getMonth() + 1) + "_"
            + currentdate.getFullYear() + "_"
            + currentdate.getHours() + "_"
            + currentdate.getMinutes() + "_"
            + currentdate.getSeconds();
    }
    else {
        var index = orderData.findIndex(object => {
            return object.id === id;
        });
        orderData.splice(index, 1);
    }

    orderData.push({
        id: id,
        productId: product,
        productType: productType,
        product: productIdWithName[product],
        discount: discount,
        price: price,
        unit: unit,
        total: total
    });
    AssetPro.Order.table1.rows().remove().draw();
    AssetPro.Order.table1.rows.add(orderData).draw();

    CalCulateGrandTotal();

    $('#Order_add_modal').modal('hide');
});

function CalCulateGrandTotal() {
    var amount = 0;

    for (var i = 0; i < orderData.length; i++) {
        amount += parseFloat(orderData[i].total);
    }
    grandTotalAmount = amount;
    $('#amount').text(amount);
}

AssetPro.Order.ResetProductForm = function () {
    $('#id').val('');
    $('#product').val('');
    $('#productType').val('');
    $('#unit').val(0);
    $('#discount').val(0);
    $('#price').val(0);
    $('#total').val(0);
};

AssetPro.Order.Add = function (data) {
    AssetPro.Order.ResetProductForm();
    jQuery.noConflict();
    $('#Order_add_modal').modal('show');
}

AssetPro.Order.GetAllProductOfOrder = function (id, dimmerId, orderId) {
    AssetPro.Datables.ShowDimmer(dimmerId);
    var component = '#' + id;
    $(component).DataTable();

    if (orderId === '') {
        AssetPro.Order.ShowProductsOfOrder(orderData, component, dimmerId);
    }
    else {
        appClient.get('/order/getById/' + orderId, null,
            function (data) {
                $('#customer').val(data.data.customerId);
                $('#orderstatus').val(data.data.orderStatus);
                $('#amount').text(data.data.amount);
                orderData = JSON.parse(data.data.products);
                grandTotalAmount = parseFloat(data.data.amount);
                AssetPro.Order.ShowProductsOfOrder(orderData, component, dimmerId);
            }
        )
    }
}

AssetPro.Order.ShowProductsOfOrder = function (orderData, component ,dimmerId) {
    $(component).dataTable().fnDestroy();

    AssetPro.Order.table1 = $(component).DataTable({
        "aLengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        "processing": true,
        "serverSide": false,
        "filter": true,
        "orderMulti": false,
        "bAutoWidth": false,
        "data": orderData,
        "dom": 'Bfrtip',
        "buttons": [
            'csv', 'excel', 'pdf', 'print'
        ],
        "columnDefs":
            [{
                "targets": [0],
                "visible": false,
                "searchable": false
            }
            ],
        "columns": [

            { "data": "productId", "name": "Product Id", "autoWidth": true },
            { "data": "product", "name": "Product", "autoWidth": true },
            { "data": "price", "name": "Price", "autoWidth": true },
            { "data": "unit", "name": "Unit", "autoWidth": true },
            { "data": "total", "name": "Total", "autoWidth": true },
            {
                "render": function (data, type, full, meta) {
                    var btn = "<a title='Edit' class='label label-info icon-left update' onclick=AssetPro.Order.EditProductOfOrder('" + encodeURIComponent(full.id) + "') ><i class='entypo-pencil'></i></a>";
                    btn = btn + "<a title='Delete' class='label label-danger icon-left delete'  onclick=AssetPro.Order.DeleteProductOfOrder('" + encodeURIComponent(full.id) + "')> <i class='entypo-trash'></i></a>";
                    return btn;
                },
                "width": 300
            }
        ]
    });
    AssetPro.Datables.HideDimmer(dimmerId);
    AssetPro.Datables.SetDdl(component);
}

AssetPro.Order.EditProductOfOrder = function (id) {
    console.log(id);

    var i = orderData.findIndex(object => {
        return object.id === id;
    });

    $('#id').val(orderData[i].id);
    $('#discount').val(orderData[i].discount);
    $('#productType').val(orderData[i].productType);
    $('#unit').val(orderData[i].unit);
    $('#discount').val(orderData[i].discount);
    $('#price').val(orderData[i].price);
    $('#total').val(orderData[i].total);

    makeProductDropdown(orderData[i].productType);

    $('#product').val(orderData[i].productId);

    jQuery.noConflict();
    $('#Order_add_modal').modal('show');
}

AssetPro.Order.DeleteProductOfOrder = function (id) {
    var index = orderData.findIndex(object => {
        return object.id === id;
    });
    orderData.splice(index, 1);

    CalCulateGrandTotal();

    AssetPro.Order.table1.rows().remove().draw();
    AssetPro.Order.table1.rows.add(orderData).draw();
}

AssetPro.Order.Save = function () {
    var customerId = $('#customer').val();
    var orderStatus = $('#orderstatus').val();
    var products = JSON.stringify(orderData);
    var paymentStatus = "Due";
    var amount = grandTotalAmount;
    var paidAmount = 0.00;
    var dueAmount = amount;

    appClient.post('/order/create', {
        customerId, orderStatus, products, paymentStatus, amount, paidAmount, dueAmount
    }, function (response) {
        if (response.data.isSuccess) {
            AssetPro.Settings.Toast('Success', 'Order creation has been Succeed', 'Success');
            window.location.href = "/order";
        }
        else {
            AssetPro.Settings.Toast('Error', 'Order creation has been Failed!', 'error');
            window.location.href = "/order";
        }
    })
}

AssetPro.Order.Update = function (id) {
    var id = id;
    var customerId = $('#customer').val();
    var orderStatus = $('#orderstatus').val();
    var products = JSON.stringify(orderData);
    var amount = grandTotalAmount;

    appClient.post('/order/update/' + id, {
        id, customerId, orderStatus, products, amount
    }, function (response) {
        if (response.data.isSuccess) {
            jQuery.noConflict();
            AssetPro.Settings.Toast('Success', 'Order update has been Succeed', 'Success');
            window.location.href = "/order";
        }
        else {
            jQuery.noConflict();
            AssetPro.Settings.Toast('Error', 'Order update has been Failed!', 'error');
            window.location.href = "/order";
        }
    })
}

AssetPro.Order.DownloadInvoice = function (id) {
    appClient.post('/order/downloadInvoice/' + id, {},
        function (response) {
            debugger;
            if (response != null) {
                jQuery.noConflict();
                var file = window.location.origin + "/invoice/" + response.fileName
                window.open(file, '_blank', 'fullscreen=yes');
            }
            else {
                jQuery.noConflict();
            }
    })
}