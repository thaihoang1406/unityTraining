using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CoroutineController : MonoBehaviour
{
    public InputField inputTask1;
    public InputField inputTask2;
    public Text textTask1;
    public Text textTask2;
    public Button buttonParallel;
    public Button buttonRace;
    public Text result;

    bool isDoneTask1=false;
    bool isDoneTask2=false;
    int task1Input;
    int task2Input;

    bool isParallel = true;

    void Start()
    {
        Button btn = buttonParallel.GetComponent<Button>();
        btn.onClick.AddListener(processParallel);
        btn = buttonRace.GetComponent<Button>();
        btn.onClick.AddListener(processRace);
    }

    void processParallel()
    {
        task1Input = int.Parse(inputTask1.text);
        task2Input = int.Parse(inputTask2.text);
        isParallel = true;
        StartCoroutine(processTask1());
        StartCoroutine(processTask2());
    }

    void processRace()
    {
        task1Input = int.Parse(inputTask1.text);
        task2Input = int.Parse(inputTask2.text);
        isParallel = false;
        StartCoroutine(processTask1());
        StartCoroutine(processTask2());
    }

    IEnumerator processTask1()
    {
        for (int i = 1; i <= task1Input; i++)
        {
            textTask1.text = i.ToString() + "/" + task1Input.ToString();
            yield return new WaitForSeconds(0.2f);
        }
        isDoneTask1 = true;
        if (!isParallel)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator processTask2()
    {
        for (int i = 1; i <= task1Input; i++)
        {
            textTask2.text = i.ToString() + "/" + task2Input.ToString();
            yield return new WaitForSeconds(0.2f);
        }
        isDoneTask2 = true;
        if (!isParallel)
        {
            StopAllCoroutines();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isParallel)
        {
            if (isDoneTask1 && isDoneTask2)
                result.text = "Done";
            else
                result.text = "Working...";
        }
        else
        {
            if (isDoneTask1 || isDoneTask2)
                result.text = "Done";
            else
                result.text = "Working...";
        }
    }
}
