using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCubeController : MonoBehaviour
{
    private  MainInput _mainInput;
    private float InputAxisX;
    private float InputAxisY;
    private float InputTotalAxisX;
    private float InputTotalAxisY;
    private bool IsMove;
    private Vector2 movementVector;

    private PhotonView view;

    void Start()
    {
        _mainInput = new MainInput();
        _mainInput.Enable();
        _mainInput.Player.Movement.performed += ctx => GetInputMovement(ctx.ReadValue<Vector2>());
        view = GetComponent<PhotonView>();
        var mat = GetComponent<MeshRenderer>().material;
        mat.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }


    void Update()
    {
        if(!view.IsMine)
        {
            return;
        }
        if(IsMove)
        {
            Movement();
        }
    }

    private void Movement()
    {
        gameObject.transform.Translate(Time.deltaTime * movementVector * 10);
    }


    private void GetInputMovement(Vector2 movementVector)
    {
        this.movementVector = movementVector;
        InputAxisX = movementVector.x;
        InputAxisY = movementVector.y;
        CheckAxisTotal();
    }
    private void CheckAxisTotal()
    {
        InputTotalAxisX = InputAxisX > 0 ? 1 : InputAxisX < 0 ? -1 : 0;
        InputTotalAxisY = InputAxisY > 0 ? 1 : InputAxisY < 0 ? -1 : 0;
        IsMove = InputTotalAxisX != 0 || InputTotalAxisY != 0;
    }
}
