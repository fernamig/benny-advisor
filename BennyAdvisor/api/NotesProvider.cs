using System;
using System.Collections.Generic;
using System.Linq;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class NotesProvider
    {
        readonly CollectionProvider<NoteModel> Provider;

        public NotesProvider()
        {
            Provider = new CollectionProvider<NoteModel>("notes");
        }

        public IEnumerable<NoteModel> Get(string id)
        {
            return Provider.Get(id).OrderByDescending(x => x.Date);
        }

        public void Add(string id, NoteModel note)
        {
            note.Date = DateTime.UtcNow;
            Provider.Set(id, Provider.Get(id).Concat(new[] { note }));
        }
    }
}
