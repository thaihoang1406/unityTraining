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

            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    temp.GetComponentInChildren<Animator>().SetTrigger("Green");
                    break;
                case 1:
                    temp.GetComponentInChildren<Animator>().SetTrigger("Red");
                    break;
                case 2:
                    temp.GetComponentInChildren<Animator>().SetTrigger("Blue");
                    break;

            }
            

            temp.SetActive(true);
            listPos.Add(temp);
        }

        checkAndPlayIdle();

        if (listPos.Count > index)
            if (listPos[index].GetComponentInChildren<Rotator>().isdie == false)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, listPos[index].transform.position, 0.1f);
            }
            else
                index++;
        
    }

    void checkAndPlayIdle()
    {
        bool idle = true;
        foreach(var obj in listPos)
        {
            if (obj.active == true)
            {
                idle = false;
                break;
            }
        }

        if (idle)
        {
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().SetTrigger("Idle");
        }
        else
            GetComponent<Animator>().enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("Die");
            other.gameObject.GetComponent<Rotator>().isdie = true;
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
