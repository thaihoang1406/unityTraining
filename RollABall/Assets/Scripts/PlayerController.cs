using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject PickUpFrefab;
    public LayerMask layermask;
    public Text counttext;
    public Text wintext;

    private int index;
    private List<GameObject> listPos;
    private Rigidbody rb;
    private int count;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        index = 0;
        SetCountText();
        wintext.text = "";
        listPos = new List<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 100, layermask);
            GameObject temp = PickUpFrefab.Spawn(hit.point);
            temp.SetActive(true);
            listPos.Add(temp);
        }
        if (listPos.Count > index)
            if (listPos[index].active == true)
                transform.position = Vector3.MoveTowards(transform.position, listPos[index].transform.position, 0.1f);
            else
                index++;
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.transform.parent.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    void SetCountText()
    {
        counttext.text = "Count: " + count.ToString();
        if(count >= 10)
        {
            wintext.text = "You win";
        }
    }
}
