using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeltaShare.Service;

namespace DeltaShare.ViewModel
{
    public partial class JoinPoolViewModel : BaseViewModel
    {
        private readonly PoolJoinService joinService;
        [ObservableProperty]
        private string poolCodeInputText = string.Empty;

        public JoinPoolViewModel(PoolJoinService joinService)
        {
            this.joinService = joinService;
        }

        [RelayCommand]
        private async Task ClickJoinPoolBtn()
        {
            await joinService.SendInfoToPoolCreator(PoolCodeInputText);
        }
    }
}
