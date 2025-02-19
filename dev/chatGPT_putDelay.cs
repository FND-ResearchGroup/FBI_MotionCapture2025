using System.Collections.Generic;
using UnityEngine;

public class DelayedMotion : MonoBehaviour
{
    public float delayInSeconds = 0.5f; // Temps de retard souhaité (ex : 500 ms)
    private Queue<Vector3> positionBuffer = new Queue<Vector3>(); // Stocke les positions
    private Queue<Quaternion> rotationBuffer = new Queue<Quaternion>(); // Stocke les rotations
    private float timeBetweenFrames;
    private int frameDelay;

    void Start()
    {
        // Calcul du nombre de frames à attendre en fonction du délai
        timeBetweenFrames = Time.fixedDeltaTime; // Temps entre chaque FixedUpdate
        frameDelay = Mathf.RoundToInt(delayInSeconds / timeBetweenFrames);
    }

    void FixedUpdate()
    {
        // Ajouter la position actuelle reçue du motion tracking à la file d'attente
        positionBuffer.Enqueue(transform.position);
        rotationBuffer.Enqueue(transform.rotation);

        // Appliquer la position retardée si le buffer est assez grand
        if (positionBuffer.Count > frameDelay)
        {
            transform.position = positionBuffer.Dequeue();
            transform.rotation = rotationBuffer.Dequeue();
        }
    }
}
