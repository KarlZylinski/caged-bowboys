using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    private PlayerSelector _player_selector;

	public void Start()
	{
	    _player_selector = GameObject.Find("PlayerSelector").GetComponent<PlayerSelector>();
		Screen.showCursor = false;
	}

	void Update()
	{
	    const string xbox360_controller_join_button = "button 2";
	    const string ps4_controller_join_button = "button 1";
	    const string xbox360_controller_leave_button = "button 1";
	    const string ps4_controller_leave_button = "button 2";

	    for (var i = 0; i < 4; ++i)
	    {
	        
            if (Input.GetKeyDown("joystick " + (i + 1).ToString() + " " + xbox360_controller_leave_button) && _player_selector.InputTypes[i] == PlayerInputType.X360)
	            _player_selector.InputTypes[i] = PlayerInputType.None;
            else if (Input.GetKeyDown("joystick " + (i + 1).ToString() + " " + ps4_controller_leave_button) && _player_selector.InputTypes[i] == PlayerInputType.Ps4)
	            _player_selector.InputTypes[i] = PlayerInputType.None;
	        else if (Input.GetKeyDown("joystick " + (i + 1).ToString() + " " + xbox360_controller_join_button))
	            _player_selector.InputTypes[i] = PlayerInputType.X360;
	        else if (Input.GetKeyDown("joystick " + (i + 1).ToString() + " " + ps4_controller_join_button))
	            _player_selector.InputTypes[i] = PlayerInputType.Ps4;
	    }

		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
}
