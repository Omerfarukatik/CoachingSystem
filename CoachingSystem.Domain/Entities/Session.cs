using System;

namespace CoachingSystem.Domain.Entities;

public class Session
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    
    // Hangi koça ait olduğunu gösterir
    public Guid CoachId { get; set; } 
    
    // İlişki tanımları (EF Core için)
    public User? Coach { get; set; } 
}
