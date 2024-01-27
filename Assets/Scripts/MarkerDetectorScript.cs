using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MarkerDetectorScript : MonoBehaviour {
    [SerializeField]
    private ARTrackedImageManager imageManager;

    [SerializeField]
    TextMeshProUGUI locationName;

    // Update is called once per frame
    void Update () {
        foreach (var trackedImage in imageManager.trackables) {
            locationName.text = trackedImage.referenceImage.name;
        }
    }
}
