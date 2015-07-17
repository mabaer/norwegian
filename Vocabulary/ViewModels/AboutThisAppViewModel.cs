using System;
using AppStudio.Common;
using Windows.ApplicationModel;

namespace Vocabulary.ViewModels
{
    public class AboutThisAppViewModel : ObservableBase
    {
        public string Publisher
        {
            get
            {
                return "Marc-André Bär";
            }
        }

        public string AppVersion
        {
            get
            {
                return string.Format("{0}.{1}.{2}.{3}", Package.Current.Id.Version.Major, Package.Current.Id.Version.Minor, Package.Current.Id.Version.Build, Package.Current.Id.Version.Revision);
            }
        }

        public string AboutText
        {
            get
            {
                return "This app can be used to learn norwegian vocabulary and includes additional content like no" +
    "rwegian news, helpful sentences or the improvment of your pronunciation.\nCopyright 2015";
            }
        }
    }
}

