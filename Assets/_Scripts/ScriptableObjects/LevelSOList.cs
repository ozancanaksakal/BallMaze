using System;
using UnityEngine;

[Serializable]
public struct LevelSO {
    public string levelName;
    public Sprite sprite;
    public Loader.Scene scene;
    public int maxMoveNumber;

}

[CreateAssetMenu]
public class LevelSOList : ScriptableObject {
    public LevelSO[] list;
}