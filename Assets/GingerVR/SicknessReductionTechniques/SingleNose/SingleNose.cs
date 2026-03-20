using UnityEngine;

[ExecuteInEditMode]
public class SingleNose : MonoBehaviour
{
    [Header("Paramètres du Nez")]
    [Range(0, 1)] public float yPosition = .5f;
    [Range(0, 1)] public float zPosition = .5f;
    [Range(0f, 1f)] public float noseWidth = 1;
    [Range(0f, 1f)] public float noseFlatness = 1;
    public Color noseColor = Color.white;

    void Update()
    {
        // Calcul des dimensions et de la position basés sur GingerVR
        float zPos = Mathf.Lerp(0.4f, 0.8f, zPosition);
        float yPos = Mathf.Lerp(-0.5f, 0.5f, yPosition);
        float xScale = Mathf.Lerp(0.05f, 0.15f, noseWidth);
        float yScale = Mathf.Lerp(0.05f, 0.25f, 1 - noseFlatness);
        float zScale = 0.1f;

        // Applique les transformations localement à la caméra
        transform.localScale = new Vector3(xScale, yScale, zScale);
        transform.localPosition = new Vector3(0, yPos, zPos);

        // Gestion de la couleur (Compatible Standard et URP)
        Renderer rend = GetComponent<Renderer>();
        if (rend != null && rend.sharedMaterial != null)
        {
            // URP utilise "_BaseColor", le mode Standard utilise "_Color"
            if (rend.sharedMaterial.HasProperty("_BaseColor"))
                rend.sharedMaterial.SetColor("_BaseColor", noseColor);
            else
                rend.sharedMaterial.color = noseColor;
        }
    }
}