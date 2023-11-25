using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
   [SerializeField] UserInputPhone _input;

    private void OnEnable()
    {
        
        //_input.
    }

    private void Update()
    {
        //_input.
    }


    private void HandleMovement()
    {
        Vector2 touchPosition = new Vector2(_input.TouchPosition.x, _input.TouchPosition.y);
        //do logic
    }
}
