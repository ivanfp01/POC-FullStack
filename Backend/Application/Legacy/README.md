# Legacy (congelado)

Este directorio contiene el código histórico de dummies movido fuera del build para preservar la referencia del profesor.

- Queda excluido de compilación mediante reglas en el .csproj.
- No se registran sus servicios ni endpoints.
- No se recomienda modificar; se mantendrá como referencia hasta limpieza final en otra rama.

## Archivos movidos:
- DummyEntity/ - Casos de uso (Commands, Queries, Handlers)
- ApplicationServices/ - Servicios de aplicación
- DataTransferObjects/ - DTOs
- DomainEvents/ - Eventos de dominio
- Integrations/ - Eventos de integración y handlers
- Repositories/ - Interfaces de repositorios