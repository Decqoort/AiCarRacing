using UnityEngine;

public class ScriptableObjjectController : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] scriptableObjectsList;
    [SerializeField] private MapDisplay mapDisplay;
    private int currentIndex;

    private void Awake()
    {
        ChangeScriptableObject(0);
    }

    public void ChangeScriptableObject(int change)
    {
        currentIndex += change;
        if (currentIndex < 0) currentIndex = scriptableObjectsList.Length - 1;
        else if (currentIndex >= scriptableObjectsList.Length) currentIndex = 0;

        if (mapDisplay != null) mapDisplay.DisplayMap((Map)scriptableObjectsList[currentIndex]);
    }
}
