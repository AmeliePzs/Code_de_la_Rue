using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Deplacement : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private RectTransform rectTransform;
    private Canvas canvas;

    void Start()
    {
        // R�cup�ration du RectTransform de l'image
        rectTransform = GetComponent<RectTransform>();

        // R�cup�ration du canvas
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Ce GameObject doit �tre enfant d'un Canvas.");
        }
    }

    void Update()
    {
        DragAndDrop();
    }

    public void DragAndDrop()
    {
        // V�rifie s'il y a un ou plusieurs touch�s sur l'�cran
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // D�but du touch, v�rifie si l'utilisateur touche l'objet
                    if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, touch.position, canvas.worldCamera))
                    {
                        isDragging = true;

                        // Conversion des coordonn�es de l'�cran en coordonn�es locales
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, touch.position, canvas.worldCamera, out originalLocalPointerPosition);
                        originalPanelLocalPosition = rectTransform.localPosition;

                        Debug.Log("Drag started");
                    }
                    break;

                case TouchPhase.Moved:
                    // Si l'objet est en cours de drag, d�placez-la avec le doigt
                    if (isDragging)
                    {
                        Vector2 localPointerPosition;
                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, touch.position, canvas.worldCamera, out localPointerPosition))
                        {
                            Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                            rectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    // Fin du drag
                    if (isDragging)
                    {
                        isDragging = false;
                        Debug.Log("Drag ended");
                    }
                    break;
            }
        }
    }
}
