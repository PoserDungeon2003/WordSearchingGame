using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleLoginLogoutButton : MonoBehaviour
{
    public GameObject googleLoginButton;
    public GameObject logoutButton;

    void Update()
    {
        if (ApiClient.Instance.GetAccessToken() != null)
        {
            googleLoginButton.SetActive(false);
            logoutButton.SetActive(true);
        }
        else
        {
            googleLoginButton.SetActive(true);
            logoutButton.SetActive(false);
        }
    }
}
