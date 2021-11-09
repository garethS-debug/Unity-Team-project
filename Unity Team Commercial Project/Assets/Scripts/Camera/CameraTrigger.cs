using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [Header("What Camer Angle")]
    public bool setCamTriggerFrontFacing;
    public bool setCamTriggerOverShoulder;

    [Header("Only trigger once")]
    public bool CamTriggered;

    //  public Vector3 NewCamPosition;

    //   public Vector3 NewPlayerPosition;

    public PlayerCameraController cam;


    public float CamRotiation;


    private void Start()
    {
        //  print("Wrap andgle : " + WrapAngle(this.transform.rotation.y));

        CamRotiation = this.transform.rotation.eulerAngles.y;
        //print(" CaM ROT : " + CamRotiation);



    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            if (other.gameObject.GetComponent<NetworkedPlayerController>()._camControll != null)
            {

                print("Entered into gameobject");
                cam = other.gameObject.GetComponent<NetworkedPlayerController>()._camControll;




                //set players Position = collider's rotation 
                //other.transform.rotation = Quaternion.Euler(other.transform.rotation.eulerAngles.x, CamRotiation, other.transform.rotation.eulerAngles.z);
                other.transform.GetComponent<NetworkedPlayerController>().toRot = this.transform.rotation.eulerAngles;



                if (/* setCamTriggerFrontFacing  &&  cam != null  && */ CamTriggered == false)
                {
                    if (setCamTriggerOverShoulder == true && cam != null)
                    {
                        print("Cam is now over shoulder");
                        cam.SetCamOverShoulder(CamRotiation);
                        CamTriggered = true;
                    }




                    if (setCamTriggerFrontFacing == true && cam != null)
                    {
                        print("Cam is now frontfacing");

                        //set camera rotation
                        cam.SetCamFrontFacing(CamRotiation);
                        CamTriggered = true;
                    }


                }

                if (cam == null)
                {
                    // print("Cam is null");
                    return;
                }

                if (CamTriggered == true)
                {
                    //  print("Trigger is null");
                    return;
                }
            }
        }
    }


    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

    private static float UnwrapAngle(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }

}
