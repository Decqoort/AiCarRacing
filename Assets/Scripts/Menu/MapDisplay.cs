using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text mapName;
    [SerializeField] Image mapImage;

    private Map currentMap;

    public void DisplayMap(Map map)
    {
        currentMap = map;
        mapName.text = map.name;
        mapImage.sprite = map.mapSprite;
    }

    public Map GetCurrentMap()
    { 
        return currentMap;
    }
}
