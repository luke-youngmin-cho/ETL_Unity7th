using Assets.Scripts.UI;
using Google.XR.ARCoreExtensions;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

public class UIGeospatialStatus : MonoBehaviour
{
    private TMP_Text _latitude;
    private TMP_Text _longitude;
    private TMP_Text _altitude;
    private TMP_Text _horizontalAccuracy;
    private TMP_Text _verticalAccuracy;
    private TMP_Text _yawAccuracy;
    private TMP_Text _earthState;
    private TMP_Text _earthTrackingState;
    [SerializeField] private AREarthManager _arEarthManager;
    private IGPS _gps;

    private void Awake()
    {
        FieldInfo[] fieldInfos = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        StringBuilder stringBuilder = new StringBuilder(40);

        for (int i = 0; i < fieldInfos.Length; i++)
        {
            if (fieldInfos[i].FieldType.IsSubclassOf(typeof(MonoBehaviour)) == false)
                continue;

            if (TypeStringTable.IsValid(fieldInfos[i].FieldType) == false)
                continue;

            stringBuilder.Clear();
            stringBuilder.Append((TypeStringTable.GetString(fieldInfos[i].FieldType)));
            stringBuilder.Append(char.ToUpper(fieldInfos[i].Name[1]));
            stringBuilder.Append(fieldInfos[i].Name.Substring(2));

            Transform child = transform.Find(stringBuilder.ToString());

            if (child.TryGetComponent(fieldInfos[i].FieldType, out Component component))
                fieldInfos[i].SetValue(this, component);
        }

#if UNITY_EDITOR
        _gps = new MockUnitOfWork().gps;
#elif UNITY_ANDROID
        _gps = new UnitOfWork().gps;
#else
#endif
    }

    private void Update()
    {
        var pose = _arEarthManager.EarthState == EarthState.Enabled && _arEarthManager.EarthTrackingState == TrackingState.Tracking
                                ? _arEarthManager.CameraGeospatialPose : new GeospatialPose();
        _latitude.text = pose.Latitude.ToString();
        _longitude.text = pose.Longitude.ToString();
        _altitude.text = pose.Altitude.ToString();
        _horizontalAccuracy.text = pose.HorizontalAccuracy.ToString();
        _verticalAccuracy.text = pose.VerticalAccuracy.ToString();
        _yawAccuracy.text = pose.OrientationYawAccuracy.ToString();
        _earthState.text = _arEarthManager.EarthState.ToString();
        _earthTrackingState.text = _arEarthManager.EarthTrackingState.ToString();
    }
}
