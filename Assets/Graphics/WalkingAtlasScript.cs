using UnityEngine;
using UnityEditor;
using System.IO;

public class WalkingAtlasScript : MonoBehaviour {
    private static string spritesheet1Path = "Assets/Graphics/nohy-v-ramu.png";
    private static string spritesheet2Path = "Assets/Graphics/nohy-v-ramu 1 - varianta 2.png";
    private static string movedFolderPath = "Assets/Graphics/moved";

    [MenuItem("Tools/Swap Sprite Sheets")]
    public static void SwapSpriteSheets() {
        // Ensure the moved folder exists
        if (!AssetDatabase.IsValidFolder(movedFolderPath)) {
            string parentFolder = Path.GetDirectoryName(movedFolderPath);
            string newFolderName = Path.GetFileName(movedFolderPath);
            AssetDatabase.CreateFolder(parentFolder, newFolderName);
        }

        // Move spritesheet1 to the moved folder
        string spritesheet1NewPath = Path.Combine(movedFolderPath, Path.GetFileName(spritesheet1Path));
        AssetDatabase.MoveAsset(spritesheet1Path, spritesheet1NewPath);

        // Rename spritesheet2 to spritesheet1
        string newSpritesheet1Path = Path.Combine(Path.GetDirectoryName(spritesheet1Path), "nohy-v-ramu.png");
        AssetDatabase.RenameAsset(spritesheet2Path, "nohy-v-ramu");

        AssetDatabase.Refresh();
        Debug.Log("Spritesheet1 moved and Spritesheet2 renamed successfully.");
    }
}
