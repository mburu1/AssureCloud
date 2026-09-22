using System;
using System.Linq.Expressions;
using Ardalis.Specification;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.Enums;

namespace AssureCloud.Domain.Specifications;

public static class OrganizationSpecifications
{
    public static Specification<Organization> ById(Guid id)
    {
        return new Specification<Organization>(x => x.Id == id);
    }

    public static Specification<Organization> ByName(string name)
    {
        return new Specification<Organization>(x => x.Name == name);
    }

    public static Specification<Organization> ByStatus(OrganizationStatus status)
    {
        return new Specification<Organization>(x => x.Status == status);
    }

    public static Specification<Organization> Active()
    {
        return new Specification<Organization>(x => x.Status == OrganizationStatus.Active);
    }

    public static Specification<Organization> WithLocations()
    {
        return new Specification<Organization>(x => x.Locations.Any());
    }

    public static Specification<Organization> WithSuppliers()
    {
        return new Specification<Organization>(x => x.Suppliers.Any());
    }

    public static Specification<Organization> Search(string searchTerm)
    {
        return new Specification<Organization>(x =>
            x.Name.Contains(searchTerm) ||
            (x.Description != null && x.Description.Contains(searchTerm)) ||
            (x.RegistrationNumber != null && x.RegistrationNumber.Contains(searchTerm)));
    }

    public static Specification<Organization> ByParentOrganization(Guid parentId)
    {
        return new Specification<Organization>(x => x.ParentOrganizationId == parentId);
    }

    public static Specification<Organization> TopLevel()
    {
        return new Specification<Organization>(x => x.ParentOrganizationId == null);
    }
}