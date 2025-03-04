using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class sceneManager : MonoBehaviour
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
    public GameObject FemaleBody;
    public GameObject MaleBody;
    public GameObject TargetCylinder;
    public GameObject CameraTask;
    public GameObject CameraQuestionnaire;

    public int managerState = 0; //0 = initiate Trial; 1 = walking task; 2 = go to questionnaire; 3 = questionnaire state; 4 = Save data; 5 = wait for the return to position; 7-8 = end of study
    public string response1;
    public string response2;


    private GameObject avatar;
    private GameObject hips;
    private collisionManager collisionManager;
    private float targetDeviation=30;
    private float positionX = 0;
    private float positionZ = 0;
    private int[] vectorRandomization = new int[84];
    private float[] practiceTrials = {0f,5f,20f,0f,-5f,-20f,5f,20f,-20f,-5f};
    private bool[] practiceTrialsCylinder = { false, false, false, true, true, true, true, true, false, false };
    private int trialNumber = -1; // first matrix indice is 0
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
        int[] originalValues = {11,11,11,11,11,11,12,12,12,12,12,12,21,21,21,21,21,21,22,22,22,22,22,22,31,31,31,31,31,31,32,32,32,32,32,32,41,41,41,41,41,41,42,42,42,42,42,42,51,51,51,51,51,51,52,52,52,52,52,52,61,61,61,61,61,61,62,62,62,62,62,62,71,71,71,71,71,71,72,72,72,72,72,72}; //84 trials (12 for each conditions)
        int[] randomizedValues = ShuffleArray(originalValues);
        Array.Copy(randomizedValues, vectorRandomization, randomizedValues.Length);

        //Preparation of the output files
        DateTime currentDate = DateTime.Now; //get the time
        string dateString = currentDate.ToString("yyyy-MM-dd"); //convert it into string
        filenamepath1 = outputFolder + dateString + "_" + participantCode +"_FBA_MotionTrack_QuestionnaireData" +".csv";
        filenamepath2 = outputFolder + dateString + "_" + participantCode + "_FBA_MotionTrack_PosX" + ".csv";
        filenamepath3 = outputFolder + dateString + "_" + participantCode + "_FBA_MotionTrack_PosZ" + ".csv";

        //record the header
        string[] Header1 = new string[] { "Pcode", "Trial", "Condition", "Accuracy", "Question1Response", "Question2Response"};
        WriteLine(Header1, filenamepath1); //write the first header line of the output file
        string[] Header2 = new string[] { "Pcode", "Trial", "PositionX every 33ms"};
        WriteLine(Header2, filenamepath2);
        string[] Header3 = new string[] { "Pcode", "Trial", "PositionZ every 33ms" };
        WriteLine(Header3, filenamepath3);
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
            // management of the rotation
            positionX = hips.transform.position.x;
            positionZ = hips.transform.position.z;
            if (positionZ <= 0.3378f && positionZ >= 0f) //gestion des 30 premier centimètre avec l'accroissement de la rotation
            {
                avatar.transform.eulerAngles = new Vector3(0, (positionZ / 0.3378f) * targetDeviation, 0); //augumentation progressive de la rotation
            }
            else if (positionZ > 0.3378f)
            {
                avatar.transform.eulerAngles = new Vector3(0, targetDeviation, 0); //fixé la rotation à la target voulue
                if (!isRecording)
                {
                    stopwatch.Start(); //start of timer for position recording
                    isRecording = true;
                    matrixX[matrixIndice] = positionX.ToString("F3"); //record position of origin
                    matrixZ[matrixIndice] = positionZ.ToString("F3");
                }
            }
            else if (positionZ < 0f)
            {
                avatar.transform.eulerAngles = new Vector3(0, 0, 0); //initialisation du trial
            }

            //record X and Z position
            elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            if (isRecording & (elapsedMilliseconds - timer) >= 33) //save a data point every 33ms (30Hz)
            {
                matrixIndice++;
                timer = elapsedMilliseconds;
                if (matrixIndice < 304)
                {
                    matrixX[matrixIndice] = positionX.ToString("F3");
                    matrixZ[matrixIndice] = positionZ.ToString("F3");
                }

            }

            // management of the target hit            
            if (collisionManager.collisionCylinder)
            {
                accuracy = "1";
                managerState++;
                UnityEngine.Debug.Log("Accuracy: " + accuracy);
            }
            else if (collisionManager.collisionEnd)
            {
                accuracy = "0";
                managerState++;
                UnityEngine.Debug.Log("Accuracy: " + accuracy);
            }

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
            if (trialNumber >= 10) //save only during the real phase and not the training
            {
                SaveDataQuestionnaire();
                SaveDataPosition(matrixX, filenamepath2); // save the position X
                SaveDataPosition(matrixZ, filenamepath3); // save the position Z
            }

            //check if end of the experiment
            if (trialNumber - 10 >= numberOfTrials - 1)
            {
                managerState = 7;
            }

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
        else if (managerState == 8)
        { }//end of the study
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
