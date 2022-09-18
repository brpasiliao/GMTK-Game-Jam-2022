using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private CinemachineVirtualCamera RabbitCam;

    [SerializeField]
    private CinemachineVirtualCamera TurtleCam;
    
    [SerializeField]
    private CinemachineVirtualCamera OwlCam;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Rabbit>().enabled)
        {
            RabbitCam.Priority = 1;
            TurtleCam.Priority = 0;
            OwlCam.Priority = 0;
        }
        else if (player.GetComponent<Turtle>().enabled)
        {
            RabbitCam.Priority = 0;
            TurtleCam.Priority = 1;
            OwlCam.Priority = 0;
        }
        else if (player.GetComponent<Owl>().enabled)
        {
            RabbitCam.Priority = 0;
            TurtleCam.Priority = 0;
            OwlCam.Priority = 1;
        }
    }
}
