using Godot;
using System;

namespace Administrator.Utilities.Extensions
{
    /// <summary> Helpful extensions for working with player input. </summary>
    public static class InputExtensions
    {
        /// <summary> Check if the any input key has been just pressed. Holding it down will not trigger this again. </summary>
        /// <param name="event"> The event to check. </param>
        /// <returns> Has a key just been pressed? </returns>
        public static Boolean IsJustPressed(this InputEventKey @event)
        {
            return @event.Pressed && !@event.Echo;
        }


        /// <summary> Check if the given key has been just pressed. Holding it down will not trigger this again. </summary>
        /// <param name="event"> The event to check. </param>
        /// <param name="key"> The keycode to check. </param>
        /// <returns> Has the key just been pressed? </returns>
        public static Boolean IsJustPressed(this InputEventKey @event, Key key)
        {
            return @event.Keycode == key && @event.Pressed && !@event.Echo;
        }


        /// <summary> Check if the given key has been just released. </summary>
        /// <param name="event"> The event to check. </param>
        /// <param name="key"> The keycode to check. </param>
        /// <returns> Has the key just been released? </returns>
        public static Boolean IsJustReleased(this InputEventKey @event, Key key)
        {
            return @event.Keycode == key && !@event.Pressed && !@event.Echo;
        }


        /// <summary> Check if any input key has been just released. </summary>
        /// <param name="event"> The event to check. </param>
        /// <returns> Has a key just been released? </returns>
        public static Boolean IsJustReleased(this InputEventKey @event)
        {
            return !@event.Pressed && !@event.Echo;
        }
    }
}
