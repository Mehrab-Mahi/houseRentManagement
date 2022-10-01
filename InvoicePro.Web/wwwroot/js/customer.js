AssetPro.Customer = {
    GetAllCustomer: ''
};

AssetPro.Customer.GetAllCustomer = function (id, dimmerId) {
    AssetPro.Datables.ShowDimmer(dimmerId);
    var component = '#' + id;
    $(component).DataTable();

    appClient.get('/customer/getall', null,
        function (response) {
            AssetPro.Customer.ShowAll(response.data, component, dimmerId);
        })
}
AssetPro.Customer.ShowAll = function (data, component, dimmerId) {
    $(component).dataTable().fnDestroy();
    AssetPro.Customer.table = $(component).DataTable({
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
            { "data": "firstName", "name": "First Name", "autoWidth": true },
            { "data": "lastName", "name": "LastName", "autoWidth": true },
            { "data": "customerType", "name": "Customer Type", "autoWidth": true },
            { "data": "mainPhone", "name": "Main Phone", "autoWidth": true },
            { "data": "email", "name": "Email", "autoWidth": true },
            { "data": "company", "name": "Company", "autoWidth": true },
            {
                "render": function (data, type, full, meta) {
                    var btn = "<a title='Edit' class='label label-info icon-left update' onclick=AssetPro.Customer.Edit('" + encodeURIComponent(full.id) + "') ><i class='entypo-pencil'></i></a>";
                    btn = btn + "<a title='Delete' class='label label-danger icon-left delete'  onclick=DeleteEntity('" + encodeURIComponent(full.id) + "','Customer','" + component + "')> <i class='entypo-trash'></i></a>";
                    return btn;
                }
            }
        ]
    });
    AssetPro.Datables.HideDimmer(dimmerId);
    AssetPro.Datables.SetDdl(component);
}

AssetPro.Customer.Edit = function (id) {
    $('#customer-id').val(id);

    appClient.get('/customer/getCustomer/' + id, null,
        function (response) {
            if (response) {
                var data = response.data;
                $('#firstname').val(data.firstName);
                $('#lastname').val(data.lastName);
                $('#customertype').val(data.customerType);
                $('#mainphone').val(data.mainPhone);
                $('#alternativephone').val(data.alternativePhone);
                $('#primarycontact').val(data.primaryContact);
                $('#secondarycontact').val(data.secondaryContact);
                $('#email').val(data.email);
                $('#company').val(data.company);
                $('#street1').val(data.street1);
                $('#street2').val(data.street2);
                $('#city').val(data.city);
                $('#state').val(data.state);
                $('#zip').val(data.zip);

                jQuery.noConflict();
                $('#Customer_add_modal').modal('show');
            }
            else {
                AssetPro.Settings.Toast('Error', 'An error occured on Getting Customer Details', 'error');
            }
        })
}

$("#Customer_add_modal").submit(function (e) {
    e.preventDefault();

    var id = $('#customer-id').val();
    var firstName = $("#firstname").val();
    var lastName = $("#lastname").val();
    var customerType = $("#customertype").val();
    var mainPhone = $("#mainphone").val();
    var alternativePhone = $("#alternativephone").val();
    var primaryContact = $("#primarycontact").val();
    var secondaryContact = $("#secondarycontact").val();
    var email = $("#email").val();
    var company = $("#company").val();
    var street1 = $("#street1").val();
    var street2 = $("#street2").val();
    var city = $("#city").val();
    var state = $("#state").val();
    var zip = $("#zip").val();
    
    var msg = 'create';

    if (id === '') {
        appClient.post('/customer/create', {
            firstName, lastName, customerType, mainPhone, alternativePhone,
            primaryContact, secondaryContact, email, company, street1, street2, city, state, zip
        }, function (response) {
            if (response.data.isSuccess) {
                AssetPro.Settings.Toast('Success', 'Customer ' + msg + ' has been Succeed', 'Success');
                $('#Customer_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
            else {
                AssetPro.Settings.Toast('Error', 'Customer ' + msg + ' has been Failed!', 'error');
                $('#Customer_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
        })

    } else {
        msg = 'update';
        appClient.put('/customer/update/' + id, {
            firstName, lastName, customerType, mainPhone, alternativePhone,
            primaryContact, secondaryContact, email, company, street1, street2, city, state, zip
        }, function (response) {
            if (response.data.isSuccess) {
                AssetPro.Settings.Toast('Success', 'Customer ' + msg + ' has been Succeed', 'Success');
                $('#Customer_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
            else {
                AssetPro.Settings.Toast('Error', 'Customer ' + msg + ' has been Failed!', 'error');
                $('#Customer_add_modal').modal('hide');
                AssetPro.Settings.ReloadDt();
            }
        })
    }
});

AssetPro.Customer.ResetCustomerForm = function () {
    $('#customer-id').val('');
    $('#firstname').val('');
    $('#lastname').val('');
    $('#customertype').val('');
    $('#mainphone').val('');
    $('#alternativephone').val('');
    $('#primarycontact').val('');
    $('#secondarycontact').val('');
    $('#email').val('');
    $('#company').val('');
    $('#street1').val('');
    $('#street2').val('');
    $('#city').val('');
    $('#state').val('');
    $('#zip').val('');
};

AssetPro.Customer.Add = function (id) {
    debugger;
    AssetPro.Customer.ResetCustomerForm();
    jQuery.noConflict();
    $('#Customer_add_modal').modal('show');
}