**Descripción del repositorio**

- `api.checkin/` aloja una Web API .NET 6 que expone endpoints de asistencia, configura Swagger, CORS y la inyección de dependencias para repositorios basados en Dapper, y lee la cadena de conexión de MySQL mediante `MySQLConfiguration`.

- `mobile-checkin-I6A12/` es una aplicación Ionic/Angular que inicializa los módulos de Ionic, define rutas con carga perezosa para los flujos de inicio de sesión y asistencia, y se comunica con la API a través de servicios de Angular.

- `web.pissa-asistenca/` contiene una pila ASP.NET MVC más antigua con proyectos separados de Entidades, DAO, Negocio y UI, todos compartiendo un `DataContext` de MySQL configurado para la misma base de datos.



- `windows-service/` mantiene un esqueleto de servicio de Windows que actualmente solo inicia una instancia del servicio `regasistencia` con ganchos de ciclo de vida vacíos, lo que sugiere necesidades futuras de procesamiento en segundo plano.



- `ReverseProxy.txt` documenta la URL expuesta externamente utilizada por el cliente móvil y otras integraciones.



**Conceptos clave del backend**

- Controladores como `IngenieroController` y `AsistenciaController` delegan todo el trabajo a interfaces de repositorio, manteniendo la lógica HTTP liviana y centralizando la lógica de acceso a datos.



- Los repositorios se basan en Dapper con SQL sin procesar para las operaciones CRUD, por lo que los cambios en la forma de la base de datos o en el manejo de conexiones ocurren aquí; observa el control manual de entradas duplicadas en `ControlAsistenciaRepository.InsertAsistencia`, que devuelve valores centinela utilizados por los clientes.



- Los modelos en `api.checkin.model` reflejan los esquemas de las tablas y se comparten entre la API y los clientes, lo que facilita extender las cargas útiles cuando cambian las columnas.



**Aspectos esenciales de la aplicación móvil**

- La página de inicio de sesión valida las credenciales, muestra alertas de Ionic y navega a la vista de asistencia cuando la autenticación tiene éxito, pasando la carga útil del ingeniero mediante el estado del enrutador.



- `AuthService` y `AsistenciaService` centralizan las llamadas HTTP, apuntando a la URL del proxy inverso definida en `ReverseProxy.txt`; cambia esta URL base para distintos entornos.



- `AsistenciaPage` administra permisos, captura coordenadas GPS, representa un mapa de Google y envía registros de entrada/salida utilizando los valores centinela devueltos por la API; depende en gran medida de `PermissionsService` e `IonLoaderService` para la experiencia de usuario.



**Aspectos destacados del portal web**

- Los controladores MVC coordinan escenarios de informes complejos (por ejemplo, selección dinámica de proyectos, filtrado de asistencia) y dependen de métodos de la capa de Negocio, que a su vez encapsulan consultas DAO contra el proveedor MySQL de Entity Framework.



- Las entidades como `control_asistencia`, `ingeniero` y modelos de vista de soporte residen en `pissa.Asistencia.Entities`, lo que garantiza que la aplicación MVC y el DAO compartan contratos de datos.

**Pistas para tus próximas profundizaciones**

1. **Configuración del entorno**: inventaría dónde viven las cadenas de conexión (appsettings, Web.config, constructores DAO) y planifica una estrategia unificada de secretos (secretos de usuario, variables de entorno o un vault) para simplificar los despliegues.



2. **Autenticación y seguridad**: la API actualmente expone un GET simple para el inicio de sesión con credenciales en la cadena de consulta; considera aplicar HTTPS, autenticación basada en tokens y hashing de contraseñas en los clientes móvil y web.



3. **Procesamiento en segundo plano**: el servicio de Windows es un esqueleto; aclara sus responsabilidades previstas (por ejemplo, exportaciones programadas, notificaciones) y complétalas o retíralo en favor de servicios hospedados dentro de la API.



4. **Reutilización y duplicación de código**: evalúa consolidar la lógica compartida (por ejemplo, cálculos de asistencia, definiciones de DTO) entre la API y la pila MVC heredada para evitar divergencias, tal vez introduciendo una biblioteca de clases compartida o migrando las funciones web a la API más reciente.



5. **Pruebas y monitoreo**: agrega pruebas unitarias/de integración para las rutas SQL de los repositorios e introduce telemetría (registro, analítica) en los clientes móvil y web para rastrear las tasas de éxito de las entradas/salidas.



Con estos puntos de referencia, un recién llegado puede navegar por el repositorio, comprender el flujo de datos desde los clientes móvil/web hasta la API y la base de datos, e identificar áreas estratégicas para modernizar o reforzar.
