using System;
using SimpleTaskManagerAPI.Models;

namespace SimpleTaskManagerAPI.Repositories;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public IEnumerable<TaskItem> GetAll() => _tasks;

    public TaskItem? GetById(int id) => _tasks.FirstOrDefault(t => t.Id == id);
    public void Add(TaskItem task)
    {
        task.Id = _nextId++;
        _tasks.Add(task);
    }

    public void Update(TaskItem task)
    {
        var existing = GetById(task.Id);
        if (existing is null) return;

        existing.Title = task.Title;
        existing.Description = task.Description;
        existing.DueDate = task.DueDate;
        existing.IsCompleted = task.IsCompleted;
    }

    public void Delete(int id)
    {
        var task = GetById(id);
        if (task is not null) _tasks.Remove(task);
    }

    public void MarkComplete(int id)
    {
        var task = GetById(id);
        if (task is not null)
        {
            task.IsCompleted = true;
        }
    }

    public bool Exists(int id) => _tasks.Any(t => t.Id == id);

}
