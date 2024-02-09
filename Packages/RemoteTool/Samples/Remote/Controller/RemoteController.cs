using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Madbox;
using Samples.Helpers;
using Samples.Remote.DTO;
using Samples.Remote.Interface;
using UnityEngine;

namespace Samples.Remote.Controller
{
    public class RemoteController : IRemoteController
    {
        private const string API_URL = "https://script.googleusercontent.com/macros/echo?user_content_key=2cq3sIllZtEJbHxkcFny73LK8bwP_gbXYuE0C1jrkPWRKyipDW2ImNcR4N6efuIkpmf9PV3mRDGLR6G6byPHfcoE_Gzx77Zjm5_BxDlH2jW0nuo2oDemN9CCS2h10ox_1xSncGQajx_ryfhECjZEnEa9Z8-z3wRVlUaM9j2ZJp8cp0ZD_eEW8Vf-RCROpKWEAv0jOY2HrsspmBg0IRZU5Gc3KsdNg-aD-qNTlt0XkBkGbNKFuA0Shw&lib=M5YioJdcbZ146fiZV43VHb_-qERZM9JSA";

        public event Action<string> RemoteUpdated;
        public event Action Initialized;
        public bool IsInitialized { get; private set; }
        public static MadboxSheet MadboxSheet { get; private set; }
        
        private static MadboxSheet _default = new MadboxSheet
        {
            MovementConfig = new List<MovementConfig>
            {
                new MovementConfig{type = "run",speed = 5},
                new MovementConfig{type = "climb",speed = 5},
                new MovementConfig{type = "fly",speed = 5},
            },
            FogConfig = new List<FogConfig>
            {
                new FogConfig{key = "fogEnabled", value = true},
                new FogConfig{key = "fogDensity", value = 0.02},
            }
        };

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        
        public async void Init()
        {
            await FetchAndUpdateRemote();
            Repeater.RepeatTask(async () =>
            {
                await FetchAndUpdateRemote();
            },3, _cancellationTokenSource.Token);
            Application.quitting += OnQuit;

            Debug.Log($"<color=green>{GetType().Name} Initialized</color>");
            IsInitialized = true;
            Initialized?.Invoke();
        }

        private async Task FetchAndUpdateRemote()
        {
            MadboxSheet = await RemoteTool.DownloadAndParse(API_URL, _default);
            Debug.Log($"RemoteUpdated");
            MainThreadDispatcher.AddMainThreadCallback(() => RemoteUpdated?.Invoke(RemoteTool.LATEST_DATA));
        }

        private void OnQuit()
        {
            Debug.Log($"Quitting");
            _cancellationTokenSource.Cancel();
        }
    }
}