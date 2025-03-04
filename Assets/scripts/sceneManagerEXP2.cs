using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class sceneManagerEXP2 : MonoBehaviour
{
    public string participantCode;
    public enum langue
    { fr, de, en }
    public langue selectLangue;
    public enum gender
    { Female, Male}
    public gender selectGender;
    public int numberOfTrials;
    public string outputFolder;
    public GameObject AvatarF;
    public GameObject AvatarM;
    public GameObject FemaleBody;
    public GameObject MaleBody;    
    public GameObject CameraTask;
    public GameObject CameraQuestionnaire;

    public int managerState = 1; //0 = initiate Trial; 1 = walking task; 2 = go to questionnaire; 3 = questionnaire state; 4 = Save data; 5 = wait for the return to position; 7-8 = end of study
    public string response1;
    public string response2;


    private Vector3[,] hips = new Vector3[320, 2]; //dans la colonne 1 ce sera les translation et en colonne 2 les rotations
    private Vector3[,] spine = new Vector3[320, 2];
    private Vector3[,] chest = new Vector3[320, 2];
    private Vector3[,] neck = new Vector3[320, 2];
    private Vector3[,] head = new Vector3[320, 2];
    private Vector3[,] shoulderL = new Vector3[320, 2];
    private Vector3[,] upperarmL = new Vector3[320, 2];
    private Vector3[,] forerarmL = new Vector3[320, 2];
    private Vector3[,] handL = new Vector3[320, 2];
    private Vector3[,] shoulderR = new Vector3[320, 2];
    private Vector3[,] upperarmR = new Vector3[320, 2];
    private Vector3[,] forerarmR = new Vector3[320, 2];
    private Vector3[,] handR = new Vector3[320, 2];
    private Vector3[,] thighL = new Vector3[320, 2];
    private Vector3[,] shinL = new Vector3[320, 2];
    private Vector3[,] footL = new Vector3[320, 2];
    private Vector3[,] thighR = new Vector3[320, 2];
    private Vector3[,] shinR = new Vector3[320, 2];
    private Vector3[,] footR = new Vector3[320, 2];

    private GameObject Bhips;
    private GameObject Bspine;
    private GameObject Bchest;
    private GameObject Bneck;
    private GameObject Bhead;
    private GameObject BshoulderL;
    private GameObject BupperarmL;
    private GameObject BforerarmL;
    private GameObject BhandL;
    private GameObject BshoulderR;
    private GameObject BupperarmR;
    private GameObject BforerarmR;
    private GameObject BhandR;
    private GameObject BthighL;
    private GameObject BshinL;
    private GameObject BfootL;
    private GameObject BthighR;
    private GameObject BshinR;
    private GameObject BfootR;

    private GameObject Bhips2;
    private GameObject Bspine2;
    private GameObject Bchest2;
    private GameObject Bneck2;
    private GameObject Bhead2;
    private GameObject BshoulderL2;
    private GameObject BupperarmL2;
    private GameObject BforerarmL2;
    private GameObject BhandL2;
    private GameObject BshoulderR2;
    private GameObject BupperarmR2;
    private GameObject BforerarmR2;
    private GameObject BhandR2;
    private GameObject BthighL2;
    private GameObject BshinL2;
    private GameObject BfootL2;
    private GameObject BthighR2;
    private GameObject BshinR2;
    private GameObject BfootR2;


    private float targetDeviation=30;
    //private float positionX = 0;
    //private float positionZ = 0;
    private int[] vectorRandomization = new int[144];
    private float[] practiceTrials = {0f,5f,20f,0f,-5f,-20f,5f,20f,-20f,-5f};
    private int trialNumber = -1; // first matrix indice is 0
    private int recordIndice = -1; //indice save position
    private int playIndice = -1; //indice play position
    private string accuracy;

    private string filenamepath1;
    private string filenamepath2;
    private string filenamepath3;

    private long timer = 0;
    private Stopwatch stopwatch;
    private long elapsedMilliseconds;
    private bool isRecording = false;
    private string[] matrixX; //elle aura une dimention de 304 élément qui est le maximal autorisé (10s). Le trial au dessus devront être supprimé
    private string[] matrixZ;
    private int matrixIndice = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       if (selectGender.ToString()=="Female")
        {
            FemaleBody.SetActive(true);
            MaleBody.SetActive(false);
            AvatarF.SetActive(true);
            AvatarM.SetActive(false);
        }
       else
        {
            FemaleBody.SetActive(false);
            MaleBody.SetActive(true);
            AvatarF.SetActive(false);
            AvatarM.SetActive(true);
        }

        //assign the GameObjects
        Bhips = GameObject.Find("B-hips");
        Bspine = GameObject.Find("B-spine");
        Bchest = GameObject.Find("B-chest");
        Bneck = GameObject.Find("B-neck");
        Bhead = GameObject.Find("B-head");
        BshoulderL = GameObject.Find("B-shoulder.L");
        BupperarmL = GameObject.Find("B-upperArm.L");
        BforerarmL = GameObject.Find("B-forerarm.L");
        BhandL = GameObject.Find("B-hand.L");
        BshoulderR = GameObject.Find("B-shoulder.R");
        BupperarmR = GameObject.Find("B-upperArm.R");
        BforerarmR = GameObject.Find("B-forearm.R");
        BhandR = GameObject.Find("B-hand.R");
        BthighL = GameObject.Find("B-thigh.L");
        BshinL = GameObject.Find("B-shin.L");
        BfootL = GameObject.Find("B-foot.L");
        BthighR = GameObject.Find("B-thigh.R");
        BshinR = GameObject.Find("B-shin.R");
        BfootR = GameObject.Find("B-foot.R");

        Bhips2 = GameObject.Find("B-hips2");
        Bspine2 = GameObject.Find("B-spine2");
        Bchest2 = GameObject.Find("B-chest2");
        Bneck2 = GameObject.Find("B-neck2");
        Bhead2 = GameObject.Find("B-head2");
        BshoulderL2 = GameObject.Find("B-shoulder.L2");
        BupperarmL2 = GameObject.Find("B-upperArm.L2");
        BforerarmL2 = GameObject.Find("B-forerarm.L2");
        BhandL2 = GameObject.Find("B-hand.L2");
        BshoulderR2 = GameObject.Find("B-shoulder.R2");
        BupperarmR2 = GameObject.Find("B-upperArm.R2");
        BforerarmR2 = GameObject.Find("B-forearm.R2");
        BhandR2 = GameObject.Find("B-hand.R2");
        BthighL2 = GameObject.Find("B-thigh.L2");
        BshinL2 = GameObject.Find("B-shin.L2");
        BfootL2 = GameObject.Find("B-foot.L2");
        BthighR2 = GameObject.Find("B-thigh.R2");
        BshinR2 = GameObject.Find("B-shin.R2");
        BfootR2 = GameObject.Find("B-foot.R2");


        ////generate the experimental sucession
        //int[] originalValues = { 11, 11, 11, 11, 11, 11, 12, 12, 12, 12, 12, 12, 21, 21, 21, 21, 21, 21, 22, 22, 22, 22, 22, 22, 31, 31, 31, 31, 31, 31, 32, 32, 32, 32, 32, 32, 41, 41, 41, 41, 41, 41, 42, 42, 42, 42, 42, 42, 51, 51, 51, 51, 51, 51, 52, 52, 52, 52, 52, 52, 61, 61, 61, 61, 61, 61, 62, 62, 62, 62, 62, 62, 71, 71, 71, 71, 71, 71, 72, 72, 72, 72, 72, 72 }; //84 trials (12 for each conditions)
        //int[] randomizedValues = ShuffleArray(originalValues);
        //Array.Copy(randomizedValues, vectorRandomization, randomizedValues.Length);

        ////Preparation of the output files
        //DateTime currentDate = DateTime.Now; //get the time
        //string dateString = currentDate.ToString("yyyy-MM-dd"); //convert it into string
        //filenamepath1 = outputFolder + dateString + "_" + participantCode + "_FBA_MotionTrack_QuestionnaireData" + ".csv";
        //filenamepath2 = outputFolder + dateString + "_" + participantCode + "_FBA_MotionTrack_PosX" + ".csv";
        //filenamepath3 = outputFolder + dateString + "_" + participantCode + "_FBA_MotionTrack_PosZ" + ".csv";

        ////record the header
        //string[] Header1 = new string[] { "Pcode", "Trial", "Condition", "Accuracy", "Question1Response", "Question2Response" };
        //WriteLine(Header1, filenamepath1); //write the first header line of the output file
        //string[] Header2 = new string[] { "Pcode", "Trial", "PositionX every 33ms" };
        //WriteLine(Header2, filenamepath2);
        //string[] Header3 = new string[] { "Pcode", "Trial", "PositionZ every 33ms" };
        //WriteLine(Header3, filenamepath3);
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
            UnityEngine.Debug.Log("Trial number: " + trialNumber);

            matrixX = new string[304]; //304 is the length max (10s)
            matrixZ = new string[304];
            matrixIndice = 0;
            stopwatch = new Stopwatch();
            timer = 0;

        }
        else if (managerState == 1)
        {
            //save current position
            recordIndice++;
            hips[recordIndice, 0] = Bhips.transform.localPosition; hips[recordIndice, 1] = Bhips.transform.localRotation.eulerAngles;
            spine[recordIndice, 0] = Bspine.transform.localPosition; spine[recordIndice, 1] = Bspine.transform.localRotation.eulerAngles;
            chest[recordIndice, 0] = Bchest.transform.localPosition; chest[recordIndice, 1] = Bchest.transform.localRotation.eulerAngles;
            neck[recordIndice, 0] = Bneck.transform.localPosition; neck[recordIndice, 1] = Bneck.transform.localRotation.eulerAngles;
            head[recordIndice, 0] = Bhead.transform.localPosition; head[recordIndice, 1] = Bhead.transform.localRotation.eulerAngles;
            shoulderL[recordIndice, 0] = BshoulderL.transform.localPosition; shoulderL[recordIndice, 1] = BshoulderL.transform.localRotation.eulerAngles;
            upperarmL[recordIndice, 0] = BupperarmL.transform.localPosition; upperarmL[recordIndice, 1] = BupperarmL.transform.localRotation.eulerAngles;
            forerarmL[recordIndice, 0] = BforerarmL.transform.localPosition; forerarmL[recordIndice, 1] = BforerarmL.transform.localRotation.eulerAngles;
            handL[recordIndice, 0] = BhandL.transform.localPosition; handL[recordIndice, 1] = BhandL.transform.localRotation.eulerAngles;
            shoulderR[recordIndice, 0] = BshoulderR.transform.localPosition; shoulderR[recordIndice, 1] = BshoulderR.transform.localRotation.eulerAngles;
            upperarmR[recordIndice, 0] = BupperarmR.transform.localPosition; upperarmR[recordIndice, 1] = BupperarmR.transform.localRotation.eulerAngles;
            forerarmR[recordIndice, 0] = BforerarmR.transform.localPosition; forerarmR[recordIndice, 1] = BforerarmR.transform.localRotation.eulerAngles;
            handR[recordIndice, 0] = BhandR.transform.localPosition; handR[recordIndice, 1] = BhandR.transform.localRotation.eulerAngles;
            thighL[recordIndice, 0] = BthighL.transform.localPosition; thighL[recordIndice, 1] = BthighL.transform.localRotation.eulerAngles;
            shinL[recordIndice, 0] = BshinL.transform.localPosition; shinL[recordIndice, 1] = BshinL.transform.localRotation.eulerAngles;
            footL[recordIndice, 0] = BfootL.transform.localPosition; footL[recordIndice, 1] = BfootL.transform.localRotation.eulerAngles;
            thighR[recordIndice, 0] = BthighR.transform.localPosition; thighR[recordIndice, 1] = BthighR.transform.localRotation.eulerAngles;
            shinR[recordIndice, 0] = BshinR.transform.localPosition; shinR[recordIndice, 1] = BshinR.transform.localRotation.eulerAngles;
            footR[recordIndice, 0] = BfootR.transform.localPosition; footR[recordIndice, 1] = BfootR.transform.localRotation.eulerAngles;

            //playback
            playIndice++;
            Bhips2.transform.localPosition = hips[playIndice, 0]; Bhips2.transform.localRotation = Quaternion.Euler(hips[playIndice, 1]);
            Bspine2.transform.localPosition = spine[playIndice, 0]; Bspine2.transform.localRotation = Quaternion.Euler(spine[playIndice, 1]);
            Bchest2.transform.localPosition = chest[playIndice, 0]; Bchest2.transform.localRotation = Quaternion.Euler(chest[playIndice, 1]);
            Bneck2.transform.localPosition = neck[playIndice, 0]; Bneck2.transform.localRotation = Quaternion.Euler(neck[playIndice, 1]);
            Bhead2.transform.localPosition = head[playIndice, 0]; Bhead2.transform.localRotation = Quaternion.Euler(head[playIndice, 1]);
            BshoulderL2.transform.localPosition = shoulderL[playIndice, 0]; BshoulderL2.transform.localRotation = Quaternion.Euler(shoulderL[playIndice, 1]);
            BupperarmL2.transform.localPosition = upperarmL[playIndice, 0]; BupperarmL2.transform.localRotation = Quaternion.Euler(upperarmL[playIndice, 1]);
            BforerarmL2.transform.localPosition = forerarmL[playIndice, 0]; BforerarmL2.transform.localRotation = Quaternion.Euler(forerarmL[playIndice, 1]);
            BhandL2.transform.localPosition = handL[playIndice, 0]; BhandL2.transform.localRotation = Quaternion.Euler(handL[playIndice, 1]);
            BshoulderR2.transform.localPosition = shoulderR[playIndice, 0]; BshoulderR2.transform.localRotation = Quaternion.Euler(shoulderR[playIndice, 1]);
            BupperarmR2.transform.localPosition = upperarmR[playIndice, 0]; BupperarmR2.transform.localRotation = Quaternion.Euler(upperarmR[playIndice, 1]);
            BforerarmR2.transform.localPosition = forerarmR[playIndice, 0]; BforerarmR2.transform.localRotation = Quaternion.Euler(forerarmR[playIndice, 1]);
            BhandR2.transform.localPosition = handR[playIndice, 0]; BhandR2.transform.localRotation = Quaternion.Euler(handR[playIndice, 1]);
            BthighL2.transform.localPosition = thighL[playIndice, 0]; BthighL2.transform.localRotation = Quaternion.Euler(thighL[playIndice, 1]);
            BshinL2.transform.localPosition = shinL[playIndice, 0]; BshinL2.transform.localRotation = Quaternion.Euler(shinL[playIndice, 1]);
            BfootL2.transform.localPosition = footL[playIndice, 0]; BfootL2.transform.localRotation = Quaternion.Euler(footL[playIndice, 1]);
            BthighR2.transform.localPosition = thighR[playIndice, 0]; BthighR2.transform.localRotation = Quaternion.Euler(thighR[playIndice, 1]);
            BshinR2.transform.localPosition = shinR[playIndice, 0]; BshinR2.transform.localRotation = Quaternion.Euler(shinR[playIndice, 1]);
            BfootR2.transform.localPosition = footR[playIndice, 0]; BfootR2.transform.localRotation = Quaternion.Euler(footR[playIndice, 1]);


            ////record X and Z position
            //elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            //if (isRecording & (elapsedMilliseconds- timer) >= 33) //save a data point every 33ms (30Hz)
            //{
            //    matrixIndice++;
            //    timer = elapsedMilliseconds;


            //}



        }
        else if (managerState == 2) //go to questionnaire
        {
            isRecording = false; //stop the recording of position
            stopwatch.Stop();

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
            if (trialNumber>=10) //save only during the real phase and not the training
            {
                SaveDataQuestionnaire();
                SaveDataPosition(matrixX, filenamepath2); // save the position X
                SaveDataPosition(matrixZ, filenamepath3); // save the position Z
            }

            //check if end of the experiment
            if (trialNumber - 11 >= numberOfTrials)
            {
                managerState = 7;
            }

            managerState++;
        }
        else if (managerState == 5) //wait participant return to initiale position
        {
            

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
        if (trialNumber < 10) //10 Practice trials
        {
            output = practiceTrials[trialNumber];
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
              

        return output;

    }

    //function to add a line to the csvfile
    private void WriteLine(string[] values, string filePath)
    {
        // Convertir les valeurs en une ligne CSV formatée
        string line = string.Join(",", values);

        // Ajouter la ligne au fichier
        using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
        {
            writer.WriteLine(line);
        }
    }

    private void SaveDataQuestionnaire()
    {
        int temp = trialNumber - 9;

        string[] data = new string[]
        {
            participantCode,
            temp.ToString(),
            vectorRandomization[trialNumber - 10].ToString(),
            accuracy,
            response1,
            response2            
        };

        WriteLine(data, filenamepath1);
    }

    private void SaveDataPosition(string[] positions, string filePath)
    {
        int temp = trialNumber - 9;

        string[] data = new string[] { participantCode, temp.ToString()}.Concat(positions).ToArray(); //pour concatenate le vecteur de position
        
        WriteLine(data, filePath);
    }
}
