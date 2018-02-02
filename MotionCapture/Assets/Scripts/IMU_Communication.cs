using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Abstract singleton class to manage communitcation properties
/// and tasks that are common in all COMM channels.
/// </summary>
public abstract class IMU_Communication : Singleton<IMU_Communication>
{
    public List<GameObject> bodyParts = new List<GameObject>();

    /// <summary>
    /// Delegate event callback that executes when calibration data is received.
    /// </summary>
    public event Action<string> CalibrationReceived;

    /// <summary>
    /// By calling or overriding this method, derived classes can invoke the event indirectly.
    /// </summary>
    /// <param name="jsonData"></param>
    protected virtual void OnCalibrationReceived(string jsonData)
    {
        if (CalibrationReceived != null)
        {
            CalibrationReceived(jsonData);
        }
    }

    /// <summary>
    /// Virtual fuction to be overwritten by sub COMM class and used as Thread routine.
    /// </summary>
    public virtual void FetchData()
    {
        Debug.Log("Starting IMU data fetching thread...");
    }

    /// <summary>
    /// Initializes communication thread
    /// </summary>
    public virtual void InitComm()
    {
        Debug.Log("Initiating COMM Channel...");
    }

    /// <summary>
    /// Overwritten by sub COMM class to close any open ports when terminating communication
    /// </summary>
    public virtual void OnCommTerminate()
    {
        Debug.Log("Closing communication!");
    }

    void OnApplicationQuit()
    {
        OnCommTerminate();
    }
}