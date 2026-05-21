using UnityEditor;
using UnityEngine;

public class GameQuitter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("quit"))
        {
            Application.Quit();
           //EditorApplication.isPlaying = false;
        }
    }
}
