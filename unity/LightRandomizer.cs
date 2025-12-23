using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

// ------------------------------------------------------------
// Randomizer modyfikujący barwę (HSV) i jasność materiałów wszystkich obiektów
// posiadających komponent Labeling. Wpływa zarówno na obiekty pierwszego planu,
// jak i na tło.
// ------------------------------------------------------------

[AddRandomizerMenu("Custom/Light Randomizer")]
public class LightRandomizer : Randomizer
{
    [Range(0.5f, 1.5f)]
    public float brightnessRange = 0.3f; 

    [Range(-0.2f, 0.2f)]
    public float hueShiftRange = 0.1f; 

    protected override void OnIterationStart()
    {
        var labeledObjects = Object.FindObjectsOfType<UnityEngine.Perception.GroundTruth.LabelManagement.Labeling>();

        foreach (var labeledObject in labeledObjects)
        {
            var renderer = labeledObject.GetComponent<Renderer>();
            if (renderer == null) continue;

            var mat = renderer.material;
            Color baseColor = mat.color;

            Color.RGBToHSV(baseColor, out float h, out float s, out float v);

            h = Mathf.Repeat(h + Random.Range(-hueShiftRange, hueShiftRange), 1f);
            v = Mathf.Clamp01(v * Random.Range(1f - brightnessRange, 1f + brightnessRange));

            Color newColor = Color.HSVToRGB(h, s, v);
            mat.color = newColor;
        }
    }
}