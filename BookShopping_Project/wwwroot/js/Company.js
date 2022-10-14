﻿var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').dataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "postalCode", "width": "15%" },
            { "data": "phoneNo", "width": "15%" },
            {
                "data": "isAuthorizedCompany",
                "render": function (data) {
                    if (data) {
                        return `<input type="checkbox" checked disabled />`
                    }
                    else {
                        return `<input type="checkbox" disabled />`
                    }
                }
            },
            {
            
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="text-center">
                        <a class="btn btn-info" href="/Admin/Company/Upsert/${data}">
                            <i class="fas fa-edit"></i>
                        </a>
                        <a class="btn btn-danger" onclick=Delete('/Admin/Company/Delete/${data}')>
                            <i class="fas fa-trash-alt"></i>
                        </a>
                    </div>
                `;
                }
            }
        ]
    })
}
function Delete(url) {
    swal({
        title: "want to delete Company?",
        buttons: true,
        icon: "warning",
        text: "Delete Information!!",
        dangerModel: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.api().ajax.reload();
                    }
                    else {
                        toastr.error(data.message)
                    }
                }
            })
        }
    })
}