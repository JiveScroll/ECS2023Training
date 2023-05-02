using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace TMG.Zombies
{
    public partial class CameraControllerSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerBaseTag>();
        }

        protected override void OnUpdate()
        {
            var baseEntity = SystemAPI.GetSingletonEntity<PlayerBaseTag>();
            var baseScale = SystemAPI.GetComponent<LocalTransform>(baseEntity).Scale;

            var cameraSingleton = CameraSingleton.Instance;
            if (cameraSingleton == null) return;
            var positionFactor = (float)SystemAPI.Time.ElapsedTime * cameraSingleton.Speed;
            var height = cameraSingleton.HeightAtScale(baseScale);
            var radius = cameraSingleton.RadiusAtScale(baseScale);

            cameraSingleton.transform.position = new Vector3
            {
                x = Mathf.Cos(positionFactor) * radius,
                y = height,
                z = Mathf.Sin(positionFactor) * radius
            };
            cameraSingleton.transform.LookAt(Vector3.zero, Vector3.up);
        }
    }
}