using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private PlayerView _playerView;

    private UpdateService _updateService;
    private PlayerController _playerController;

    private void Awake()
    {
        PlayerModel playerModel = new PlayerModel();
        _updateService = new UpdateService();
        _playerController = new(_updateService, _playerView, playerModel);
    }

    private void Update()
    {
        _updateService.OnUpdate();
    }
}

