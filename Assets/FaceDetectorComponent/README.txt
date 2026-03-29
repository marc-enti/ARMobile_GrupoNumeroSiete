================================================================================
  FACEDETECTORCOMPONENT — README
================================================================================

Componente de Unity para detección facial usando AR Foundation 6 con ARCore.
Detecta la posición y orientación de la cara en tiempo real y la expone a través
de eventos C# para que otros componentes puedan reaccionar.

--------------------------------------------------------------------------------
  ESTRUCTURA
--------------------------------------------------------------------------------

  Scripts/      Lógica principal del sistema
  Prefabs/      Prefab de ARFace con malla facial
  Materials/    Material del LineRenderer del GazeVisualizer
  Scenes/       Escena base de AR configurada

--------------------------------------------------------------------------------
  CÓMO FUNCIONA
--------------------------------------------------------------------------------

El sistema sigue una arquitectura en capas:

  1. ARFaceTracker
     Se engancha al ARFaceManager de AR Foundation y en cada Update itera las
     caras detectadas. Convierte los datos de ARFace en FaceData (posición y
     dirección de mirada) y lanza eventos.

  2. FaceTrackerManager
     Punto de entrada para el resto de la app. Re-expone los eventos
     OnFaceDetected y OnFaceLost para que cualquier script pueda escucharlos
     sin conocer la implementación concreta.

  3. IFaceTracker
     Interfaz que desacopla el tracker del manager, facilitando sustituir
     la implementación (por ejemplo, para tests).

  4. FaceData
     Contenedor simple con Position, GazeDirection e IsTracking.

  5. GazeVisualizer
     Dibuja una línea 3D desde el centro de la cara en la dirección de mirada.
     Útil para verificar que la posición y la normal de la cabeza se detectan
     correctamente.

--------------------------------------------------------------------------------
  SETUP RÁPIDO
--------------------------------------------------------------------------------

  1. Añade un GameObject vacío a la escena con los componentes ARFaceTracker
     y FaceTrackerManager.

  2. Añade el componente ARFaceManager al XR Origin (o a la AR Session Origin)
     de tu escena si no lo tiene ya.

  3. Asigna ese ARFaceManager al campo correspondiente del ARFaceTracker.


  4. Añade el prefab de ARFace en el campo "Face Prefab" del ARFaceManager
     para ver la máscara 3D superpuesta sobre la cara detectada.

  5. Opcionalmente, añade GazeVisualizer a cualquier GameObject para visualizar
     la dirección de mirada. Asigna el material desde la carpeta Materials.

  6. Para reaccionar a los datos en tus propios scripts:

       _manager.OnFaceDetected += data => Debug.Log(data.Position);
       _manager.OnFaceLost     += id   => Debug.Log("Lost: " + id);

--------------------------------------------------------------------------------
  CONFIGURACIÓN DEL XR ORIGIN
--------------------------------------------------------------------------------

Para que la cámara frontal funcione correctamente y no se quede en negro,
es necesario eliminar del prefab XR Origin (XR Rig) los siguientes elementos:

  - Input System      el componente de inputs del rig
  - Plane Detection   el gestor de detección de planos
  - Ray Cast          el gestor de raycasting

No se ha determinado cuál de los tres era el causante exacto del problema,
pero con los tres eliminados la cámara frontal funciona correctamente.

Si la cámara frontal se queda en negro al arrancar, revisa que ninguno de
estos tres esté activo en el XR Origin.

--------------------------------------------------------------------------------
  NOTAS
--------------------------------------------------------------------------------

  Eye tracking no disponible
  Se intentó obtener la posición individual de cada ojo (face.leftEye,
  face.rightEye) para dibujar una línea de gaze por ojo, pero en el dispositivo
  de prueba ambas posiciones devuelven
  siempre (0, 0, 0). ARCore no expone los huesos oculares en este hardware,
  por lo que el GazeVisualizer usa la posición central de la cara y su forward
  como aproximación de la dirección de mirada.

================================================================================
