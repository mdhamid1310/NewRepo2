using System.ComponentModel.DataAnnotations;
using System;

public class Questions
{
    public int Id { get; set; }

    [Required(ErrorMessage = "SLNO is required.")]
    public int SLNO { get; set; }

    [Required(ErrorMessage = "Customer Name is required.")]
    [MaxLength(100)] // Adjust the length as needed
    public string CUSTNAME { get; set; }

    [MaxLength(255)] // Adjust the length as needed
    public string Question { get; set; } = "nothing";

    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Now; // Default to the current date

    [MaxLength(255)] // Adjust the length as needed
    public string Answer { get; set; } = "nothing";
}

