using UnityEngine;
public class tileScript : MonoBehaviour
{
    public Vector3 targetPos;
    public Vector3 correctPos;
    public int number;
    public bool inrightplace = false;
    // Start is called before the first frame update
    void Awake()
    {
        targetPos = transform.position;
        correctPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.05f);
        if (targetPos == correctPos)
        {
            inrightplace = true;
        }
        else inrightplace = false;
    }
}
