﻿using Analyser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyser.Core.Services
{
    public class LogParserService : ILogParserService
    {
        IContext context;
        public LogParserService(IContext context)
        {
            this.context = context;
        }
    }
}