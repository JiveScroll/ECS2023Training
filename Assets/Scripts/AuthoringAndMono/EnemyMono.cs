using Unity.Entities;
using UnityEngine;
public class EnemyMono : MonoBehaviour
{
    public float RiseRate;

    public float WalkSpeed;
    public float WalkAmplitude;
    public float WalkFrequency;

    public float DamagePerSecond;
    public float AttackMovementAmplitude;
    public float AttackMovementFrequency;
}

public class EnemyBaker : Baker<EnemyMono>
{

    public override void Bake(EnemyMono authoring)
    {
        Entity enemyEntity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(enemyEntity, new EnemyRiseRate
        {
            Value = authoring.RiseRate
        });

        AddComponent(enemyEntity, new EnemyMovementProperties
        {
            WalkSpeed = authoring.WalkSpeed,
            WalkAmplitude = authoring.WalkAmplitude,
            WalkFrequency = authoring.WalkFrequency
        });

        AddComponent(enemyEntity, new EnemyAttackProperties
        {
            DamagePerSecond = authoring.DamagePerSecond,
            AttackMovementAmplitude = authoring.AttackMovementAmplitude,
            AttackMovementFrequency = authoring.AttackMovementFrequency
        });

        AddComponent<EnemyTimer>(enemyEntity);
        AddComponent<EnemyHeading>(enemyEntity);
        AddComponent<NewEnemyTag>(enemyEntity);
    }
}