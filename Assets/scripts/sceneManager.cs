using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
//using System.Diagnostics;
//using System.Threading;

public class sceneManager : MonoBehaviour
{
    public enum langue
    { fr, de, en }
    public langue selectLangue;
    public enum gender
    { Female, Male}
    public gender selectGender;
    public GameObject FemaleBody;
    public GameObject MaleBody;
    public GameObject TargetCylinder;
    public GameObject CameraTask;
    public GameObject CameraQuestionnaire;

    public int managerState = 0; //0 = initiate Trial; 1 = walking task; 2 = questionnaire; 3 = Save data and wait for the return to position; 4 = feedback training; 5 = end of task 
    public string response1;
    public string response2;


    private GameObject avatar;
    private GameObject hips;
    private collisionManager collisionManager;
    private float targetDeviation=30;
    private float positionZ = 0;
    private int[] vectorRandomization = new int[84];
    private float[] practiceTrials = {0f,5f,20f,0f,-5f,-20f,5f,20f,-20f,-5f};
    private bool[] practiceTrialsCylinder = { false, false, false, true, true, true, true, true, false, false };
    private int trialNumber = -1; // first matrix indice is 0
    private string accuracy;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       if (selectGender.ToString()=="Female")
        {
            FemaleBody.SetActive(true);
            MaleBody.SetActive(false);
            avatar = FemaleBody;
        }
       else
        {
            FemaleBody.SetActive(false);
            MaleBody.SetActive(true);
            avatar = MaleBody;
        }
        hips = GameObject.Find("B-hips"); //to find the actual position 
        collisionManager = hips.GetComponent<collisionManager>(); // store collisionManager^script associate with hips

        //generate the experimental sucession
        int[] originalValues = {11,11,11,11,11,11,12,12,12,12,12,12,21,21,21,21,21,21,22,22,22,22,22,22,31,31,31,31,31,31,32,32,32,32,32,32,41,41,41,41,41,41,42,42,42,42,42,42,51,51,51,51,51,51,52,52,52,52,52,52,61,61,61,61,61,61,62,62,62,62,62,62,71,71,71,71,71,71,72,72,72,72,72,72};
        int[] randomizedValues = ShuffleArray(originalValues);
        Array.Copy(randomizedValues, vectorRandomization, randomizedValues.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (managerState == 0) //Initialisation of the trial
        {
            trialNumber++;
            targetDeviation = conditionSelector();

            CameraTask.SetActive(true);
            CameraQuestionnaire.SetActive(false);
            managerState++;

        }
        else if (managerState == 1)
        {
            // management of the rotation
            positionZ = hips.transform.position.z;
            if (positionZ <= 0.3378 && positionZ >= 0) //gestion des 30 premier centimètre avec l'accroissement de la rotation
            {
                avatar.transform.eulerAngles = new Vector3(0, (positionZ / 0.3378f) * targetDeviation, 0); //augumentation progressive de la rotation
            }
            else if (positionZ > 0.3378)
            {
                avatar.transform.eulerAngles = new Vector3(0, targetDeviation, 0); //fixé la rotation à la target voulue
            }
            else if (positionZ < 0)
            {
                avatar.transform.eulerAngles = new Vector3(0, 0, 0); //initialisation du trial
            }

            // management of the target hit            
            if (collisionManager.collisionCylinder)
            {
                accuracy = "1";
                managerState++;
            }
            else if (collisionManager.collisionEnd)
            {
                accuracy = "0";
                managerState++;
            }

        }
        else if (managerState == 2) //go to questionnaire
        {
            CameraTask.SetActive(false);
            CameraQuestionnaire.SetActive(true);
            managerState++;
        }
        else if (managerState == 3) //questionnaire state
        {
            //manage by the script questionnaireManager
        }
        else if (managerState == 4) //save data
        {
            //data saving
            Debug.Log(accuracy);
            managerState++;
        }
        else if (managerState == 5) //wait participant return to initiale position
        {
            //start next trial
            if (collisionManager.collisionStart)
            {
                managerState = 0;
            }

        }

    }


    private int[] ShuffleArray(int[] array)
    {
        List<int> list = new List<int>(array);
        System.Random random = new System.Random();

        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = random.Next(i, list.Count);

            // Échanger les éléments
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        return list.ToArray();
    }

    private float conditionSelector()
    {
        float output=0f;
        bool cylinderSide=false;
        if (trialNumber < 9) //10 Practice trials
        {
            output = practiceTrials[trialNumber];
            cylinderSide= practiceTrialsCylinder[trialNumber];
        }
        else
        {
            if (vectorRandomization[trialNumber-10] == 11) {output = 0f; cylinderSide=false;}
            else if (vectorRandomization[trialNumber - 10] == 12) {output = 0f; cylinderSide = true;}
            else if (vectorRandomization[trialNumber - 10] == 21) {output = 7.5f; cylinderSide = false;}
            else if (vectorRandomization[trialNumber - 10] == 22) {output = 7.5f; cylinderSide = true;}
            else if (vectorRandomization[trialNumber - 10] == 31) {output = -7.5f; cylinderSide = false;}
            else if (vectorRandomization[trialNumber - 10] == 32) {output = -7.5f; cylinderSide = true;}
            else if (vectorRandomization[trialNumber - 10] == 41) {output = 15f; cylinderSide = false;}
            else if (vectorRandomization[trialNumber - 10] == 42) {output = 15f; cylinderSide = true;}
            else if (vectorRandomization[trialNumber - 10] == 51) {output = -15f; cylinderSide = false;}
            else if (vectorRandomization[trialNumber - 10] == 52) {output = -15f; cylinderSide = true;}
            else if (vectorRandomization[trialNumber - 10] == 61) {output = 30f; cylinderSide = false;}
            else if (vectorRandomization[trialNumber - 10] == 62) {output = 30f; cylinderSide = true;}
            else if (vectorRandomization[trialNumber - 10] == 71) {output = -30f; cylinderSide = false;}
            else if (vectorRandomization[trialNumber - 10] == 72) {output = -30f; cylinderSide = true;}
        }

        if (cylinderSide) //cylindre à gauche (condition pair)
        {
            TargetCylinder.transform.position = new Vector3(-0.3574f, TargetCylinder.transform.position.y, TargetCylinder.transform.position.z);
        }
        else //cylindre à droite (condition impair)
        {
            TargetCylinder.transform.position = new Vector3(0.3574f, TargetCylinder.transform.position.y, TargetCylinder.transform.position.z);
        }

        return output;

    }
}
