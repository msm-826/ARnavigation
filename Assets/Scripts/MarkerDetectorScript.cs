using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class MarkerDetectorScript : MonoBehaviour {
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    [SerializeField]
    TextMeshProUGUI locationName;

    [SerializeField]
    Button retryButton;

    [SerializeField]
    private ARSession arSession;


    private bool isDetecting = true;

    private void Awake () {
        Debug.Log("Awake: Setting up event listeners");
        trackedImageManager.trackedImagesChanged += OnChanged;
        retryButton.onClick.AddListener(() => {
            Debug.Log("Retry button clicked: Restarting AR session");
            arSession.Reset();
            locationName.text = string.Empty;
            isDetecting = true;
        });
        Debug.Log("Awake: MarkerDetectorScript enabled");
    }

    private void OnDisable () {
        Debug.Log("OnDisable: Removing event listeners");
        trackedImageManager.trackedImagesChanged -= OnChanged;
        Debug.Log("OnDisable: MarkerDetectorScript disabled");
    }

    private void OnChanged (ARTrackedImagesChangedEventArgs eventArgs) {
        Debug.Log($"OnChanged: Detecting images: {isDetecting}");
        if (!isDetecting) return;

        foreach (var newImage in eventArgs.added) {
            Debug.Log($"OnChanged: New image detected: {newImage.referenceImage.name}");
            locationName.text = newImage.referenceImage.name;
            isDetecting = false;
            Debug.Log("OnChanged: Stopped detection");
        }
    }
}