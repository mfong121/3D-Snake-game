using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerController playerController;
    private int colCount = 0;
    
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Food")
        {
            playerController.AddSegment();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Player")
        {
/*            Debug.Log("body collision!");*/
            colCount++;
            if (colCount > 1)
            {
                playerController.gameEnd();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //die
        playerController.gameEnd();
    }

    void Start()
    {
        playerController = this.GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
