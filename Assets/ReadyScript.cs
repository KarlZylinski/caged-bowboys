using UnityEngine;

public class ReadyScript : MonoBehaviour
{
    public int PlayerNum;
    private Sprite _default_sprite;
    private SpriteRenderer _renderer;
    private TitleScreen _title_screen;
    private PlayerSelector _player_selector;
    public Sprite NotReadySprite;
    public Sprite ReadySprite;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _title_screen = GameObject.Find("Main Camera").GetComponent<TitleScreen>();
        _player_selector = GameObject.Find("PlayerSelector").GetComponent<PlayerSelector>();
    }

    void Update()
    {
        _renderer.sprite = _title_screen.Ready[PlayerNum]
            ? ReadySprite
            : NotReadySprite;

        _renderer.enabled = _player_selector.InputTypes[PlayerNum] != PlayerInputType.None;
    }
}
