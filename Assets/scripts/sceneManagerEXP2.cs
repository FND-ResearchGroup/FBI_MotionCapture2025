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
    { Female, Male }
    public gender selectGender;
    public string SkeletonAssetName;    
    public string outputFolder;
    public int numberOfTrials;
    public GameObject AvatarF;
    public GameObject AvatarM;
    public GameObject FemaleBody;
    public GameObject MaleBody;
    public GameObject CameraTask;
    public GameObject CameraQuestionnaire;
    public GameObject CanavasCross;

    public int managerState = 0; //0 = initiate Trial; 1 = wait for participant start; 2 = Red cross of 2s (and start of body recording depend on the delay); 3 = Playback of body for 3s; 4 = questionnaire state; 5 = Save data; 7 = end of study
    public string response1;
    public string response2;

    private int nbTF = 1100; //determine le nombre de point temporelle qui seront sauvegarder
    private Vector3[,] hips;
    private Vector3[,] spine;
    private Vector3[,] chest;
    private Vector3[,] neck;
    private Vector3[,] head;
    private Vector3[,] shoulderL;
    private Vector3[,] upperarmL;
    private Vector3[,] forerarmL;
    private Vector3[,] handL;
    private Vector3[,] shoulderR;
    private Vector3[,] upperarmR;
    private Vector3[,] forerarmR;
    private Vector3[,] handR;
    private Vector3[,] thighL;
    private Vector3[,] shinL;
    private Vector3[,] footL;
    private Vector3[,] toeL;
    private Vector3[,] thighR;
    private Vector3[,] shinR;
    private Vector3[,] footR;
    private Vector3[,] toeR;

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
    private GameObject BtoeL;
    private GameObject BthighR;
    private GameObject BshinR;
    private GameObject BfootR;
    private GameObject BtoeR;

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
    private GameObject BtoeL2;
    private GameObject BthighR2;
    private GameObject BshinR2;
    private GameObject BfootR2;
    private GameObject BtoeR2;
    private GameObject Avatar;

    private int[] vectorRandomization = new int[144];
    private int trialNumber = -1; // first matrix indice is 0
    private int recordIndice = -1; //indice save position
    private int playIndice = -1; //indice play position
    private bool participantHasPress = false; //record if the participant press on the mouse to start the trial

    private string filenamepath1;
    private string filenamepath2;
    private string filenamepath3;
    private string filenamepath4;
    private string filenamepath5;

    private long timer = 0;
    private Stopwatch stopwatch;
    private long elapsedMilliseconds;
    private long currentDelay;
    private string[] matrixtoeLZ;
    private string[] matrixtoeRZ;
    private string[] matrixhipsZ;
    private string[] matrixTime;
    private int nbMatrixPoint = 650; //600 est le minimum et je rajoute 80 de rab
    private int matrixIndice = -1;

    void Awake()
    {
        //deploy the Skeleton Asset Name
        FemaleBody.GetComponent<OptitrackSkeletonAnimator>().SkeletonAssetName = SkeletonAssetName;
        MaleBody.GetComponent<OptitrackSkeletonAnimator>().SkeletonAssetName = SkeletonAssetName;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (selectGender.ToString() == "Female")
        {
            FemaleBody.SetActive(true);
            MaleBody.SetActive(false);
            AvatarF.SetActive(true);
            AvatarM.SetActive(false);
            Avatar = AvatarF;
        }
        else
        {
            FemaleBody.SetActive(false);
            MaleBody.SetActive(true);
            AvatarF.SetActive(false);
            AvatarM.SetActive(true);
            Avatar = AvatarM;
        }

        //assign the GameObjects
        Bhips = GameObject.Find("B-hips");
        Bspine = GameObject.Find("B-spine");
        Bchest = GameObject.Find("B-chest");
        Bneck = GameObject.Find("B-neck");
        Bhead = GameObject.Find("B-head");
        BshoulderL = GameObject.Find("B-shoulder.L");
        BupperarmL = GameObject.Find("B-upperArm.L");
        BforerarmL = GameObject.Find("B-forearm.L");
        BhandL = GameObject.Find("B-hand.L");
        BshoulderR = GameObject.Find("B-shoulder.R");
        BupperarmR = GameObject.Find("B-upperArm.R");
        BforerarmR = GameObject.Find("B-forearm.R");
        BhandR = GameObject.Find("B-hand.R");
        BthighL = GameObject.Find("B-thigh.L");
        BshinL = GameObject.Find("B-shin.L");
        BfootL = GameObject.Find("B-foot.L");
        BtoeL = GameObject.Find("B-toe.L");
        BthighR = GameObject.Find("B-thigh.R");
        BshinR = GameObject.Find("B-shin.R");
        BfootR = GameObject.Find("B-foot.R");
        BtoeR = GameObject.Find("B-toe.R");

        Bhips2 = GameObject.Find("B-hips2");
        Bspine2 = GameObject.Find("B-spine2");
        Bchest2 = GameObject.Find("B-chest2");
        Bneck2 = GameObject.Find("B-neck2");
        Bhead2 = GameObject.Find("B-head2");
        BshoulderL2 = GameObject.Find("B-shoulder.L2");
        BupperarmL2 = GameObject.Find("B-upperArm.L2");
        BforerarmL2 = GameObject.Find("B-forearm.L2");
        BhandL2 = GameObject.Find("B-hand.L2");
        BshoulderR2 = GameObject.Find("B-shoulder.R2");
        BupperarmR2 = GameObject.Find("B-upperArm.R2");
        BforerarmR2 = GameObject.Find("B-forearm.R2");
        BhandR2 = GameObject.Find("B-hand.R2");
        BthighL2 = GameObject.Find("B-thigh.L2");
        BshinL2 = GameObject.Find("B-shin.L2");
        BfootL2 = GameObject.Find("B-foot.L2");
        BtoeL2 = GameObject.Find("B-toe.L2");
        BthighR2 = GameObject.Find("B-thigh.R2");
        BshinR2 = GameObject.Find("B-shin.R2");
        BfootR2 = GameObject.Find("B-foot.R2");
        BtoeR2 = GameObject.Find("B-toe.R2");


        //generate the experimental sucession
        int[] originalValues = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 50, 75, 100, 150, 225, 300, 450, 600, 750, 900, 1050, 1125, 1200, 1275, 1350, 25, 50, 75, 100, 150, 225, 300, 450, 600, 750, 900, 1050, 1125, 1200, 1275, 1350, 25, 50, 75, 100, 150, 225, 300, 450, 600, 750, 900, 1050, 1125, 1200, 1275, 1350, 25, 50, 75, 100, 150, 225, 300, 450, 600, 750, 900, 1050, 1125, 1200, 1275, 1350, 25, 50, 75, 100, 150, 225, 300, 450, 600, 750, 900, 1050, 1125, 1200, 1275, 1350, 25, 50, 75, 100, 150, 225, 300, 450, 600, 750, 900, 1050, 1125, 1200, 1275, 1350, 25, 50, 75, 100, 150, 225, 300, 450, 600, 750, 900, 1050, 1125, 1200, 1275, 1350, 25, 50, 75, 100, 150, 225, 300, 450, 600, 750, 900, 1050, 1125, 1200, 1275, 1350 }; //144 trials (8 for each of the 16 conditions + 16 no delay)
        int[] randomizedValues = ShuffleArray(originalValues);
        Array.Copy(randomizedValues, vectorRandomization, randomizedValues.Length);

        //Preparation of the output files
        DateTime currentDate = DateTime.Now; //get the time
        string dateString = currentDate.ToString("yyyy-MM-dd"); //convert it into string
        filenamepath1 = outputFolder + dateString + "_" + participantCode + "_FBA_Task2_QuestionnaireData" + ".csv";
        filenamepath2 = outputFolder + dateString + "_" + participantCode + "_FBA_Task2_toeL_Z" + ".csv";
        filenamepath3 = outputFolder + dateString + "_" + participantCode + "_FBA_Task2_toeR_Z" + ".csv";
        filenamepath4 = outputFolder + dateString + "_" + participantCode + "_FBA_Task2_hips_Z" + ".csv";
        filenamepath5 = outputFolder + dateString + "_" + participantCode + "_FBA_Task2_recordingTiming" + ".csv";

        ////record the header
        string[] Header1 = new string[] { "Pcode", "Trial", "Delay", "Question1Response", "Question2Response" };
        WriteLine(Header1, filenamepath1); //write the first header line of the output file
        string[] Header2 = new string[] { "Pcode", "Trial", "toeL_PositionZ" };
        WriteLine(Header2, filenamepath2);
        string[] Header3 = new string[] { "Pcode", "Trial", "toeR_PositionZ" };
        WriteLine(Header3, filenamepath3);
        string[] Header4 = new string[] { "Pcode", "Trial", "hips_PositionZ" };
        WriteLine(Header4, filenamepath4);
        string[] Header5 = new string[] { "Pcode", "Trial", "Timing in ms" };
        WriteLine(Header5, filenamepath5);

        Time.fixedDeltaTime = 0.005f; //fix update toutes les 5ms
    }

    // Update is called once per frame
    void FixedUpdate()  // void FixedUpdate()
    {
        if (managerState == 0) //Initialisation du trial
        {
            trialNumber++;
            currentDelay = (long)vectorRandomization[trialNumber];
            UnityEngine.Debug.Log("Trial number: " + trialNumber + " ; Delay: " + currentDelay);

            Avatar.SetActive(false);
            CanavasCross.SetActive(false);
            CameraTask.SetActive(true);
            CameraQuestionnaire.SetActive(false);            

            //empty the body recording
            hips = new Vector3[nbTF, 2]; //dans la colonne 1 ce sera les translation et en colonne 2 les rotations
            spine = new Vector3[nbTF, 2];
            chest = new Vector3[nbTF, 2];
            neck = new Vector3[nbTF, 2];
            head = new Vector3[nbTF, 2];
            shoulderL = new Vector3[nbTF, 2];
            upperarmL = new Vector3[nbTF, 2];
            forerarmL = new Vector3[nbTF, 2];
            handL = new Vector3[nbTF, 2];
            shoulderR = new Vector3[nbTF, 2];
            upperarmR = new Vector3[nbTF, 2];
            forerarmR = new Vector3[nbTF, 2];
            handR = new Vector3[nbTF, 2];
            thighL = new Vector3[nbTF, 2];
            shinL = new Vector3[nbTF, 2];
            footL = new Vector3[nbTF, 2];
            toeL = new Vector3[nbTF, 2];
            thighR = new Vector3[nbTF, 2];
            shinR = new Vector3[nbTF, 2];
            footR = new Vector3[nbTF, 2];
            toeR = new Vector3[nbTF, 2];

            recordIndice = -1;
            playIndice = -1;

            //Initialisation of the recording vectors
            matrixIndice = -1;            
            matrixtoeLZ = new string[nbMatrixPoint];
            matrixtoeRZ = new string[nbMatrixPoint];
            matrixhipsZ = new string[nbMatrixPoint];
            matrixTime = new string[nbMatrixPoint];

            stopwatch = new Stopwatch();
            timer = 0;

            participantHasPress = false; //reset the listening of mouse press

            managerState++;
        }
        else if (managerState == 1) //en attente du participant start
        {
            if (participantHasPress) //clique droite ou gauche de la souris
            {
                CanavasCross.SetActive(true); //trun on the cross
                stopwatch.Start();
                managerState++;
            }
        }
        else if (managerState == 2) //pre-stim of 2s
        {
            elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            if (elapsedMilliseconds >= 2000 - currentDelay) //start of the recording
            {
                bodyPositionRecording();
            }
            
            if (elapsedMilliseconds >= 2000)
            {
                Avatar.SetActive(true); //show the avatar
                timer = elapsedMilliseconds; //record the precise time of playback start
                managerState++;
            }
        }
        else if (managerState == 3) //start the playback of the mouvement
        {
            elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            if (elapsedMilliseconds- timer <= 3000) //durée de 3s du playback
            {
                bodyPositionRecording(); //continue a sauvé de mouvement
                bodyPositionPlayback(); // playback du recording 

                //save data
                matrixIndice++;
                if (matrixIndice < nbMatrixPoint - 1)
                {
                    matrixtoeLZ[matrixIndice] = BtoeL.transform.position.z.ToString("F3");
                    matrixtoeRZ[matrixIndice] = BtoeR.transform.position.z.ToString("F3");
                    matrixhipsZ[matrixIndice] = Bhips.transform.position.z.ToString("F3");
                    matrixTime[matrixIndice] = (elapsedMilliseconds - timer).ToString();
                }
                
            }
            else
            {
                CameraTask.SetActive(false);
                CameraQuestionnaire.SetActive(true);                
                stopwatch.Stop();
                managerState++;
            }          

        }
        else if (managerState == 4) //questionnaire state
        {
            //manage by the script questionnaireManagerEXP2
        }
        else if (managerState == 5) //save data
        {
            //data saving
            SaveDataQuestionnaire();
            SaveDataPosition(matrixtoeLZ, filenamepath2); // save the toeL
            SaveDataPosition(matrixtoeRZ, filenamepath3); // save the toeR
            SaveDataPosition(matrixhipsZ, filenamepath4); // save the hips
            SaveDataPosition(matrixTime, filenamepath5); // save the position Z


            //check if end of the experiment
            if (trialNumber >= numberOfTrials-1)
            {
                managerState = 7; //terminate the study
            }
            else
            {
                managerState=0; //go to next trial
            }
                        
        }
        else if (managerState == 7) //end of the study
        { }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) | Input.GetMouseButtonDown(1))
        {
            participantHasPress = true;
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

    private void bodyPositionRecording()
    {
        if (recordIndice < nbTF-1)
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
             toeL[recordIndice, 0] = BtoeL.transform.localPosition; toeL[recordIndice, 1] = BtoeL.transform.localRotation.eulerAngles;
             thighR[recordIndice, 0] = BthighR.transform.localPosition; thighR[recordIndice, 1] = BthighR.transform.localRotation.eulerAngles;
             shinR[recordIndice, 0] = BshinR.transform.localPosition; shinR[recordIndice, 1] = BshinR.transform.localRotation.eulerAngles;
             footR[recordIndice, 0] = BfootR.transform.localPosition; footR[recordIndice, 1] = BfootR.transform.localRotation.eulerAngles;
             toeR[recordIndice, 0] = BtoeR.transform.localPosition; toeR[recordIndice, 1] = BtoeR.transform.localRotation.eulerAngles;
        }
    }

    private void bodyPositionPlayback()
    {
        if (playIndice < nbTF-1)
        {
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
            BtoeL2.transform.localPosition = toeL[playIndice, 0]; BtoeL2.transform.localRotation = Quaternion.Euler(toeL[playIndice, 1]);
            BthighR2.transform.localPosition = thighR[playIndice, 0]; BthighR2.transform.localRotation = Quaternion.Euler(thighR[playIndice, 1]);
            BshinR2.transform.localPosition = shinR[playIndice, 0]; BshinR2.transform.localRotation = Quaternion.Euler(shinR[playIndice, 1]);
            BfootR2.transform.localPosition = footR[playIndice, 0]; BfootR2.transform.localRotation = Quaternion.Euler(footR[playIndice, 1]);
            BtoeR2.transform.localPosition = toeR[playIndice, 0]; BtoeR2.transform.localRotation = Quaternion.Euler(toeR[playIndice, 1]);
        }   
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
        int temp = trialNumber + 1;

        string[] data = new string[]
        {
            participantCode,
            temp.ToString(),
            vectorRandomization[trialNumber].ToString(),           
            response1,
            response2            
        };

        WriteLine(data, filenamepath1);
    }

    private void SaveDataPosition(string[] positions, string filePath)
    {
        int temp = trialNumber + 1;

        string[] data = new string[] { participantCode, temp.ToString()}.Concat(positions).ToArray(); //pour concatenate le vecteur de position
        
        WriteLine(data, filePath);
    }
}
