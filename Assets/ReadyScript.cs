using UnityEngine;

public class ReadyScript : MonoBehaviour
{
    public int PlayerNum;
    private Sprite _default_sprite;
    private SpriteRenderer _renderer;
    private TitleScreen _title_screen;
    public Sprite NotReadySprite;
    public Sprite ReadySprite;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _title_screen = GameObject.Find("Main Camera").GetComponent<TitleScreen>();
    }

    void Update()
    {
        _renderer.sprite = _title_screen.Ready[PlayerNum]
            ? ReadySprite
            : NotReadySprite;
    }
}
