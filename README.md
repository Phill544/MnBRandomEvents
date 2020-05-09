# Mount & Blade II: Bannerlord Random Events

A mod for Bannerlord that adds random events to the single player campaign. The project itself is very straight-forward. If you would like to contribute, below should include everything you need to get started.

## [Want to create your own random events?](RandomEventCreation.md)


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

# Want to add events to your own mod?
This is also quite easy! These steps will guide you:
(Assuming you already have a mod set up)
1. Add "RandomEvents.dll" to your project's references

2. Create the event that you would like (Follow the guide for creating an event for this project if you need to.) However there will be a key difference with how you handle the random event's data class.
You will need to handle your own RandomEventData instance, it also needs to be accessed from the random event's constructor. A simple example of this is:
```cs
public static class RandomEventDataSettings
{
	public static MyExternalDllModData MyExternalDllModData { get; private set; } = new MyExternalDllModData("MyExternalDllModData", 1024);
}
```
Then in the random event itself you would put
```cs
public MyExternalDllMod() : base(RandomEventDataSettings.MyExternalDllModData)
{}
```

3. The last thing to do is to add the random event data to the random event generator as it cannot be done automatically. This is very simple to do, you just need to make sure that you add the random event data after the random event generator is created. Currently, the generator is created in the "OnGameStart" function of the MBSubModuleBase, any time after that should be fine to add the event. I recommend doing it during the CampaignEvents.OnSessionLaunchedEvent. An example of how this is done is:
```cs
private void OnSessionLaunched(CampaignGameStarter obj)
{
	// Add other events
	RandomEventBehavior.Instance.RandomEventGenerator.AddEvent(RandomEventDataSettings.MyExternalDllModData);
}
```
The main line to take note of is:
```cs
RandomEventBehavior.Instance.RandomEventGenerator.AddEvent(RandomEventDataSettings.MyExternalDllModData);
```
This just adds the random event data to the generator so that the event can be instantiated when necessary.

4. That should be it! One cool thing to note is that you should also be able to use the console to debug your random event, by calling randomevent.run myexternaldllmod -- How cool, happy modding!
