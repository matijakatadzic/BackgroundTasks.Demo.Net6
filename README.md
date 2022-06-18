# Run and manage periodic background tasks in ASP.NET Core 6 with C#

Sometimes your web app needs to do work in the background periodically e.g. to sync data. This article provides a walkthrough how to implement such a background task and how to enabled/disable a background task during runtime using a RESTful API and hosted services.
Here’s the intro from the Microsoft Docs, read more here: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services

In ASP.NET Core, background tasks can be implemented as hosted services. A hosted service is a class with background task logic that implements the IHostedService interface.

## Business Logic
So starting from the empy ASP.NET Core 6 template let’s create a simple sample service that represents our business logic that should be invoked by the periodic background tasks. For this demo it’s enough to have a simple method in here to simulate a task and write to the logger.

## Periodic Background Service
Next we need to create the background service that runs a timer for the periodic invocations. This service needs to implement the IHostedService interface in order to registered as a hosted service. To facilitate handling a hosted service we’ll use the BackgroundService base class that handles most of the hosted service house keeping for us and offers the ExecuteAsync method that’s called to run the background service.
In the ExecuteAsync method we can now add a timer with a while loop. I recommend using the PeriodicTimer as it does not block resources (Read more here https://www.ilkayilknur.com/a-new-modern-timer-api-in-dotnet-6-periodictimer). The loop shall run while no cancellation of the background service is requested in the CancellationToken and wait for the next tick of the timer.
It’s a good practise to wrap any invocations inside this while loop into a try catch so that when one run of the while loop fails, it doesn’t break the entire method meaning the periodic loop continues.
Remember we want to control whether the periodic service is running externally using an API later? So we need a control property and use it in the loop.

The last step for our background service is to actually invoke the sample service to execute the business logic. Chances are we want to use a scoped service here. Since no scoped is created for a hosted service by default we need to create one using the IServiceScopeFactory and then get the actual service from the scope. So here is our full class implementation using a scoped service.
## Manage the background service
To represent the current state of the background service let’s introduce a record with a IsEnabled property.
A get route shall return the current state of our background service.
And a put route shall let us set the desired state of our background service:

Note that we inject the background service into each route above? Since it’s not possible to inject a hosted service through dependency injection we need to add this service as a singleton first, then use it for the hosted service registration.

## Demo
All setup, time to play around!

When starting the app the periodic service will try to execute every 5 seconds as intended but will skip the execution of the business logic as the IsEnabled property is set to false by default.

To enable the background service we need to call the newly created put route.
After this request the service now is enabled and executes the business logic in the SampleService.

## Conclusion
Creating a hosted service and using a timer for periodic updates is straight forward. Just keep in mind you need to create a scope first using the IServiceScopeFactory when you want to use a scoped service in a hosted service and to explicitly add the hosted service as a singleton and using this instance when adding the hosted service to use access the hosted service in a controller/route.

NOTE: When hosting this app e.g. in IIS or Azure App Service make sure the app is set to Always on otherwise the hosted service will be shut down after a while
