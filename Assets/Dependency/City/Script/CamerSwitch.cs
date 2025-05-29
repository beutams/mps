using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerSwitch : MonoBehaviour {
	public GameObject camera1;
	public GameObject camera2;
	public GameObject camera3;
	public GameObject camera4;
	public GameObject camera5;
	public GameObject camera6;
	public GameObject camera7;
	public GameObject camera8;
	public GameObject camera9;
	public GameObject camera10;


	void setCamera01 (){
		onActiveFalse ();
		camera1.SetActive(true);
	}
	void setCamera02 (){
		onActiveFalse ();
		camera2.SetActive(true);
    }
	void setCamera03 (){
		onActiveFalse ();
		camera3.SetActive(true);
    }
	void setCamera04 (){
		onActiveFalse ();
		camera4.SetActive(true);
    }
	void setCamera05 (){
		onActiveFalse ();
		camera5.SetActive(true);
    }
	void setCamera06 (){
		onActiveFalse ();
		camera6.SetActive(true);
    }
	void setCamera07 (){
		onActiveFalse ();
		camera7.SetActive(true);
    }
	void setCamera08 (){
		onActiveFalse ();
		camera8.SetActive(true);
    }
	void setCamera09 (){
		onActiveFalse ();
		camera9.SetActive(true);
    }
	void setCamera10 (){
		onActiveFalse ();
		camera10.SetActive(true);
    }

	void onActiveFalse()
	{
		camera1.SetActive(false);
		camera2.SetActive(false);
        camera3.SetActive(false);
        camera4.SetActive(false);
        camera5.SetActive(false);
        camera6.SetActive(false);
        camera7.SetActive(false);
        camera8.SetActive(false);
        camera9.SetActive(false);
        camera10.SetActive(false);
    }
}
