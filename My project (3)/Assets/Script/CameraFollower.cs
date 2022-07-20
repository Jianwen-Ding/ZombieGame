using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    //Deals with collider of camera
    [SerializeField]
    float distanceBelow;
    OVRCameraRig CameraRig = null;
    // Start is called before the first frame update
    void Start()
    {
        OVRCameraRig[] CameraRigs = gameObject.transform.parent.GetComponentsInChildren<OVRCameraRig>();

        if (CameraRigs.Length == 0)
            Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
        else if (CameraRigs.Length > 1)
            Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
        else
            CameraRig = CameraRigs[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(CameraRig != null)
        {
            gameObject.transform.position = new Vector3(CameraRig.centerEyeAnchor.position.x, CameraRig.centerEyeAnchor.position.y - distanceBelow, CameraRig.centerEyeAnchor.position.z);
        }
    }
}
