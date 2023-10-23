using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyPool : ObjectPool
{
    [Serializable]
    public new class Settings
    {
        public float poolSize;
        public List<Vector2> validSides = new List<Vector2>() { Vector3.left, Vector3.right, Vector3.up, Vector3.down };
        public Vector2 minOffsetInsideScreen, maxOffsetInsideScreen;
    }

    public Settings enemySettings;

    public void InstantiateEnemies(int amount)
    {
        // Calculate the number of enemies to spawn on each side
        int enemiesPerSide = amount / enemySettings.validSides.Count;

        // Instantiate enemies for each side
        foreach (var item in enemySettings.validSides)
        {
            InstantiateEnemies(enemiesPerSide, item);
        }
    }

    private void InstantiateEnemies(int amount, Vector3 direction)
    {
        for (int i = 0; i < amount; i++)
        {
            var enemy = GetObjectFromPool();
            enemy.transform.SetPositionAndRotation(GetRandomPointOnEdge(direction), Quaternion.identity);
        }
    }

    // Function to get a random point on the specified edge
    private Vector3 GetRandomPointOnEdge(Vector3 direction)
    {
        // Determine the screen's boundaries
        float screenLeft = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        float screenRight = Camera.main.ViewportToWorldPoint(Vector3.one).x;
        float screenBottom = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
        float screenTop = Camera.main.ViewportToWorldPoint(Vector3.one).y;
        // Add the offset inside the screen
        var offset = new Vector3(Random.Range(enemySettings.minOffsetInsideScreen.x, enemySettings.maxOffsetInsideScreen.x)
            , Random.Range(enemySettings.minOffsetInsideScreen.y, enemySettings.maxOffsetInsideScreen.y), 0);

        if (direction == Vector3.left)
            return new Vector3(screenLeft + offset.x, Random.Range(screenBottom, screenTop), 0);
        else if (direction == Vector3.right)
            return new Vector3(screenRight - offset.x, Random.Range(screenBottom, screenTop), 0);
        else if (direction == Vector3.up)
            return new Vector3(Random.Range(screenLeft, screenRight), screenTop - offset.y, 0);
        else
            return new Vector3(Random.Range(screenLeft, screenRight), screenBottom + offset.x, 0);
    }
}
