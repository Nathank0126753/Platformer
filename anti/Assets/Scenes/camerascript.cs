using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerascript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Vector3 offset;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;


    }

    // Update is called once per frame
  
  void Update () 
  {
      player = GameObject.FindWithTag("Player").transform;
      transform.position = new Vector3 (player.position.x + offset.x, player.position.y + offset.y, offset.z);
  }
}
