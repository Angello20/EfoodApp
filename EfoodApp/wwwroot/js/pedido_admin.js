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
        },
        "ajax": {
            "url": "/Admin/Pedido/ObtenerTodos",
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
                    return `
                        <div class="text-center">
                            <a onclick=Delete("/Admin/Pedido/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-trash-fill"></i>
                            </a>
                            <a onclick="cambiarEstado(${data})" class="btn btn-warning text-white" style="cursor:pointer; margin-left: 5px;">
                                <i class="bi bi-x-circle-fill"></i> Cancelar
                            </a>
                        </div>
                    `;
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

function cambiarEstado(id) {
    swal({
        title: "¿Está seguro de cambiar el estado del pedido a 'Cancelada'?",
        text: "Este cambio de estado no se podrá revertir.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((aceptar) => {
        if (aceptar) {
            $.ajax({
                type: "POST",
                url: '/Admin/Pedido/CambiarEstadoPedido',
                data: {
                    id: id,
                    nuevoEstado: 'Cancelada'
                },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload(null, false);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
