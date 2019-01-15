using System;
using UnityEngine.Experimental.Input.Utilities;

////TODO: add way for interactions to advertise the layouts they work with

////REVIEW: what about putting an instance of one of these on every resolved control instead of sharing it between all controls resolved from a binding?

////REVIEW: can we have multiple interactions work together on the same binding? E.g. a 'Press' causing a start and a 'Release' interaction causing a performed

////REVIEW: have a default interaction so that there *always* is an interaction object when processing triggers?

namespace UnityEngine.Experimental.Input
{
    /// <summary>
    /// Interface for interaction patterns that drive actions.
    /// </summary>
    /// <remarks>
    /// By default, actions will start when a source control leaves its default state
    /// and will be completed when the control goes back to that state. Interactions can customize
    /// this and also implement logic that signals cancellations (which the default logic never
    /// triggers).
    ///
    /// Interactions can be stateful and mutate state over time.
    /// </remarks>
    public interface IInputInteraction
    {
        void Process(ref InputInteractionContext context);
        void Reset();
    }

    /// <summary>
    /// Identical to <see cref="IInputInteraction"/> except that it allows an interaction to explicitly
    /// advertise the value it expects.
    /// </summary>
    /// <typeparam name="TValue">Type of values expected by the interaction</typeparam>
    /// <remarks>
    /// Advertising the value type will an interaction type to be filtered out in the UI if the value type
    /// it has is not compatible with the value type expected by the action.
    /// </remarks>
    public interface IInputInteraction<TValue> : IInputInteraction
        where TValue : struct
    {
    }

    internal static class InputInteraction
    {
        public static TypeTable s_Interactions;

        public static Type GetValueType(Type interactionType)
        {
            if (interactionType == null)
                throw new ArgumentNullException(nameof(interactionType));

            return TypeHelpers.GetGenericTypeArgumentFromHierarchy(interactionType, typeof(IInputInteraction<>), 0);
        }
    }
}
