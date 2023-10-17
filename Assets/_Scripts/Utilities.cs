using UnityEngine;

public static class Utilities
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position) {
        //position.z = camera.nearClipPlane;
        Vector3 newPos = new Vector3(position.x, position.y, camera.nearClipPlane);
        
        return camera.ScreenToWorldPoint(newPos);
    }
    //public static Vector3 ScreenToWorld(Camera camera, Vector3 position) {
    //    float yPos = camera.nearClipPlane;
    //    Vector3 newPos = new Vector3(position.x, yPos, position.y);
    //    var deneme = camera.ScreenToWorldPoint(newPos);
    //    Debug.Log(deneme);
    //    return deneme;
    //}
}