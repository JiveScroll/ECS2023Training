using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public struct PlayerBaseProperties : IComponentData
{

}

public struct BaseHealth : IComponentData
{
    public float Value;
    public float MaxValue;
}

public struct PlayerBaseTag : IComponentData { }

