using FieldOps.Domain.Common;

namespace FieldOps.Domain.Technicians;

public sealed class Technician : Entity
{
    private readonly HashSet<string> _skills = new(StringComparer.OrdinalIgnoreCase);

    private Technician() { Name = null!; }

    public Technician(string name, string? email, string? phone, IEnumerable<string>? skills = null)
    {
        Name = Guard.Required(name, nameof(Name));
        Email = Guard.Optional(email);
        Phone = Guard.Optional(phone);

        foreach (var skill in skills ?? [])
        {
            AddSkill(skill);
        }
    }

    public string Name { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public bool IsActive { get; private set; } = true;
    public IReadOnlyCollection<string> Skills => _skills;

    public void AddSkill(string skill) => _skills.Add(Guard.Required(skill, "Skill"));

    public void RemoveSkill(string skill) => _skills.Remove(skill);

    public bool HasSkill(string skill) => _skills.Contains(skill);

    public void Deactivate() => IsActive = false;

    public void Reactivate() => IsActive = true;
}
