using System;
using Microsoft.AspNetCore.Http;

namespace IndecisionEngine.Controllers
{
    public static class StateHelper
    {
        private const string StateKey = "story.state";

        public static void SetState(HttpContext context, string state)
        {
            context.Session.SetString(StateKey, state ?? string.Empty);
        }

        public static string GetState(HttpContext context)
        {
            return context.Session.GetString(StateKey);
        }

        public static string Update(HttpContext httpContext, string effects)
        {
            var state = GetState(httpContext);

            // TODO: Apply effects for real
            state += ";" + effects;

            SetState(httpContext, state);
            return state;
        }
    }
}
