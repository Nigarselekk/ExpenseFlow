

using System.Text.Json.Serialization;

namespace ExpenseFlow.Application.Requests;

public class ExpenseRequest
{
  
  public int CategoryId { get; set; }
  public decimal Amount { get; set; }
  public string Description { get; set; } = null!;
  public string? Location { get; set; }

  [JsonIgnore]
  public Guid PersonnelId { get; set; }
}

