﻿using System;

namespace HeroesIcons
{
    [Serializable]
    public class IconException : Exception
    {
        public IconException(string message, Exception ex)
            :base(message, ex)
        {

        }

        public IconException(string message)
            : base(message)
        {

        }
    }
}
