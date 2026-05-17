# Proyecto de Inventario

Este proyecto tiene como objetivo practicar y fortalecer habilidades en el desarrollo de software.

Se trata de un sistema de inventario que incluirá la gestión de usuarios, productos y ventas.

En esta primera fase se implementará la gestión de productos.

El proyecto será desarrollado utilizando .NET 10 y el lenguaje de programación C#.

Además, se incluirán pruebas de integración para validar el correcto funcionamiento de los componentes del sistema.

## 2026-05-17

### Avances
- Implementación de consultas paginadas usando LIMIT y OFFSET.
- Integración inicial de filtros dinámicos con ILIKE.
- Integracion de eliminar registros usnado el Id
 
### Decisiones técnicas
- Se identificó un problema de aislamiento de datos entre usuarios.
- Actualmente los productos no están asociados a un propietario específico.
- Próximo cambio: agregar relación entre `Producto` y `Usuario` para garantizar que cada usuario solo pueda consultar sus propios registros.

### Próximos pasos
- Implementar autenticación con JWT.
- Aplicar filtros por usuario autenticado.
- Optimizar consultas paginadas.