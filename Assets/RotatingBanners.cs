using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Rotates any banner around its Z axis by the defined amount, great for sinkholes for players to step into (Eventually these sink holes will be the change of level)
/// </summary>
public class RotatingBanners : MonoBehaviour
{
    [SerializeField] float rotationsPerSecond;



    public void Update()
    {
        transform.Rotate(Vector3.up * 360f * Time.deltaTime * rotationsPerSecond, Space.Self);
    }
}
