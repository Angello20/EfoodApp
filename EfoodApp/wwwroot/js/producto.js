let datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Admin/Producto/ObtenerTodos"
        },
        "columns": [
            { "data": "codigo" },
            { "data": "descripcion" },
            { "data": "linea.descripcion" },
            {
                "data": "contenido",
                "className": "contenido-column"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                <div class="text-center">
                    <a href="/Admin/Producto/UpSert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                        <i class="bi bi-pencil-square"></i>
                    </a>

                    <a href="/Admin/Producto/AgregarPrecios/${data}" class="btn btn-info text-white" style="cursor:pointer">
                                <i class="bi bi-tag"></i> Agregar Precios
                    </a>

                    <a onclick=Delete("/Admin/Producto/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                        <i class="bi bi-trash-fill"></i>
                    </a>
                </div>
            `;
                },
                "orderable": false
            }
        ]
    });
}

function Delete(url) {

    swal({
        title: "Esta seguro de Eliminar el Producto?",
        text: "Este registro no se podra recuperar",
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
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}



$('#formAgregarPrecio').on('submit', function (e) {
    e.preventDefault();
    var productoId = $('#txtProductoId').val();
    var tipoPrecioId = $('#ddlTipoPrecio').val();
    var monto = $('#txtMonto').val();

    $.ajax({
        url: '/Admin/Producto/AgregarPrecioProducto',
        type: 'POST',
        dataType: 'json',
        data: {
            productoId: productoId,
            precioId: tipoPrecioId,
            monto: monto
        },
        success: function (data) {
            if (data.success) {
                // Actualizar la tabla de precios o mostrar mensaje
            } else {
                // Mostrar mensaje de error
            }
        },
        error: function (err) {
            // Manejar el error
        }
    });
});



