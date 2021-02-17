using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Testing");
    }

    // Update is called once per frame
    void Update()
    {
        //Detect when the space button is pressed down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key was pressed.");

            // Move the object upward in world space 50 unit/second.
            transform.Translate(Vector3.up * Time.deltaTime * 50, Space.World);
        }

        //Detect when the up arrow key is pressed down
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Up Arrow key was pressed.");

            // Move the object forward in world space 20 unit/second.
            transform.Translate(Vector3.forward * Time.deltaTime * 20, Space.World);
        }

         //Detect when the left arrow key is pressed down
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Left Arrow key was pressed.");

            // Move the object left in world space 20 unit/second.
            transform.Translate(Vector3.left * Time.deltaTime * 20, Space.World);
        }

        //Detect when the right arrow key is pressed down
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Right Arrow key was pressed.");

            // Move the object right in world space 20 unit/second.
            transform.Translate(Vector3.right * Time.deltaTime * 20, Space.World);
        }

        //Detect when the down arrow key is pressed down
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Down Arrow key was pressed.");

            // Move the object backwards in world space 20 unit/second.
            transform.Translate(Vector3.back * Time.deltaTime * 20, Space.World);
        }
           
    }
}
