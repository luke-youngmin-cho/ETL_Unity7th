using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Android;

public class GPS : MonoBehaviour, IGPS
{

    public float latitude
    {
        get => _latitude;
        set
        {
            _latitude = value;
            isDirty = true;
        }
    }

    public float longitude
    {
        get => _longitude;
        set
        {
            _longitude = value;
            isDirty = true;
        }
    }

    public bool isDirty
    {
        get
        {
            if (_isDirty)
            {
                _isDirty = false;
                return true;
            }

            return false;
        }
        set
        {
            _isDirty = value;
        }
    }
    [SerializeField] private float _latitude;
    [SerializeField] private float _longitude;
    [SerializeField] private bool _isDirty;
    [SerializeField] private float _refreshPeriod = 1.0f;

#if UNITY_ANDROID

    private void Start()
    {
        StartCoroutine(C_RunGPS());
    }

    public IEnumerator C_RunGPS()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);

            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                OpenAppSettings();
            }
        }

        Input.location.Start();
        Debug.Log("Wait for location initialized");

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {
            LocationInfo locationInfo = Input.location.lastData;

            while (true)
            {
                locationInfo = Input.location.lastData;
                latitude = locationInfo.latitude;
                longitude = locationInfo.longitude;

                yield return new WaitForSeconds(_refreshPeriod);
            }
        }

        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
    }

    void OpenAppSettings()
    {
        // 설정 화면으로 유도하는 대화상자 표시
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            AndroidJavaObject launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", Application.identifier);
            currentActivity.Call("startActivity", launchIntent);

            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", Application.identifier, null);
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uriObject);

            intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
            intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
            currentActivity.Call("startActivity", intentObject);
        }
    }

    void OnPermissionDenied(string permission)
    {
        Permission.RequestUserPermission(Permission.FineLocation);
    }
#endif
}


