using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 2 Azioni:
 *
 * (1) Crea un collider nella direzione in cui è orientata l'entità lungo (X) dove x è settabile dal programmatore
 * - Se questo collider colpisce il collider di un oggetto con il tag "Carryable" raccoglilo ed assegnalo al Rigidbody del throw
 * - Sposta l'oggetto in cima all'entità corrente
 *
 */

public class Pickup : MonoBehaviour
{

    [Header("Throw Settings")]
    public Throw throwScript;

    public KeyCode grabButton = KeyCode.E;
    public float grabDistance = 1.0f;

    private Collider2D col;
    private RaycastHit2D grabCheck; 

    // Start is called before the first frame update
    void Start()
    {
        col = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(grabButton))
        {
            Vector2 editedTransform = new Vector2(transform.position.x - (col.bounds.size.x * transform.localScale.x), transform.position.y+5);//+5 perchè il centro dell'oggetto è più sotto del previsto..
            grabCheck = Physics2D.Raycast(editedTransform, Vector2.right * transform.localScale.x,grabDistance);
            Debug.Log("Raycast start:" + editedTransform + "\nRaycasty direction: "+ (Vector2.right * transform.localScale.x));
            Debug.Log(grabCheck.collider + " //// " + grabCheck.collider.tag);
            if (grabCheck.collider != null && grabCheck.collider.tag == "Carryable")
            {
                Debug.Log("Hit");
                Rigidbody2D objRigidbody = grabCheck.collider.gameObject.GetComponent<Rigidbody2D>();
                grabCheck.collider.gameObject.transform.parent = transform;
                grabCheck.collider.gameObject.transform.position = new Vector2(transform.position.x,transform.position.y+10);
                objRigidbody.isKinematic = true;
                throwScript.rbToThrow = objRigidbody;
            }
        }
        // This should be done in the throw
        if (Input.GetKeyUp(grabButton))
        {
            if (grabCheck.collider != null && grabCheck.collider.tag == "Carryable")
            { 
                grabCheck.collider.gameObject.transform.parent = null;
                grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
    }
}
