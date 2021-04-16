using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConroller : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;
    private float speed;

    private void Start()
    {
        speed = 3.5f;
        if (!player)
            player = Hero.Instance.transform;
    }

    private void Update()
    {
        pos = player.position;
        pos.z = -10f;
        //pos.y += 3f;

        transform.position = Vector3.Lerp(transform.position, pos,speed* Time.deltaTime);
    }
}
