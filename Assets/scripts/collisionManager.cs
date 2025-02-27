using UnityEngine;

public class collisionManager : MonoBehaviour
{

    public bool collisionCylinder = false;
    public bool collisionEnd = false;
    public bool collisionStart = false;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            collisionCylinder=true;
            collisionStart = false;
        } 
        else if (other.gameObject.tag == "Respawn")
        {
            collisionEnd = true;
            collisionStart = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "StartZone")
        {
            collisionStart = true;
            collisionCylinder = false;
            collisionEnd = false;
        }
            
       
    }



}
