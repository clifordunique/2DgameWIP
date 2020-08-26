using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1f);
        transform.localPosition += Vector3.up * 0.1f;
    }
}
