using UnityEngine;

public enum PlayerInputType
{
    None,
    X360,
    Ps4
}

public class PlayerSelector : MonoBehaviour
{
    public PlayerInputType[] InputTypes = new PlayerInputType[4];

	void Start () {
	    if (GameObject.Find("PlayerSelector") != gameObject)
	    {
	        Destroy(gameObject);
	        return;
	    }

	    DontDestroyOnLoad(gameObject);
	}
}
