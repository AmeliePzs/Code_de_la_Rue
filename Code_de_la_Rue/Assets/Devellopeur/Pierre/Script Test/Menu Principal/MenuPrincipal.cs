using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{

    [SerializeField] private GameObject panelBoutton;
    [SerializeField] private GameObject panelParametres;
    [SerializeField] private GameObject panelLexique;
    [SerializeField] private GameObject panelSelection;

    private void Start()
    {
        // Pour s'assurer que le menu principal soit toujours ouvert � l'ouverture de la scene ( au cas ou le panel soit desactivez dans l'editeur)
        panelBoutton.SetActive(true);
        panelParametres.SetActive(false);
    }

    // M�thode pour charger la sc�ne
    public void OpenScene(string Scene)
    {
        // V�rifie que la scene existe
        if (!string.IsNullOrEmpty(Scene))
        {
            // Charge la sc�ne
            SceneManager.LoadScene(Scene);
        }
        else
        {
            Debug.LogWarning("Scene name is not assigned.");
        }
    }

    public void OpenParam�tres()
    {
        panelBoutton.SetActive(false);
        panelParametres.SetActive(true);
    }

    public void AppliquerParametres()
    {
        panelBoutton.SetActive(true);
        panelParametres.SetActive(false);

        // Mettre en place la sauvegarde et l'application des param�tres
    }

    public void OpenLexique()
    {
        panelBoutton.SetActive(false);
        panelLexique.SetActive(true);
    }

    public void OpenSelection()
    {
        panelBoutton.SetActive(false);
        panelSelection.SetActive(true);
    }
}