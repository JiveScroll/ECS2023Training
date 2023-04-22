using System.Xml.Schema;
using Unity.Collections;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
public partial struct SpawnSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        //Do not run update unless at least one Entity with GameWorldProperties exists
        state.RequireForUpdate<GameWorldProperties>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //Immediately Disables the System, we only want to run once.
        state.Enabled = false;

        Entity gameWorldEntity = SystemAPI.GetSingletonEntity<GameWorldProperties>();
        GameWorldAspect gameWorldAspect = SystemAPI.GetAspectRW<GameWorldAspect>(gameWorldEntity);

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var builder = new BlobBuilder(Allocator.Temp);
        ref var spawnPoints = ref builder.ConstructRoot<EnemySpawnPointsBlob>();
        BlobBuilderArray<float3> arrayBuilder = builder.Allocate(ref spawnPoints.Value, gameWorldAspect.NumberSpawnersToSpawn);

        var spawnerOffset = new float3(0f, -2f, 1f);

        for (int i = 0; i < gameWorldAspect.NumberSpawnersToSpawn; i++)
        {
            Entity newSpawner = ecb.Instantiate(gameWorldAspect.SpawnerPrefab);
            LocalTransform newSpawnerTransform = gameWorldAspect.GetRandomSpawnerTransform();
            ecb.SetComponent(newSpawner, newSpawnerTransform);
            
            float3 newEnemySpawnPoint = newSpawnerTransform.Position + spawnerOffset;
            arrayBuilder[i] = newEnemySpawnPoint;
        }

        var blobAsset = builder.CreateBlobAssetReference<EnemySpawnPointsBlob>(Allocator.Persistent);
        ecb.SetComponent(gameWorldEntity, new EnemySpawnPoints { Value = blobAsset });

        ecb.Playback(state.EntityManager);
    }
}
