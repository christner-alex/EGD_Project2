using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SituationManager : MonoBehaviour {
    public List<GameObject> situations;
    public List<string> messages;
    int currentlyDisplayed = 1; // 0 means situation, 1 means message
    public GameObject currentSituation;
    public int currentSituationIndex;

    public Text messageObject;
    public string currentMessage;
    public int currentMessageIndex;
     public float transitionDuration;
    public AnimationCurve transitionCurveOn;
    public AnimationCurve transitionCurveOff;
    public AnimationCurve cameraTransition;
    public Color[] floorMessageColors;
    public Color floorSituationColor;
    public GameObject floor;
    public Color textColor;
    public List<string> listOfWords;
    public List<Transform> placesCameraGoForBlocks;
    public List<Transform> placesCameraGoForBouncyBall;
    public List<Transform> placesCameraGoForSeeSaw;

    public List<GameObject> blockSituations;
    public List<GameObject> bouncyBallSituations;
    public List<GameObject> seeSawSituations;

    List<List<GameObject>> allSituations;
    List<List<Transform>> allCameras;
    // Use this for initialization
    void Start () {
		listOfWords = new List<string>();
        allSituations = new List<List<GameObject>>();
        allCameras = new List<List<Transform>>();
        allSituations.Add(blockSituations);
        allSituations.Add(bouncyBallSituations);
        allSituations.Add(seeSawSituations);

        allCameras.Add(placesCameraGoForBlocks);
        allCameras.Add(placesCameraGoForBouncyBall);
        allCameras.Add(placesCameraGoForSeeSaw);

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void TriggerNext() //if there's a change on stage
    {
        if (currentlyDisplayed == 0) //transition situation off and message on
        {
            StartCoroutine("TransitionSituationOff", currentSituation);
            currentMessageIndex++;
            currentMessageIndex = currentMessageIndex % messages.Count;
            StopCoroutine("TransitionTextOn");
            StartCoroutine("TransitionTextOn", messages[currentMessageIndex]);
            currentMessage = messages[currentMessageIndex];
        }
        if (currentlyDisplayed==1) //transition situation on and message off
        {
            StartCoroutine("TransitionTextOff");
            currentSituationIndex++;
            currentSituationIndex = currentSituationIndex % situations.Count;

            //list of a list, selects random element (which is a list) in larger list, then random element in the selected list
            int randomSituation = Random.Range(0, allSituations.Count);

            GameObject selectedSituation = allSituations[randomSituation][Random.Range(0, allSituations[randomSituation].Count)];
            StartCoroutine("TransitionSituationOn", selectedSituation);
            StartCoroutine("TransitionCamera", randomSituation);

        }
        currentlyDisplayed++;
        currentlyDisplayed = currentlyDisplayed % 2;

    }
    IEnumerator TransitionCamera(int index) //move camera to correct position based on situation
    {
        float startTime = Time.time;
        Vector3 startPosition= Camera.main.transform.transform.position;
        Quaternion startRot= Camera.main.transform.transform.rotation;
        while(Time.time-startTime<=1)
        {
            Camera.main.transform.transform.position = Vector3.Lerp(startPosition, allCameras[index][Random.Range(0, allCameras[index].Count)].position, cameraTransition.Evaluate((Time.time - startTime) /1));
            Camera.main.transform.transform.rotation = Quaternion.Lerp(startRot, allCameras[index][Random.Range(0, allCameras[index].Count)].rotation, cameraTransition.Evaluate((Time.time - startTime) / 1));
            yield return new WaitForEndOfFrame();

        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator TransitionTextOn(string t) //transition messagae text on
    {
        messageObject.text = "";
        listOfWords.Clear();
        messageObject.color = new Color(textColor.r, textColor.g, textColor.b, 1);

        messageObject.text = "";
        StartCoroutine("TransitionFloorColorToMessage");


        Color randomColor = floorMessageColors[Random.Range(0, floorMessageColors.Length)];
        for (int i = 0; i < messages[currentMessageIndex].Split(' ').Length; i++) // construct a list 
        {
            listOfWords.Add("<color=#" + ColorUtility.ToHtmlStringRGB(textColor)+ "01" +">" + messages[currentMessageIndex].Split(' ')[i] + "</color>");

        }

        // call coroutine to continually match text to list
        StartCoroutine("UpdateTextToList");

        for (int i =0;i<messages[currentMessageIndex].Split(' ').Length;i++) // go through each and trigger a change
        {
            StartCoroutine("ChangeTransparencyOfWordOnList", i);
            if(messages[currentMessageIndex].Split(' ')[i].Contains(",")) 
            {
                yield return new WaitForSeconds(1f); //message speed with comma
            }
            else
            {
                yield return new WaitForSeconds(.25f); //message speed normally
            }
        }

        StopCoroutine("UpdateTextToList");

        messageObject.text =  messages[currentMessageIndex]+" ";

        yield return new WaitForEndOfFrame();
    }

    IEnumerator UpdateTextToList() //updates the message's transparency for transitionining it on
    {
        while(true)
        {
            messageObject.text = "";
            for (int i = 0; i < listOfWords.Count; i++)
            {
                messageObject.text += listOfWords[i]+" ";

            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();

    }

    IEnumerator ChangeTransparencyOfWordOnList(int i) //changes the transparency of an individual word in the list
    {
        string hexColor = 0.ToString("00");
        string startTag = "<color=#" + ColorUtility.ToHtmlStringRGB(textColor);

        float startTime = Time.time;
        while(Time.time-startTime<.5f)
        {
            hexColor = ((int)(((Time.time - startTime) / .5f) * 255f)).ToString("X");
            if (hexColor.Length == 1)
            {
                hexColor = "0" + hexColor;
            }
            listOfWords[i] = startTag + hexColor + ">" + messages[currentMessageIndex].Split(' ')[i] + "</color>";
            yield return new WaitForEndOfFrame();

        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator TransitionTextOff() //transitions all the text off at once
    {
        StartCoroutine("TransitionFloorColorToSituation");

        float startTime = Time.time;
        Color startColor = floor.GetComponent<Renderer>().material.color;
        while (messageObject.color.a > 0)
        {
            messageObject.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0),transitionCurveOff.Evaluate((Time.time - startTime) / transitionDuration));

            floor.GetComponent<Renderer>().material.color = Color.Lerp(startColor, floorSituationColor, transitionCurveOff.Evaluate((Time.time - startTime) / transitionDuration));
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
    IEnumerator TransitionFloorColorToMessage() //changes the color of the floor for messages
    {
        float startTime = Time.time;
        Color startColor = floor.GetComponent<Renderer>().material.color;
        Color targetColor = floorMessageColors[Random.Range(1, floorMessageColors.Length)];
        while (messageObject.color.a > 0)
        {
            floor.GetComponent<Renderer>().material.color = Color.Lerp(startColor, targetColor, transitionCurveOff.Evaluate((Time.time - startTime) / transitionDuration));
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
    IEnumerator TransitionFloorColorToSituation() //changes the color of the floor for situations
    {
        float startTime = Time.time;
        Color startColor = floor.GetComponent<Renderer>().material.color;
        while (messageObject.color.a > 0)
        {
            floor.GetComponent<Renderer>().material.color = Color.Lerp(startColor, floorSituationColor, transitionCurveOff.Evaluate((Time.time - startTime) / transitionDuration));
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
    IEnumerator TransitionSituationOn(GameObject g) //physically moves the situation into the right spot
    {
        GameObject tmp =  (Instantiate(g, g.transform.position + new Vector3(50,0,0), Quaternion.identity)) as GameObject;
        currentSituation = tmp;

        float startTime = Time.time;
        while (Time.time-startTime<1.5f)
        {
            tmp.transform.position = Vector3.Lerp(g.transform.position + new Vector3(50, 0, 0), g.transform.position,transitionCurveOn.Evaluate((Time.time - startTime) / (transitionDuration*1.5f)));
            if(transitionCurveOn.Evaluate((Time.time - startTime) / transitionDuration) > .8f)
            {


                Physics.gravity = new Vector3(0, -22, 0);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
    IEnumerator TransitionSituationOff(GameObject g) //physically moves the situation off stage
    {
        Physics.gravity=new Vector3(0,0,0);
        float startTime = Time.time;
        Vector3 startPosition = g.transform.position;
        while (g.transform.position.x > startPosition.x - 50&&Time.time-startTime<4)
        {
            g.transform.position = Vector3.Lerp(startPosition, new Vector3(startPosition.x - 50, startPosition.y, startPosition.z),transitionCurveOff.Evaluate((Time.time - startTime) / (transitionDuration * 1.5f)));
            yield return new WaitForEndOfFrame();
        }
        Destroy(g);
        yield return new WaitForEndOfFrame();
    }
}
