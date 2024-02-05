using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private void OnEnable() {
        trackedImageManager.trackedImagesChanged += OnChanged;
    }

    private void OnDisable () {
        trackedImageManager.trackedImagesChanged -= OnChanged;
    }

    private void OnChanged (ARTrackedImagesChangedEventArgs eventArgs) {
        if (!isDetecting) return;

        foreach (var newImage in eventArgs.added) {
            locationName.text = newImage.referenceImage.name;
            isDetecting = false;
        }
    }

    public void OnARSessionExit (int SceneIndex) {
        SceneManager.LoadScene(SceneIndex, LoadSceneMode.Single);
        arSession.Reset();
    }

    public void onRetryClick () { 
        arSession.Reset();
        locationName.text = string.Empty;
        isDetecting = true;
    }
}