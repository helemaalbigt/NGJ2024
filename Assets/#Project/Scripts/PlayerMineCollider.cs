using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMineCollider : MonoBehaviour
{
    public void SetPosition(Vector3 aPosition)
    {
        transform.position = aPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Mine>())
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Die");
    }
}
