using UnityEngine;

public class InputTests : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Axis 1: scale
        // Axis 2: quit
        Debug.Log("Scale Axis: " + Input.GetAxis("scale"));
        Debug.Log("Quit Axis: " + Input.GetAxis("quit"));
    }
}
