using UnityEngine;

[CreateAssetMenu (fileName = "New", menuName = "Scriptable Objects/Map")]
public class Map : ScriptableObject
{
    public int mapIndex;
    public string mapName;
    public Sprite mapSprite;
    public Object sceneToLoad;
}
