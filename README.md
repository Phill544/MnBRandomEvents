# Mount & Blade II: Bannerlord Random Events

A mod for Bannerlord that adds random events to the single player campaign. The project itself is very straight-forward. If you would like to contribute, below should include everything you need to get started.



## Brief Overview
No one wants to waffle on forever, so there is a brief summary of the main classes of the project in order to give you an idea.


### RandomEventSubmodule
This class is required by Bannerlord for the mod to function. It has two functions.
- Loads both the general settings, as well as the random event settings (Discussed later)
- Instantiates and adds the RandomEventBehavior to the game.

### RandomEventBehavior
This is the main class of the project. It controls setting up the random event generation, as well as deciding when to execute the events. Things to note are:
- You can access its static instance by calling 'RandomEventBehavior.Instance'. (Important for creating random events in external .dll files)
- You can manually call random events using the in-built command line, very helpful for testing.

### RandomEventGenerator
This class is responsible for the random selection of an event. The selection is weighted, therefore if you increase the weight of an event, it is more likely to be selected.

### BaseEvent
This abstract class is what every random event inherits from. There are four main methods that much be implemented.
- CanExecuteEvent() : This is called before an event is run to determine whether all of the dersired criteria have been met, for example you would return false if the event is to do with the player's kingdom if they aren't yet part of one.
- StartEvent() : This is the main function that does work. It will also change the most depending on each event (check out some of the ones in the repo for examples.) This is where you can run your logic to effect the player in whatever way you see fit. **Ensure that at some stage you call StopEvent() otherwise no other events will run**
- StopEvent() : This is used for cleaning up anything you need to, as well as notifying the random event system that your event is complete and therefore to start generating them again. It is very important that every random event calls this at some point after StartEvent() is called!
- CancelEvent() : This is called only if the current event needs to be interrupted. If this is called you need to end the event immediately.

### RandomEventData
This is an abstract class that must be derived from for every event. (I usually call the derived class XData, where X is the name of the random event class.) Is is a very import class that is used for generating the event instances themselves. Things to note:
- You must override the GetBaseEvent() class, you simpily want to return a new instance of the random event you wish to run.

### PartySetup
A static class that contains some functions to more easily set up parties on the map. This isn't a critical class, but I find it helpful to use to quickly set up scenarios.

## Want to add an event to this project?
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
