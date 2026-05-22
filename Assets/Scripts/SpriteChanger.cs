using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    //Declare our variables here
    public SpriteRenderer spriteRenderer;
    public Color spriteColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //GetComponent is a method that searches for a component of the specified type (in this case, SpriteRenderer) attached to the same GameObject that this script is attached to. If it finds the component, it returns a reference to it; if it doesn't find it, it returns null.
        // Since this is not equal to null , it means that the SpriteRenderer component was successfully found and assigned to the spriteRenderer variable. If it were null, it would indicate that the component was not found on the GameObject, and trying to access it would result in a NullReferenceException error. Which was green.

        //Color change instructions goes here
        //Change the color property of the SpriteRenderer compnent to red
        // This line down here is checking if the spriteRenderer variable is not null (null is finding if it has anything) before trying to change its color. This is a good practice to avoid potential errors if the component is not found.
        if (spriteRenderer != null)
        {
            spriteRenderer.color = spriteColor;
        }
    }

        // Update is called once per frame
        void Update()
    { 

    }
}

