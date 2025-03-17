using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class questionnaireManagerEXP2 : MonoBehaviour
{

    public GameObject SceneManager;
    public GameObject questionText;
    public GameObject PanelBox1;
    public GameObject PanelBox2;
    public GameObject PanelBox3;
    public GameObject PanelBox4;
    public GameObject text2;
    public GameObject text3;

    private sceneManagerEXP2 sceneManager;
    private string LanguageText;
    private int questionaireState=0;

    private int scrollValue = 0;  // Variable qui sera modifiée par la molette

    private Stopwatch stopwatch;
    private long elapsedMilliseconds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneManager = SceneManager.GetComponent<sceneManagerEXP2>(); //store the link to the script sceneManager 
        LanguageText = sceneManager.selectLangue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneManager.managerState == 4 & questionaireState == 0) //Initilise the quesionnaire 1
        {
            questionaireState++;
            PanelBox1.SetActive(false);
            PanelBox4.SetActive(false);
            PanelBox2.SetActive(true);
            PanelBox3.SetActive(true);
            PanelBox2.GetComponent<Image>().color = new Color(0.28f, 0.76f, 0.3f);
            PanelBox3.GetComponent<Image>().color = Color.white;
            scrollValue = 0;

            if (LanguageText == "fr")
            {
                questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Est-ce que le mouvement montré à l'écran correspondait au mouvement que vous venez d'effectuer ?";
                text2.GetComponent<TMPro.TextMeshProUGUI>().text = "Oui";
                text3.GetComponent<TMPro.TextMeshProUGUI>().text = "Non";
            }
            else if (LanguageText == "de")
            {
                questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Entsprach die auf dem Bildschirm gezeigte Bewegung der, die Sie gerade gemacht haben?";
                text2.GetComponent<TMPro.TextMeshProUGUI>().text = " Ya";
                text3.GetComponent<TMPro.TextMeshProUGUI>().text = "Nein";
            }
            else
            {
                questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Did the movement shown on the screen correspond to the movement you just performed?";
                text2.GetComponent<TMPro.TextMeshProUGUI>().text = " Yes";
                text3.GetComponent<TMPro.TextMeshProUGUI>().text = " No";
            }
            
            stopwatch = new Stopwatch();

        }
        else if (questionaireState == 1) //attente réponse pour la première question
        {

            if (Input.GetMouseButtonDown(0) & scrollValue > 0) //décrémente
            { 
                scrollValue = 0;
                PanelBox2.GetComponent<Image>().color = new Color(0.28f, 0.76f, 0.3f);
                PanelBox3.GetComponent<Image>().color = Color.white;
            }
            else if (Input.GetMouseButtonDown(1) & scrollValue < 1) //incémente
            {
                scrollValue = 1;
                PanelBox2.GetComponent<Image>().color = Color.white;  
                PanelBox3.GetComponent<Image>().color = new Color(0.28f, 0.76f, 0.3f);
            }

            if (Input.GetMouseButtonDown(2)) //réponse sélectionner détecter
            {
                if (scrollValue == 0)
                {
                    PanelBox2.GetComponent<Image>().color = new Color(0.84f, 0.07f, 0.17f);
                    sceneManager.response1 = "1";
                    stopwatch.Start();
                    questionaireState++;

                }
                else
                {
                    PanelBox3.GetComponent<Image>().color = new Color(0.84f, 0.07f, 0.17f);
                    sceneManager.response1 = "0";
                    stopwatch.Start();
                    questionaireState++;
                }
            }

        }
        else if (questionaireState == 2) //validation response 1 animation
        {
            elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            if (elapsedMilliseconds >= 500) { questionaireState++; }// Si 500ms se sont écoulées       
        }
        else if (questionaireState == 3) //Initilise the quesionnaire 2
        {
            questionaireState++;
            scrollValue = 0;
            PanelBox1.SetActive(true);
            PanelBox4.SetActive(true);
            PanelBox1.GetComponent<Image>().color = new Color(0.28f, 0.76f, 0.3f); 
            PanelBox2.GetComponent<Image>().color = Color.white;
            PanelBox3.GetComponent<Image>().color = Color.white;
            PanelBox4.GetComponent<Image>().color = Color.white;
            text2.GetComponent<TMPro.TextMeshProUGUI>().text = "33 %";
            text3.GetComponent<TMPro.TextMeshProUGUI>().text = "67 %";

            if (LanguageText == "fr")
            {
                questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "A quel point êtes-vous sûr de votre réponse ?";
            }
            else if (LanguageText == "de")
            {
                questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Wie sicher sind Sie sich bei Ihrer Antwort?";
            }
            else
            {
                questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "How sure are you of your response?";
            }
            stopwatch.Stop();
            stopwatch = new Stopwatch();
        }
        else if (questionaireState == 4)//attente réponse pour la deuxième question
        {

            if (Input.GetMouseButtonDown(1) & scrollValue < 3)               
            {
                scrollValue = scrollValue + 1; // Incrémente
            }
            else if (Input.GetMouseButtonDown(0) & scrollValue > 0)
            {
                scrollValue = scrollValue - 1; // Décrémente
            }

            if (scrollValue == 0)
            {
                PanelBox1.GetComponent<Image>().color = new Color(0.28f, 0.76f, 0.3f);
                PanelBox2.GetComponent<Image>().color = Color.white;
                PanelBox3.GetComponent<Image>().color = Color.white;
                PanelBox4.GetComponent<Image>().color = Color.white;
            }
            else if (scrollValue == 1)
            {
                PanelBox1.GetComponent<Image>().color = Color.white;
                PanelBox2.GetComponent<Image>().color = new Color(0.28f, 0.76f, 0.3f);
                PanelBox3.GetComponent<Image>().color = Color.white;
                PanelBox4.GetComponent<Image>().color = Color.white;
            }
            else if (scrollValue == 2)
            {
                PanelBox1.GetComponent<Image>().color = Color.white;
                PanelBox2.GetComponent<Image>().color = Color.white;
                PanelBox3.GetComponent<Image>().color = new Color(0.28f, 0.76f, 0.3f);
                PanelBox4.GetComponent<Image>().color = Color.white;
            }
            else if (scrollValue == 3)
            {
                PanelBox1.GetComponent<Image>().color = Color.white;
                PanelBox2.GetComponent<Image>().color = Color.white;
                PanelBox3.GetComponent<Image>().color = Color.white;
                PanelBox4.GetComponent<Image>().color = new Color(0.28f, 0.76f, 0.3f);
            }


            if (Input.GetMouseButtonDown(2)) //réponse sélectionner détecter
            {
                if (scrollValue == 0)
                {
                    PanelBox1.GetComponent<Image>().color = new Color(0.84f, 0.07f, 0.17f);
                    PanelBox2.GetComponent<Image>().color = Color.white;
                    PanelBox3.GetComponent<Image>().color = Color.white;
                    PanelBox4.GetComponent<Image>().color = Color.white;
                    sceneManager.response2 = "0";

                }
                else if (scrollValue == 1)
                {
                    PanelBox1.GetComponent<Image>().color = Color.white;
                    PanelBox2.GetComponent<Image>().color = new Color(0.84f, 0.07f, 0.17f);
                    PanelBox3.GetComponent<Image>().color = Color.white;
                    PanelBox4.GetComponent<Image>().color = Color.white;
                    sceneManager.response2 = "1";
                }
                else if (scrollValue == 2)
                {
                    PanelBox1.GetComponent<Image>().color = Color.white;
                    PanelBox2.GetComponent<Image>().color = Color.white;
                    PanelBox3.GetComponent<Image>().color = new Color(0.84f, 0.07f, 0.17f);
                    PanelBox4.GetComponent<Image>().color = Color.white;
                    sceneManager.response2 = "2";
                }
                else if (scrollValue == 3)
                {
                    PanelBox1.GetComponent<Image>().color = Color.white;
                    PanelBox2.GetComponent<Image>().color = Color.white;
                    PanelBox3.GetComponent<Image>().color = Color.white;
                    PanelBox4.GetComponent<Image>().color = new Color(0.84f, 0.07f, 0.17f);
                    sceneManager.response2 = "3";
                }

                stopwatch.Start();
                questionaireState++;

            }

        }
        else if (questionaireState == 5)//end of questionnaire, repositionner
        {
            elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            if (elapsedMilliseconds >= 500) // Faire attendre 500ms
            {
                PanelBox1.SetActive(false);
                PanelBox2.SetActive(false);
                PanelBox3.SetActive(false);
                PanelBox4.SetActive(false);
                                
                questionaireState = 0;
                sceneManager.managerState = 5;                
            }
        }
        else if (sceneManager.managerState == 7) //Fin de l'étude
        {
            PanelBox1.SetActive(false);
            PanelBox2.SetActive(false);
            PanelBox3.SetActive(false);
            PanelBox4.SetActive(false);

            if (LanguageText == "fr")
            {
                questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Cette expérience est terminée. Merci de votre participation!";
            }
            else if (LanguageText == "de")
            {
                questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Dieses Experiment ist abgeschlossen. Vielen Dank für Ihre Teilnahme!";
            }
            else
            {
                questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "This experiment is over. Thank you for your participation!";
            }
        }

        
    } 
}
