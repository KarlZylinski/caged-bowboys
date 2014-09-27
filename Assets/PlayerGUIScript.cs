using UnityEngine;

public class PlayerGUIScript : MonoBehaviour
{
	public string PlayerNum = "1";
	private ArmControl _arm;

	// Use this for initialization
	void Start ()
	{
		Hide();
	}
	
	public void Show()
	{
		GetComponent<SpriteRenderer>().enabled = true;
		transform.FindChild("1").GetComponent<SpriteRenderer>().enabled = true;
		transform.FindChild("2").GetComponent<SpriteRenderer>().enabled = true;
		transform.FindChild("3").GetComponent<SpriteRenderer>().enabled = true;
		transform.FindChild("4").GetComponent<SpriteRenderer>().enabled = true;
		transform.FindChild("1Loaded").GetComponent<SpriteRenderer>().enabled = false;
		transform.FindChild("2Loaded").GetComponent<SpriteRenderer>().enabled = false;
		transform.FindChild("3Loaded").GetComponent<SpriteRenderer>().enabled = false;
		transform.FindChild("4Loaded").GetComponent<SpriteRenderer>().enabled = false;
	}

	public void Hide()
	{

		GetComponent<SpriteRenderer>().enabled = false;
		transform.FindChild("1").GetComponent<SpriteRenderer>().enabled = false;
		transform.FindChild("2").GetComponent<SpriteRenderer>().enabled = false;
		transform.FindChild("3").GetComponent<SpriteRenderer>().enabled = false;
		transform.FindChild("4").GetComponent<SpriteRenderer>().enabled = false;
		transform.FindChild("1Loaded").GetComponent<SpriteRenderer>().enabled = false;
		transform.FindChild("2Loaded").GetComponent<SpriteRenderer>().enabled = false;
		transform.FindChild("3Loaded").GetComponent<SpriteRenderer>().enabled = false;
		transform.FindChild("4Loaded").GetComponent<SpriteRenderer>().enabled = false;
	}

	public void LoadBullet(int index)
	{
		transform.FindChild(index.ToString()).GetComponent<SpriteRenderer>().enabled = false;
		transform.FindChild(index.ToString() + "Loaded").GetComponent<SpriteRenderer>().enabled = true;
	}
}
