using UnityEngine;
using TMPro;

public class VRDataMonitor : MonoBehaviour
{
    public TextMeshProUGUI uiText;

    private Vector3 lastWorldPos;
    private Vector3 lastLocalPos;
    private Quaternion lastWorldRot;

    public float currentMalaiseScore = 0f;
    private float fpsThreshold = 72f; // Seuil critique en VR

    // --- NOUVEAU : Délai pour éviter le pic de démarrage ---
    private float warmupTimer = 0f;
    public float warmupDuration = 2.0f; // Attend 2 secondes avant de mesurer

    void Start()
    {
        lastWorldPos = transform.position;
        lastLocalPos = transform.localPosition;
        lastWorldRot = transform.rotation;
    }

    void Update()
    {
        // --- NOUVEAU : Phase d'initialisation ---
        if (warmupTimer < warmupDuration)
        {
            warmupTimer += Time.deltaTime;

            // On continue de mettre à jour les positions pour éviter 
            // un "saut" au moment où le timer se termine.
            lastWorldPos = transform.position;
            lastLocalPos = transform.localPosition;
            lastWorldRot = transform.rotation;

            if (uiText != null)
                uiText.text = "Initialisation des capteurs...";

            return; // On stoppe l'exécution de la frame ici
        }

        // 1. Vitesse du Monde (Bateau + Tête)
        float worldVelocity = (transform.position - lastWorldPos).magnitude / Time.deltaTime;

        // 2. Vitesse Relative (Mouvements IRL)
        float localVelocity = (transform.localPosition - lastLocalPos).magnitude / Time.deltaTime;

        // 3. Rotation réelle de la tête
        float angularVelocity = Quaternion.Angle(transform.rotation, lastWorldRot) / Time.deltaTime;

        // 4. Calculs pour le score
        float conflict = Mathf.Abs(worldVelocity - localVelocity);
        float rotationFactor = angularVelocity / 45f;

        float fps = 1.0f / Time.unscaledDeltaTime;
        float fpsPenalty = (fps < fpsThreshold) ? (fpsThreshold / fps) : 1f;

        // Accumulation (Seulement si le bateau bouge un minimum)
        if (conflict > 0.1f)
        {
            currentMalaiseScore += (conflict * (1f + rotationFactor) * fpsPenalty) * Time.deltaTime;
        }

        // Affichage final
        if (uiText != null)
        {
            uiText.text = $"FPS: {fps:F0}\n" +
                          $"Stimulus Visuel: {worldVelocity:F2} m/s\n" +
                          $"Mouvement IRL: {localVelocity:F2} m/s\n" +
                          $"Rotation Tête: {angularVelocity:F1} °/s\n" +
                          $"\n<b>Malaise Score: {currentMalaiseScore:F1}</b>";
        }

        // Sauvegarde pour la prochaine frame
        lastWorldPos = transform.position;
        lastLocalPos = transform.localPosition;
        lastWorldRot = transform.rotation;
    }
}