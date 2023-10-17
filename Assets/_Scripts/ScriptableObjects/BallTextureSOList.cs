using System;
using UnityEngine;

[Serializable]
public struct BallTextureSO
{
    public int cost;
    public Material material;
    public Sprite texturePicture;
}


[CreateAssetMenu]
public class BallTextureSOList : ScriptableObject
{
    public BallTextureSO[] list;
}
