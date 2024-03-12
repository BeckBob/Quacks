using System;
using UnityEngine;

public class FunctionTimer : MonoBehaviour
{
 
    public static FunctionTimer Create(Action action, float timer)
    {
        GameObject gameObject = new GameObject("FunctionTimer", typeof(MonoBehaviourHook));
        FunctionTimer functionTimer = gameObject.AddComponent<FunctionTimer>();
        functionTimer.Initialize(action, timer);

        return functionTimer;
    }

    private class MonoBehaviourHook : MonoBehaviour
    {
        public Action onUpdate;

        private void Update()
        {
            if (onUpdate != null)
                onUpdate();
        }
    }

    private Action action;
    private float timer;
    private bool isDestroyed;

    public void Initialize(Action action, float timer)
    {
        this.action = action;
        this.timer = timer;
        isDestroyed = false;

        var hook = GetComponent<MonoBehaviourHook>();
        hook.onUpdate = UpdateFunctionTimer;
    }

    private void UpdateFunctionTimer()
    {
        if (!isDestroyed)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                action();
                DestroySelf();
            }
        }
    }

    private void DestroySelf()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }
}