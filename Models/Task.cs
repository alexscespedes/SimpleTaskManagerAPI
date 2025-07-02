using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleTaskManagerAPI.Models;

public class TaskItem
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(TaskItem), nameof(validateDueDate))]
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; } = false;

    // Custom Validator for DueDate.
    public static ValidationResult? validateDueDate(DateTime dueDate)
    {
        return dueDate.Date < DateTime.Today ? new ValidationResult("Due date must be today or a future date.") : ValidationResult.Success;
    }
}
