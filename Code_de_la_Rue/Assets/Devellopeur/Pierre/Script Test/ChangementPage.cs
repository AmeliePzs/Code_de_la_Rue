using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ChangementPage : MonoBehaviour
{

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
}