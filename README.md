# Mount & Blade II: Bannerlord Random Events

Enhances the single-player campaign with dynamic random events. This README details the mod components for contribution and creation.

### RandomEventSubmodule
- Required for mod functionality, loads settings, and adds RandomEventBehavior.

### RandomEventBehavior
- Manages event generation and execution. Accessible via static instance for external DLLs. Includes a command line for testing.

### RandomEventGenerator
- Handles weighted random event selection. Determines the frequency of each event.

### BaseEvent (Abstract Class)
- Core methods: `CanExecuteEvent`, `StartEvent`, `StopEvent`, `CancelEvent`. Each random event inherits from this class.

### RandomEventData (Abstract Class)
- Essential for generating event instances. Override `GetBaseEvent()` to return new instances.

### PartySetup (Static Class)
- Simplifies party setup on the map.

### Console Commands
- `randomevent.run`: Triggers a specific event.
- `randomevent.next`: Launches the next event in the queue.
- `randomevent.cancelevent`: Cancels the current event.
- `randomevent.list`: Lists all events.
- `randomevent.queue`: Shows upcoming events.
- `randomevent.history`: Displays executed event history.

Recent updates include queue-based event management and history tracking.
