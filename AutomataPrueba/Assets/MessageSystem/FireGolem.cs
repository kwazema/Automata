using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGolem : MonoBehaviour, FloorMessage
{
    [SerializeField]
    private SpatialIndex spatial;
    private ChangeColorShader changeColor;
    private int lastState;

    private void Awake()
    {
        changeColor = GetComponent<ChangeColorShader>();
    }

    public void getFloorInfo(SpatialIndex.FLOOR_STATUS state)
    {
        if (CharacterMove.checkWithFloor((int)state, (int)SpatialIndex.FLOOR_STATUS.LAVA))
        {
            if (CheckIfChangeFloor((int)state))
                changeColor.StartLerpColor(2, Color.red);

            transform.localScale += Vector3.one * Time.deltaTime;
        }
        else if (CharacterMove.checkWithFloor((int)state, (int)SpatialIndex.FLOOR_STATUS.WATER))
        {
            if (CheckIfChangeFloor((int)state))
                changeColor.StartLerpColor(1.2f, Color.blue);


            if (transform.localScale.x >= 0.2f)
                transform.localScale -= Vector3.one * Time.deltaTime / 4;
        }
        else
        {
            if (CheckIfChangeFloor((int)state))
                changeColor.StartLerpColor(1.3f, Color.yellow);
        }
    }

    private bool CheckIfChangeFloor(int state)
    {
        //bool value = (actualState != state) ? true : false;
        bool value = false;

        if (lastState != state)
        {
            value = true;
            lastState = state;
        }

        return value;
    }

    void Update()
    {
        spatial.getFloorState(transform.position.x, transform.position.z, gameObject);
    }
}
