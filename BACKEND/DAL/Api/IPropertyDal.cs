using DAL.Models;

namespace DAL.Api;

public interface IPropertyDal
{
    List<Property> GetAllProperties();
    List<Property> GetPropertiesByUser(int userId);
    void AssignPropertyToUser(int userId, int propertyId);
    void UnassignPropertyFromUser(int userId, int propertyId);
}
