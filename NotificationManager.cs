using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NotificationManager1 : MonoBehaviour {

	//Internal refrence to all notifications.
	private Dictionary <string , List<Component>> Listeners = new Dictionary<string , List<Component>>();

	//Adding a Listener to a notification in Listeners list.
	public void AddListener (Component Listener , string NotificationName){

		//Add Listener to the dictonary (If the Event is new, register it in the dictonery as a new list). 
		if (!Listeners.ContainsKey(NotificationName))
			Listeners.Add(NotificationName , new List<Component>());
		//Adds the listeners to the specified event list (NotificationName).
		Listeners [NotificationName].Add (Listener);
	}
	//Posting Notifications to Listeners.
	public void PostNotification(string NotificationName){

		//Checks if the Event exists in the dictionary. If not, exit the function.
		if (! Listeners.ContainsKey (NotificationName))
			return;
		else {
			//Notify all registered Listerners for the spicified event.
			foreach (Component Listener in Listeners[NotificationName]){
				Listener.SendMessage(NotificationName,Listener,SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	//Removing the unneeded Listeners from the List of spicified Notification in the dictonary
	public void RemoveListener (Component Listener , string NotificationName){
		//Checks if the Event exists in the dictionary. If not, exit the function.
		if (! Listeners.ContainsKey (NotificationName))
			return;
		else {
			//Loops through spicified Notification list, compares the id of it's elements with the sender's ID
			//If matches, Remove.
			for (int i = Listeners[NotificationName].Count-1; i>=0; i++){
				//The copmaring condition.
				if (Listeners[NotificationName][i].GetInstanceID == Listener.GetInstanceID)
					Listeners[NotificationName].RemoveAt(i);//It's a match, remove the Listener from the List.
			}
		}
	}
	//Removing Redundancy
	public void RemoveRedundancies(){
		//Creating new temporay dictionary 
		Dictionary<string , List<Component>> TmpListeners = new Dictionary<string, List<Component>> ();
		//Copying Listeners Events into the temporary Dictionary
		foreach (KeyValuePair<string , List<Component>> Item in Listeners) {
			//Looping through every event's list
			for ( int i = Item.Value.Count-1; i>=0; i--){
				//If the current item of the list is a null reference, remove it.
				if ( Item.Value[i] == null)
					Item.Value.RemoveAt(i);
				//If the current item is still registered, add it to the temporary Listerner dictionary
				if(Item.Value.Count>0)
					TmpListeners.Add(Item.Key,Item.Value);
			}
			//Replacing Listeners dicronary, with new optimized one. 
			Listeners = TmpListeners;
		}
	}
	//Removes all Listeners
	public void ClearListeners(){
		Listeners.Clear ();
	}
	public void onLevelWasLoaded(){
		//Call redundancy removal function in case for any left over from previous scenes.
		RemoveRedundancies ();
	}
}
