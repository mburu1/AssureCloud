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
        var spec = new Specification<Organization>();
        spec.Query.Where(x => x.Id == id);
        return spec;
    }

    public static Specification<Organization> ByName(string name)
    {
        var spec = new Specification<Organization>();
        spec.Query.Where(x => x.Name == name);
        return spec;
    }

    public static Specification<Organization> ByStatus(OrganizationStatus status)
    {
        var spec = new Specification<Organization>();
        spec.Query.Where(x => x.Status == status);
        return spec;
    }

    public static Specification<Organization> Active()
    {
        var spec = new Specification<Organization>();
        spec.Query.Where(x => x.Status == OrganizationStatus.Active);
        return spec;
    }

    public static Specification<Organization> WithLocations()
    {
        var spec = new Specification<Organization>();
        spec.Query.Where(x => x.Locations.Any());
        return spec;
    }

    public static Specification<Organization> WithSuppliers()
    {
        var spec = new Specification<Organization>();
        spec.Query.Where(x => x.Suppliers.Any());
        return spec;
    }

    public static Specification<Organization> Search(string searchTerm)
    {
        var spec = new Specification<Organization>();
        spec.Query.Where(x =>
            x.Name.Contains(searchTerm) ||
            (x.Description != null && x.Description.Contains(searchTerm)) ||
            (x.RegistrationNumber != null && x.RegistrationNumber.Contains(searchTerm)));
        return spec;
    }

    public static Specification<Organization> ByParentOrganization(Guid parentId)
    {
        var spec = new Specification<Organization>();
        spec.Query.Where(x => x.ParentOrganizationId == parentId);
        return spec;
    }

    public static Specification<Organization> TopLevel()
    {
        var spec = new Specification<Organization>();
        spec.Query.Where(x => x.ParentOrganizationId == null);
        return spec;
    }
}