using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public HUDInfo hudInfo;
    public PlayerInfo player;
    public List<GameObject> domainList;
    public Camera mainCam = null;

    public enum ELEMENTS
    {
        FIRE = 0,
        ICE,
        LIGHTNING
    };

    //returns reference to crosshair in game
    public GameObject crosshairGO
    {
        get { return hudInfo.crosshairGO; }
    }

    //returns ray/line of crosshair in 3d world space (mainly used for aiming, directing objects to real world space with crosshair)
    public Ray crosshairToRay
    {
        get
        {
            Ray ray = mainCam.ScreenPointToRay(new Vector3(
                    crosshairGO.transform.position.x,
                    crosshairGO.transform.position.y,
                    0));

            return ray;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DomainExpansion(ELEMENTS domain)
    {
        GameObject temp = Instantiate(domainList[(int)domain]);
        yield return new WaitForSeconds(13.0f);
        Destroy(temp);
    }
}
