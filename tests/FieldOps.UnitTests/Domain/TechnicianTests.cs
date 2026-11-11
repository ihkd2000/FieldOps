using FieldOps.Domain.Technicians;

namespace FieldOps.UnitTests.Domain;

public class TechnicianTests
{
    [Fact]
    public void Skills_are_case_insensitive_and_deduplicated()
    {
        var technician = new Technician("Sam Rivera", null, null, ["HVAC", "hvac", "Plumbing"]);

        Assert.Equal(2, technician.Skills.Count);
        Assert.True(technician.HasSkill("Hvac"));
    }

    [Fact]
    public void Deactivate_and_reactivate_toggle_status()
    {
        var technician = new Technician("Sam Rivera", null, null);

        technician.Deactivate();
        Assert.False(technician.IsActive);

        technician.Reactivate();
        Assert.True(technician.IsActive);
    }
}
