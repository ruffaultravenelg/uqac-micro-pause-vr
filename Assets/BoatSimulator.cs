using UnityEngine;

public class BoatSimulator : MonoBehaviour
{
    [Header("Paramètres de Tangage (Pitch)")]
    public float pitchAmplitude = 5f;
    public float pitchSpeed = 1.0f;

    [Header("Paramètres de Roulis (Roll)")]
    public float rollAmplitude = 8f;
    public float rollSpeed = 0.8f;

    [Header("Paramètres de Flottaison (Heave)")]
    public float heaveAmplitude = 0.5f;
    public float heaveSpeed = 1.2f;

    private Vector3 startPos;

    void Start() => startPos = transform.position;

    void Update()
    {
        // Calcul des oscillations
        float pitch = Mathf.Sin(Time.time * pitchSpeed) * pitchAmplitude;
        float roll = Mathf.Cos(Time.time * rollSpeed) * rollAmplitude;
        float heave = Mathf.Sin(Time.time * heaveSpeed) * heaveAmplitude;

        // Application de la rotation
        transform.localRotation = Quaternion.Euler(pitch, transform.rotation.eulerAngles.y, roll);
        
        // Application du mouvement vertical
        transform.position = startPos + new Vector3(0, heave, 0);
    }
}