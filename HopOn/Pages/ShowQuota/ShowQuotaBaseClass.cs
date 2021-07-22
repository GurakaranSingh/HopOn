using HopOn.Core.Contract;
using HopOn.Data;
using HopOn.Model;
using HopOn.Model.Model;
using HopOn.Model.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HopOn.Pages.ShowQuota
{
    public class ShowQuotaBaseClass : ComponentBase
    {
        public ShowQuotaViewModel model { get; set; }
        [Inject]
        private IUploadUtilityHelperServices _UploadUtilityHelperServices { get; set; }

        public void UpdateQUota()
        {
            model =  _UploadUtilityHelperServices.GetQuota();
            StateHasChanged();
        }
        protected override Task OnInitializedAsync()
        {
             UpdateQUota();
            return base.OnInitializedAsync();
        }
        public void RefreshQuota()
        {
             UpdateQUota();
        }
    }
}
