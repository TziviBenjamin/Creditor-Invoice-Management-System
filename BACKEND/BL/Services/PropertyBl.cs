using BL.Api;
using DAL.Api;
using DAL.Models;

namespace BL.Services;

public class PropertyBl : IPropertyBl
{
    private readonly IPropertyDal _propertyDal;

    public PropertyBl(IPropertyDal propertyDal)
    {
        _propertyDal = propertyDal;
    }

    public List<Property> GetAllProperties() => _propertyDal.GetAllProperties();

    public List<Property> GetPropertiesByUser(int userId) => _propertyDal.GetPropertiesByUser(userId);

    public void AssignPropertyToUser(int userId, int propertyId) =>
        _propertyDal.AssignPropertyToUser(userId, propertyId);

    public void UnassignPropertyFromUser(int userId, int propertyId) =>
        _propertyDal.UnassignPropertyFromUser(userId, propertyId);
}
