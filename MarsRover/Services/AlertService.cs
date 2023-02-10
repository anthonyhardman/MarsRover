using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Services
{
    public class AlertService
    {
        public async Task ShowAlertAsync(string title, string message, string cancel = "Ok")
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel); 
        }

        public async Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
