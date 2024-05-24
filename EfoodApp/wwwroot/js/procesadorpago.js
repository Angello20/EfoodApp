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
            "url": "/Admin/ProcesadorPago/ObtenerTodos"
        },
        "columns": [
            { "data": "codigo" },
            { "data": "procesador" },
            { "data": "tipo" },
            {
                "data": "estado",
                "render": function (data, type, row) {
                    return data ? 'Sí' : 'No'; 
                },
                "width": "20%" 
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                <div class="text-center">
                    <a href="/Admin/ProcesadorPago/UpSert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                        <i class="bi bi-pencil-square"></i>
                    </a>

                    <a onclick=Delete("/Admin/ProcesadorPago/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
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


