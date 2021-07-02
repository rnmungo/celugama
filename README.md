# CeluGama Web System
Web destinada a gestionar la logística de las ventas producidas por la plataforma
de E-Commerce MercadoLibre.
## Tecnologías utilizadas
El BackEnd es un Web API Core de C#, mientras que el FrontEnd se encuentra
desarrollado con ReactJS.
## Acciones posibles
- Visualizar las ventas, realizar una búsqueda minusiosa pudiendo filtrar por rango
de fecha/hora, estado del envío, nombre, apellido, apodo o número de pedido.
- Descargar un Excel con los mismos filtros expresados anteriormente.
- Descargar un PDF con los rótulos de despacho para cada pedido que se encuentra en
estado **listo para envío**.
## API
La aplicación contiene algunos EndPoints, los cuales son consumidos por la aplicación
y tareas programadas.
### Orders
```
GET /api/orders?page={PAGE}&from={FROM_DATE}&to={TO_DATE}&shipping_status={STATUS}&search={SEARCH}
    -h Content-Type: application/json

Usado para obtener los pedidos, respetando los filtros seleccionados.
```
FROM_DATE & TO_DATE have the following format: **dd-MM-yyyy HH:mm:ss**
```
GET /api/orders/{ORDER_ID}
    -h Content-Type: application/json

Usado para obtener los items de un pedido.
```
### Notifications (MercadoLibre)
```
POST /api/notifications
    -b { resource: '/orders/{ORDER_ID}', application_id: 1241231233, topic: 'orders' }
    -h Content-Type: application/json

Usado para recibir las notificaciones de MercadoLibre. Respeta el topico orders.
```
### Tokens
```
POST /api/tokens
    -h Content-Type: application/json

Usado para obtener el Access Token de MercadoLibre.
```
```
PUT /api/tokens/refresh
    -h Content-Type: application/json

Usado para refrescar el Access Token de MercadoLibre.
```