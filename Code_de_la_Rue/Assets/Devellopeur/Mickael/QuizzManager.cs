using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

public class QuizManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text questionText;
    public Button[] boutonsReponse; // Tableau de boutons de r�ponses (jusqu'� 4)

    [Header("Quiz Data")]
    private List<QuizQuestion> quizQuestions = new List<QuizQuestion>(); // Liste des questions
    private int indexQuestionActu = 0;
    private bool[] selectionJoueur; // S�lections du joueur pour la question actuelle

    private int totalScore = 0; // Score total du joueur

    [Header("CSV Source")]
    public string csvURL = "https://docs.google.com/spreadsheets/d/1abcdEfGhijKlMnOpQrStuvWxYZ1234/export?format=csv"; // URL du fichier CSV

    // Structure repr�sentant une question du quiz
    [System.Serializable]
    public class QuizQuestion
    {
        public string question;
        public string[] reponses;
        public List<int> reponseCorrectQuestion;
    }

    void Start()
    {
        StartCoroutine(TelechargementDataQuiz()); // D�marrer le t�l�chargement du CSV au d�marrage
    }

    // T�l�chargement et lecture du fichier CSV
    IEnumerator TelechargementDataQuiz()
    {
        // Utiliser un WebClient pour t�l�charger le fichier CSV
        using (WebClient client = new WebClient())
        {
            string csvData = client.DownloadString(csvURL);
            AnalyseFichierCSV(csvData); // Parser les donn�es apr�s le t�l�chargement
        }

        yield return null; // Continuer apr�s la fin de la coroutine
    }

    // Lecture et analyse du fichier CSV
    void AnalyseFichierCSV(string csvData)
    {
        StringReader reader = new StringReader(csvData);
        string line;

        // Ignorer la premi�re ligne
        reader.ReadLine();

        while ((line = reader.ReadLine()) != null)
        {
            // Diviser chaque ligne par les virgules (','), ajuster selon votre format
            string[] fields = line.Split(',');

            // V�rifier si la ligne contient suffisamment de colonnes (6 dans votre cas : question + 4 r�ponses + indices des bonnes r�ponses)
            if (fields.Length < 6)
            {
                Debug.LogWarning("Ligne mal format�e ou incompl�te : " + line);
                continue; // Ignorer cette ligne et passer � la suivante
            }

            QuizQuestion question = new QuizQuestion();
            question.question = fields[0];
            question.reponses = new string[4];

            for (int i = 0; i < 4; i++)
            {
                question.reponses[i] = fields[i + 1];
            }

            // Extraire les bonnes r�ponses (qui peuvent �tre multiples)
            question.reponseCorrectQuestion = new List<int>();
            string[] reponsesCorrect = fields[5].Split(',');
            foreach (string laReponseCorrect in reponsesCorrect)
            {
                if (int.TryParse(laReponseCorrect, out int indexReponse))
                {
                    question.reponseCorrectQuestion.Add(indexReponse - 1); // Convertir en index 0-based
                }
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
        if (indexQuestionActu >= quizQuestions.Count)
        {
            EndQuiz(); // Fin du quiz si toutes les questions ont �t� pos�es
            return;
        }

        // Obtenir la question actuelle
        QuizQuestion questionActuel = quizQuestions[indexQuestionActu];

        questionText.text = questionActuel.question;

        // Initialiser les s�lections du joueur
        selectionJoueur = new bool[questionActuel.reponses.Length];

        // Afficher les r�ponses et gestion boutons
        for (int i = 0; i < boutonsReponse.Length; i++)
        {
            if (i < questionActuel.reponses.Length)
            {
                boutonsReponse[i].gameObject.SetActive(true);
                boutonsReponse[i].GetComponentInChildren<Text>().text = questionActuel.reponses[i];
                int indexBouton = i; // Stocker l'index (pour le click)
                boutonsReponse[i].onClick.RemoveAllListeners(); // Nettoyer les anciens listeners
                boutonsReponse[i].onClick.AddListener(() => OnAnswerButtonClicked(indexBouton)); // Ajouter d'un listener
            }
            else
            {
                boutonsReponse[i].gameObject.SetActive(false); // D�sactiver les boutons non utilis�s
            }
        }
    }

    // Appel�e lorsqu'un bouton de r�ponse est cliqu�
    void OnAnswerButtonClicked(int index)
    {
        // Inverser la s�lection (s�lectionner/d�s�lectionner)
        selectionJoueur[index] = !selectionJoueur[index];

        // Mettre � jour la couleur du bouton pour indiquer la s�lection
        UpdateButtonColor(index);
    }

    // Change la couleur du bouton en fonction de la s�lection
    void UpdateButtonColor(int index)
    {
        ColorBlock colors = boutonsReponse[index].colors;
        if (selectionJoueur[index])
        {
            colors.normalColor = Color.green;
        }
        else
        {
            colors.normalColor = Color.white;
        }
        boutonsReponse[index].colors = colors;
    }

    // Calcul du score pour 1 question
    public void CalculDuScore()
    {
        QuizQuestion questionActuel = quizQuestions[indexQuestionActu];
        int questionScore = 0;

        for (int i = 0; i < questionActuel.reponses.Length; i++)
        {
            if (selectionJoueur[i] == questionActuel.reponseCorrectQuestion.Contains(i))
            {
                questionScore++; // Ajouter des points si la r�ponse est correcte
            }
        }

        totalScore += questionScore;
        indexQuestionActu++; // Passer � la question suivante
        LoadQuestion(); // Charger la question suivante
    }

    // Fin du quiz
    public void EndQuiz()
    {
        Debug.Log("Quiz termin� ! Score final : " + totalScore);
        // Ici, vous pouvez afficher le score final ou charger une nouvelle sc�ne
    }
}