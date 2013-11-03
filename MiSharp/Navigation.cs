using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;

namespace MiSharp
{
    public interface INavigationService
    {
        void Navigate(Type viewModelType, object modelParams);
    }

    // This is just to help with some reflection stuff
    public interface IViewModelParams
    {
    }

    public interface IViewModelParams<T> : IViewModelParams
    {
        // It contains a single method which will pass arguments to the viewmodel after the nav service has instantiated it from the container
        void ProcessParameters(T modelParams);
    }

    public class NavigationService : INavigationService
    {
        // Depends on the aggregator - this is how the shell or any interested VMs will receive
        // notifications that the user wants to navigate to someplace else
        private readonly IEventAggregator _aggregator;

        public NavigationService(IEventAggregator aggregator)
        {
            _aggregator = aggregator;
        }

        // And the navigate method goes:
        public void Navigate(Type viewModelType, object modelParams)
        {
            // Resolve the viewmodel type from the container
            object viewModel = IoC.GetInstance(viewModelType, null);

            // Inject any props by passing through IoC buildup
            IoC.BuildUp(viewModel);

            // Check if the viewmodel implements IViewModelParams and call accordingly
            IEnumerable<Type> interfaces = viewModel.GetType().GetInterfaces()
                .Where(x => typeof (IViewModelParams).IsAssignableFrom(x) && x.IsGenericType);

            // Loop through interfaces and find one that matches the generic signature based on modelParams...
            foreach (Type @interface in interfaces)
            {
                Type type = @interface.GetGenericArguments()[0];
                MethodInfo method = @interface.GetMethod("ProcessParameters");

                if (type.IsInstanceOfType(modelParams))
                {
                    // If we found one, invoke the method to run ProcessParameters(modelParams)
                    method.Invoke(viewModel, new[] {modelParams});
                }
            }

            // Publish an aggregator event to let the shell/other VMs know to change their active view
            _aggregator.Publish(new NavigationEventMessage((IScreen) viewModel));
        }
    }

    public class NavigationEventMessage
    {
        public NavigationEventMessage(IScreen viewModel)
        {
            ViewModel = viewModel;
        }

        public IScreen ViewModel { get; private set; }
    }
}