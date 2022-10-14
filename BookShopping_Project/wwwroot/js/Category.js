var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').dataTable({
        "ajax": {
            "url": "/Admin/Category/GetAll"
        },
        "columns": [
            {
                "data": "name", "width": "70%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                        <a class="btn btn-info" href="/Admin/Category/Upsert/${data}">
                               <i class="fas fa-edit"></i>
                        </a>
                        <a class="btn btn-danger" onclick=Delete('/Admin/Category/Delete/${data}')>
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
        title: "want to delete category?",
        buttons: true,
        icon: "warning",
        text: "Delete Information!!",   
        dangerModel:true
    }).then((willDelete)=> {
        if (willDelete){
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data){
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
