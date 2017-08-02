using System;
using System.Windows;
using EWPFLang.ELang;
using KISLogger;

namespace EWPF_Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object i_Sender, StartupEventArgs i_E)
        {
            try
            {
                ELanguageRepository.Initialize(LanguageStorageType.Xml);
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
            }
        }
    }
}
