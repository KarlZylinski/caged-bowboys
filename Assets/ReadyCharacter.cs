using System;
using UnityEngine;

public class ReadyCharacter : MonoBehaviour
{
    public Sprite NotSelected;
    public Sprite NotReady;
    public Sprite Ready;
    private PlayerSelector _player_selector;
    private TitleScreen _title_screen;
    private SpriteRenderer _renderer;
    public int PlayerNum = 0;

    private Color TintColor()
    {
        if (PlayerNum == 0)
            return Color.white;

        if (PlayerNum == 1)
            return new Color(0.5f, 0.9f, 0.6f);

        if (PlayerNum == 2)
            return new Color(0.9f, 0.5f, 0.6f);

        if (PlayerNum == 3)
            return new Color(0.26f, 0.86f, 0.89f);

        throw new NotImplementedException();
    }
	public void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _title_screen = GameObject.Find("Main Camera").GetComponent<TitleScreen>();
        _player_selector = GameObject.Find("PlayerSelector").GetComponent<PlayerSelector>();
	}
	
	public void Update()
    {
	    _renderer.sprite = _player_selector.InputTypes[PlayerNum] != PlayerInputType.None
            ? _title_screen.Ready[PlayerNum]
                ? Ready
                : NotReady
            : NotSelected;

	    _renderer.color = TintColor();
    }
}
