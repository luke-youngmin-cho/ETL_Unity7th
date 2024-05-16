using System.Collections;
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
        yield return new WaitUntil(() => Input.location.status == LocationServiceStatus.Running);

        LocationInfo li = Input.location.lastData;
        /*latitude = li.latitude;
       longitude = li.longitude;
       latitude_text.text = "위도 : " + latitude.ToString();
       longitude_text.text = "경도 : " + longitude.ToString();
       */
        //위치 정보 수신 시작 체크

        //위치 데이터 수신 시작 이후 resendTime 경과마다 위치 정보를 갱신하고 출력
        while (true)
        {
            li = Input.location.lastData;
            latitude = li.latitude;
            longitude = li.longitude;

            yield return new WaitForSeconds(_refreshPeriod);
        }
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


