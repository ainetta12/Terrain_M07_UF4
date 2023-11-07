using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicloDeDia : MonoBehaviour
{

    [SerializeField] private float _velocity = 5;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_velocity * Time.deltaTime,0,0);
    }
}
