// Guids.cs
// MUST match guids.h
using System;

namespace MicrosoftIT.AutoHideToolbarExample
{
    static class GuidList
    {
        public const string guidAutoHideToolbarExamplePkgString = "fa4e082d-4fef-402a-9056-d0987c183ff7";
        public const string guidAutoHideToolbarExampleCmdSetString = "efbdc202-9ecf-4734-91f6-34cfef71ee82";
        public const string guidToolWindowPersistanceString = "54577230-b515-4067-b071-bb0be00f2a9c";

        public static readonly Guid guidAutoHideToolbarExampleCmdSet = new Guid(guidAutoHideToolbarExampleCmdSetString);
    };
}