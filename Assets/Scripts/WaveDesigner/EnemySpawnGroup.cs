using UnityEngine;

[System.Serializable]
public class EnemySpawnGroup
{
    [Tooltip("Nombre identificativo del grupo de enemigos.")]
    public string groupName = "Enemy Group";

    [Required]
    [Tooltip("Prefab del enemigo que aparecerá en este grupo.")]
    public GameObject enemyPrefab;

    [Tooltip("Tiempo en segundos antes de que aparezca el primer enemigo.")]
    [Min(0f)]
    public float spawnDelay = 0f;

    [Tooltip("Número de enemigos que aparecerán.")]
    [Min(1)]
    public int enemyCount = 5;

    [Tooltip("Tiempo en segundos entre la aparición de cada enemigo.")]
    [Min(0.01f)]
    public float spawnInterval = 1f;

    [Tooltip("Multiplicador de dificultad de este grupo.")]
    [Range(0.1f, 5f)]
    public float difficulty = 1f;

    /// <summary>
    /// Momento en el que aparecerá el último enemigo del grupo.
    /// </summary>
    public float LastSpawnTime
    {
        get
        {
            if (enemyCount <= 1)
                return spawnDelay;

            return spawnDelay + (enemyCount - 1) * spawnInterval;
        }
    }
}