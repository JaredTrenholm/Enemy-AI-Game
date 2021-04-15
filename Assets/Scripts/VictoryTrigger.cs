using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public GameObject Player;
    public GameObject Cube;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pickup")
        {
            if (Cube.GetComponent<Pickup>().pickedUp == false)
            {
                Player.GetComponent<PlayerMain>().Victory();
            }
        }
    }
}
