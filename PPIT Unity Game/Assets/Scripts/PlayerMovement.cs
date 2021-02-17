using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
            transform.Translate(Vector3.up * Time.deltaTime , Space.World);
        }

        // Detect when space key has been released
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Space key was released.");
        }

        //Detect when the W key is pressed down
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W key was pressed.");

            // Move the object forward in world space 20 unit/second.
            transform.Translate(Vector3.forward * Time.deltaTime , Space.World);
        }

        // Detect when W key has been released
        if (Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log("W key was released.");
        }

         //Detect when the A key is pressed down
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A key was pressed.");

            // Move the object left in world space 20 unit/second.
            transform.Translate(Vector3.left * Time.deltaTime , Space.World);
        }

        // Detect when A key has been released
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("A key was released.");
        }

        //Detect when the D key is pressed down
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D key was pressed.");

            // Move the object right in world space 20 unit/second.
            transform.Translate(Vector3.right * Time.deltaTime , Space.World);
        }

        // Detect when D key has been released
        if (Input.GetKeyUp(KeyCode.D))
        {
            Debug.Log("D key was released.");
        }

        //Detect when the S key is pressed down
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S key was pressed.");

            // Move the object backwards in world space 20 unit/second.
            transform.Translate(Vector3.back * Time.deltaTime , Space.World);
        }

        // Detect when S key has been released
        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log("S key was released.");
        }
    }
}
