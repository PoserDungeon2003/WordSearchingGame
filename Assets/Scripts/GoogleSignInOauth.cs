using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleSignInOauth : BaseServiceBootstrapper
{
    public ClientDataObject androidGoogleClientDataObject;
    public ClientDataObject googleClientDataObjectForEditorOnly;
    public ClientDataObject WindowsClientDataObject;
    public ClientDataObject iOSClientDataObject;

    protected override void RegisterServices()
    {
        OpenIDConnectService openIDConnectService = new OpenIDConnectService();
        openIDConnectService.OidcProvider = new GoogleOidcProvider();
        if (!ServiceManager.ServiceExists<OpenIDConnectService>())
        {
            ServiceManager serviceManager = new ServiceManager();
        }

#if UNITY_EDITOR
        openIDConnectService.OidcProvider.ClientData = googleClientDataObjectForEditorOnly.clientData;
        openIDConnectService.RedirectURI = "https://www.youtube.com/";
        openIDConnectService.ServerListener.ListeningUri = "http://127.0.0.1:52229/";
#elif UNITY_ANDROID
        openIDConnectService.OidcProvider.ClientData = androidGoogleClientDataObject.clientData;
        openIDConnectService.RedirectURI = "com.pru.wordsearchinggame:/";
#elif UNITY_IOS
        openIDConnectService.OidcProvider.ClientData = iOSClientDataObject.clientData;
        openIDConnectService.RedirectURI = "com.pru.wordsearchinggame:/";
#else
        openIDConnectService.OidcProvider.ClientData = WindowsClientDataObject.clientData;
        openIDConnectService.RedirectURI = "com.pru.wordsearchinggame:/";
        openIDConnectService.ServerListener.ListeningUri = "http://127.0.0.1:52229/";
#endif
        ServiceManager.RegisterService(openIDConnectService);
    }

    protected override void UnRegisterServices()
    {
        if (ServiceManager.ServiceExists<OpenIDConnectService>())
        {
            Debug.Log("Unregistering OpenIDConnectService: " + ServiceManager.ServiceExists<OpenIDConnectService>());
            ServiceManager.RemoveService<OpenIDConnectService>();
        }
    }
}
