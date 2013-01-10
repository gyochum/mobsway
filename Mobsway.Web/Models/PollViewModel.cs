using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mobsway.Business.Entity;

namespace Mobsway.Web.Models
{
    public class PollViewModel
    {

        public Poll Poll
        {
            get;
            set;
        }

        public List<Poll> Polls
        {
            get;
            set;
        }

        public bool PollHasOptions
        {
            get;
            set;
        }

    }
}