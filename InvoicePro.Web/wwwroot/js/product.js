AssetPro.Product = {
    GetAllProduct: ''
};

AssetPro.Product.GetAllProduct = function (id, dimmerId) {
    AssetPro.Datables.ShowDimmer(dimmerId);
    var component = '#' + id;
    $(component).DataTable();

    appClient.get('/product/getall', null,
        function (response) {
            AssetPro.Product.ShowAll(response.data, component, dimmerId);
        })
}

AssetPro.Product.ShowAll = function (data, component, dimmerId) {
    $(component).dataTable().fnDestroy();
    AssetPro.Product.table = $(component).DataTable({
        //"order": [[1, "asc"]],

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
            [{
                "targets": [0],
                "visible": false,
                "searchable": false
            }
            ],
        "columns": [

            { "data": "id", "name": "id", "autoWidth": true },
            { "data": "productCode", "name": "Product Code", "autoWidth": true },
            { "data": "productName", "name": "Product Name", "autoWidth": true },
            {
                "render": function (data, type, full, meta) {
                    var btn = "<a title='Watch' class='label label-success icon-left update' onclick=AssetPro.Product.WatchDescription('" + encodeURIComponent(full.productDescription) + "') ><i class='entypo-eye'></i></a>";
                    return btn;
                }
            },
            { "data": "productType", "name": "Product Type", "autoWidth": true },
            {
                "render": function (data, type, full, meta) {
                    var btn = "<a title='Watch' class='label label-success icon-left update' onclick=AssetPro.Product.WatchImage('" + full.imagePath + "') ><i class='entypo-eye'></i></a>";
                    return btn;
                }
            },
            { "data": "available", "name": "Available", "autoWidth": true },
            { "data": "averageCosting", "name": "Average Costing", "autoWidth": true },
            { "data": "salesPrice", "name": "Sales Price", "autoWidth": true },
            { "data": "salesPerWeek", "name": "Sales/Week", "autoWidth": true },
            {
                "render": function (data, type, full, meta) {
                    var btn = "<a title='Edit' class='label label-info icon-left update' onclick=AssetPro.Product.Edit('" + encodeURIComponent(full.id) + "') ><i class='entypo-pencil'></i></a>";
                    btn = btn + "<a title='Delete' class='label label-danger icon-left delete'  onclick=DeleteEntity('" + encodeURIComponent(full.id) + "','Product','" + component + "')> <i class='entypo-trash'></i></a>";
                    return btn;
                },
                "width": 300
            }
        ]
    });
    AssetPro.Datables.HideDimmer(dimmerId);
    AssetPro.Datables.SetDdl(component);
}

AssetPro.Product.ConvertToBase64 = function (btn, elementId) {
    var id = '#' + elementId;
    if (btn.files && btn.files[0]) {
        var f = btn.files[0];

        var reader = new FileReader();
        reader.onload = (function (theFile) {
            return function (e) {
                var binaryData = e.target.result;
                $(id).val(binaryData);
            };
        })(f);

        reader.readAsDataURL(btn.files[0]);
    }
}

AssetPro.Product.WatchImage = function (imagePath) {
    $('#image').attr("src", imagePath);
    jQuery.noConflict();
    $('#Product_image_modal').modal('show');
}

AssetPro.Product.WatchDescription = function (description) {
    description = description.replace(/%20/g, " ");
    description = description.replace(/%0A/g, "<br />");
    $('#description').html(description);
    jQuery.noConflict();
    $('#Product_description_modal').modal('show');
}

AssetPro.Product.Edit = function (id) {
    $('#product-id').val(id);

    appClient.get('/product/getProduct/' + id, null,
        function (response) {
            if (response) {
                var data = response.data;
                $('#productcode').val(data.productCode);
                $('#productname').val(data.productName);
                $('#productdescription').val(data.productDescription);
                $('#producttype').val(data.productType);
                $('#vendor').val(data.vendor);
                $('#available').val(data.available);
                $('#averagecosting').val(data.averageCosting);
                $('#salesprice').val(data.salesPrice);
                $('#ProductImageURL').attr("src", data.imagePath);

                jQuery.noConflict();
                $('#Product_add_modal').modal('show');
            }
            else {
                AssetPro.Settings.Toast('Error', 'An error occured on Getting Product Details', 'error');
            }
        })
}

$("#Product_add_modal").submit(function (e) {
    e.preventDefault();

    var id = $('#product-id').val();
    var productCode = $("#productcode").val();
    var productName = $("#productname").val();
    var productDescription = $("#productdescription").val();
    var productType = $("#producttype").val();
    var vendor = $("#vendor").val();
    var available = $("#available").val();
    var averageCosting = $("#averagecosting").val();
    var salesPrice = $("#salesprice").val();
    var imagePath = $('#img').val();

    if (imagePath === "0") {
        imagePath = null;
    }

    var msg = 'create';

    if (id === '') {
        appClient.post('/product/create', {
            productCode, productName, productDescription, productType, vendor, available,
            averageCosting, salesPrice, imagePath
        }, function (response) {
            if (response.data.isSuccess) {
                AssetPro.Settings.Toast('Success', 'Product ' + msg + ' has been Succeed', 'Success');
                $('#Product_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
            else {
                AssetPro.Settings.Toast('Error', 'Product ' + msg + ' has been Failed!', 'error');
                $('#Product_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
        })

    } else {
        msg = 'update';
        appClient.put('/product/update/' + id, {
            productCode, productName, productDescription, productType, vendor, available,
            averageCosting, salesPrice, imagePath
        }, function (response) {
            if (response.data.isSuccess) {
                AssetPro.Settings.Toast('Success', 'Product ' + msg + ' has been Succeed', 'Success');
                $('#Product_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
            else {
                AssetPro.Settings.Toast('Error', 'Product ' + msg + ' has been Failed!', 'error');
                $('#Product_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
        })
    }
});

AssetPro.Product.ResetProductForm = function () {
    $('#product-id').val('');
    $('#productcode').val('');
    $('#productname').val('');
    $('#productdescription').val('');
    $('#producttype').val('');
    $('#vendor').val('');
    $('#available').val('');
    $('#averagecosting').val('');
    $('#salesprice').val('');
    $('#ProductImageURL').attr("src", "https://via.placeholder.com/200x150");
};

AssetPro.Product.Add = function (id) {
    AssetPro.Product.ResetProductForm();
    jQuery.noConflict();
    $('#Product_add_modal').modal('show');
}