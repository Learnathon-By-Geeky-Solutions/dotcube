using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DeltaShare.ViewModel
{
    public partial class JoinPoolViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string poolCodeInputText = string.Empty;

        public JoinPoolViewModel()
        {

        }

        [RelayCommand]
        void ClickJoinPoolBtn()
        {
            ShowDebugMsg($"Join Pool with\npool code: {PoolCodeInputText}");
        }
    }
}
