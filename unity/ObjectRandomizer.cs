using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;
using System.Collections.Generic;

// ------------------------------------------------------------
// Randomizer odpowiedzialny za losowe rozmieszczanie wielu obiektów (owoców)
// w scenie Unity.
//
// W każdej iteracji:
// - instancjonuje losową liczbę obiektów z listy prefabów,
// - kontroluje maksymalną liczbę obiektów,
// - losuje skalę (zależną od klasy obiektu),
// - losuje pozycję w przestrzeni viewport kamery,
// - zapobiega nachodzeniu obiektów poprzez sprawdzanie minimalnej odległości
//   zależnej od skali instancji,
// - losuje orientację (pełny obrót wokół osi X, Y i Z),
// - usuwa obiekty z poprzedniej iteracji.
// ------------------------------------------------------------

[AddComponentMenu("Perception/Randomizers/Multiple Fruits Camera Randomizer (No Overlap)")]
public class ObjectRandomizer : Randomizer
{
    [Tooltip("Lista dostępnych prefabów owoców")]
    public List<GameObject> fruitPrefabs = new List<GameObject>();

    [Tooltip("Maksymalna liczba instancji każdego prefabrykatu")]
    public int maxInstancesPerPrefab = 3;

    [Tooltip("Łączna maksymalna liczba owoców na obrazie")]
    public int maxTotalFruits = 7;

    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    public float rotXMin = 0f;
    public float rotXMax = 360f;
    public float rotZMin = 0f;
    public float rotZMax = 360f;

    public float distanceFromCamera = 25f;
    public float minDistance = 3.0f;

    private List<GameObject> spawnedFruits = new List<GameObject>();

    protected override void OnIterationStart()
    {
        foreach (var obj in spawnedFruits)
            if (obj != null)
                GameObject.Destroy(obj);
        spawnedFruits.Clear();

        if (fruitPrefabs.Count == 0)
        {
            Debug.LogWarning("Brak przypisanych prefabów owoców!");
            return;
        }

        int totalFruits = 0;

        foreach (var prefab in fruitPrefabs)
        {
            if (totalFruits >= maxTotalFruits)
                break;

            int remaining = maxTotalFruits - totalFruits;
            int instanceCount = Random.Range(1, Mathf.Min(maxInstancesPerPrefab, remaining) + 1);

            for (int i = 0; i < instanceCount; i++)
            {
                float scale;
                int prefabIndex = fruitPrefabs.IndexOf(prefab);
                if (prefabIndex == 0) // tomato
                    scale = Random.Range(40f, 80f);
                else if (prefabIndex == 1) // pear
                    scale = Random.Range(500f, 900f);    
                else if (prefabIndex == 2) // apple
                    scale = Random.Range(2f, 7f);
                else
                    scale = Random.Range(minScale, maxScale);

                float radius = scale * minDistance;

                Vector3 worldPos = Vector3.zero;
                int attempts = 0;
                bool validPos = false;

                do
                {
                    float x = Random.Range(0.1f, 0.9f);
                    float y = Random.Range(0.1f, 0.9f);
                    Vector3 viewportPos = new Vector3(x, y, distanceFromCamera);
                    worldPos = Camera.main.ViewportToWorldPoint(viewportPos);

                    validPos = true;
                    foreach (var existing in spawnedFruits)
                    {
                        float dist = Vector3.Distance(existing.transform.position, worldPos);
                        float minRequired = radius + (existing.transform.localScale.x * minDistance);
                        if (dist < minRequired)
                        {
                            validPos = false;
                            break;
                        }
                    }
                    
                    attempts++;
                } while (!validPos && attempts < 30);

                Quaternion rot = Quaternion.Euler(
                    Random.Range(rotXMin, rotXMax),
                    Random.Range(0f, 360f),
                    Random.Range(rotZMin, rotZMax)
                );

                var instance = GameObject.Instantiate(prefab, worldPos, rot);
                instance.transform.localScale = Vector3.one * scale;

                spawnedFruits.Add(instance);
                totalFruits++;
            }
        }
    }
}
