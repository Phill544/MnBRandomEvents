# Event Creation
Adding events is easy! Below are the steps in order to create your own random events for this project.

1. Download the repo and make sure you can build the .dll.

2. Create a new class derived from BaseEvent

3. Create a new class derived from RandomEventData (I usually name it XData, where X is whatever you called the class in step 2)

4. Inside of your Data class:
- Implement the GetBaseEvent() class, which should return a new instance of the class you made in step 2.
- Add any public variables that you wish to have as editable json values (e.g. percent chance to success, number of troops to spawn, etc)
- Assign those values in a constructor.

5. Go to RandomEventSettings.cs and add a new entry for your random event class, just copy the previous line and update what you need. Things to note:
- The string that you provide is what you use to call it in the command line (for example, randomevent.run MyNewEvent)
- The values that you put in here will be the default values for the event.
- Once you do this, technically the event is now part of the mod! (Although if you're following this guide then you will still have compile errors which we will deal with now!)

6. Go back to the base event that you created, to get the project buildable, there's a few things we need to do.
- Implement all of the abstract methods.
- Define the logic in CanExecuteEvent() for when the event can run (if it can always run, just return true)
- In StopEvent() add the line "OnEventCompleted.Invoke();". (This tells the random event system that it can start generating other random events again)
- If you're not keeping any special state, CancelEvent() can be left blank.
- Add a constructor and inside of its base() call, add the data object instance that you created in RandomEventSettings.cs
For example
```cs
public MyNewEvent() : base(Settings.RandomEvents.MyNewEventData)
{}
```

7. Add StopEvent(); inside of your StartEvent() function. This ensures that your event has been properly stopped.

8. Now you can technically run an event! To help see this, in your StartEvent() function, above where you called StopEvent() add:
```cs
InformationManager.ShowInquiry(
	new InquiryData("I've Done it!",
		$"I was able to create my mod: {this.RandomEventData.EventType}!",
		true,
		false,
		"Woohoo",
		null,
		null,
		null
		),
	true);
```
(You may need to add some references)

9. From here you should be able to add to your mod as you like, congrats!
