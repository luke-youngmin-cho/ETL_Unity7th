using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrackedImageHandler : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager _trackedImageManager;
    [SerializeField] private GameObject[] _prefabs;
    private Dictionary<string, GameObject> _spawnedObjects;
    [SerializeField] private ARCameraManager _cameraManager;
    [SerializeField] private ARRaycastManager _raycastManager;

    private void Awake()
    {
        _spawnedObjects = new Dictionary<string, GameObject>();

        for (int i = 0; i < _prefabs.Length; i++)
        {
            GameObject gameObject = Instantiate(_prefabs[i]);
            gameObject.name = _prefabs[i].name;
            gameObject.SetActive(false);
            _spawnedObjects.Add(gameObject.name, gameObject);
        }
    }

    IEnumerator C_OnEnable()
    {
        if (_raycastManager != null)
            _raycastManager.enabled = false;

        yield return new WaitForSeconds(2.0f);
        yield return new WaitForEndOfFrame();
        
        _cameraManager.requestedFacingDirection = CameraFacingDirection.User;
        _trackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
    }

    private void OnEnable()
    {
        //StartCoroutine(C_OnEnable());
        if (_raycastManager != null)
            _raycastManager.enabled = false;
        _cameraManager.requestedFacingDirection = CameraFacingDirection.User;
        _trackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
    }

    private void OnDisable()
    {
        if (_raycastManager != null)
            _raycastManager.enabled = true;
        _cameraManager.requestedFacingDirection = CameraFacingDirection.World;
        _trackedImageManager.trackedImagesChanged -= OnTrackedImageChanged;
    }

    private void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackedImage in args.added)
        {
            _spawnedObjects[trackedImage.referenceImage.name].SetActive(true);
            UpdateSpawnedObject(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in args.updated)
        {
            UpdateSpawnedObject(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in args.removed)
        {
            _spawnedObjects[trackedImage.referenceImage.name].SetActive(false);
        }
    }

    private void UpdateSpawnedObject(ARTrackedImage trackedImage)
    {
        _spawnedObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        _spawnedObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
    }
}
