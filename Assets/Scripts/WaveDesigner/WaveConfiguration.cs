using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewWaveConfiguration",
    menuName = "Survivor Wave Designer/Wave Configuration"
)]
public class WaveConfiguration : ScriptableObject
{
    // -----------------------------
    // GENERAL CONFIGURATION
    // -----------------------------

    [Tooltip("Nombre identificativo de la oleada.")]
    public string waveName = "New Wave";

    [Tooltip("Duración total de la oleada en segundos.")]
    [Min(1f)]
    public float duration = 60f;

    [Tooltip("Multiplicador global de dificultad de la oleada.")]
    [Range(0.1f, 5f)]
    public float waveDifficulty = 1f;

    [Tooltip("Sonido que se reproducirá al comenzar la oleada.")]
    public AudioClip warningAudio;


    // -----------------------------
    // ENEMY GROUPS
    // -----------------------------

    [Tooltip("Grupos de enemigos que forman esta oleada.")]
    public List<EnemySpawnGroup> enemyGroups = new List<EnemySpawnGroup>();


    // -----------------------------
    // BOSS CONFIGURATION
    // -----------------------------

    [Tooltip("Indica si esta oleada contiene un boss.")]
    public bool hasBoss = false;

    [Required]
    [Tooltip("Prefab del boss.")]
    public GameObject bossPrefab;

    [Tooltip("Momento de aparición del boss en segundos.")]
    [Min(0f)]
    public float bossSpawnTime = 45f;
}