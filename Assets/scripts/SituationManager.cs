using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ListWrapper
{
    public List<GameObject> innerList;
}



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
    public List<ListWrapper> allSituations;

    //List<List<GameObject>> allSituations;
    public List<ListWrapper> allCameras;
    public Transform startingCameraPosition;
    bool transitioning;
    public float[] likelyhoodOfShowingText;
    int messageLikelyhoodIndex;
    public List<int> situationOrder;
    int situationOrderCurrentIndex;
    public Color[] possibleWarmColors;
    public Color[] possibleCoolColors;
    public Light leftLight;
    public Light rightLight;
    public float minLightIntensity;
    public float maxLightIntensity;
    // Use this for initialization
    void Start () {
        Camera.main.transform.position = startingCameraPosition.position;
        Camera.main.transform.rotation = startingCameraPosition.rotation;
		listOfWords = new List<string>();
        situationOrder = new List<int>();

        for (int i=0;i< allSituations.Count; i++)
        {
            int value = Random.Range(0, allSituations.Count);
            while (situationOrder.Contains(value))
            {
                value = Random.Range(0, allSituations.Count);
            }


            situationOrder.Add(value);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void TriggerNext() //if there's a change on stage
    {
        if(!transitioning)
        {
            if (currentlyDisplayed == 0) // currently showing situation (and needs to change)
            {
                StartCoroutine("TransitionSituationOff", currentSituation); //always transition the situation off
                bool shouldShowMessage; //determine if message should show

                if (Random.Range(0f,1f)<= likelyhoodOfShowingText[messageLikelyhoodIndex])
                {
                    shouldShowMessage = true;
                    messageLikelyhoodIndex = 0;
                }
                else
                {
                    messageLikelyhoodIndex = (messageLikelyhoodIndex + 1) % likelyhoodOfShowingText.Length;
                    shouldShowMessage = false;
                }

                if (shouldShowMessage) //if a message should show, transition it on
                {
                    currentMessageIndex++;
                    currentMessageIndex = currentMessageIndex % messages.Count;
                    StopCoroutine("TransitionTextOn");
                    StartCoroutine("TransitionTextOn", messages[currentMessageIndex]);
                    currentMessage = messages[currentMessageIndex];
                    currentlyDisplayed = 1;
                }
                else
                {

                    int randomSituation = situationOrder[situationOrderCurrentIndex];

                    GameObject selectedSituation = allSituations[randomSituation].innerList[Random.Range(0, allSituations[randomSituation].innerList.Count)];
                    StartCoroutine("TransitionSituationOn", selectedSituation);
                    StartCoroutine("TransitionCamera", randomSituation);
                    StartCoroutine("TransitionLightColor");
                    currentlyDisplayed = 0;

                }
            }
            else if (currentlyDisplayed == 1) //currently showing message (and needs to change)
            {
                StartCoroutine("TransitionTextOff");
                currentSituationIndex++;
                currentSituationIndex = currentSituationIndex % situations.Count;

                //list of a list, selects random element (which is a list) in larger list, then random element in the selected list
                int randomSituation = situationOrder[situationOrderCurrentIndex];

                GameObject selectedSituation = allSituations[randomSituation].innerList[Random.Range(0, allSituations[randomSituation].innerList.Count)];
                StartCoroutine("TransitionSituationOn", selectedSituation);
                StartCoroutine("TransitionCamera", randomSituation);
                StartCoroutine("TransitionLightColor");
                currentlyDisplayed = 0;
            }
  
        }
       

    }
    IEnumerator TransitionLightColor()
    {
        bool leftCoolSide=false;
        if(Random.Range(0,2)==0)
        {
            leftCoolSide = true;
        }
        float startTime = Time.time;
        Color startLeftColor = leftLight.color;
        Color startRightColor = rightLight.color;
        Color targetLeftColor;
        Color targetRightColor;
        float rightLightIntensity = Random.Range(minLightIntensity, maxLightIntensity);
        float leftLightIntensity = Random.Range(minLightIntensity, maxLightIntensity);
        float leftStartIntensity = leftLight.intensity;
        float rightStartIntensity = rightLight.intensity;
        if (leftCoolSide)
        {
            targetLeftColor = possibleCoolColors[Random.Range(0, possibleCoolColors.Length)];
            targetRightColor = possibleWarmColors[Random.Range(0, possibleWarmColors.Length)];
        }
        else
        {
            targetRightColor = possibleCoolColors[Random.Range(0, possibleCoolColors.Length)];
            targetLeftColor = possibleWarmColors[Random.Range(0, possibleWarmColors.Length)];
        }
        while (Time.time-startTime<1f)
        {
            leftLight.color = Color.Lerp(startLeftColor, targetLeftColor, cameraTransition.Evaluate((Time.time - startTime) / 1));
            rightLight.color = Color.Lerp(startRightColor, targetRightColor, cameraTransition.Evaluate((Time.time - startTime) / 1));
            leftLight.intensity = Mathf.Lerp(leftStartIntensity, leftLightIntensity, cameraTransition.Evaluate((Time.time - startTime) / 1));
            rightLight.intensity = Mathf.Lerp(rightStartIntensity, rightLightIntensity, cameraTransition.Evaluate((Time.time - startTime) / 1));

            yield return new WaitForEndOfFrame();

        }
        yield return new WaitForEndOfFrame();
    }
    IEnumerator TransitionCamera(int index) //move camera to correct position based on situation
    {
        float startTime = Time.time;
        Vector3 startPosition= Camera.main.transform.transform.position;
        Quaternion startRot= Camera.main.transform.transform.rotation;
        int randomCameraWithinSituation = Random.Range(0, allCameras[index].innerList.Count);
        while (Time.time-startTime<=1)
        {

            Camera.main.transform.transform.position = Vector3.Lerp(startPosition, allCameras[index].innerList[randomCameraWithinSituation].transform.position, cameraTransition.Evaluate((Time.time - startTime) /1));
            Camera.main.transform.transform.rotation = Quaternion.Lerp(startRot, allCameras[index].innerList[randomCameraWithinSituation].transform.rotation, cameraTransition.Evaluate((Time.time - startTime) / 1));
            yield return new WaitForEndOfFrame();

        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator TransitionTextOn(string t) //transition messagae text on
    {
        transitioning = true;
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
        transitioning = false;
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

        transitioning = true;
        yield return new WaitForSeconds(.75f);
        float randomValue = Random.Range(0, 1f);
        if (randomValue >= .9)
        {
            Physics.gravity = Vector3.zero;
        }
        else if(randomValue>=.7f)
        {
            Physics.gravity = new Vector3(0,-4, 0);
        }
        else
        {
            Physics.gravity = new Vector3(0, -22, 0);
        }
        if(situationOrder[situationOrderCurrentIndex]==5) //5 is balloon, so override to make it zero g
        {
            Physics.gravity = new Vector3(0, -1, 0);
        }
        if (situationOrder[situationOrderCurrentIndex] == 3) //with toothpicks, don't go zero g
        {
            Physics.gravity = new Vector3(0, -22, 0);
        }
        if (situationOrder[situationOrderCurrentIndex] == 6) //with trash, don't go zero g
        {
            Physics.gravity = new Vector3(0, -22, 0);
        }
        GameObject tmp =  (Instantiate(g, g.transform.position + new Vector3(50,0,0), Quaternion.identity)) as GameObject;
        currentSituation = tmp;

        float startTime = Time.time;
        while (Time.time-startTime<1.5f)
        {
            tmp.transform.position = Vector3.Lerp(g.transform.position + new Vector3(50, 0, 0), g.transform.position,transitionCurveOn.Evaluate((Time.time - startTime) / (transitionDuration*1.5f)));
            if(transitionCurveOn.Evaluate((Time.time - startTime) / transitionDuration) > .9f)
            {


                Physics.autoSimulation = true;
            }
            yield return new WaitForEndOfFrame();
        }

        situationOrderCurrentIndex = (situationOrderCurrentIndex + 1) % situationOrder.Count;
        transitioning = false;
        yield return new WaitForEndOfFrame();
    }
    IEnumerator TransitionSituationOff(GameObject g) //physically moves the situation off stage
    {
        Physics.autoSimulation = false;
        float startTime = Time.time;
        Vector3 startPosition = g.transform.position;
        while (g.transform.position.x > startPosition.x - 50&&Time.time-startTime<1.5f)
        {
            g.transform.position = Vector3.Lerp(startPosition, new Vector3(startPosition.x - 200, startPosition.y, startPosition.z),transitionCurveOff.Evaluate((Time.time - startTime) / (1.5f * 1.5f)));
            yield return new WaitForEndOfFrame();
        }
        Destroy(g);
        yield return new WaitForEndOfFrame();
    }
}
