using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MarkerDetectorScript : MonoBehaviour {
    [SerializeField]
    private ARTrackedImageManager imageManager;

    [SerializeField]
    TextMeshProUGUI locationName;

    // Update is called once per frame
    void Update () {
        foreach (var trackedImage in imageManager.trackables) {
            if (trackedImage.trackingState == TrackingState.Tracking) {
                locationName.text = trackedImage.referenceImage.name;
            } else {
                locationName.text = string.Empty;
            }
        }
    }
}
