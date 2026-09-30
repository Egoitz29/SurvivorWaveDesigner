using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NuevaConfiguracionOleada",
    menuName = "Survivor Wave Designer/Configuraci�n de Oleada"
)]
public class WaveConfiguration : ScriptableObject
{
    // -----------------------------
    // CONFIGURACI�N GENERAL
    // -----------------------------

    [Tooltip("Nombre identificativo de la oleada.")]
    public string waveName = "Nueva Oleada";

    [Tooltip("Duraci�n total de la oleada en segundos.")]
    [Min(1f)]
    public float duration = 60f;

    [Tooltip("Multiplicador global de dificultad de la oleada.")]
    [Range(0.1f, 5f)]
    public float waveDifficulty = 1f;

    [Tooltip("Curva de dificultad general de la oleada a lo largo del tiempo.")]
    public AnimationCurve difficultyCurve =
        AnimationCurve.Linear(0f, 1f, 1f, 1f);

    [Tooltip("Sonido que se reproducir� al comenzar la oleada.")]
    public AudioClip warningAudio;

    // -----------------------------
    // GRUPOS DE ENEMIGOS
    // -----------------------------

    [Tooltip("Grupos de enemigos que forman esta oleada.")]
    public List<EnemySpawnGroup> enemyGroups = new List<EnemySpawnGroup>();

    // -----------------------------
    // CONFIGURACI�N DEL BOSS
    // -----------------------------

    [Tooltip("Indica si esta oleada contiene un boss.")]
    public bool hasBoss = false;

    [Required]
    [Tooltip("Prefab del boss.")]
    public GameObject bossPrefab;

    [Tooltip("Momento de aparici�n del boss en segundos.")]
    [Min(0f)]
    public float bossSpawnTime = 45f;

#if UNITY_EDITOR

    // -----------------------------
    // NOTAS DE DISE�ADOR
    // Solo existen dentro del Editor.
    // No se incluyen en la build final.
    // -----------------------------

    [TextArea(4, 10)]
    [Tooltip("Notas internas para los dise�adores de la oleada.")]
    public string designerNotes = "";

#endif
}
