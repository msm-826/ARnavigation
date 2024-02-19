using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class MarkerDetectorScript : MonoBehaviour {
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] TMP_Text locationName;
    [SerializeField] Button retryButton;
    [SerializeField] Button backButton;
    [SerializeField] private ARSession arSession;


    private bool isDetecting = true;

    public void Start () {
        backButton.onClick.AddListener(() => {
            SceneManager.LoadScene("MainMenuScene");
            arSession.Reset();
        });
        retryButton.onClick.AddListener(() => {
            arSession.Reset();
            locationName.text = string.Empty;
            isDetecting = true;
        });
    }

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
            Handheld.Vibrate();
            isDetecting = false;
        }
    }
}