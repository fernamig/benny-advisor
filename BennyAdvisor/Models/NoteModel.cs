using System;

namespace BennyAdvisor.Models
{
    public class NoteModel
    {
        public string Context { get; set; }
        public string Note { get; set; }
        public string Source { get; set; }
        public DateTime Date { get; set; }
    }

    public class NoteContextModel
    {
        public string ContextType { get; set; }
        public string ContextId { get; set; }
    }

    public class NewNoteViewModel
    {
        public string Note { get; set; }
        public string StudentId { get; set; }
        public string CreatorId { get; set; }
        public string Permissions { get; set; }
        public NoteContextModel Context { get; set; }
    }

    public class NewNoteModel
    {
        public string Type { get; set; }
        public NewNoteViewModel Attributes { get; set; }
    }

    public class NoteAttributesModel : NewNoteViewModel
    {
        public string Source { get; set; }
    }

    public class Note2Model
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public NoteAttributesModel Attributes { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }
    }
}
