using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject PickUpFrefab;
    public GameObject PickUpDieParticle;
    public GameObject PlayerRespawnParticle;
    public GameObject PlayerDieParticle;
    public LayerMask layermask;
    public Text counttext;
    public Text wintext;

    private float currentTime;
    private int state;
    private int index;
    private List<GameObject> listPos;
    private Rigidbody rb;
    private int count;
    // Start is called before the first frame update
    void Start()
    {
        state = 0;
        rb = GetComponent<Rigidbody>();
        count = 0;
        index = 0;
        SetCountText();
        wintext.text = "";
        listPos = new List<GameObject>();
    }

    void initPickUp()
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
                    temp.GetComponentInChildren<Rotator>().isRed = true; ;
                    break;
                case 2:
                    temp.GetComponentInChildren<Animator>().SetTrigger("Blue");
                    break;

            }


            temp.SetActive(true);
            listPos.Add(temp);
        }
    }

    void Update()
    {
        initPickUp();

        if(state == 1)
        {
            currentTime = Time.time;
            state = 2;
        }

        if(state == 2)
        {
            if (Time.time - currentTime > 2)
            {
                gameObject.transform.position = new Vector3(0, 0, 0);
                GameObject temp = Instantiate(PlayerRespawnParticle, gameObject.transform.position, Quaternion.identity);
                temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y + 1, temp.transform.position.z);
                currentTime = Time.time;
                state = 3;
            }
        }

        if (state == 3)
        {
            if (Time.time - currentTime > 2)
            {
                state = 0;
            }
        }

        checkAndPlayIdle();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == 0)
        {
            if (listPos.Count > index)
                if (listPos[index].GetComponentInChildren<Rotator>().isdie == false)
                {
                    transform.position = Vector3.MoveTowards(transform.position, listPos[index].transform.position, 0.05f);
                    transform.LookAt(listPos[index].transform);
                    GetComponent<Animator>().SetTrigger("Walk");}
                else
                    index++;
        }

    }

    void checkAndPlayIdle()
    {
        bool idle = true;
        foreach(var obj in listPos)
        {
            if (obj.GetComponentInChildren<Rotator>().isdie == false)
            {
                idle = false;
                break;
            }
        }

        if (idle)
            GetComponent<Animator>().SetTrigger("Idle");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hoang");
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("Die");

            if(other.gameObject.GetComponent<Rotator>().isRed)
            {
                //die
                state = 1;
                GetComponent<Animator>().SetTrigger("Idle");
                Instantiate(PickUpDieParticle,other.gameObject.transform.position, Quaternion.identity);
                Instantiate(PlayerDieParticle, gameObject.transform.position, Quaternion.identity);
                

            }

            

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
