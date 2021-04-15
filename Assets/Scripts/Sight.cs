using UnityEngine;

public class Sight : MonoBehaviour
{
    public bool CanSee = false;
    public bool InTrigger = false;
    public GameObject Player;
    public Camera PlayerCam;
    private RaycastHit hit = new RaycastHit();

    private void Update()
    {
        if (InTrigger == true)
        {
            if (Physics.Raycast(transform.position, Vector3.forward, out hit))
            {
                CanSee = true;
                for (int x = 1; x < 1;)
                {


                    if (hit.collider.gameObject == this.gameObject)
                    {
                        Physics.Raycast(hit.point, Vector3.forward, out hit);
                    }
                    else
                    {
                        x = 1;
                        if ((hit.collider.gameObject == Player) || (hit.collider.gameObject == PlayerCam))
                        {

                            Debug.Log("Player Hit");
                        }
                        else
                        {
                            CanSee = false;
                            Debug.Log("Nothing hit");
                        }
                    }
                }

            }
            else
            {
                Debug.Log("Constantly Triggering");
                CanSee = true; // should never trigger but in place in case player was missed but there was no wall
            }

        }
        else
        {
        }



    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {

            InTrigger = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

            InTrigger = false;
            CanSee = false;

        }
    }
}
