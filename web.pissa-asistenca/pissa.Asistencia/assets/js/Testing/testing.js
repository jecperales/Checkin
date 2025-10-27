
var app = angular.module('myApp', []);
app.controller('myCtrl', function ($scope, $http) {

    $scope.fecha_in;
    $scope.fecha_out;
    $scope.time;

    $scope.gEntrada = function () {
        console.log("Test 2");
        console.log("New Date");
        console.log(new Date());
        console.log("\n");


        console.log("Time only");
        console.log(new Date().toTimeString());
        console.log("splited time Time only");
        console.log(new Date().toTimeString().split(' ')[0]);


        console.log("Date to ISO only date + Time part");
        $scope.fecha_out = new Date().toISOString().substring(0, 10) + " " + new Date().toTimeString().split(' ')[0];
        console.log($scope.fecha_out);
        console.log("\n");
        

        //console.log("FULL ISO String");
        //console.log(new Date().toISOString())
        //console.log("\n");

        //console.log("replaced ISO String");
        //console.log(String(new Date().toISOString()).replace('T', ' ').replace('Z', '').substring(0, 19));
        //console.log("\n");
        //$scope.fecha_in = String(new Date().toISOString()).replace('T', ' ').replace('Z', '').substring(0, 19);

        //console.log("UTC String");
        //console.log(new Date().toUTCString());
        //console.log("\n");

        //console.log("to Date String");
        //console.log(new Date().toDateString());
        //console.log("\n");

        var asistencia = {
            latitud: 0.001,
            longitud: 0.001,
            fecha_hora_movil: new Date(),
            fecha_hora_registro: $scope.fecha_out,
            id_ingeniero: 79,
            fecha_hora_salida: $scope.fecha_in,
            s_latitud: 0.001,
            s_longitud: 0.001
        };

        $http.post("Testing/saveDateTime", asistencia)
            .then(function (response) {
                if (response) {
                    swal("Success", "Registro guardado exitosamente", "success");
                }
            })
            .catch(function (error) {
                console.log(error);
                swal("Error", "Hubo un error interno al guardar el registro", "error")
            });
    }

    $scope.gSalida = function () {
        $scope.fecha_out = new Date().toLocaleString();

        console.log("Test 2");
        console.log("New Date");
        console.log(new Date());
        console.log("\n");


        console.log("Time only");
        console.log(new Date().toTimeString());
        console.log("splited time Time only");
        console.log(new Date().toTimeString().split(' ')[0]);
    }

})