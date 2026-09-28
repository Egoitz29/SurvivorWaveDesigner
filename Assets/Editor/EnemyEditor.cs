using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    private SerializedProperty vida;
    private SerializedProperty ataque;
    private SerializedProperty velocidad;
    private SerializedProperty targetObject;

    private bool show = false;

    private void OnEnable()
    {
        vida = serializedObject.FindProperty("vida");
        ataque = serializedObject.FindProperty("ataque");
        velocidad = serializedObject.FindProperty("velocidad");
        targetObject = serializedObject.FindProperty("target");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Botón Show / Hide
        if (GUILayout.Button(show ? "Hide" : "Show"))
        {
            show = !show;
        }

        // Solo mostramos los atributos si pulsamos Show
        if (show)
        {
            EditorGUILayout.Space();

            EditorGUILayout.LabelField(
                "Enemy Attributes",
                EditorStyles.boldLabel
            );

            EditorGUILayout.PropertyField(vida);
            EditorGUILayout.PropertyField(ataque);
            EditorGUILayout.PropertyField(velocidad);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(targetObject);

            EditorGUILayout.Space();

            // Cogemos el GameObject asignado como Target
            GameObject targetGameObject =
                targetObject.objectReferenceValue as GameObject;

            if (targetGameObject != null)
            {
                Enemy targetEnemy =
                    targetGameObject.GetComponent<Enemy>();

                if (targetEnemy != null)
                {
                    EditorGUILayout.LabelField(
                        "Target Enemy",
                        EditorStyles.boldLabel
                    );

                    EditorGUILayout.LabelField(
                        "Vida",
                        targetEnemy.vida.ToString()
                    );

                    EditorGUILayout.LabelField(
                        "Ataque",
                        targetEnemy.ataque.ToString()
                    );

                    EditorGUILayout.LabelField(
                        "Velocidad",
                        targetEnemy.velocidad.ToString()
                    );
                }
                else
                {
                    EditorGUILayout.HelpBox(
                        "El Target no tiene un componente Enemy.",
                        MessageType.Warning
                    );
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
