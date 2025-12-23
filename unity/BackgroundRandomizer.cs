using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

// ------------------------------------------------------------
// Randomizer tła sceny dla Unity Perception.
// W każdej iteracji losowo wybiera teksturę tła i przypisuje
// ją do obiektu typu Quad pełniącego rolę tła sceny.
// Tekstury ładowane są z katalogu Resources.
// ------------------------------------------------------------

[AddComponentMenu("Perception/Randomizers/Background Image Randomizer")]
public class BackgroundImageRandomizer : Randomizer
{
    [Tooltip("Tag przypisany do Quad, który pełni rolę tła")]
    public string quadTag = "BackgroundImage";

    [Tooltip("Ścieżka do folderu z teksturami")]
    public string folderPath = "background_images/coffee_tables";

    private MeshRenderer quadRenderer;
    private Texture[] backgroundTextures;

    protected override void OnAwake()
    {
        GameObject quad = GameObject.FindWithTag(quadTag);
        if (quad == null)
        {
            Debug.LogError("Nie znaleziono obiektu z tagiem: " + quadTag);
            return;
        }

        quadRenderer = quad.GetComponent<MeshRenderer>();
        if (quadRenderer == null)
        {
            Debug.LogError("Quad nie ma MeshRenderer");
            return;
        }

        backgroundTextures = Resources.LoadAll<Texture>(folderPath);
        if (backgroundTextures.Length == 0)
        {
            Debug.LogError("Nie znaleziono tekstur w folderze Resources/" + folderPath);
        }
    }

    protected override void OnIterationStart()
    {
        if (quadRenderer == null || backgroundTextures == null || backgroundTextures.Length == 0)
            return;

        int index = Random.Range(0, backgroundTextures.Length);
        quadRenderer.material.mainTexture = backgroundTextures[index];
    }
}
