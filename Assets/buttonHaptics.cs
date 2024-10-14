using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonHaptics : MonoBehaviour
{

    private XRUIInputModule InputModule => EventSystem.current.currentInputModule as XRUIInputModule;


    public void OnPointerEnter(PointerEventData eventData)
    {

        XRRayInteractor interactor = InputModule.GetInteractor(eventData.pointerId) as XRRayInteractor;

        if (!interactor) { return; }

        interactor.xrController.SendHapticImpulse(.5f, .5f);

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        XRRayInteractor interactor = InputModule.GetInteractor(eventData.pointerId) as XRRayInteractor;

        if (!interactor) { return; }

        interactor.xrController.SendHapticImpulse(.5f, .5f);

    }

}

