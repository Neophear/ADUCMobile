﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ADUCMobile.Models
{
    public enum MenuItemType
    {
        Stiig,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
