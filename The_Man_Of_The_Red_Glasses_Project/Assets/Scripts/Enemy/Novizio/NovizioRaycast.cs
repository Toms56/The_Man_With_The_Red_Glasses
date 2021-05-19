using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovizioRaycast : MonoBehaviour
{
    [SerializeField]
    private Transform castPoint;

    [SerializeField]
    private Transform player;

    [SerializeField] private float agroRange;

    [SerializeField] private float speed;

    private Rigidbody rb;
    
    private bool isAgro = false;

    private bool isSearching;

    private bool isFacingLeft;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSeePlayer(agroRange))
        {
            isAgro = true;
        }
        else
        {
            if (isAgro)
            {
                if (!isSearching)
                {
                    isSearching = true;
                    Invoke("StopChasingPlayer",5);
                }
            }
        }

        if (isAgro)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        if (transform.position.x < player.position.x)
        {
            rb.velocity = new Vector2(-speed,0);
            transform.localScale = new Vector2(1,1);
        }
        else
        {
            rb.velocity = new Vector2(-speed,0);
            transform.localScale = new Vector2(-1,1);
        }
    }

    void StopChasingPlayer()
    {
        rb.velocity = new Vector2(0, 0);
    }

    bool CanSeePlayer(float distance)
    {
        bool val = false;
        float castDist = distance;
        if (isFacingLeft)
        {
            castDist = -distance;
        }

        Vector2 endPos = castPoint.position + Vector3.left * castDist;

        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Action"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                val = true;
            }
            else
            {
                val = false;
            }
            Debug.DrawLine(castPoint.position, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(castPoint.position, endPos, Color.blue);
        }
        return val;
    }
}
