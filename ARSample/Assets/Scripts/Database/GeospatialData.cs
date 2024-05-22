using System;
using System.Collections.Generic;
using UnityEngine;

public class GeospatialData : ScriptableObject
{
    [Serializable]
    public class Pose
    {
        public double latitude;
        public double longitude;
        public double altitude;
        public Quaternion eunRotation;
    }

    public List<Pose> poses;
}
