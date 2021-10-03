using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RococoAttack : MonoBehaviour
{

    [SerializeField] private RococoAtkType rococoType = RococoAtkType.Idle;
    [SerializeField] private float jumpSpeed = 20.0f;

    private Animator rococoAnim;
    private Transform mouthStart;
    private RaycastHit2D[] mouthRange;

    private bool bitten = false;
    
    private void Awake()
    {
        Destroy(GameObject.Find("Rococo"));
        TryGetComponent(out rococoAnim);
        mouthStart = transform.Find("bone_1");
    }

    private void Update()
    {
        if (rococoType == RococoAtkType.Jump && !bitten)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + jumpSpeed * Time.deltaTime);
        }
        mouthRange = rococoType == RococoAtkType.Idle ? Physics2D.RaycastAll(mouthStart.position, Vector2.down) : Physics2D.RaycastAll(mouthStart.position, Vector2.right * 8);
        foreach (RaycastHit2D ray in mouthRange)
        {
            if (ray.collider.name.ToLower().Contains("ant") || ray.collider.name.ToLower().Contains("tyr"))
            {
                if(rococoType == RococoAtkType.Jump)
                {
                    rococoAnim.SetTrigger("Bite");
                }
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
