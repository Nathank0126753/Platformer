using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public NewBehaviourScript player;
    private bool actOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        player = GameObject.FindWithTag("Player").GetComponent<NewBehaviourScript>();
        if (player.hasTouchedEnder && !actOnce){
            transform.Translate(new Vector3(+13f, 0f, 0f));
            actOnce = true;
        }
        if (!player.hasTouchedEnder){
            actOnce = false;
        }
    }
}
