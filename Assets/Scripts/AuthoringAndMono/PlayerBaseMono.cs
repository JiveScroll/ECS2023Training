using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.AuthoringAndMono
{
    public class PlayerBaseMono : MonoBehaviour
    {
        public float BaseHealth;
    }

    public class PlayerBaseBaker : Baker<PlayerBaseMono>
    {
        public override void Bake(PlayerBaseMono authoring)
        {
            Entity playerBaseEntity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<PlayerBaseTag>(playerBaseEntity);

            AddComponent(playerBaseEntity, new BaseHealth
            {
                Value = authoring.BaseHealth,
                MaxValue = authoring.BaseHealth
            });

            AddBuffer<BaseDamageBufferElement>(playerBaseEntity);
        }
    }
}
