using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [Header("GUI")]
    private float windowDpi;
    public GameObject[] SceneObjects;
    private int Prefab;
    private int ActiveObject;
    private bool GUIswitcher = true;
    private float deltaTime = 0.0f;
    private string currentPrefabName = "";

    void Start()
    {
        if (Screen.dpi < 1) windowDpi = 1;
        if (Screen.dpi < 200) windowDpi = 1;
        else windowDpi = Screen.dpi / 200f;
        Counter(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown("n"))
            GUIswitcher = !GUIswitcher;

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        float screenWidth = Screen.width;
        float buttonWidth = 120 * windowDpi;
        float buttonHeight = 40 * windowDpi;
        float buttonSpacing = 10 * windowDpi;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = (int)(18 * windowDpi),
            normal = { textColor = Color.black, background = Texture2D.grayTexture }
        };

        if (GUIswitcher)
        {
            float centerX = (screenWidth - (3 * buttonWidth + 2 * buttonSpacing)) / 2;
            float posY = 10 * windowDpi;

            if (GUI.Button(new Rect(centerX, posY, buttonWidth, buttonHeight), "Prev", buttonStyle))
            {
                Counter(-1);
            }
            if (GUI.Button(new Rect(centerX + buttonWidth + buttonSpacing, posY, buttonWidth, buttonHeight), "Play Again", buttonStyle))
            {
                Counter(0);
            }
            if (GUI.Button(new Rect(centerX + 2 * (buttonWidth + buttonSpacing), posY, buttonWidth, buttonHeight), "Next", buttonStyle))
            {
                Counter(+1);
            }

            GUIStyle prefabTextStyle = new GUIStyle
            {
                fontSize = 20,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };

            float textPosX = screenWidth / 2;
            float textPosY = posY + buttonHeight + 20 * windowDpi; 
            GUI.Label(new Rect(textPosX - 100, textPosY, 200, 30), "Playing: " + currentPrefabName, prefabTextStyle);
        }

        int fps = Mathf.RoundToInt(1.0f / deltaTime);
        GUIStyle fpsStyle = new GUIStyle
        {
            fontSize = 20,
            normal = { textColor = Color.white }
        };
        GUI.Label(new Rect(10, 10, 200, 30), "FPS: " + fps, fpsStyle);
    }

    void Counter(int count)
    {
        if (SceneObjects == null || SceneObjects.Length == 0)
        {
            Debug.LogError("Scene Objects belum diisi!");
            return;
        }

        foreach (GameObject obj in SceneObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        Prefab = (ActiveObject + count + SceneObjects.Length) % SceneObjects.Length;
        ActiveObject = Prefab;

        if (SceneObjects[ActiveObject] != null)
        {
            SceneObjects[ActiveObject].SetActive(true);
            currentPrefabName = SceneObjects[ActiveObject].name;
        }
    }
}
