using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{

    public float DefaultFieldOfView = 60f;
    public float ZoomFieldOfView = 30f;
    public float ZoomSpeed= 5f;
	
    //TODO: Move zoom keyhandling to the input system
	void Update ()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, ZoomFieldOfView, ZoomSpeed * Time.deltaTime);
        }
        else if (Camera.main.fieldOfView != DefaultFieldOfView)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, DefaultFieldOfView, ZoomSpeed * Time.deltaTime);
        }
	}
}
