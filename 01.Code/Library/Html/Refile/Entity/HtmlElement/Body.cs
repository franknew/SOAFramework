﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOAFramework.Library.Refile
{
    public class Body: Element
    {
        public List<Element> Elements { get; set; }
        public List<Img> Imgs { get; set; }
    }
}
