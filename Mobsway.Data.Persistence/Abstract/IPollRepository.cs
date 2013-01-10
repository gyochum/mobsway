using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobsway.Business.Entity;

namespace Mobsway.Data.Persistence
{
    public interface IPollRepository
    {

        List<Poll> GetAllPolls(bool activeOnly);
        List<Poll> GetUserPolls(string username);
        Poll GetPoll(string pollId);
        Poll Save(Poll poll);
        bool Delete(Poll poll);
        bool Delete(string pollId);

    }
}
