using UnityEditor;

[CustomEditor(typeof(LightController))]
public class LightControllerEditor : Editor
{

    SerializedProperty bulbLightProp;
    SerializedProperty bulbAnimatorProp;
    SerializedProperty isBlinkingProp;
    SerializedProperty minBlinkDelayProp;
    SerializedProperty maxBlinkDelayProp;
    SerializedProperty isOnProp;

    public override void OnInspectorGUI()
    {
        // Update the serialized object's representation
        serializedObject.Update();

        // Draw the properties manually
        EditorGUILayout.PropertyField(bulbLightProp);
        EditorGUILayout.PropertyField(bulbAnimatorProp);
        EditorGUILayout.PropertyField(isOnProp);

        // Disable 'Is Blinking' field if 'Is On' is false
        using (new EditorGUI.DisabledGroupScope(!isOnProp.boolValue))
        {
            EditorGUILayout.PropertyField(isBlinkingProp);

            // Disable 'Min Blink Delay' and 'Max Blink Delay' fields if 'Is Blinking' is false
            using (new EditorGUI.DisabledGroupScope(!isBlinkingProp.boolValue))
            {
                EditorGUILayout.PropertyField(minBlinkDelayProp);
                EditorGUILayout.PropertyField(maxBlinkDelayProp);
            }
        }

        // Apply any changes made in the inspector
        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        // Link the properties with their serialized counterparts
        bulbLightProp = serializedObject.FindProperty("bulbLight");
        bulbAnimatorProp = serializedObject.FindProperty("bulbAnimator");
        isBlinkingProp = serializedObject.FindProperty("isBlinking");
        minBlinkDelayProp = serializedObject.FindProperty("minBlinkDelay");
        maxBlinkDelayProp = serializedObject.FindProperty("maxBlinkDelay");
        isOnProp = serializedObject.FindProperty("isOn");
    }
}
