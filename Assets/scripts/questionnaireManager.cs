using UnityEngine;
using UnityEngine.UI;

public class questionnaireManager : MonoBehaviour
{

    public GameObject SceneManager;
    public GameObject questionText;
    public GameObject PanelBox1;
    public GameObject PanelBox2;
    public GameObject PanelBox3;
    public GameObject PanelBox4;
    public GameObject text2;
    public GameObject text3;

    private sceneManager sceneManager;
    private string LanguageText;
    private int questionaireState=0;

    private int scrollValue = 0;  // Variable qui sera modifiée par la molette
    private float scrollInput;

    private float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneManager = SceneManager.GetComponent<sceneManager>(); //store the link to the script sceneManager 
        LanguageText = sceneManager.selectLangue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneManager.managerState == 2 & questionaireState == 0) //Initilise the quesionnaire 1
        {
            questionaireState++;
            PanelBox1.SetActive(false);
            PanelBox4.SetActive(false);
            PanelBox2.SetActive(true);
            PanelBox3.SetActive(true);
            PanelBox2.GetComponent<Image>().color = Color.white;
            PanelBox3.GetComponent<Image>().color = Color.white;

            if (LanguageText == "fr")
            {
                questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Est-ce que le mouvement montré à l'écran correspondait au mouvement que vous venez d'effectuer ?";
                text2.GetComponent<TMPro.TextMeshProUGUI>().text = "Oui";
                text3.GetComponent<TMPro.TextMeshProUGUI>().text = "Non";
            }
            else if (LanguageText == "de")
            {
                questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Entspricht die auf dem Bildschirm gezeigte Bewegung der Bewegung, die Sie gerade ausgeführt haben?";
                text2.GetComponent<TMPro.TextMeshProUGUI>().text = " Ya";
                text3.GetComponent<TMPro.TextMeshProUGUI>().text = "Nein";
            }
            else
            {
                questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Did the movement shown on the screen correspond to the movement you just performed?";
                text2.GetComponent<TMPro.TextMeshProUGUI>().text = " Yes";
                text3.GetComponent<TMPro.TextMeshProUGUI>().text = " No";
            }

        }
        else if (questionaireState == 1) //attente réponse pour la première question
        {
            if (Input.GetMouseButtonDown(0))
            {
                PanelBox2.GetComponent<Image>().color = new Color(0.84f, 0.07f, 0.17f);
                sceneManager.response1 = "1";
                timer = 0f;
                questionaireState++;
            }
            else if (Input.GetMouseButtonDown(1)) 
            {
                PanelBox3.GetComponent<Image>().color = new Color(0.84f, 0.07f, 0.17f);
                sceneManager.response1 = "0";
                timer = 0f;
                questionaireState++;
            }

        }
        else if (questionaireState == 2) //validation response 1 animation
        {
            timer += Time.deltaTime; // Faire attendre 0.5s

            if (timer >= 0.5f) { questionaireState++; }// Si 0.5s écoulées       
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
        }
        else if (questionaireState == 4)//attente réponse pour la deuxième question
        {
            scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInput > 0) // Molette vers le haut
            {
                if (scrollValue < 3) { scrollValue = scrollValue + 1; } // Incrémente
            }
            else if (scrollInput < 0) // Molette vers le bas
            {
                if (scrollValue > 0) { scrollValue = scrollValue - 1; } // Décrémente
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

                timer = 0f;
                questionaireState++;

            }

        }
        else if (questionaireState == 5)//end of questionnaire, repositionner
        {
            timer += Time.deltaTime; // Faire attendre 0.5s

            if (timer >= 0.5f) // Si 0.5s écoulées
            {
                PanelBox1.SetActive(false);
                PanelBox2.SetActive(false);
                PanelBox3.SetActive(false);
                PanelBox4.SetActive(false);

                if (LanguageText == "fr")
                {
                    questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Merci ! Veuillez retourner à la position de départ.";
                }
                else if (LanguageText == "de")
                {
                    questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Vielen Dank! Bitte kehren Sie in die Ausgangsposition zurück.";
                }
                else
                {
                    questionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Thank you! Please return to the starting position.";
                }

                questionaireState = 0;
                sceneManager.managerState = 4;                
            }
        }
        
    } 
}
