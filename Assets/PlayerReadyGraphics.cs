using UnityEngine;

public class PlayerReadyGraphics : MonoBehaviour
{
    public int PlayerNum;
    public Sprite XBox360Sprite;
    public Sprite Ps4Sprite;
    private Sprite _default_sprite;
    private PlayerSelector _player_selector;
    private SpriteRenderer _renderer;

    public void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _default_sprite = _renderer.sprite;
        _player_selector = GameObject.Find("PlayerSelector").GetComponent<PlayerSelector>();
    }

	public void Update () {
	    if (_player_selector.InputTypes[PlayerNum] == PlayerInputType.Ps4)
	        _renderer.sprite = Ps4Sprite;

	    if (_player_selector.InputTypes[PlayerNum] == PlayerInputType.X360)
	        _renderer.sprite = XBox360Sprite;

	    if (_player_selector.InputTypes[PlayerNum] == PlayerInputType.None)
            _renderer.sprite = _default_sprite;
	}
}
