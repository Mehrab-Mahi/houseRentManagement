﻿AssetPro.Role = {
    GetAllRoles: ''
};

AssetPro.Role.GetAllRoles = function (id, dimmerId) {
    AssetPro.Datables.ShowDimmer(dimmerId);
    var component = '#' + id;
    $(component).DataTable();

    appClient.get('/roles/getall', null,
        function (response) {
            AssetPro.Role.ShowAll(response.data, component, dimmerId);
        })
}
AssetPro.Role.ShowAll = function (data, component, dimmerId) {
    $(component).dataTable().fnDestroy();
    AssetPro.Role.table = $(component).DataTable({
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
                    var btn = "<a title='Edit' class='label label-info icon-left update' onclick=AssetPro.Role.Edit('" + encodeURIComponent(full.id) + "') ><i class='entypo-pencil'></i></a>";
                    btn = btn + "<a title='Delete' class='label label-danger icon-left delete'  onclick=DeleteEntity('" + encodeURIComponent(full.id) + "','Role','" + component + "')> <i class='entypo-trash'></i></a>";
                    return btn;
                }
            }
        ]
    });
    AssetPro.Datables.HideDimmer(dimmerId);
    AssetPro.Datables.SetDdl(component);
}

AssetPro.Role.Edit = function (id) {
    $('#role-id').val(id);

    appClient.get('/roles/getroletree/' + id, null,
        function (data) {
            if (data) {
                $('#name').val(data.role.name);
                var menuIds = data.menuIds;

                $('#tree').jstree("destroy").empty();
                jQuery.noConflict();

                $('#tree')
                    .on('loaded.jstree', treeLoaded)
                    .jstree({
                        plugins: ["checkbox"],
                        'core': {
                            'data': data.data,
                            three_state: true,
                            "multiple": true
                        },
                        "checkbox": {
                            "keep_selected_style": false
                        }
                    }).on('changed.jstree', function (e, data) {
                        var i, j = [];
                        r = [];
                        for (i = 0, j = data.selected.length; i < j; i++) {
                            var selected = data.selected[i];
                            r.push(selected);
                        }
                    });

                function treeLoaded(event, data) {
                    data.instance.select_node(menuIds);
                }

                jQuery.noConflict();
                $('#Role_add_modal').modal('show');
            }
            else {
                AssetPro.Settings.Toast('Error', 'An error occured on Getting Role Details', 'error');
            }
        })
}

$("#Role_add_modal").submit(function (e) {
    e.preventDefault();
    var id = $('#role-id').val();

    var msg = 'create';
    var api = '';
    if (id === '') {
        api = '/roles/create';
    } else {
        msg = 'update';
        api = '/roles/update';
    }

    var name = $("#name").val();

    appClient.post(api, {
        id: id,
        name: name,
        accessControlIds: r
    },
        function (response) {
            if (response.isSuccess) {
                AssetPro.Settings.Toast('Success', 'Role  ' + msg + ' has been Succeed', 'Success');
                $('#Role_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
            else {
                AssetPro.Settings.Toast('Error', 'Role ' + msg + ' has been Succeedl', 'error');
                $('#Role_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
        })
});

AssetPro.Role.ResetRoleForm = function () {
    $('#role-id').val('');
    $("#name").val('');
};

AssetPro.Role.Add = function (id) {
    AssetPro.Role.ResetRoleForm();

    jQuery.noConflict();
    $('#Role_add_modal').modal('show');

    appClient.post('/roles/getmenutree', null,
        function (response) {
            if (response) {
                $('#tree').jstree("destroy").empty();
                jQuery.noConflict();

                $('#tree').jstree({
                    plugins: ["checkbox"],
                    'core': {
                        'data': response,
                        "multiple": true
                    },
                    "checkbox": {
                        "keep_selected_style": false
                    }
                }).on('changed.jstree', function (e, data) {
                    var i, j = [];
                    r = [];
                    for (i = 0, j = data.selected.length; i < j; i++) {
                        var selected = data.selected[i];
                        r.push(selected);
                    }
                });;
            }
        })
}