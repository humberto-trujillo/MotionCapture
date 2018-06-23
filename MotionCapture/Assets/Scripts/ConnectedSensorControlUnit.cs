﻿using System;
using UnityEngine;

public class ConnectedSensorControlUnit : MonoBehaviour 
{
	public delegate void ItemDropped(ConnectedSensor sensorItem);
	public event ItemDropped OnSensorDropped;
    
	/// <summary>
    /// Operate all drag and drop requests and events from children cells
    /// </summary>
    /// <param name="desc"> request or event descriptor </param>
    void OnSimpleDragAndDropEvent(DragAndDropCell.DropEventDescriptor desc)
    {
        // Get control unit of source cell
		ConnectedSensorControlUnit sourceSheet = desc.sourceCell.GetComponentInParent<ConnectedSensorControlUnit>();
        // Get control unit of destination cell
		ConnectedSensorControlUnit destinationSheet = desc.destinationCell.GetComponentInParent<ConnectedSensorControlUnit>();
        switch (desc.triggerType)                                               // What type event is?
        {
            case DragAndDropCell.TriggerType.DropRequest:                       // Request for item drag (note: do not destroy item on request)
                Debug.Log("Request " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                break;
            case DragAndDropCell.TriggerType.DropEventEnd:                      // Drop event completed (successful or not)
                if (desc.permission == true)                                    // If drop successful (was permitted before)
                {
                    Debug.Log("Successful drop " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
					ConnectedSensor sensorItem = desc.item.GetComponent<ConnectedSensor>();
					//sensorItem.boneType = desc.destinationCell.boneType;
                    try
					{
						sensorItem.boneType = (BoneType)Enum.Parse(typeof(BoneType), desc.destinationCell.name);
                    }
                    catch (Exception ex)
                    {
						Debug.Log(ex);
						sensorItem.boneType = BoneType.None;
                    }
					if(OnSensorDropped != null)
					{
						OnSensorDropped(sensorItem);
					}
                }
                else                                                            // If drop unsuccessful (was denied before)
                {
                    Debug.Log("Denied drop " + desc.item.name + " from " + sourceSheet.name + " to " + destinationSheet.name);
                }
                break;
            case DragAndDropCell.TriggerType.ItemAdded:                         // New item is added from application
                Debug.Log("Item " + desc.item.name + " added into " + destinationSheet.name);
                break;
            case DragAndDropCell.TriggerType.ItemWillBeDestroyed:               // Called before item be destructed (can not be canceled)
                Debug.Log("Item " + desc.item.name + " will be destroyed from " + sourceSheet.name);
                break;
            default:
                Debug.Log("Unknown drag and drop event");
                break;
        }
    }

    /// <summary>
    /// Add item in first free cell
    /// </summary>
    /// <param name="item"> new item </param>
    public void AddItemInFreeCell(DragAndDropItem item)
    {
        foreach (DragAndDropCell cell in GetComponentsInChildren<DragAndDropCell>())
        {
            if (cell != null)
            {
                if (cell.GetItem() == null)
                {
                    cell.AddItem(Instantiate(item.gameObject).GetComponent<DragAndDropItem>());
                    break;
                }
            }
        }
    }

	public ConnectedSensor AddItemInFreeCell(ConnectedSensor sensorItemPrefab)
    {
		ConnectedSensor connectedSensor = null;
        foreach (DragAndDropCell cell in GetComponentsInChildren<DragAndDropCell>())
        {
            if (cell != null)
            {
                if (cell.GetItem() == null)
                {
					connectedSensor = Instantiate(sensorItemPrefab.gameObject).GetComponent<ConnectedSensor>();
					cell.AddItem(connectedSensor.GetComponent<DragAndDropItem>());
                    break;
                }
            }
        }
		return connectedSensor;
    }

    /// <summary>
    /// Remove item from first not empty cell
    /// </summary>
    public void RemoveFirstItem()
    {
        foreach (DragAndDropCell cell in GetComponentsInChildren<DragAndDropCell>())
        {
            if (cell != null)
            {
                if (cell.GetItem() != null)
                {
                    cell.RemoveItem();
                    break;
                }
            }
        }
    }
}