
var app = angular.module('myApp', []);
app.controller('myCtrl', function ($scope, $http) {
    
    $scope.horarios = [
        { nombre: "Juan Jair", apellidoPaterno: "Ramos", apellidMaterno: "Valle", horaEntrada: "08:00:00", horaSalida: "16:00:00" },
        { nombre: "Marvin Mauricio", apellidoPaterno: "Cordova", apellidMaterno: "Huaman", horaEntrada: "22:00:00", horaSalida: "06:00:00" },
        { nombre: "Jowert Lito", apellidoPaterno: "Ramirez", apellidMaterno: "Carranza", horaEntrada: "08:00:00", horaSalida: "18:00:00" },
        { nombre: "Dawson Bray", apellidoPaterno: "Espinosa", apellidMaterno: "Flores", horaEntrada: "19:00:00", horaSalida: "07:00:00" },
        { nombre: "Vicente", apellidoPaterno: "Torres", apellidMaterno: "Azurza", horaEntrada: "19:00:00", horaSalida: "07:00:00" },
        { nombre: "Adrian Uriel", apellidoPaterno: "Pizarro", apellidMaterno: "Quevedo", horaEntrada: "08:00:00", horaSalida: "17:00:00" },
        { nombre: "Jesus Manuel", apellidoPaterno: "Diaz", apellidMaterno: "Perez", horaEntrada: "08:00:00", horaSalida: "18:00:00" },
        { nombre: "William Martin", apellidoPaterno: "Chambio", apellidMaterno: "Valera", horaEntrada: "08:00:00", horaSalida: "18:00:00" },
        { nombre: "Rogger Soe", apellidoPaterno: "Moscol", apellidMaterno: "Soto", horaEntrada: "08:00:00", horaSalida: "18:00:00" },
        { nombre: "Marlon", apellidoPaterno: "Abanto", apellidMaterno: "Suaña", horaEntrada: "14:00:00", horaSalida: "22:00:00" },
        { nombre: "Rony Cristhian", apellidoPaterno: "Custodio", apellidMaterno: "Yllescas", horaEntrada: "22:00:00", horaSalida: "06:00:00" },
        { nombre: "Renato Ronny", apellidoPaterno: "Vertiz", apellidMaterno: "Dioces", horaEntrada: "07:00:00", horaSalida: "17:00:00" },
        { nombre: "Roberto Carlos", apellidoPaterno: "Quinto", apellidMaterno: "Alayo", horaEntrada: "07:00:00", horaSalida: "17:00:00" },
        { nombre: "Julio Cesar", apellidoPaterno: "Orozco", apellidMaterno: "Evangelista", horaEntrada: "07:00:00", horaSalida: "15:00:00" },
        { nombre: "Pablo Bernardo", apellidoPaterno: "Huamani", apellidMaterno: "Rojas", horaEntrada: "07:00:00", horaSalida: "17:00:00" },
        { nombre: "Kevin Jhonatan", apellidoPaterno: "Chahuayo", apellidMaterno: "Hernandez", horaEntrada: "07:00:00", horaSalida: "17:30:00" },
        { nombre: "Walter Santiago", apellidoPaterno: "Santa Cruz", apellidMaterno: "Cienfuegos", horaEntrada: "07:30:00", horaSalida: "18:00:00" },
        { nombre: "Jesus Manuel", apellidoPaterno: "Cruz", apellidMaterno: "Villarreal", horaEntrada: "15:00:00", horaSalida: "23:00:00" },
        { nombre: "Irwin Daniel", apellidoPaterno: "Osorio", apellidMaterno: "Fernandez", horaEntrada: "07:00:00", horaSalida: "15:00:00" },
        { nombre: "Luigi", apellidoPaterno: "Manuyama", apellidMaterno: "Cubas", horaEntrada: "07:00:00", horaSalida: "17:30:00" },
        { nombre: "Edni", apellidoPaterno: "Saravia", apellidMaterno: "Pinare", horaEntrada: "07:00:00", horaSalida: "17:30:00" },
        { nombre: "Percy Alonso", apellidoPaterno: "Huamani", apellidMaterno: "Mantilla", horaEntrada: "07:30:00", horaSalida: "18:00:00" },
        { nombre: "Guillermo Antonio", apellidoPaterno: "Cano", apellidMaterno: "Fernandez", horaEntrada: "09:00:00", horaSalida: "18:00:00" },
        { nombre: "Jhon Ling", apellidoPaterno: "Banegas", apellidMaterno: "Ventura", horaEntrada: "07:00:00", horaSalida: "19:00:00" },
        { nombre: "Daniel", apellidoPaterno: "Ramirez", apellidMaterno: "", horaEntrada: "15:00:00", horaSalida: "22:00:00" },
        { nombre: "Benjamin", apellidoPaterno: "Ramirez", apellidMaterno: "", horaEntrada: "08:00:00", horaSalida: "18:00:00" },
        { nombre: "Omar", apellidoPaterno: "Cuadros", apellidMaterno: "", horaEntrada: "07:00:00", horaSalida: "17:00:00" }
    ]

    $scope.Tasistencia = [];

    $scope.profile = document.getElementById("profile").value;
    $scope.proyectoId = document.getElementById("proyectoId").value;
    $scope.fecha_inicio = document.getElementById("fi").value;
    $scope.fecha_fin = document.getElementById("ff").value;

    document.getElementById('fecha1').value = $scope.fecha_inicio;
    document.getElementById('fecha2').value = $scope.fecha_fin;

    $scope.proyectosLst = [];   
    $scope.proyectoDesc = '';   

    const ctx  = document.getElementById('Grafica1');
    const ctx2 = document.getElementById('Grafica2');
    const ctx3 = document.getElementById('Grafica3');
    const ctx4 = document.getElementById('Grafica4');

    $scope.getProyectos = function () {
        $http.post("getProyectos","Asistencia")
            .then(function (response) {
                if (response.data.length > 0) {                         
                    $scope.proyectosLst = response.data;
                }
                else {
                    swal("Reporte de asistencia", "No hay datos para el rango de fecha especificados", "info")
                }

            })
            .catch(function (err) {
                swal("Error Exception.", "Ocurrio un error en la aplicacion por favor contacte a su administrador!.", "error")
            });
    }

    $scope.busca_asistencia = function () {

        var filtro = {
            profile: document.getElementById('profile').value,
            fecha1: $('#fecha1').val(),
            fecha2: $('#fecha2').val(),
            proyectos: $('#select2Multiple').val(),
            paisReporte: document.getElementById('pais-reporte').value
        }
      
        var param = JSON.stringify(filtro);        

        if (!!filtro.fecha1 == false) {
            swal("Datos incorrectos", "Debes ingresar una fecha de incio!!!", "warning");
            $('#fecha1').focus();
            return;
        }

        if (!!filtro.fecha2 == false) {
            swal("Datos incorrectos", "Debes ingresar una fecha de fin!!!", "warning");
            $('#fecha2').focus();
            return;
        }

        if (filtro.fecha1 > filtro.fecha2) {
            swal("Datos incorrectos", "La fecha de inicio no puede ser mayor a la fecha de fin", "warning");
            $('#fecha1').focus();
            return;
        }

        if (filtro.proyectos == undefined || filtro.proyectos == "" || filtro.proyectos == null) {
            swal("Datos incorrectos", "Selecciona un proyecto", "warning");
            return;
        }

        $http.post("generaAsistencias", filtro, { headers: { 'Content-Type': 'application/json' } })
            .then(function (response) {
                if (response.data.length > 0) {
                    $scope.Tasistencia = [];
                    $scope.Tasistencia = response.data;                                                           

                    //GENERAMOS LAS GRAFICAS
                    $scope.graficarSinSalida();
                    $scope.graficarRetardos(); 
                    $scope.graficarNocumpleHorario();
                    $scope.graficaNoRegistraEntradaNiSalida();
                }
                else {
                    swal("Reporte de asistencia", "No hay datos para el rango de fecha especificados", "info")
                }
            })
            .catch(function (err) {
                swal("Error Exception.", "Ocurrio un error en la aplicacion al obtener los registros de asistencia. Por favor contacte a su administrador!.", "error")
            });
    }

    $scope.getHoraEntrada = function (el) {
        var he = $scope.horarios.filter(function (h) {
            return `${h.nombre} ${h.apellidoPaterno}` === el
        }).map(function (it) {
            return it.horaEntrada
        });

        return he;
    }

    $scope.getNombreIngenieroOnly = function ()
    {
        var ingNames = $scope.Tasistencia.map(function (el) {
            return `${el.Nombre} ${el.last_name_p}`
        }).filter(function (value, index, array) {
            return array.indexOf(value) === index
        });

        return ingNames;
    }

    //GRAFICAS
    $scope.graficarSinSalida = function () {

        $scope.chartSinSalida = {
            labels: [],
            data: []
        }

        let chartStatus = Chart.getChart("Grafica1");
        if (chartStatus != undefined)
        {
            chartStatus.destroy();
        }

        /* Obtenemos los nombres unicos de los ingenieros */
        $scope.chartSinSalida.labels = $scope.getNombreIngenieroOnly();

        /*Contamos los registros que tienen retardo*/
        $scope.chartSinSalida.labels.forEach(function (el) {
            let count = $scope.Tasistencia.reduce(
                (cnt, item) =>
                    el === `${item.Nombre} ${item.last_name_p}` &&
                    item.fecha_hora_salida === "No hay registro" ? cnt += 1 : cnt, 0
            )

            $scope.chartSinSalida.data.push(count);
        });       

        new Chart(ctx, {
            type: 'bar',
            data: {                
                labels: $scope.chartSinSalida.labels,
                datasets: [{
                    label: 'S I N    S A L I D A',                    
                    data: $scope.chartSinSalida.data,
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }

    $scope.graficarRetardos = function () {

        $scope.chartRetardos = {
            labels: [],
            data: []
        }

        let chartStatus2 = Chart.getChart("Grafica2");
        if (chartStatus2 != undefined) {
            chartStatus2.destroy();
        }

        /* Obtenemos los nombres unicos de los ingenieros */
        $scope.chartRetardos.labels = $scope.getNombreIngenieroOnly();        

        /*Contamos los registros que tienen retardos*/
        /*
         Se uso for loop porque el forEach muta el elemento del array regresa un undefined.
         */
        for (let el of $scope.chartRetardos.labels) {

            let horario = $scope.getHoraEntrada(el);

            if (horario.length != 0) {

                var tolerancia = new Date("01/01/1900 " + horario[0]);
                tolerancia.setMinutes(tolerancia.getMinutes() + 10);

                let count = $scope.Tasistencia.reduce(
                    (cnt, item) => 
                        `${item.Nombre} ${item.last_name_p}` === el &&
                           new Date("01/01/1900 " + item.hora_entrada) > tolerancia ? cnt += 1 : cnt, 0                    
                )

                $scope.chartRetardos.data.push(count);
            }
        }        

        new Chart(ctx2, {
            type: 'bar',
            data: {
                labels: $scope.chartRetardos.labels,
                datasets: [{
                    label: 'R E T A R D O S',
                    data: $scope.chartRetardos.data,
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });    
    }    

    $scope.graficarNocumpleHorario = function () {

        $scope.chartNoCumple = {
            labels: [],
            data: []
        }

        let chartStatus3 = Chart.getChart("Grafica3");
        if (chartStatus3 != undefined) {
            chartStatus3.destroy();
        }

        /* Obtenemos los nombres unicos de los ingenieros */
        $scope.chartNoCumple.labels = $scope.getNombreIngenieroOnly();  

        for (let el of $scope.chartNoCumple.labels) {

            let counter = 0;

            let count = $scope.Tasistencia.reduce(
                (cnt,item) => {
                    if (el === `${item.Nombre} ${item.last_name_p}` &&
                        item.fecha_hora_salida != "No hay registro") {

                        if (Math.abs(new Date(item.fecha_hora_registro + " " + item.hora_entrada) -
                            new Date(item.fecha_hora_salida + " " + item.hora_salida)) / 36e5 < 8) {
                            counter += 1;
                        }
                    }
                    else if (el === `${item.Nombre} ${item.last_name_p}` &&
                             item.fecha_hora_salida === "No hay registro") {
                        counter += 1;
                    }
                }                                 
            );

            $scope.chartNoCumple.data.push(counter);
        }

        new Chart(ctx3, {
            type: 'bar',
            data: {
                labels: $scope.chartNoCumple.labels,
                datasets: [{
                    label: 'NO CUMPLE HORARIO',
                    data: $scope.chartNoCumple.data,
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                },
                options: {
                    plugins: {
                        enabled: false                       
                    }
                }
            }
        });
    }     

    $scope.graficaNoRegistraEntradaNiSalida = function () {

        let f1 = $('#fecha1').val();
        let f2 = $('#fecha2').val();

        $scope.chartFaltas = {
            labels: [],
            data: []
        }

        let chartStatus4 = Chart.getChart("Grafica4");
        if (chartStatus4 != undefined) {
            chartStatus4.destroy();
        }

        /* Obtenemos los nombres unicos de los ingenieros */
        $scope.chartFaltas.labels = $scope.getNombreIngenieroOnly();

        console.log(f1);
        console.log(f2);

        let diferenciaEnTiempo = new Date(f2) - new Date(f1);//2505600000
        //let dayCounterByDateRange = new Date($scope.fecha_fin).getTime() - new Date($scope.fecha_inicio).getTime();//2505600000
        let diferenciaEnDias = diferenciaEnTiempo / (1000 * 3600 * 24) + 1;

        console.log("Contador de dias por el rango especificado");
        console.log(diferenciaEnDias);

        /*Contamos los registros que tienen retardos*/
        /*
         Se uso for loop porque el forEach muta el elemento del array regresa un undefined.
         */   

        //new Chart(ctx4, {
        //    type: 'bar',
        //    data: {
        //        labels: $scope.chartRetardos.labels,
        //        datasets: [{
        //            label: 'SIN ENTRADA NI SALIDA',
        //            data: $scope.chartRetardos.data,
        //            borderWidth: 1
        //        }]
        //    },
        //    options: {
        //        scales: {
        //            y: {
        //                beginAtZero: true
        //            }
        //        }
        //    }
        //}); 
    }

    $scope.addDays = function (date, days)
    {
        let newDate = new Date(date);

        newDate.setDate(newDate.getDate() + days);

        return newDate;
    }
    
    $scope.getProyectos();

})