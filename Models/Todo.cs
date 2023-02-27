using System;

namespace WPFTodo.Models;
public class Todo {
    public string Id { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime AddedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsCompleted => CompletedAt != null;

    public Todo() { }

    public Todo(string title, string description, bool completed = false) {
        Id = Guid.NewGuid().ToString();
        Title = title;
        Description = description;
        AddedAt = DateTime.Now;

        if (completed) {
            CompletedAt = DateTime.Now;
        }
    }

    public void CopyTo(Todo target) {
        target.Title = Title;
        target.Description = Description;
        target.AddedAt = AddedAt;
        target.CompletedAt = CompletedAt;
    }

    public Todo Duplicate() {
        return new Todo() {
            Id = Guid.NewGuid().ToString(),
            Title = Title,
            Description = Description,
            AddedAt = AddedAt,
            CompletedAt = CompletedAt
        };
    }
}
