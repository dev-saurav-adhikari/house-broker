using HouseBroker.Application.DTOs;

namespace HouseBroker.Application.Constants;

public static class CacheKeys
{
    public const string CommissionSettings = "settings_commission";
    public const string CommissionsKey = "commissions_all";
    public const string PropertiesVersionKey = "prop_version";

    public static string AllProperties(int version, PropertyFilterDto filter)
    {
        return $"prop_list:v{version}:" +
               $"p{filter.PageNumber}_s{filter.PageSize}_" +
               $"search_{filter.Search ?? "null"}_" +
               $"prov_{filter.ProvinceId?.ToString() ?? "null"}_" +
               $"dist_{filter.DistrictId?.ToString() ?? "null"}_" +
               $"ward_{filter.WardNumber?.ToString() ?? "null"}_" +
               $"type_{((int?)filter.PropertyType)?.ToString() ?? "null"}_" +
               $"min_{filter.MinPrice?.ToString() ?? "null"}_" +
               $"max_{filter.MaxPrice?.ToString() ?? "null"}";
    }

    public static string BrokerProperties(long brokerId) => $"{brokerId}_properties";
}