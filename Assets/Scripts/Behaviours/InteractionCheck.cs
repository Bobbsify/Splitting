using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCheck : MonoBehaviour
{
    [SerializeField] private GameObject interactionPrefab;
    private GameObject interactionPrefabInstantiated;
    private bool isPlayerInRange = false;

    public void Update()
    {
        if (isPlayerInRange)
        {
            if (interactionPrefabInstantiated == null)
            {
                interactionPrefabInstantiated = Instantiate(interactionPrefab);
                interactionPrefabInstantiated.transform.parent = transform;
                Collider2D col;
                TryGetComponent(out col);
                interactionPrefabInstantiated.transform.localPosition = new Vector3(0, col.bounds.extents.y, 0);
            }
        }
        else
        {
            Destroy(interactionPrefabInstantiated);
            interactionPrefabInstantiated = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.ToLower() == "player")
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.ToLower() == "player")
        {
            isPlayerInRange = false;
        }
    }
}
