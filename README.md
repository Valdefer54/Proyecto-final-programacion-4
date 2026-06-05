# Parcial 1 - API de Gestión de Inventario y Ventas

API RESTful para la gestión de inventario, ventas y control de costos mediante el método **CPP (Costo Promedio Ponderado)**. Desarrollada como proyecto parcial para el curso de Programación 4.

## Tecnologías

- **.NET 8** (ASP.NET Core Web API)
- **C# 12**
- **JWT Bearer** para autenticación y autorización
- **Mapster** para mapeo DTO/Entidad
- **In-memory** (datos volátiles, sin base de datos persistente)

## Arquitectura

```
Controladores → Servicios → Repositorios (Listas en memoria)
```

- **Controladores**: Endpoints HTTP, validación y autorización
- **Servicios**: Lógica de negocio (Kardex CPP, control de stock, etc.)
- **Repositorios**: Acceso a datos en memoria con datos semilla
- **DTOs**: Objetos de transferencia para request/response

## Modelos Principales

| Entidad         | Descripción                              |
|----------------|------------------------------------------|
| Usuario         | Autenticación, roles (Admin/Empleado)    |
| Categoria       | Clasificación de productos               |
| Producto        | Productos del inventario                 |
| Inventario      | Stock actual por producto                |
| Venta           | Cabecera de venta con detalles           |
| KardexMovimiento| Movimientos de entrada/salida con CPP    |

## Roles

- **Administrador** — Acceso completo a todos los endpoints
- **Empleado** — Acceso de solo lectura y creación de ventas

## Endpoints

### Auth (`/api/auth`)

| Método | Ruta           | Auth  | Descripción              |
|--------|----------------|-------|--------------------------|
| POST   | `/api/auth/login`    | Anónimo | Iniciar sesión (recibe JWT) |
| POST   | `/api/auth/register` | Anónimo | Registrar nuevo usuario    |
| GET    | `/api/auth`          | Admin  | Listar usuarios            |
| GET    | `/api/auth/{id}`     | Auth   | Obtener usuario por ID     |

### Categorías (`/api/categorias`)

| Método | Ruta                | Auth  | Descripción        |
|--------|---------------------|-------|--------------------|
| GET    | `/api/categorias`       | Auth  | Listar todas       |
| GET    | `/api/categorias/{id}`  | Auth  | Obtener por ID     |
| POST   | `/api/categorias`       | Admin | Crear              |
| PUT    | `/api/categorias/{id}`  | Admin | Actualizar         |
| DELETE | `/api/categorias/{id}`  | Admin | Eliminar           |

### Productos (`/api/productos`)

| Método | Ruta                                       | Auth  | Descripción             |
|--------|---------------------------------------------|-------|-------------------------|
| GET    | `/api/productos`                            | Auth  | Listar todos            |
| GET    | `/api/productos/{id}`                       | Auth  | Obtener por ID          |
| GET    | `/api/productos/categoria/{categoriaId}`    | Auth  | Filtrar por categoría   |
| GET    | `/api/productos/disponibilidad/{activo}`    | Auth  | Filtrar por estado      |
| GET    | `/api/productos/bajo-stock`                 | Admin | Productos con stock bajo|
| POST   | `/api/productos`                            | Admin | Crear (crea inventario automáticamente) |
| PUT    | `/api/productos/{id}`                       | Admin | Actualizar completo     |
| PATCH  | `/api/productos/{id}/precio`                | Admin | Actualizar precio       |
| PATCH  | `/api/productos/{id}/categoria/{categoriaId}`| Admin| Asignar categoría       |
| DELETE | `/api/productos/{id}`                       | Admin | Eliminar                |

### Inventarios (`/api/inventarios`)

| Método | Ruta                                | Auth  | Descripción                  |
|--------|--------------------------------------|-------|------------------------------|
| GET    | `/api/inventarios`                   | Auth  | Listar todos                 |
| GET    | `/api/inventarios/{id}`              | Auth  | Obtener por ID               |
| GET    | `/api/inventarios/producto/{productoId}` | Auth | Obtener por producto      |
| GET    | `/api/inventarios/bajo-stock`        | Admin | Inventarios bajo stock mínimo|
| POST   | `/api/inventarios`                   | Admin | Crear registro               |
| PUT    | `/api/inventarios/{id}`              | Admin | Actualizar (genera Kardex)   |
| DELETE | `/api/inventarios/{id}`              | Admin | Eliminar                     |

### Ventas (`/api/ventas`)

| Método | Ruta                         | Auth  | Descripción                      |
|--------|-------------------------------|-------|----------------------------------|
| GET    | `/api/ventas`                 | Admin | Listar todas                     |
| GET    | `/api/ventas/{id}`            | Auth  | Obtener por ID                   |
| GET    | `/api/ventas/usuario/{usuarioId}` | Admin | Ventas por usuario           |
| GET    | `/api/ventas/rango?inicio=&fin=` | Admin | Ventas por rango de fechas  |
| POST   | `/api/ventas`                 | Auth  | Crear venta (valida stock, registra Kardex) |
| PUT    | `/api/ventas/{id}/confirmar` | Admin  | Confirmar venta                  |
| PUT    | `/api/ventas/{id}/cancelar`   | Admin | Cancelar (revierte stock)        |

### Kardex (`/api/kardex`)

| Método | Ruta                          | Auth  | Descripción                       |
|--------|-------------------------------|-------|-----------------------------------|
| GET    | `/api/kardex/{productoId}`    | Auth  | Movimientos + saldo CPP           |
| GET    | `/api/kardex/{productoId}/saldo` | Auth | Saldo actual CPP                |
| POST   | `/api/kardex/entrada`         | Admin | Registrar entrada (ajuste manual) |
| PUT    | `/api/kardex/{movimientoId}`  | Admin | Actualizar concepto/observaciones |
| DELETE | `/api/kardex/{movimientoId}`  | Admin | Eliminar movimiento               |

## Datos Semilla

| Usuario     | Email            | Contraseña | Rol          |
|-------------|------------------|------------|--------------|
| Admin       | admin@test.com   | admin123   | Administrador|

Al iniciar la aplicación se crean automáticamente 2 categorías, 2 productos y sus inventarios correspondientes.

## Cómo ejecutar

```bash
dotnet run
```

La API estará disponible en `http://localhost:5000`. Incluye una [colección de Postman](Parcial1.postman_collection.json) con todas las peticiones preconfiguradas.
