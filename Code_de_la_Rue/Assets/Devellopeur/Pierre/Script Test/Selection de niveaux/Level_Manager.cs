using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Manager : MonoBehaviour
{

    public GameObject PanelSelection;
    public GameObject PanelButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void Retour()
    {
        PanelSelection.SetActive(false);
        PanelButton.SetActive(true);
    }
}
