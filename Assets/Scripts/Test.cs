using UnityEngine;
using System.Collections;


using UnityStandardAssets.CrossPlatformInput;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 tap = new Vector2(CrossPlatformInputManager.GetAxis("TapX"), CrossPlatformInputManager.GetAxis("TapY"));
        if (tap.x != 0 && tap.y != 0)
            Debug.LogFormat("Tap: {0}", tap);

        Vector2 R = new Vector2(CrossPlatformInputManager.GetAxis("RHorizontal"), CrossPlatformInputManager.GetAxis("RVertical"));
        if (R.x != 0 && R.y != 0)
            Debug.LogFormat("Right: {0}", R);

        // Vector2 L = new Vector2(CrossPlatformInputManager.GetAxis("LHorizontal"), CrossPlatformInputManager.GetAxis("LVertical"));
        // Debug.LogFormat("Left: {0}", L);
    }
}
