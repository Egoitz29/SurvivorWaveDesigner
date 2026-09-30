using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveConfiguration))]
public class WaveConfigurationEditor : Editor
{
    private SerializedProperty waveName;
    private SerializedProperty duration;
    private SerializedProperty warningAudio;
    private SerializedProperty waveDifficulty;
    private SerializedProperty difficultyCurve;
    private SerializedProperty designerNotes;

    private SerializedProperty enemyGroups;

    private SerializedProperty hasBoss;
    private SerializedProperty bossPrefab;
    private SerializedProperty bossSpawnTime;

    private bool showGeneral = true;
    private bool showEnemyGroups = true;
    private bool showBoss = true;
    private bool showQuickTools = true;
    private bool showAnalysis = true;
    private bool showValidation = true;
    private bool showTimeline = true;
    private bool showDifficultyCurve = true;
    private bool showDesignerNotes = true;

    private void OnEnable()
    {
        waveName = serializedObject.FindProperty("waveName");
        duration = serializedObject.FindProperty("duration");
        warningAudio = serializedObject.FindProperty("warningAudio");
        waveDifficulty = serializedObject.FindProperty("waveDifficulty");
        difficultyCurve = serializedObject.FindProperty("difficultyCurve");
        designerNotes = serializedObject.FindProperty("designerNotes");

        enemyGroups = serializedObject.FindProperty("enemyGroups");

        hasBoss = serializedObject.FindProperty("hasBoss");
        bossPrefab = serializedObject.FindProperty("bossPrefab");
        bossSpawnTime = serializedObject.FindProperty("bossSpawnTime");
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

        DrawDifficultyCurve();

        EditorGUILayout.Space(8);

        DrawWaveValidation();

        EditorGUILayout.Space(8);

        DrawWaveTimeline();

        EditorGUILayout.Space(8);

        DrawDesignerNotes();

        serializedObject.ApplyModifiedProperties();
    }

    // =========================================================
    // CONFIGURACI�N GENERAL
    // =========================================================

    private void DrawGeneralConfiguration()
    {
        showGeneral = EditorGUILayout.Foldout(
            showGeneral,
            "Configuraci�n General",
            true
        );

        if (!showGeneral)
            return;

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(
            waveName,
            new GUIContent("Nombre de la oleada")
        );

        EditorGUILayout.PropertyField(
            duration,
            new GUIContent("Duraci�n")
        );

        EditorGUILayout.PropertyField(
            waveDifficulty,
            new GUIContent("Dificultad global")
        );

        EditorGUILayout.PropertyField(
            warningAudio,
            new GUIContent("Audio de aviso")
        );

        EditorGUI.indentLevel--;
    }

    // =========================================================
    // GRUPOS DE ENEMIGOS
    // =========================================================

