using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Management; //Add that library for XR Plugins

public class XRSwitcher : MonoBehaviour
{
    string xrPluginNames = "";
    void Start()
    {
        //You can get every single activated Plug-in Providers name with that for loop
        for(int i = 0; i < XRGeneralSettings.Instance.Manager.activeLoaders.Count; i++)
        {
            xrPluginNames += i + ": " + XRGeneralSettings.Instance.Manager.activeLoaders[i].name + "\n";
        }     
        
	    //for me the first one was ArCore and second one was cardboard
        //Enter your plugin index to load plugin
        StartXR(0); //That should starts ArCore if enabled both ArCore and cardboard.
        StartXR(1); //And that should starts Cardboard if enabled both ArCore and cardboard

        //This will be stop all XR plugins then you can start a new one
        StopXR();
    }

    void Update()
    {}

    XRLoader m_SelectedXRLoader;

    void StartXR(int loaderIndex)
    {
        // Once a loader has been selected, prevent the RuntimeXRLoaderManager from
        // losing access to the selected loader
        if (m_SelectedXRLoader == null)
        {
            m_SelectedXRLoader = XRGeneralSettings.Instance.Manager.activeLoaders[loaderIndex];
        }
        StartCoroutine(StartXRCoroutine());
    }

    IEnumerator StartXRCoroutine()
    {
        Debug.Log("Init XR loader");

        var initSuccess = m_SelectedXRLoader.Initialize();
        if (!initSuccess)
        {
            Debug.LogError("Error initializing selected loader.");
        }
        else
        {
            yield return null;
            Debug.Log("Start XR loader");
            var startSuccess = m_SelectedXRLoader.Start();
            if (!startSuccess)
            {
                yield return null;
                Debug.LogError("Error starting selected loader.");
                m_SelectedXRLoader.Deinitialize();
            }
        }
    }

    void StopXR()
    {
        Debug.Log("Stopping XR Loader...");
        m_SelectedXRLoader.Stop();
        m_SelectedXRLoader.Deinitialize();
        m_SelectedXRLoader = null;
        Debug.Log("XR Loader stopped completely.");
    }
}