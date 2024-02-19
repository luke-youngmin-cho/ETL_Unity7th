using System;

public interface IHp
{
    float hp { get; }
    float hpMin { get; }
    float hpMax { get; }
    event Action<float> onHpDepleted;
    void DepleteHp(float value);
}
