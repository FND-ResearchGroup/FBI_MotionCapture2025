using UnityEngine;

public class collisionManager : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            Debug.Log("Collision");
        }
    }

    
}
