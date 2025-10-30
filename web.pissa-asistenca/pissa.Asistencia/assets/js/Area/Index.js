
var app = angular.module('myApp', []);
app.controller('myCtrl', function ($scope, $http) {

    $scope.ingenierosArr = [];
    $scope.proyectosArr = [];
    $scope.proyecto = {
        id: 0,
        nombre_proyecto: "",
        estatus:0
    }

    $scope.btnUpdSave = 0; 

    $scope.getProyectos = function () {
        $http.get("Area/getProyectos")
            .then(function (response) {
                if (response.data.length > 0) {
                    $scope.proyectosArr = response.data;
                }
                else {
                    swal("Administracion de Proyectos", "No hay datos para mostrar", "info");
                }                
            })
            .catch(function (err) {                
                swal("Error Exception.", "Ocurrio un error en la aplicacion al obtener los registros de los proyectos. Por favor contacte a su administrador!.", "error");                    
            })
    }

    $scope.getIngenieros = function (p) {

        $http.get("Area/getIngenieros?idProyecto=" + p.id)
            .then(function (response) {
                $scope.ingenierosArr = response.data;
                /*$('#modalIngTable').modal('show');*/               
            })
            .catch(function (error) {
                swal("Error Exception.", "Ocurrio un error en la aplicacion al obtener la lista de ingenieros. Por favor contacte a su administrador!.", "error");                    
            });       
    }

    $scope.showModal = function (p) {

        $scope.proyecto.id = p.id;
        $scope.proyecto.nombre_proyecto = "";
        $scope.proyecto.estatus = p.estatus;

        $scope.getIngenieros($scope.proyecto);

        $('#modalIngTable').modal('show');
    }

    $scope.eliminarIngeniero = function (i) {
        
        swal({
            title: "Esta seguro que desea eliminar a: " + i.name + " " + i.last_name_p + " " + i.last_name_m,
            text: "Este cambio no se podra deshacer!.",
            icon: "warning",
            buttons: true,
            dabgerMode: true
        })
        .then((confirm) =>{            
            if (confirm)
            {
                $http.post("Area/desactivarIngeniero?idIngeniero=" + i.id_ingeniero)
                    .then(function (response) {
                        if (response.data) {
                            $scope.getIngenieros($scope.proyecto);
                            swal("Elimiar Ingeniero.", "El igeniero fue removido correctamente.", "success");
                        }
                    })
                    .catch(function (error) {
                        swal("Error Exception.", "Ocurrio un error al tratar de eliminar al ingeniero. Por favor contacte a su administrador!.", "error");                    
                    });
            }
        });
    }

    $scope.eliminarTodosLosIngenieros = function () {

        swal({
            title: "Esta seguro que desea eliminar todos los ingenieros pertenecientes a " + $scope.proyecto.nombre_proyecto,
            text: "Eliminar a todos los igenieros.",
            icon: "info",
            buttons: true,
            dangerMode: false,

        })
        .then((confirm) => {
            if (confirm) {
                $http.post("Area/desactivarTodosLosIngenieros?idProyecto=" + $scope.proyecto.id)
                    .then(function (response) {
                        if (response.data) {
                            $scope.ingenierosArr = [];
                            swal("Todos los ingenieros fueron eliminados correctamente.", "Eliminacion correcta!", "success");
                        }
                    })
                    .catch(function (error) {
                        swal("Error Exception.", "Ocurrio un error al eliminar a todos los ingenieros. Por favor contacte a su administrador!.", "error");                    
                    });
            }
        });
    }

    $scope.eliminarProyecto = function (p) {

        $scope.proyecto.id = p.id;
        $scope.proyecto.nombre_proyecto = "";
        $scope.proyecto.estatus = p.estatus;

        $scope.getIngenieros(p);

        swal({
            title: "Seguro que desea eliminar este proyecto/area?",
            text: "Este proyecto/area tiene " + $scope.ingenierosArr.length + " ingenieros asignados y se borraran permanente mente.",
            icon: "warning",
            buttons: true,
            dangerMode: true
        }).then((confirm) => {
            if (confirm) {
                $http.post("Area/eliminarProyecto?idProyecto=" + p.id)
                .then(function (response) {
                    if (response.data) {
                        $scope.getProyectos();
                        swal("El proyecto y sus ingenieros fueron eliminados correctamente", "Eliminacion correcta!", "success");
                        $scope.limpiar();
                    }
                })
                .catch(function (error) {
                    swal("Error Exception.", "Ocurrio un error al eliminar el proyecto/area. Por favor contacte a su administrador!.", "error");
                });
            }
        });
    }

    $scope.saveUpdate = function () {
        if ($scope.btnUpdSave == 0)
        {
            $scope.guardar();
        }

        if ($scope.btnUpdSave == 1) {
            $scope.actualizar();
        }
    }

    $scope.guardar = function () {
        
        if ($scope.proyecto.nombre_proyecto != "") {
            swal({
                title: "Esta seguro de agregar un nuevo proyecto?",
                text: "Proyectos",
                icon: "info",
                buttons: true,
                dangerMode: false,

            })
            .then((confirm) => {
                if (confirm) {                               
                    let exists = $scope.proyectosArr.find(p => p.nombre_proyecto === $scope.proyecto.nombre_proyecto);                    

                    if (exists != undefined) {
                        swal("El proyecto ya existe!.", "Proyectos", "error");
                    }
                    else {

                        $scope.proyecto.estatus = 1;

                        $http.post("Area/insertarProyecto", $scope.proyecto)
                            .then(function (response) {
                                if (response.data === 1 && response.status == 200) {
                                    $scope.limpiar();
                                    $scope.getProyectos();
                                    swal("Registro guardado exitosamente!.", "Success.", "success");
                                }
                                else {
                                    swal("Hubo un error al guardar el regidtro!.", "Intente nuevamente.", "Error");
                                }                                
                            })
                            .catch(function (err) {
                                swal("Error Exception.", "Ocurrio un error en la aplicacion al obtener los registros de los proyectos. Por favor contacte a su administrador!.", "error");                    
                            })
                    }                    
                }
                else {
                    swal("No se guardo ningun proyecto!.", "Proyectos", "info")
                }

            });
        }
        else {
            swal("El nombre del proyecto no puede ir vacio!.", "Administración de Proyectos", "error");
        }
    }

    $scope.getRowData = function (p)
    {
        $scope.btnUpdSave = 1;

        if (p.nombre_proyecto == "") {
            swal("Error de edición.", "El nombre del proyecto no puede ir vacio.", "error");
            return;
        }

        if (p.id == 0) {
            swal("Error de edición", "Hubo un error al guardar los cambios. Por favor consulte a su administrador del sistema", "Error");
            return;
        }

        $scope.proyecto.id = p.id;
        $scope.proyecto.nombre_proyecto = p.nombre_proyecto;
        $scope.proyecto.estatus = p.estatus;
    }

    $scope.actualizar = function () {       

        $http.post("Area/actualizarProyecto", $scope.proyecto)
            .then(function (response) {
                if (response.data == 1) {
                    $scope.limpiar();
                    $scope.getProyectos();
                    swal("Edición Correcta", "Los cambios se guardaron exitosamente.", "success");
                }
                else
                {
                    swal("Error de edición","Hubo un error al guardar los cambios. Consulte a s administrador","error")
                }
            })
            .catch(function (err) {                
                swal("Error Exception","Hubo un error al comunicarse con el servidor. Contacte a su administrador","error")
            });

    }

    $scope.limpiar = function () {
        $scope.proyecto.id = 0;
        $scope.proyecto.nombre_proyecto = "";
        $scope.estatus = 0;

        $scope.btnUpdSave = 0;
    };

    $scope.getProyectos();    
})