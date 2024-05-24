using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SemanticType
{
    Unlabeled,
    Sky,
    Building,
    Tree,
    Road,
    Sidewalk,
    Terrain,
    Structure,
    Object,
    Vehicle,
    Person,
    Water,
    Default,
}

public static class Semantics
{
    public static Dictionary<SemanticType, Color> colorTable { get; private set; } = new Dictionary<SemanticType, Color>
    {
        { SemanticType.Unlabeled, new Color(0, 0, 0, 0)},
        { SemanticType.Sky,          new Color(0.3803921f, 0.545098f, 0.7490196f, 1)},
        { SemanticType.Building,    new Color(0.3098038f, 0.3137254f, 0.3098038f, 0.4f)},
        { SemanticType.Tree,          new Color(0.3098039f, 0.572549f, 0.2392156f, 0.4f)},
        { SemanticType.Road,        new Color(0.5490196f, 0.2156862f, 0.8745099f, 0.4f)},
        { SemanticType.Sidewalk,   new Color(0.8941177f, 0.2274509f, 0.8862746f, 0.4f)},
        { SemanticType.Terrain,     new Color(0.7176471f, 0.972549f, 0.6705883f, 0.4f)},
        { SemanticType.Structure, new Color(0.8313726f, 0.7450981f, 0.6078432f, 0.4f)},
        { SemanticType.Object,      new Color(0.8862746f, 0.8745099f, 0.2862745f, 0.4f)},
        { SemanticType.Vehicle,    new Color(0.04313721f, 0.06274506f, 0.8784314f, 0.4f)},
        { SemanticType.Person,     new Color(0.9215687f, 0.2f, 0.1411764f, 0.4f)},
        { SemanticType.Water,       new Color(0.454902f, 0.909804f, 0.854902f, 0.4f)},
        { SemanticType.Default,    new Color(1.0f, 1.0f, 1.0f, 0.4f)},
    };
}
