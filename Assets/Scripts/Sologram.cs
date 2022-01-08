using UnityEngine;
using UnityEngine.UI;

public class Sologram : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private RawImage rawImage;

    public Vector3 CamPosition => mainCam.transform.position - transform.position;
    public Quaternion CamRotation => mainCam.transform.rotation;

    public Texture Image {
        get => rawImage.texture;
        set => rawImage.texture = value;
    }
    
    void Start() {
        mainCam = Camera.main;
    }

    void Update()
    {
        transform.LookAt(mainCam.transform);
    }
}
