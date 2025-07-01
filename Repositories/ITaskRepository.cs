using System;
using SimpleTaskManagerAPI.Models;

namespace SimpleTaskManagerAPI.Repositories;

public interface ITaskRepository
{
    IEnumerable<TaskItem> GetAll();
    TaskItem? GetById(int id);
    void Add(TaskItem task);
    void Update(TaskItem task);
    void Delete(int id);
    void MarkComplete(int id);
    bool Exists(int id);
}
