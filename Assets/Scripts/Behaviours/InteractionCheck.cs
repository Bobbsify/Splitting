using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCheck : MonoBehaviour
{
    [SerializeField] private List<string> tagsToCheck = new List<string> { "Carryable", "Interactable" };
    [Tooltip("list of prefabs to show for each tag (element 0 corresponds to element 0 of other list)")]
    [SerializeField] private List<GameObject> buttonToShow;

    [SerializeField] private Collider2D wallCheck;
    private List<GameObject> interactableObjects;
    private List<GameObject> buttonsShown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in tagsToCheck)
        {
            if (collision.tag.ToLower() == tag.ToLower())
            {
                interactableObjects.Add(collision.gameObject);
                GameObject showBtn = Instantiate(buttonsShown.ToArray()[tagsToCheck.IndexOf(tag)]);
                buttonsShown.Add(showBtn);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (string tag in tagsToCheck)
        {
            if (collision.tag.ToLower() == tag.ToLower())
            {
                buttonsShown.RemoveRange(interactableObjects.IndexOf(collision.gameObject),1);
                interactableObjects.Remove(collision.gameObject);
            }
        }
    }
}
