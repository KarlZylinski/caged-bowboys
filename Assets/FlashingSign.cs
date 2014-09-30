using UnityEngine;
using System.Collections;

public class FlashingSign : MonoBehaviour
{
    public Sprite On;
    public Sprite Off;

    private SpriteRenderer _renderer;
    private bool _on;

    public void Start()
    {
        _on = false;
        _renderer = GetComponent<SpriteRenderer>();
    }

	void Update ()
	{
	    if (_on && Random.Range(1, 1000) < 15)
	        _on = false;
        else if (!_on && Random.Range(1, 1000) < 100)
            _on = true;

	    _renderer.sprite = _on ? On : Off;
	}
}
