public interface IGPS
{
    /// <summary>
    /// 위도
    /// </summary>
    float latitude { get; }

    /// <summary>
    /// 경도
    /// </summary>
    float longitude { get; }

    /// <summary>
    /// 갱신 여부
    /// </summary>
    bool isDirty { get; }
}