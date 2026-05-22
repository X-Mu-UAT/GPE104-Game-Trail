using UnityEngine;

public class LocalSpace : MonoBehaviour
{
    private Vector3 myLocalPosition;
    private Vector3 myWorldPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Logic goes inside the braces
        // Get the local position of the GameObject this script is attached to and store it in the myLocalPosition variable
        Vector3 localPos = transform.localPosition;

        // Change the local position of the GameObject
        transform.localPosition = new Vector3(2f, 0f, 0f);

        //Print the local position to the console
        Debug.Log("Local Position: " + transform.localPosition);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
