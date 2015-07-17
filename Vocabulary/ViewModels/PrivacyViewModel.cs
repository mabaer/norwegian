using System;
using AppStudio.Common;

namespace Vocabulary.ViewModels
{
    public class PrivacyViewModel : ObservableBase
    {
        public Uri Url
        {
            get
            {
                return new Uri(UrlText, UriKind.RelativeOrAbsolute);
            }
        }
        public string UrlText
        {
            get
            {
                return "http://1drv.ms/1MrXQji"; //https://onedrive.live.com/redir?resid=D4D1D9EE7ABAE534!77767&authkey=!AOL6yOAgVItbnE0&ithint=file%2ctxt
            }
        }
    }
}

