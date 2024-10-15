using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.XR.Interaction.Toolkit;


public class ButtonHaptics : MonoBehaviour
{
    // Get the InputModule from the current EventSystem
    private XRUIInputModule InputModule => EventSystem.current.currentInputModule as XRUIInputModule;

    // Method called on pointer enter event
    public void OnPointerEnter(BaseEventData baseEventData)
    {
        // Cast the base event data to PointerEventData
        PointerEventData eventData = baseEventData as PointerEventData;

        if (eventData == null)
        {
            Debug.LogWarning("EventData is not of type PointerEventData.");
            return;
        }

        // Get the XRRayInteractor using the pointerId from the event data
        XRRayInteractor interactor = InputModule.GetInteractor(eventData.pointerId) as XRRayInteractor;

        if (interactor == null)
        {
            Debug.LogWarning("No XRRayInteractor found for pointer enter.");
            return;
        }

        // Send haptic feedback (stronger on enter)
        interactor.xrController.SendHapticImpulse(1.0f, 0.3f);
    }

    // Method called on pointer exit event
    public void OnPointerExit(BaseEventData baseEventData)
    {
        // Cast the base event data to PointerEventData
        PointerEventData eventData = baseEventData as PointerEventData;

        if (eventData == null)
        {
            Debug.LogWarning("EventData is not of type PointerEventData.");
            return;
        }

        // Get the XRRayInteractor using the pointerId from the event data
        XRRayInteractor interactor = InputModule.GetInteractor(eventData.pointerId) as XRRayInteractor;

        if (interactor == null)
        {
            Debug.LogWarning("No XRRayInteractor found for pointer exit.");
            return;
        }

        // Send haptic feedback (weaker on exit)
        interactor.xrController.SendHapticImpulse(0.2f, 0.2f);
    }
}

