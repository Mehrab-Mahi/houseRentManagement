AssetPro.CustomerType = {
    GetAllCustomerType: ''
};

AssetPro.CustomerType.GetAllCustomerType = function (id, dimmerId) {
    AssetPro.Datables.ShowDimmer(dimmerId);
    var component = '#' + id;
    $(component).DataTable();

    appClient.get('/customertype/getall', null,
        function (response) {
            AssetPro.CustomerType.ShowAll(response.data, component, dimmerId);
        })
}
AssetPro.CustomerType.ShowAll = function (data, component, dimmerId) {
    $(component).dataTable().fnDestroy();
    AssetPro.CustomerType.table = $(component).DataTable({
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
            { "data": "name", "name": "Name", "autoWidth": true },
            { "data": "description", "name": "Description", "autoWidth": true },
            {
                "render": function (data, type, full, meta) {
                    var dt = moment(full.createTime).format("dddd, MMMM Do YYYY, h:mm:ss a");
                    var btn = btn = "<span><i class='entypo-calendar'></i>" + dt + " </span>";
                    return btn;
                },
            },
            { "data": "createdBy", "name": "Created By", "autoWidth": true },
            {
                "render": function (data, type, full, meta) {
                    var btn = "<a title='Edit' class='label label-info icon-left update' onclick=AssetPro.CustomerType.Edit('" + encodeURIComponent(full.id) + "') ><i class='entypo-pencil'></i></a>";
                    btn = btn + "<a title='Delete' class='label label-danger icon-left delete'  onclick=DeleteEntity('" + encodeURIComponent(full.id) + "','CustomerType','" + component + "')> <i class='entypo-trash'></i></a>";
                    return btn;
                }
            }
        ]
    });
    AssetPro.Datables.HideDimmer(dimmerId);
    AssetPro.Datables.SetDdl(component);
}

AssetPro.CustomerType.Edit = function (id) {
    $('#customer-type-id').val(id);

    appClient.get('/customertype/getCustomerType/' + id, null,
        function (response) {
            if (response) {
                var data = response.data;
                $('#name').val(data.name);
                $('#description').val(data.description);

                jQuery.noConflict();
                $('#CustomerType_add_modal').modal('show');
            }
            else {
                AssetPro.Settings.Toast('Error', 'An error occured on Getting Customer Type Details', 'error');
            }
        })
}

$("#CustomerType_add_modal").submit(function (e) {
    e.preventDefault();
    var id = $('#customer-type-id').val();
    var name = $("#name").val();
    var description = $("#description").val();
    var msg = 'create';
    var api = '';

    if (id === '') {
        appClient.post('/customertype/create', {
            name: name,
            description: description
        }, function (response) {
            if (response.data.isSuccess) {
                AssetPro.Settings.Toast('Success', 'Asset Type  ' + msg + ' has been Succeed', 'Success');
                $('#CustomerType_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
            else {
                AssetPro.Settings.Toast('Error', 'Asset Type ' + msg + ' has been Failed!', 'error');
                $('#CustomerType_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
        })

    } else {
        msg = 'update';
        appClient.put('/customertype/update/' + id, {
            name: name,
            description: description
        }, function (response) {
            if (response.data.isSuccess) {
                AssetPro.Settings.Toast('Success', 'Customer Type  ' + msg + ' has been Succeed', 'Success');
                $('#CustomerType_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
            else {
                AssetPro.Settings.Toast('Error', 'Customer Type ' + msg + ' has been Failed!', 'error');
                $('#CustomerType_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
        })
    }
});

AssetPro.CustomerType.ResetCustomerTypeForm = function () {
    $('#customer-type-id').val('');
    $("#name").val('');
    $("#description").val('');
};

AssetPro.CustomerType.Add = function (id) {
    AssetPro.CustomerType.ResetCustomerTypeForm();
    jQuery.noConflict();
    $('#CustomerType_add_modal').modal('show');

}