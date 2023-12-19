# Mount & Blade II: Bannerlord Random Events

A mod for Bannerlord that adds random events to the single player campaign. The project itself is very straight-forward. If you would like to contribute, below should include everything you need to get started.

## [Want to create your own random events?](RandomEventCreation.md)

## [Want to add events to your own mod?](ExternalDllEvents.md)

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
This abstract class is what every random event inherits from. There are four main methods that must be implemented.
- CanExecuteEvent() : This is called before an event is run to determine whether all of the desired criteria have been met, for example you would return false if the event is to do with the player's kingdom if they aren't yet part of one.
- StartEvent() : This is the main function that does work. It will also change the most depending on each event (check out some of the ones in the repo for examples.) This is where you can run your logic to effect the player in whatever way you see fit. **Ensure that at some stage you call StopEvent() otherwise no other events will run**
- StopEvent() : This is used for cleaning up anything you need to, as well as notifying the random event system that your event is complete and therefore to start generating them again. It is very important that every random event calls this at some point after StartEvent() is called!
- CancelEvent() : This is called only if the current event needs to be interrupted. If this is called you need to end the event immediately.

### RandomEventData
This is an abstract class that must be derived from for every event. (I usually call the derived class XData, where X is the name of the random event class.) This is a very important class that is used for generating the event instances themselves. Things to note:
- You must override the GetBaseEvent() class, you simply want to return a new instance of the random event you wish to run.

### PartySetup
A static class that contains some functions to more easily set up parties on the map. This isn't a critical class, but I find it helpful to use to quickly set up scenarios.
