using UnityEngine;
using System.Collections;

public class PressXScript : MonoBehaviour
{
    public int PlayerNum;
    private Sprite _default_sprite;
    private SpriteRenderer _renderer;
    private PlayerSelector _player_selector;
    public Sprite XBox360Sprite;
    public Sprite Ps4Sprite;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _default_sprite = _renderer.sprite;
        _player_selector = GameObject.Find("PlayerSelector").GetComponent<PlayerSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player_selector.InputTypes[PlayerNum] == PlayerInputType.Ps4)
            _renderer.sprite = Ps4Sprite;

        if (_player_selector.InputTypes[PlayerNum] == PlayerInputType.X360)
            _renderer.sprite = XBox360Sprite;

        if (_player_selector.InputTypes[PlayerNum] == PlayerInputType.None)
            _renderer.sprite = _default_sprite;
    }
}
