using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleSignInOauth : BaseServiceBootstrapper
{
    public ClientDataObject googleClientDataObject;
    public ClientDataObject googleClientDataObjectForEditorOnly;

    protected override void RegisterServices()
    {
        OpenIDConnectService openIDConnectService = new OpenIDConnectService();
        openIDConnectService.OidcProvider = new GoogleOidcProvider();

#if !UNITY_EDITOR

        openIDConnectService.OidcProvider.ClientData = googleClientDataObject.clientData;
        openIDConnectService.RedirectURI = "com.pru.wordsearchinggame:/";
#else
        openIDConnectService.OidcProvider.ClientData = googleClientDataObjectForEditorOnly.clientData;
        openIDConnectService.RedirectURI = "https://www.youtube.com/";
        openIDConnectService.ServerListener.ListeningUri = "http://127.0.0.1:52229/";
#endif
        ServiceManager.RegisterService(openIDConnectService);
    }

    protected override void UnRegisterServices()
    {
        throw new System.NotImplementedException();
    }
}