    private void DrawEnemyGroups()
    {
        showEnemyGroups = EditorGUILayout.Foldout(
            showEnemyGroups,
            "Grupos de Enemigos",
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

            // T�TULO DEL GRUPO
            EditorGUILayout.LabelField(
                string.IsNullOrEmpty(groupName.stringValue)
                    ? "Grupo de Enemigos " + (i + 1)
                    : groupName.stringValue,
                EditorStyles.boldLabel
            );

            EditorGUILayout.Space(3);

            // CAMPOS EDITABLES
            EditorGUILayout.PropertyField(
                groupName,
                new GUIContent("Nombre del grupo")
            );

            EditorGUILayout.PropertyField(
                enemyPrefab,
                new GUIContent("Prefab del enemigo")
            );

            EditorGUILayout.PropertyField(
                spawnDelay,
                new GUIContent("Retraso inicial")
            );

            EditorGUILayout.PropertyField(
                enemyCount,
                new GUIContent("Cantidad de enemigos")
            );

            EditorGUILayout.PropertyField(
                spawnInterval,
                new GUIContent("Intervalo entre spawns")
            );

            EditorGUILayout.PropertyField(
                difficulty,
                new GUIContent("Dificultad")
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
                "Tiempo del �ltimo spawn",
                lastSpawnTime.ToString("0.00") + " s"
            );

            EditorGUILayout.LabelField(
                "Puntuaci�n de dificultad",
                difficultyScore.ToString("0.00")
            );

            EditorGUILayout.Space(5);

            // ------------------------
            // BOTONES
            // ------------------------

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Duplicar"))
            {
                DuplicateEnemyGroup(i);
            }

            if (GUILayout.Button("Eliminar"))
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

        if (GUILayout.Button("+ A�adir grupo de enemigos"))
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
            "Grupo de Enemigos " + (index + 1);

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

        groupName.stringValue += " Copia";
    }

    // =========================================================
    // CONFIGURACI�N DEL BOSS
    // =========================================================

    private void DrawBossConfiguration()
    {
        showBoss = EditorGUILayout.Foldout(
            showBoss,
            "Configuraci�n del Boss",
            true
        );

        if (!showBoss)
            return;

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(
            hasBoss,
            new GUIContent("Tiene Boss")
        );

        if (hasBoss.boolValue)
        {
            EditorGUILayout.PropertyField(
                bossPrefab,
                new GUIContent("Prefab del Boss")
            );

            EditorGUILayout.PropertyField(
                bossSpawnTime,
                new GUIContent("Tiempo de aparici�n")
            );
        }

        EditorGUI.indentLevel--;
    }

    // =========================================================
    // HERRAMIENTAS R�PIDAS
    // =========================================================

    private void DrawQuickTools()
    {
        showQuickTools = EditorGUILayout.Foldout(
            showQuickTools,
            "Herramientas R�pidas",
            true
        );

        if (!showQuickTools)
            return;

        EditorGUILayout.Space(4);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Horda"))
        {
            CreateHordePreset();
        }

        if (GUILayout.Button("Oleada con Boss"))
        {
            CreateBossWavePreset();
        }

        if (GUILayout.Button("Limpiar Oleada"))
        {
            ClearWave();
        }

        EditorGUILayout.EndHorizontal();
    }

    // =========================================================
    // AN�LISIS
    // =========================================================

    private void DrawWaveAnalysis()
    {
        showAnalysis = EditorGUILayout.Foldout(
            showAnalysis,
            "An�lisis de la Oleada",
            true
        );

        if (!showAnalysis)
            return;

        EditorGUILayout.Space(4);

        int totalGroups = enemyGroups.arraySize;
        int totalEnemies = 0;

        float totalDifficultyScore = 0f;
        float lastWaveSpawnTime = 0f;

        for (int i = 0; i < enemyGroups.arraySize; i++)
        {
            SerializedProperty group =
                enemyGroups.GetArrayElementAtIndex(i);

            SerializedProperty spawnDelay =
                group.FindPropertyRelative("spawnDelay");

            SerializedProperty enemyCount =
                group.FindPropertyRelative("enemyCount");

            SerializedProperty spawnInterval =
                group.FindPropertyRelative("spawnInterval");

            SerializedProperty difficulty =
                group.FindPropertyRelative("difficulty");

            // Total de enemigos
            totalEnemies += enemyCount.intValue;

            // Tiempo del �ltimo spawn de este grupo
            float lastSpawnTime =
                spawnDelay.floatValue +
                Mathf.Max(0, enemyCount.intValue - 1) *
                spawnInterval.floatValue;

            if (lastSpawnTime > lastWaveSpawnTime)
            {
                lastWaveSpawnTime = lastSpawnTime;
            }

            // Dificultad total del grupo
            float groupDifficulty =
                enemyCount.intValue *
                difficulty.floatValue *
                waveDifficulty.floatValue;

            totalDifficultyScore += groupDifficulty;
        }

        // Si hay Boss, tambi�n tenemos en cuenta su aparici�n
        if (hasBoss.boolValue &&
            bossSpawnTime.floatValue > lastWaveSpawnTime)
        {
            lastWaveSpawnTime = bossSpawnTime.floatValue;
        }


        float waveUsagePercentage = 0f;

        if (duration.floatValue > 0f)
        {
            waveUsagePercentage =
                (lastWaveSpawnTime / duration.floatValue) * 100f;
        }
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.LabelField(
            "Resumen de la Oleada",
            EditorStyles.boldLabel
        );

        EditorGUILayout.Space(3);
        EditorGUILayout.LabelField(
            "Duraci�n total",
            duration.floatValue.ToString("0.00") + " s"
        );

        EditorGUILayout.LabelField(
            "N�mero de grupos",
            totalGroups.ToString()
        );

        EditorGUILayout.LabelField(
            "Total de enemigos",
            totalEnemies.ToString()
        );

        EditorGUILayout.LabelField(
            "Puntuaci�n total de dificultad",
            totalDifficultyScore.ToString("0.00")
        );

        EditorGUILayout.LabelField(
            "�ltimo evento de spawn",
            lastWaveSpawnTime.ToString("0.00") + " s"
        );

        EditorGUILayout.LabelField(
            "Boss",
            hasBoss.boolValue ? "S�" : "No"
        );
        EditorGUILayout.LabelField(
    "Uso temporal de la oleada",
    waveUsagePercentage.ToString("0.0") + " %"
);

        Rect progressRect =
    GUILayoutUtility.GetRect(
        18,
        18,
        "TextField"
    );

        EditorGUI.ProgressBar(
            progressRect,
            Mathf.Clamp01(waveUsagePercentage / 100f),
            "Uso temporal: " +
            waveUsagePercentage.ToString("0.0") +
            "%"
        );

        if (hasBoss.boolValue)
        {
            EditorGUILayout.LabelField(
                "Aparici�n del Boss",
                bossSpawnTime.floatValue.ToString("0.00") + " s"
            );
        }

        EditorGUILayout.EndVertical();
    }

    // =========================================================
    // CURVA DE DIFICULTAD
    // =========================================================

    private void DrawDifficultyCurve()
    {
        showDifficultyCurve = EditorGUILayout.Foldout(
            showDifficultyCurve,
            "Curva de Dificultad",
            true
        );

        if (!showDifficultyCurve)
            return;

        EditorGUILayout.Space(4);

        EditorGUILayout.HelpBox(
            "La curva representa c�mo cambia la dificultad durante la oleada. " +
            "El eje horizontal va del inicio (0) al final (1), y el eje vertical " +
            "funciona como multiplicador de la dificultad global.",
            MessageType.Info
        );

        EditorGUILayout.PropertyField(
            difficultyCurve,
            new GUIContent("Curva de dificultad")
        );

        EditorGUILayout.Space(5);

        DrawDifficultyCurvePreview();
    }

    private void DrawDifficultyCurvePreview()
    {
        AnimationCurve curve = difficultyCurve.animationCurveValue;

        if (curve == null)
            return;

        float startDifficulty =
            curve.Evaluate(0f) * waveDifficulty.floatValue;

        float middleDifficulty =
            curve.Evaluate(0.5f) * waveDifficulty.floatValue;

        float endDifficulty =
            curve.Evaluate(1f) * waveDifficulty.floatValue;

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.LabelField(
            "Previsualizaci�n de dificultad",
            EditorStyles.boldLabel
        );

        EditorGUILayout.Space(3);

        EditorGUILayout.LabelField(
            "Inicio de la oleada",
            startDifficulty.ToString("0.00") + "x"
        );

        EditorGUILayout.LabelField(
            "Mitad de la oleada",
            middleDifficulty.ToString("0.00") + "x"
        );

        EditorGUILayout.LabelField(
            "Final de la oleada",
            endDifficulty.ToString("0.00") + "x"
        );

        EditorGUILayout.EndVertical();
    }


    // =========================================================
    // NOTAS DE DISE�ADOR
    // =========================================================

    private void DrawDesignerNotes()
    {
        showDesignerNotes = EditorGUILayout.Foldout(
            showDesignerNotes,
            "Notas de Dise�ador",
            true
        );

        if (!showDesignerNotes)
            return;

        EditorGUILayout.Space(4);

        EditorGUILayout.HelpBox(
            "Estas notas son internas para los dise�adores y no se incluyen en la build final.",
            MessageType.Info
        );

        EditorGUILayout.PropertyField(
            designerNotes,
            new GUIContent("Notas")
        );
    }

    // =========================================================
    // VALIDACI�N
    // =========================================================

    private void DrawWaveValidation()
    {
        showValidation = EditorGUILayout.Foldout(
            showValidation,
            "Validaci�n de la Oleada",
            true
        );

        if (!showValidation)
            return;

        EditorGUILayout.Space(4);

        bool hasErrors = false;
        bool hasWarnings = false;

        // =====================================================
        // 1. OLEADA SIN GRUPOS
        // =====================================================

        if (enemyGroups.arraySize == 0)
        {
            EditorGUILayout.HelpBox(
                "La oleada no contiene ning�n grupo de enemigos.",
                MessageType.Warning
            );

            hasWarnings = true;
        }

        // =====================================================
        // 2. VALIDACI�N DE LOS GRUPOS
        // =====================================================

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

            string displayName =
                string.IsNullOrEmpty(groupName.stringValue)
                    ? "Grupo " + (i + 1)
                    : groupName.stringValue;

            // -------------------------------------------------
            // Grupo sin prefab
            // -------------------------------------------------

            if (enemyPrefab.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox(
                    "\"" + displayName +
                    "\" no tiene un prefab de enemigo asignado.",
                    MessageType.Error
                );

                hasErrors = true;
            }

            // -------------------------------------------------
            // �ltimo spawn fuera de la oleada
            // -------------------------------------------------

            float lastSpawnTime =
                spawnDelay.floatValue +
                Mathf.Max(0, enemyCount.intValue - 1) *
                spawnInterval.floatValue;

            if (lastSpawnTime > duration.floatValue)
            {
                EditorGUILayout.HelpBox(
                    "\"" + displayName +
                    "\" termina de generar enemigos en el segundo " +
                    lastSpawnTime.ToString("0.00") +
                    ", pero la oleada dura solamente " +
                    duration.floatValue.ToString("0.00") +
                    " segundos.",
                    MessageType.Error
                );

                if (GUILayout.Button(
                    "Auto Fix - Ajustar intervalo de " + displayName))
                {
                    Undo.RecordObject(
                        serializedObject.targetObject,
                        "Auto Fix - Ajustar grupo"
                    );

                    FixGroupSpawnInterval(group);

                    serializedObject.ApplyModifiedProperties();

                    EditorUtility.SetDirty(
                        serializedObject.targetObject
                    );
                }

                hasErrors = true;
            }
        }

        // =====================================================
        // 3. VALIDACI�N DEL BOSS
        // =====================================================

        if (hasBoss.boolValue)
        {
            // Boss activado pero sin prefab
            if (bossPrefab.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox(
                    "El Boss est� activado, pero no tiene un prefab asignado.",
                    MessageType.Error
                );

                hasErrors = true;
            }

            // Boss aparece despu�s de terminar la oleada
            if (bossSpawnTime.floatValue > duration.floatValue)
            {
                EditorGUILayout.HelpBox(
                    "El Boss aparece en el segundo " +
                    bossSpawnTime.floatValue.ToString("0.00") +
                    ", pero la oleada termina en el segundo " +
                    duration.floatValue.ToString("0.00") +
                    ".",
                    MessageType.Error
                );

                if (GUILayout.Button(
                    "Auto Fix - Mover Boss al 75% de la oleada"))
                {
                    Undo.RecordObject(
                        serializedObject.targetObject,
                        "Auto Fix - Boss"
                    );

                    FixBossSpawnTime();

                    serializedObject.ApplyModifiedProperties();

                    EditorUtility.SetDirty(
                        serializedObject.targetObject
                    );
                }

                hasErrors = true;
            }
        }

        // =====================================================
        // 4. RESULTADO FINAL
        // =====================================================

        if (!hasErrors && !hasWarnings)
        {
            EditorGUILayout.HelpBox(
                "La configuraci�n de la oleada es v�lida.",
                MessageType.Info
            );
        }
    }
    // =========================================================
    // L�NEA TEMPORAL
    // =========================================================

