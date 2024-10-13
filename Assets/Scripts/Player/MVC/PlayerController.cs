using System;
using UnityEngine;

[Serializable]
public class PlayerController
{
    private readonly UpdateService _updateService;
    private readonly PlayerView _playerView;
    private readonly PlayerModel _playerModel;
    private readonly InputHandler _inputHandler;
    private readonly Hand _hand;

    private string _horizontalAxis = "Horizontal";
    private string _verticalAxis = "Vertical";

    public float HorizontalDirection { get; private set; }
    public float VerticalDirection { get; private set; }

    public PlayerController(UpdateService updateService, PlayerView playerView, PlayerModel playerModel)
    {
        _updateService = updateService;
        _playerView = playerView;
        _playerModel = playerModel;

        _updateService.Updated += Update;
    }

    private void Update()
    {
        HorizontalDirection = Input.GetAxis(_horizontalAxis);
        VerticalDirection = Input.GetAxis(_verticalAxis);

        float currentHorizontalSpeed = HorizontalDirection * _playerModel.Speed;
        float currentVerticalSpeed = VerticalDirection * _playerModel.Speed;

        _playerView.Rigidbody2D.velocity = new Vector2(currentHorizontalSpeed, currentVerticalSpeed);
        _playerView.Run(currentHorizontalSpeed);
    }
}