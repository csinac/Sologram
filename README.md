# Sologram
Star Wars inspired hologram system for Unity.

Sologram is composed of two components. the Sologram Renderer and the Sologram Object.

Sologram Object component is responsible for making objects renderable(?) as a hologram. 
It creates a camera at runtime that mimicks the movement of the main camera and renders the object on a render texture.

Sologram Renderer in turn displays this image on a billboard, by applying a custom shader that recreates the iconic Star Wars hologram look, while maintaining the 3D view of the object.

Cick below for a very simple video demo.

[![Sologram Demo Video](http://img.youtube.com/vi/0jvB3hF8M9U/0.jpg)](http://www.youtube.com/watch?v=0jvB3hF8M9U "Sologram")