    private void DrawWaveTimeline()
    {
        showTimeline = EditorGUILayout.Foldout(
            showTimeline,
            "L�nea Temporal de la Oleada",
            true
        );

        if (!showTimeline)
            return;

        EditorGUILayout.Space(5);

        if (duration.floatValue <= 0f)
        {
            EditorGUILayout.HelpBox(
                "La duraci�n de la oleada debe ser mayor que 0 para mostrar la l�nea temporal.",
                MessageType.Warning
            );

            return;
        }

        float timelineWidth = EditorGUIUtility.currentViewWidth - 50f;
        float rowHeight = 30f;
        float headerHeight = 48f;
        int totalRows = enemyGroups.arraySize;

        if (hasBoss.boolValue)
            totalRows++;

        float timelineHeight =
            headerHeight +
            Mathf.Max(1, totalRows) * rowHeight +
            10f;

        Rect timelineRect =
            GUILayoutUtility.GetRect(
                timelineWidth,
                timelineHeight
            );

        // Fondo general
        EditorGUI.DrawRect(
            timelineRect,
            new Color(0.15f, 0.15f, 0.15f)
        );

        DrawTimelineHeader(
            timelineRect,
            headerHeight
        );

        float currentY =
            timelineRect.y + headerHeight;

        // Dibujar grupos
        for (int i = 0; i < enemyGroups.arraySize; i++)
        {
            SerializedProperty group =
                enemyGroups.GetArrayElementAtIndex(i);

            DrawEnemyGroupTimeline(
                timelineRect,
                group,
                currentY,
                rowHeight,
                i
            );

            currentY += rowHeight;
        }

        // Dibujar Boss
        if (hasBoss.boolValue)
        {
            DrawBossTimeline(
                timelineRect,
                currentY,
                rowHeight
            );
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField(
            "0 s",
            "Fin: " + duration.floatValue.ToString("0.00") + " s"
        );
    }

    // =========================================================
    // PRESET: HORDA
    // =========================================================

    private void CreateHordePreset()
    {
        Undo.RecordObject(
            serializedObject.targetObject,
            "Crear preset de Horda"
        );

        enemyGroups.ClearArray();

        AddPresetGroup(
            "Horda - R�pida",
            0f,
            12,
            0.5f,
            0.8f
        );

        AddPresetGroup(
            "Horda - Media",
            4f,
            10,
            1f,
            1f
        );

        AddPresetGroup(
            "Horda - Pesada",
            8f,
            6,
            2f,
            1.5f
        );

        hasBoss.boolValue = false;
        bossPrefab.objectReferenceValue = null;

        EditorUtility.SetDirty(serializedObject.targetObject);

        Debug.Log("Preset de Horda creado.");
    }

    // =========================================================
    // CREAR GRUPO DE PRESET
    // =========================================================

    private void AddPresetGroup(
        string groupNameValue,
        float delay,
        int count,
        float interval,
        float difficultyValue)
    {
        int index = enemyGroups.arraySize;

        enemyGroups.InsertArrayElementAtIndex(index);

        SerializedProperty group =
            enemyGroups.GetArrayElementAtIndex(index);

        group.FindPropertyRelative("groupName").stringValue =
            groupNameValue;

        group.FindPropertyRelative("enemyPrefab").objectReferenceValue =
            null;

        group.FindPropertyRelative("spawnDelay").floatValue =
            delay;

        group.FindPropertyRelative("enemyCount").intValue =
            count;

        group.FindPropertyRelative("spawnInterval").floatValue =
            interval;

        group.FindPropertyRelative("difficulty").floatValue =
            difficultyValue;
    }

    // =========================================================
    // PRESET: OLEADA CON BOSS
    // =========================================================

    private void CreateBossWavePreset()
    {
        Undo.RecordObject(
            serializedObject.targetObject,
            "Crear preset de Oleada con Boss"
        );

        enemyGroups.ClearArray();

        AddPresetGroup(
            "Apoyo Boss - Inicial",
            0f,
            6,
            2f,
            1f
        );

        AddPresetGroup(
            "Apoyo Boss - Intermedio",
            10f,
            8,
            1.5f,
            1.2f
        );

        hasBoss.boolValue = true;

        bossPrefab.objectReferenceValue = null;

        bossSpawnTime.floatValue =
            duration.floatValue * 0.75f;

        EditorUtility.SetDirty(serializedObject.targetObject);

        Debug.Log("Preset de Oleada con Boss creado.");
    }

    // =========================================================
    // LIMPIAR OLEADA
    // =========================================================

    private void ClearWave()
    {
        bool confirm = EditorUtility.DisplayDialog(
            "Limpiar Oleada",
            "�Seguro que quieres borrar todos los grupos de enemigos y la configuraci�n del Boss?",
            "Limpiar",
            "Cancelar"
        );

        if (!confirm)
            return;

        Undo.RecordObject(
            serializedObject.targetObject,
            "Limpiar Oleada"
        );

        enemyGroups.ClearArray();

        hasBoss.boolValue = false;
        bossPrefab.objectReferenceValue = null;
        bossSpawnTime.floatValue = 0f;

        EditorUtility.SetDirty(serializedObject.targetObject);

        Debug.Log("Configuraci�n de la oleada eliminada.");
    }

    private void FixGroupSpawnInterval(SerializedProperty group)
    {
        SerializedProperty spawnDelay =
            group.FindPropertyRelative("spawnDelay");

        SerializedProperty enemyCount =
            group.FindPropertyRelative("enemyCount");

        SerializedProperty spawnInterval =
            group.FindPropertyRelative("spawnInterval");

        // Si solo hay un enemigo, no hace falta intervalo.
        if (enemyCount.intValue <= 1)
        {
            spawnDelay.floatValue =
                Mathf.Min(
                    spawnDelay.floatValue,
                    duration.floatValue
                );

            return;
        }

        float availableTime =
            duration.floatValue -
            spawnDelay.floatValue;

        // Si el delay ya est� fuera de la duraci�n,
        // lo colocamos dentro de la oleada.
        if (availableTime <= 0f)
        {
            spawnDelay.floatValue = 0f;
            availableTime = duration.floatValue;
        }

        float newInterval =
            availableTime /
            (enemyCount.intValue - 1);

        spawnInterval.floatValue =
            Mathf.Max(0.01f, newInterval);
    }

    private void FixBossSpawnTime()
    {
        bossSpawnTime.floatValue =
            duration.floatValue * 0.75f;
    }

    private void DrawTimelineHeader(
        Rect timelineRect,
        float headerHeight)
    {
        Rect headerRect = new Rect(
            timelineRect.x,
            timelineRect.y,
            timelineRect.width,
            headerHeight
        );

        EditorGUI.DrawRect(
            headerRect,
            new Color(0.10f, 0.10f, 0.10f)
        );

        // T�TULO DEL TIMELINE
        Rect titleRect = new Rect(
            timelineRect.x + 6f,
            timelineRect.y + 3f,
            timelineRect.width - 12f,
            18f
        );

        GUI.Label(
            titleRect,
            "Tiempo de la oleada",
            EditorStyles.boldLabel
        );

        int divisions = 5;

        for (int i = 0; i <= divisions; i++)
        {
            float normalized =
                i / (float)divisions;

            float time =
                duration.floatValue * normalized;

            float x =
                timelineRect.x +
                timelineRect.width * normalized;

            // L�nea vertical
            Rect lineRect = new Rect(
                x,
                timelineRect.y + 42f,
                1f,
                timelineRect.height - 42f
            );

            EditorGUI.DrawRect(
                lineRect,
                new Color(0.3f, 0.3f, 0.3f)
            );

            // TEXTO DEL TIEMPO
            float labelWidth = 45f;

            float labelX = x - labelWidth / 2f;

            // Evitamos que 0s y el �ltimo valor
            // se salgan del timeline.
            labelX = Mathf.Clamp(
                labelX,
                timelineRect.x + 2f,
                timelineRect.xMax - labelWidth - 2f
            );

            Rect textRect = new Rect(
                labelX,
                timelineRect.y + 23f,
                labelWidth,
                18f
            );

            GUIStyle centeredStyle =
                new GUIStyle(EditorStyles.miniLabel);

            centeredStyle.alignment =
                TextAnchor.MiddleCenter;

            GUI.Label(
                textRect,
                time.ToString("0") + "s",
                centeredStyle
            );
        }
    }

    private void DrawEnemyGroupTimeline(
        Rect timelineRect,
        SerializedProperty group,
        float y,
        float rowHeight,
        int index)
    {
        SerializedProperty groupName =
            group.FindPropertyRelative("groupName");

        SerializedProperty spawnDelay =
            group.FindPropertyRelative("spawnDelay");

        SerializedProperty enemyCount =
            group.FindPropertyRelative("enemyCount");

        SerializedProperty spawnInterval =
            group.FindPropertyRelative("spawnInterval");

        float lastSpawnTime =
            spawnDelay.floatValue +
            Mathf.Max(0, enemyCount.intValue - 1) *
            spawnInterval.floatValue;

        float normalizedStart =
            Mathf.Clamp01(
                spawnDelay.floatValue /
                duration.floatValue
            );

        float normalizedEnd =
            Mathf.Clamp01(
                lastSpawnTime /
                duration.floatValue
            );

        float startX =
            timelineRect.x +
            normalizedStart *
            timelineRect.width;

        float endX =
            timelineRect.x +
            normalizedEnd *
            timelineRect.width;

        float width =
            Mathf.Max(
                6f,
                endX - startX
            );

        // Dejamos espacio arriba y abajo
        Rect groupBar = new Rect(
            startX,
            y + 6f,
            width,
            rowHeight - 12f
        );

        Color barColor;

        switch (index % 3)
        {
            case 0:
                barColor = new Color(
                    0.25f,
                    0.55f,
                    0.85f
                );
                break;

            case 1:
                barColor = new Color(
                    0.30f,
                    0.75f,
                    0.40f
                );
                break;

            default:
                barColor = new Color(
                    0.85f,
                    0.55f,
                    0.20f
                );
                break;
        }

        EditorGUI.DrawRect(
            groupBar,
            barColor
        );

        string name =
            string.IsNullOrEmpty(groupName.stringValue)
                ? "Grupo " + (index + 1)
                : groupName.stringValue;

        // Margen interno para que el texto
        // no empiece pegado al borde.
        Rect labelRect = new Rect(
            groupBar.x + 5f,
            groupBar.y,
            Mathf.Max(0f, groupBar.width - 10f),
            groupBar.height
        );

        GUIStyle groupStyle =
            new GUIStyle(EditorStyles.miniLabel);

        groupStyle.alignment =
            TextAnchor.MiddleLeft;

        groupStyle.normal.textColor =
            Color.white;

        GUI.Label(
            labelRect,
            name,
            groupStyle
        );
    }

    private void DrawBossTimeline(
        Rect timelineRect,
        float y,
        float rowHeight)
    {
        float normalizedTime =
            Mathf.Clamp01(
                bossSpawnTime.floatValue /
                duration.floatValue
            );

        float x =
            timelineRect.x +
            normalizedTime *
            timelineRect.width;

        // Marcador vertical del Boss
        Rect bossMarker = new Rect(
            x - 2f,
            y + 5f,
            4f,
            rowHeight - 10f
        );

        EditorGUI.DrawRect(
            bossMarker,
            new Color(
                0.85f,
                0.15f,
                0.15f
            )
        );

        // Texto separado del marcador
        Rect bossLabel = new Rect(
            x + 9f,
            y + 5f,
            100f,
            rowHeight - 10f
        );

        GUIStyle bossStyle =
            new GUIStyle(EditorStyles.boldLabel);

        bossStyle.alignment =
            TextAnchor.MiddleLeft;

        GUI.Label(
            bossLabel,
            "BOSS",
            bossStyle
        );
    }
}