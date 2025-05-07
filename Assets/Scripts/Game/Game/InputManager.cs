using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonNetBehaviour<InputManager>
{
    private float speed = 6;
    private List<UnitController> allUnits => RoomController.instance.localPlayer.soldierList;
    private HeroController hero => RoomController.instance.localPlayer.hero;
    public bool LeftClick() => Input.GetMouseButton(0);
    public bool GClick() => Input.GetKeyDown(KeyCode.G);

    private void Update()
    {
        Gather();
        Fire();
        CameraMove();
    }
    private void CameraMove()
    {
        Vector2 axis = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            axis += new Vector2(-speed, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            axis += new Vector2(speed, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            axis += new Vector2(0, speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            axis += new Vector2(0, -speed);
        }
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + axis.x * Time.deltaTime, Camera.main.transform.position.y, Camera.main.transform.position.z + axis.y * Time.deltaTime);
    }
    private void Fire()
    {
        if(LeftClick() && hero != null)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                hero.OnFindVector3(new Vector3(hit.point.x, 0, hit.point.z));
            }
        }
    }

    public void Gather()
    {
        if (GClick())
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                foreach (var unit in allUnits)
                {
                    unit.SetMoveTarget(null, new Vector3(hit.point.x, 0, hit.point.z));
                }
            }
        }
    }
}