let datatable;

$(document).ready(function () {
    loadDataTable();
    $('#estadoPedido').on('change', function () {
        var selectedValue = $(this).val();
        datatable.destroy();
        loadDataTable(selectedValue);
    });
});

function loadDataTable(estado = "") {
    datatable = $('#tblDatos').DataTable({
        "language": {
            // ... Otras configuraciones de lenguaje ...
        },
        "ajax": {
            "url": "/Consulta/Pedido/ObtenerTodos",
            "data": {
                "estado": estado
            }
        },
        "columns": [
            { "data": "transaccionId", "width": "20%" },
            { "data": "fechaCreacion", "width": "20%" },
            { "data": "totalOrden", "width": "20%" },
            {
                "data": "id",
                "render": function (data, type, row) {
                    if (row.estadoOrden === 'En curso') {
                        return `
                            <div class="text-center">
                                <a onclick=Delete("/Consulta/Pedido/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="bi bi-trash-fill"></i>
                                </a>
                            </div>
                        `;
                    }
                    return ''; 
                },
                "width": "20%"
            }
        ],
    });
}

function Delete(url) {
    swal({
        title: "¿Está seguro de eliminar el pedido?",
        text: "Este registro no se podrá recuperar",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "POST",
                url: url,
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
