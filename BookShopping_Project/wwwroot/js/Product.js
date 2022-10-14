var datatable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    datatable = $('#tbldata').DataTable({
        "ajax": {
            "url":"/Admin/Product/GetAll"
        },
        "columns": [
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                              <a class="btn btn-info" href="/Admin/Product/Upsert/${data}">
                                   <i class="fas fa-edit"></i> 
                               </a>
                                <a class="btn btn-danger" onclick=Delete("/Admin/Product/Delete/${data}")>
                                    <i class="fas fa-trash-alt"></i>
                                </a>                               
                        </div>
                    `;
                }
            },
            { "data": "title", "width": "15%" },
            { "data": "description", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "15%" },
            
        ]
    })
}
function Delete(url) {
    swal({
        title: "Want to delete the data?",
        buttons: true,
        icon: "Warning",
        dangerModel: true,
        text: "Delete Information"
    }).then((willdelete) => {
        if (willdelete) {
            $.ajax({
                url: url,
                type: 'Delete',
                success: function (data) {
                    if (data.success) {

                        toastr.success(data.message);
                        location.reload(true); //refresh the page after delete

                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}