﻿using System;

namespace Unity.Usercentrics
{
    internal class Errors
    {
        public const string NOT_INITIALIZED_MSG = "Usercentrics is not initialized";
        public const string PLATFORM_NOT_SUPPORTED_MSG = "Usercentrics does not " +
            "support this platform. " +
            "See the docs: https://docs.usercentrics.com/cmp_in_app_sdk/latest/getting_started/install/";
    }

    public class NotInitializedException : Exception
    {
        public NotInitializedException() : base(Errors.NOT_INITIALIZED_MSG) { }
    }

    public class PlatformSupportException : Exception
    {
        public PlatformSupportException() : base(Errors.PLATFORM_NOT_SUPPORTED_MSG) { }
    }
}
