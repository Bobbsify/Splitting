using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RococoAttack : MonoBehaviour
{
    [SerializeField] private RococoAtkType rococoType = RococoAtkType.Idle;
    [SerializeField] private float jumpSpeed;

    private Animator rococoAnim;
    private Transform mouthStart;
    private RaycastHit2D[] mouthRange;

    private bool bitten = false;


    private void Awake()
    {
        TryGetComponent(out rococoAnim);
        mouthStart = transform.Find("bone_1");
    }

    private void Update()
    {
        if (rococoType == RococoAtkType.Jump && !bitten)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + jumpSpeed * Time.deltaTime);
        }
        mouthRange = Physics2D.RaycastAll(mouthStart.position, Vector2.down);
        foreach (RaycastHit2D ray in mouthRange)
        {
            if (ray.collider.name.ToLower().Contains("ant") || ray.collider.name.ToLower().Contains("tyr"))
            {
                if (!bitten)
                {
                    ray.collider.GetComponent<Animator>().SetTrigger("death");
                }
                bitten = true;
            }
        }
    }

}

public enum RococoAtkType
{
    Jump,
    Idle
}
