using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)     // Here to Detect Pucks Enter the Pot or not
    {
        if(collision.gameObject.CompareTag("PotDetector"))
        {
            StartCoroutine(DestroyPuck(collision));         // Starts Destroy Puck Coroutine
        }
    }


    private void OnTriggerExit2D(Collider2D collision)          // Here to Detect Pucks Exit the Pot or not
    {
        if (collision.gameObject.CompareTag("PotDetector"))
        {
            StopAllCoroutines();                                // Stop Destroy Puck Coroutine
        }
    }

    IEnumerator DestroyPuck(Collider2D _puck)
    {

        yield return new WaitForSeconds(1f);
        if(_puck != null)
        {
            _puck.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector3.zero;     // reduce Velocity of Pucks if the Pucks inside the Pot
            yield return new WaitForSeconds(.1f);
            _puck.transform.parent.gameObject.SetActive(false);

        }
    }
}
