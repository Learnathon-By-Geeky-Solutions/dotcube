using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeltaShare.Model;
using DeltaShare.Service;
using DeltaShare.Util;
using DeltaShare.View;

namespace DeltaShare.ViewModel
{
    public partial class SharePoolViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string qrCodeData = String.Empty;

        [ObservableProperty]
        private bool isViewSharedFilesBtnEnabled = false;

        public ObservableCollection<User> PoolUsers => StateManager.PoolUsers;

        public SharePoolViewModel(PoolCreatorClientService clientService)
        {
            QrCodeData = PoolCodeHandler.GenerateQrCodeData(NetworkHandler.GetLocalIps());

            StateManager.PoolUsers.CollectionChanged += (s, e) =>
            {
                Debug.WriteLine($"PoolUsers count: {StateManager.PoolUsers.Count}");
                if (StateManager.PoolUsers.Count > 1)
                {
                    IsViewSharedFilesBtnEnabled = true;
                }
                else
                {
                    IsViewSharedFilesBtnEnabled = false;
                }
            };
        }

        [RelayCommand]
        private async Task ClickViewSharedFilesBtn()
        {
            await Shell.Current.GoToAsync(nameof(DownloadFileView));
        }
    }
}
