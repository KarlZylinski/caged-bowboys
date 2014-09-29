using System.Linq;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    private PlayerSelector _player_selector;
    public bool[] Ready = new bool[4];

	public void Start()
	{
	    _player_selector = GameObject.Find("PlayerSelector").GetComponent<PlayerSelector>();
		Screen.showCursor = false;
	}

    void AddOrRemovePlayer(int index, PlayerInputType type)
    {
        for (var i = 0; i < 4; ++i)
            Ready[i] = false;
        
        _player_selector.InputTypes[index] = type;
    }

	void Update()
	{
	    const string xbox360_controller_join_button = "button 2";
	    const string ps4_controller_join_button = "button 1";
	    const string xbox360_controller_leave_button = "button 1";
	    const string ps4_controller_leave_button = "button 2";

	    for (var i = 0; i < 4; ++i)
	    {

	        if (Input.GetKeyDown("joystick " + (i + 1).ToString() + " " + xbox360_controller_leave_button) && _player_selector.InputTypes[i] == PlayerInputType.X360 && Ready[i])
	            Ready[i] = false;
	        else if (Input.GetKeyDown("joystick " + (i + 1).ToString() + " " + ps4_controller_leave_button) && _player_selector.InputTypes[i] == PlayerInputType.Ps4 && Ready[i])
                Ready[i] = false;
            else if (Input.GetKeyDown("joystick " + (i + 1).ToString() + " " + xbox360_controller_join_button) && _player_selector.InputTypes[i] == PlayerInputType.X360)
                Ready[i] = true;
            else if (Input.GetKeyDown("joystick " + (i + 1).ToString() + " " + ps4_controller_join_button) && _player_selector.InputTypes[i] == PlayerInputType.Ps4)
                Ready[i] = true;
	        else if (Input.GetKeyDown("joystick " + (i + 1).ToString() + " " + xbox360_controller_leave_button) && _player_selector.InputTypes[i] == PlayerInputType.X360)
                AddOrRemovePlayer(i, PlayerInputType.None);
	        else if (Input.GetKeyDown("joystick " + (i + 1).ToString() + " " + ps4_controller_leave_button) && _player_selector.InputTypes[i] == PlayerInputType.Ps4)
                AddOrRemovePlayer(i, PlayerInputType.None);
	        else if (_player_selector.InputTypes[i] == PlayerInputType.None)
	        {
	            if (Input.GetKeyDown("joystick " + (i + 1).ToString() + " " + xbox360_controller_join_button))
	                AddOrRemovePlayer(i, PlayerInputType.X360);
	            else if (Input.GetKeyDown("joystick " + (i + 1).ToString() + " " + ps4_controller_join_button))
                    AddOrRemovePlayer(i, PlayerInputType.Ps4);
	        }
	    }

	    var ready = _player_selector.InputTypes.Any(x => x != PlayerInputType.None);

	    for (var i = 0; i < 4; ++i)
	    {
	        if (_player_selector.InputTypes[i] != PlayerInputType.None && !Ready[i])
	            ready = false;
	    }

        if (ready)
		    Application.LoadLevel(1);

		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
}
