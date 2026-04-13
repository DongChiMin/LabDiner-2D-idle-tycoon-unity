using LabDiner.Restaurant;

public interface IStaffRegisterable<TStaff> where TStaff : IStaff
{
    void AssignNewStaff(TStaff staff);
}