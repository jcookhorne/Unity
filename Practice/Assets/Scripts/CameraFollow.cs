using Unity.Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public CinemachineCamera cam;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CinemachineCore.CameraActivatedEvent.AddListener(OnCameraActivated);


    }

    void OnCameraActivated(ICinemachineCamera.ActivationEventParams evt)
    {
        if (evt.IncomingCamera == (ICinemachineCamera)cam)
        {
            Debug.Log("HELLO WORLD");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }




}
