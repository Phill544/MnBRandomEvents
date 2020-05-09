# Adding events to your own mod
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
