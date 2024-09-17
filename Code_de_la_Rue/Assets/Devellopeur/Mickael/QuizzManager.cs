using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

public class QuizManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text questionText; // Le texte pour la question
    public Button[] answerButtons; // Tableau de boutons de r�ponses (jusqu'� 4)

    [Header("Quiz Data")]
    private List<QuizQuestion> quizQuestions = new List<QuizQuestion>(); // Liste des questions
    private int currentQuestionIndex = 0;
    private bool[] playerSelections; // S�lections du joueur pour la question actuelle

    private int totalScore = 0; // Score total du joueur

    [Header("CSV Source")]
    public string csvURL = "https://docs.google.com/spreadsheets/d/1abcdEfGhijKlMnOpQrStuvWxYZ1234/export?format=csv"; // URL du fichier CSV ou chemin local

    // Structure repr�sentant une question du quiz
    [System.Serializable]
    public class QuizQuestion
    {
        public string question;
        public string[] answers;
        public List<int> correctAnswerIndices;
    }

    void Start()
    {
        StartCoroutine(DownloadQuizData()); // D�marrer le t�l�chargement du CSV au d�marrage du jeu
    }

    // T�l�chargement et lecture du fichier CSV
    IEnumerator DownloadQuizData()
    {
        // Utiliser un WebClient pour t�l�charger le fichier CSV
        using (WebClient client = new WebClient())
        {
            string csvData = client.DownloadString(csvURL);
            ParseCSV(csvData); // Parser les donn�es apr�s le t�l�chargement
        }

        yield return null; // Continuer apr�s la fin de la coroutine
    }

    // Lecture et parsing du fichier CSV
    void ParseCSV(string csvData)
    {
        StringReader reader = new StringReader(csvData);
        string line;

        // Ignorer la premi�re ligne (header)
        reader.ReadLine();

        while ((line = reader.ReadLine()) != null)
        {
            // Diviser chaque ligne par les virgules (','), ajuster selon votre format
            string[] fields = line.Split(',');

            QuizQuestion question = new QuizQuestion();
            question.question = fields[0];
            question.answers = new string[4];

            for (int i = 0; i < 4; i++)
            {
                question.answers[i] = fields[i + 1];
            }

            // Extraire les bonnes r�ponses (qui peuvent �tre multiples)
            question.correctAnswerIndices = new List<int>();
            string[] correctAnswers = fields[5].Split(',');
            foreach (string correctAnswer in correctAnswers)
            {
                question.correctAnswerIndices.Add(int.Parse(correctAnswer) - 1); // Convertir en index 0-based
            }

            // Ajouter la question � la liste des questions
            quizQuestions.Add(question);
        }

        // Lancer le quiz apr�s avoir charg� les questions
        LoadQuestion();
    }

    // Charger une question du quiz
    void LoadQuestion()
    {
        if (currentQuestionIndex >= quizQuestions.Count)
        {
            EndQuiz(); // Fin du quiz si toutes les questions ont �t� pos�es
            return;
        }

        // Obtenir la question actuelle
        QuizQuestion currentQuestion = quizQuestions[currentQuestionIndex];

        // Afficher la question
        questionText.text = currentQuestion.question;

        // Initialiser les s�lections du joueur
        playerSelections = new bool[currentQuestion.answers.Length];

        // Afficher les r�ponses et g�rer les boutons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.answers.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<Text>().text = currentQuestion.answers[i];
                int buttonIndex = i; // Stocker l'index pour le onClick
                answerButtons[i].onClick.RemoveAllListeners(); // Nettoyer les anciens listeners
                answerButtons[i].onClick.AddListener(() => OnAnswerButtonClicked(buttonIndex)); // Ajouter le listener
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false); // D�sactiver les boutons non utilis�s
            }
        }
    }

    // Appel�e lorsqu'un bouton de r�ponse est cliqu�
    void OnAnswerButtonClicked(int index)
    {
        // Inverser la s�lection (s�lectionner/d�s�lectionner)
        playerSelections[index] = !playerSelections[index];

        // Mettre � jour la couleur du bouton pour indiquer la s�lection
        UpdateButtonColor(index);
    }

    // Change la couleur du bouton en fonction de la s�lection
    void UpdateButtonColor(int index)
    {
        ColorBlock colors = answerButtons[index].colors;
        if (playerSelections[index])
        {
            colors.normalColor = Color.green; // S�lectionn�
        }
        else
        {
            colors.normalColor = Color.white; // Non s�lectionn�
        }
        answerButtons[index].colors = colors;
    }

    // Calcul du score pour une question
    public void CalculateScore()
    {
        QuizQuestion currentQuestion = quizQuestions[currentQuestionIndex];
        int questionScore = 0;

        for (int i = 0; i < currentQuestion.answers.Length; i++)
        {
            if (playerSelections[i] == currentQuestion.correctAnswerIndices.Contains(i))
            {
                questionScore++; // Ajouter des points si la r�ponse est correcte
            }
        }

        totalScore += questionScore;
        currentQuestionIndex++; // Passer � la question suivante
        LoadQuestion(); // Charger la question suivante
    }

    // Fin du quiz
    public void EndQuiz()
    {
        Debug.Log("Quiz termin� ! Score final : " + totalScore);
        // Ici, vous pouvez afficher le score final ou charger une nouvelle sc�ne
    }
}