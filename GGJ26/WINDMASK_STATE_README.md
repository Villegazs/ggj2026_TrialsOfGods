# Wind Mask State - Documentación de Uso

## Descripción
Se ha creado un nuevo estado de movimiento llamado **UsingWindMaskState** que se activa mediante un evento estático.

## Archivos Creados/Modificados

### 1. **UsingWindMaskStateSO.cs** (NUEVO)
- Ubicación: `Assets/ScriptableObjects/Scipts/UsingWindMaskStateSO.cs`
- Estado de movimiento que se activa cuando se usa la máscara de viento
- Tiene duración configurable (por defecto 3 segundos)
- Permite configurar settings de movimiento específicos para este estado

### 2. **StaticEventHandler.cs** (MODIFICADO)
- Agregado evento: `OnWindMaskActivated`
- Agregado método: `RaiseWindMaskActivated()`

### 3. **CharacterMovement.cs** (MODIFICADO)
- Agregada referencia pública: `usingWindMaskStateSo`
- Implementados métodos `OnEnable()` y `OnDisable()` para suscripción al evento
- Agregado método `ActivateWindMaskState()` que cambia al estado de máscara de viento

## Cómo Usar

### Configuración en Unity:
1. **Crear el ScriptableObject del estado:**
   - Click derecho en Project → Create → Movement States → Using Wind Mask
   - Configurar propiedades:
     - `windMaskDuration`: Duración del efecto en segundos
     - `windMaskMovement`: (Opcional) Settings de movimiento especiales
     - `abilities`: Habilidades permitidas durante este estado

2. **Asignar en CharacterMovement:**
   - Seleccionar el GameObject del jugador
   - En el componente CharacterMovement, asignar el ScriptableObject creado a `Using Wind Mask State So`

### Activar el estado desde código:
```csharp
// Desde cualquier script, llamar:
StaticEventHandler.RaiseWindMaskActivated();
```

### Ejemplo de uso en otro script:
```csharp
public class WindMaskItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Activar el estado de máscara de viento
            StaticEventHandler.RaiseWindMaskActivated();
            
            // Destruir el item o desactivarlo
            Destroy(gameObject);
        }
    }
}
```

## Funcionamiento

1. Cuando se llama `StaticEventHandler.RaiseWindMaskActivated()`, se dispara el evento
2. CharacterMovement recibe el evento y ejecuta `ActivateWindMaskState()`
3. El estado actual cambia a `UsingWindMaskStateSO`
4. Durante el estado:
   - Se aplican las configuraciones de movimiento especiales (si están asignadas)
   - Se actualiza un temporizador
   - El personaje mantiene control de movimiento horizontal
   - Se aplica gravedad normalmente
5. Cuando el temporizador llega a 0:
   - El estado se termina automáticamente
   - Regresa al estado apropiado (Grounded o Air según corresponda)

## Notas
- El estado tiene logs de debug al entrar/salir para facilitar el testing
- Si `usingWindMaskStateSo` no está asignado, se muestra una advertencia en consola
- El estado es compatible con el sistema de habilidades existente
