using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveConfiguration))]
public class WaveConfigurationEditor : Editor
{
    private SerializedProperty waveName;
    private SerializedProperty duration;
    private SerializedProperty warningAudio;

    private SerializedProperty enemyGroups;

    private SerializedProperty hasBoss;
    private SerializedProperty bossPrefab;
    private SerializedProperty bossSpawnTime;
    private SerializedProperty waveDifficulty;

    private bool showGeneral = true;
    private bool showEnemyGroups = true;
    private bool showBoss = true;
    private bool showQuickTools = true;
    private bool showAnalysis = true;
    private bool showValidation = true;
    private bool showTimeline = true;

    private void OnEnable()
    {
        waveName = serializedObject.FindProperty("waveName");
        duration = serializedObject.FindProperty("duration");
        warningAudio = serializedObject.FindProperty("warningAudio");

        enemyGroups = serializedObject.FindProperty("enemyGroups");

        hasBoss = serializedObject.FindProperty("hasBoss");
        bossPrefab = serializedObject.FindProperty("bossPrefab");
        bossSpawnTime = serializedObject.FindProperty("bossSpawnTime");
        waveDifficulty = serializedObject.FindProperty("waveDifficulty");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawGeneralConfiguration();

        EditorGUILayout.Space(8);

        DrawEnemyGroups();

        EditorGUILayout.Space(8);

        DrawBossConfiguration();

        EditorGUILayout.Space(8);

        DrawQuickTools();

        EditorGUILayout.Space(8);

        DrawWaveAnalysis();

        EditorGUILayout.Space(8);

        DrawWaveValidation();

        EditorGUILayout.Space(8);

        DrawWaveTimeline();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawGeneralConfiguration()
    {
        showGeneral = EditorGUILayout.Foldout(
            showGeneral,
            "General Configuration",
            true
        );

        if (!showGeneral)
            return;

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(waveName);
        EditorGUILayout.PropertyField(duration);
        EditorGUILayout.PropertyField(warningAudio);

        EditorGUI.indentLevel--;
    }

    private void DrawEnemyGroups()
    {
        showEnemyGroups = EditorGUILayout.Foldout(
            showEnemyGroups,
            "Enemy Groups",
            true
        );

        if (!showEnemyGroups)
            return;

        EditorGUILayout.Space(4);

        for (int i = 0; i < enemyGroups.arraySize; i++)
        {
            SerializedProperty group =
                enemyGroups.GetArrayElementAtIndex(i);

            SerializedProperty groupName =
                group.FindPropertyRelative("groupName");

            SerializedProperty enemyPrefab =
                group.FindPropertyRelative("enemyPrefab");

            SerializedProperty spawnDelay =
                group.FindPropertyRelative("spawnDelay");

            SerializedProperty enemyCount =
                group.FindPropertyRelative("enemyCount");

            SerializedProperty spawnInterval =
                group.FindPropertyRelative("spawnInterval");

            SerializedProperty difficulty =
                group.FindPropertyRelative("difficulty");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            // TÍTULO DEL GRUPO
            EditorGUILayout.LabelField(
                string.IsNullOrEmpty(groupName.stringValue)
                    ? "Enemy Group " + (i + 1)
                    : groupName.stringValue,
                EditorStyles.boldLabel
            );

            EditorGUILayout.Space(3);

            // CAMPOS EDITABLES
            EditorGUILayout.PropertyField(
                groupName,
                new GUIContent("Group Name")
            );

            EditorGUILayout.PropertyField(
                enemyPrefab,
                new GUIContent("Enemy Prefab")
            );

            EditorGUILayout.PropertyField(
                spawnDelay,
                new GUIContent("Spawn Delay")
            );

            EditorGUILayout.PropertyField(
                enemyCount,
                new GUIContent("Enemy Count")
            );

            EditorGUILayout.PropertyField(
                spawnInterval,
                new GUIContent("Spawn Interval")
            );

            EditorGUILayout.PropertyField(
                difficulty,
                new GUIContent("Difficulty")
            );

            EditorGUILayout.Space(5);

            // ------------------------
            // DATOS CALCULADOS
            // ------------------------

            float lastSpawnTime =
                spawnDelay.floatValue +
                Mathf.Max(0, enemyCount.intValue - 1) *
                spawnInterval.floatValue;

            float difficultyScore =
                enemyCount.intValue *
                difficulty.floatValue *
                waveDifficulty.floatValue;

            EditorGUILayout.LabelField(
                "Last Spawn Time",
                lastSpawnTime.ToString("0.00") + " s"
            );

            EditorGUILayout.LabelField(
                "Difficulty Score",
                difficultyScore.ToString("0.00")
            );

            EditorGUILayout.Space(5);

            // ------------------------
            // BOTONES
            // ------------------------

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Duplicate"))
            {
                DuplicateEnemyGroup(i);
            }

            if (GUILayout.Button("Delete"))
            {
                enemyGroups.DeleteArrayElementAtIndex(i);

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();

                break;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);
        }

        if (GUILayout.Button("+ Add Enemy Group"))
        {
            AddEnemyGroup();
        }
    }

    private void AddEnemyGroup()
    {
        int index = enemyGroups.arraySize;

        enemyGroups.InsertArrayElementAtIndex(index);

        SerializedProperty group =
            enemyGroups.GetArrayElementAtIndex(index);

        group.FindPropertyRelative("groupName").stringValue =
            "Enemy Group " + (index + 1);

        group.FindPropertyRelative("enemyPrefab").objectReferenceValue =
            null;

        group.FindPropertyRelative("spawnDelay").floatValue = 0f;

        group.FindPropertyRelative("enemyCount").intValue = 5;

        group.FindPropertyRelative("spawnInterval").floatValue = 1f;

        group.FindPropertyRelative("difficulty").floatValue = 1f;
    }

    private void DuplicateEnemyGroup(int index)
    {
        enemyGroups.InsertArrayElementAtIndex(index);

        SerializedProperty duplicatedGroup =
            enemyGroups.GetArrayElementAtIndex(index + 1);

        SerializedProperty groupName =
            duplicatedGroup.FindPropertyRelative("groupName");

        groupName.stringValue += " Copy";
    }

    private void DrawBossConfiguration()
    {
        showBoss = EditorGUILayout.Foldout(
            showBoss,
            "Boss Configuration",
            true
        );

        if (!showBoss)
            return;

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(hasBoss);

        if (hasBoss.boolValue)
        {
            EditorGUILayout.PropertyField(bossPrefab);
            EditorGUILayout.PropertyField(bossSpawnTime);
        }

        EditorGUI.indentLevel--;
    }

    private void DrawQuickTools()
    {
        showQuickTools = EditorGUILayout.Foldout(
            showQuickTools,
            "Quick Tools",
            true
        );

        if (!showQuickTools)
            return;

        EditorGUILayout.HelpBox(
            "Las herramientas rápidas se implementarán en el siguiente paso.",
            MessageType.Info
        );
    }

    private void DrawWaveAnalysis()
    {
        showAnalysis = EditorGUILayout.Foldout(
            showAnalysis,
            "Wave Analysis",
            true
        );

        if (!showAnalysis)
            return;

        EditorGUILayout.HelpBox(
            "El análisis de la oleada se implementará más adelante.",
            MessageType.Info
        );
    }

    private void DrawWaveValidation()
    {
        showValidation = EditorGUILayout.Foldout(
            showValidation,
            "Wave Validation",
            true
        );

        if (!showValidation)
            return;

        EditorGUILayout.HelpBox(
            "La validación se implementará más adelante.",
            MessageType.Info
        );
    }

    private void DrawWaveTimeline()
    {
        showTimeline = EditorGUILayout.Foldout(
            showTimeline,
            "Wave Timeline",
            true
        );

        if (!showTimeline)
            return;

        EditorGUILayout.HelpBox(
            "El timeline visual se implementará más adelante.",
            MessageType.Info
        );
    }
}