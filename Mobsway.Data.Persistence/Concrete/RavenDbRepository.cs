using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobsway.Business.Entity;
using Raven.Client;
using Raven.Client.Document;

namespace Mobsway.Data.Persistence
{
    public class RavenDbRepository:IPollRepository
    {

        private DocumentStore store = null;
        private IDocumentSession session = null;

        public RavenDbRepository(DocumentStore s)
        {
            store = s;
            session = store.OpenSession();
        }

        public List<Poll> GetAllPolls(bool activeOnly)
        {
            if(activeOnly)
                return session.Query<Poll>().Where(x => x.IsActive == activeOnly).ToList();
            else
                return session.Query<Poll>().ToList();
        }

        public List<Poll> GetUserPolls(string username)
        {
            return session.Query<Poll>().Where(x => x.CreatedBy == username).ToList();
        }

        public Poll GetPoll(string pollId)
        {
            return session.Query<Poll>().SingleOrDefault(x => x.PollNumber == pollId);
        }

        public Poll Save(Poll poll)
        {
            session.Store(poll);
            session.SaveChanges();

            return poll;
        }

        public bool Delete(Poll poll)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string pollId)
        {
            throw new NotImplementedException();
        }
    }
}
