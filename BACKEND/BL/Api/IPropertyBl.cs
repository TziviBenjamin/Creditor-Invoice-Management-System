using DAL.Models;

namespace BL.Api;

public interface IPropertyBl
{
    List<Property> GetAllProperties();
    List<Property> GetPropertiesByUser(int userId);
    void AssignPropertyToUser(int userId, int propertyId);
    void UnassignPropertyFromUser(int userId, int propertyId);
}
