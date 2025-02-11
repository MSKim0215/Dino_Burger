using Cysharp.Threading.Tasks;
using MSKim.Manager;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSKim.UI
{
    public class GameTimerBox : MonoBehaviour
    {
        [Header("TimerBox View")]
        [SerializeField] private GameTimerBoxView view;

        private float currentTime = 0f;

        public event Action<float> OnTimerEvent;

        private const float MAXIMUM_TIME = 30;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            currentTime = MAXIMUM_TIME;

            StartTimer();
            view.Initialize(this);
        }

        private async void StartTimer()
        {
            while(true)
            {
                currentTime -= Time.deltaTime;
                OnTimerEvent?.Invoke(currentTime);

                await UniTask.Yield();

                if(currentTime <= 0f)
                {
                    for(int i = Managers.Guest.guestList.Count - 1; i >= 0 ; i--)
                    {
                        var guest = Managers.Guest.guestList[i];
                        if(guest == null)
                        {
                            Managers.Guest.guestList.Remove(guest);
                        }
                        else
                        {
                            Managers.Guest.guestList[i]?.Release();
                        }
                    }

                    currentTime = 0f;
                    SceneManager.LoadScene(0);
                    break;
                }
            }
        }
    }
}